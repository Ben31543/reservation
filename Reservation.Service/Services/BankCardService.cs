using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reservation.Data;
using Reservation.Models.BankCard;
using Reservation.Models.Common;
using Reservation.Resources.Contents;
using Reservation.Service.Interfaces;
using System;
using System.Threading.Tasks;

namespace Reservation.Service.Services
{
    public class BankCardService : IBankCardService
    {
        private readonly ApplicationContext _db;
        private readonly ILogger _logger;

        public BankCardService(ApplicationContext db, ILogger<BankCardService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<RequestResult> AttachCardToMemberAsync(AttachCardToMemberModel model)
        {
            var result = new RequestResult();

            var bankCard = await _db.BankCards.FirstOrDefaultAsync(
                i => i.Number == model.CardNumber
                  && IsValidDate(model.ValidThru, i.ValidThru, false)
                  && i.CVV == model.CVV
                  && i.Owner == model.Owner);
            if (bankCard == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.BankCardDoesNotExist;
                return result;
            }

            if (!IsValidDate(model.ValidThru, bankCard.ValidThru, true))
            {
                result.Message = LocalizationKeys.ErrorMessages.BankCardExpired;
                return result;
            }

            bankCard.IsAttached = true;

            try
            {
                await _db.SaveChangesAsync();
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                _logger.LogError(e.Message);
            }

            result.Value = bankCard.Id;
            return result;
        }

        public async Task<RequestResult> DetachCardFromMemberAsync(long bankCardId)
        {
            var result = new RequestResult();

            var card = await _db.BankCards.FirstOrDefaultAsync(i => i.Id == bankCardId);

            if (card == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.BankCardDoesNotExist;
                return result;
            }

            card.IsAttached = false;

            try
            {
                await _db.SaveChangesAsync();
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                _logger.LogError(e.Message);
            }

            return result;
        }

        private bool IsValidDate(DateTime incoming, DateTime existing, bool toCheckForExpiration)
        {
			if (toCheckForExpiration)
			{
                return incoming.Date <= existing.Date;
			}

            return incoming.Year == existing.Year && incoming.Month == existing.Month;
        }
    }
}
