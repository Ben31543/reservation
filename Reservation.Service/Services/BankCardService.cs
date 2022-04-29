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

        public BankCardService(
            ApplicationContext db,
            ILogger<BankCardService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<RequestResult> AttachCardToMemberAsync(AttachCardToMemberModel model)
        {
            var result = new RequestResult();

            var bankCard = await _db.BankCards.FirstOrDefaultAsync(
                i => i.Number == model.CardNumber
                  && i.ValidThru.Year == model.ValidThru.Year
                  && i.ValidThru.Month == i.ValidThru.Month
                  && i.ValidThru>=DateTime.Now
                  && i.CVV == model.CVV
                  && i.Owner == model.Owner);

            var member = await _db.Members.FirstOrDefaultAsync(i => i.Id == model.MemberId);

            if (bankCard == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.BankCardDoesNotExist;
                return result;
            }

            if (member == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.MemberDoesNotExist;
                return result;
            }

            if (!IsValidDate(model.ValidThru, bankCard.ValidThru, true))
            {
                result.Message = LocalizationKeys.ErrorMessages.BankCardExpired;
                return result;
            }

            member.BankCardId = bankCard.Id;

            try
            {
                await _db.SaveChangesAsync();
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                result.Message = e.Message;
                return result;
            }

            result.Value = bankCard.Id;
            return result;
        }

        public async Task<RequestResult> DetachCardFromMemberAsync(long memberId,long bankCardId)
        {
            var result = new RequestResult();

            var card = await _db.BankCards.FirstOrDefaultAsync(i => i.Id == bankCardId);

            var member = await _db.Members.FirstOrDefaultAsync(i => i.Id == memberId);

            if (card == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.BankCardDoesNotExist;
                return result;
            }

            if (member == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.MemberDoesNotExist;
                return result;
            }

            member.BankCardId = null;

            try
            {
                await _db.SaveChangesAsync();
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                result.Message = e.Message;
                return result;
            }

            return result;
        }

        public async Task<Data.Entities.BankCard> GetBankCardByIdAsync(long id)
        {
            return await _db.BankCards.FirstOrDefaultAsync(i => i.Id == id);
        }

        private bool IsValidDate(DateTime incoming, DateTime existing, bool toCheckForExpiration)
        {
            if (toCheckForExpiration)
            {
                return incoming.Date <= existing.Date;
            }

            return incoming.Year == existing.Year && incoming.Month == existing.Month && incoming>DateTime.Now;
        }
    }
}
