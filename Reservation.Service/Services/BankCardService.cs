using Microsoft.EntityFrameworkCore;
using Reservation.Data;
using Reservation.Models.BankCard;
using Reservation.Models.Common;
using Reservation.Service.Helpers;
using Reservation.Service.Interfaces;
using System;
using System.Threading.Tasks;

namespace Reservation.Service.Services
{
    public class BankCardService : IBankCardService
    {
        private readonly ApplicationContext _db;
        public BankCardService(ApplicationContext db)
        {
            _db = db;
        }

        public async Task<RequestResult> AttachCardToMemberAsync(AttachCardToMemberModel model)
        {
            var result = new RequestResult();

            var bankCard = await _db.BankCards.FirstOrDefaultAsync(i => i.Number == model.CardNumber
             && i.ValidThru.Year == model.ValidThru.Year
             && i.ValidThru.Month == model.ValidThru.Month
             && i.CVV == model.CVV
             && i.Owner == model.Owner);
            if (bankCard == null)
            {
                result.Message = ErrorMessages.BankCardDoesNotExist;
                return result;
            }

            if (!IsValidCard(bankCard.ValidThru))
            {
                result.Message = ErrorMessages.BankCardExpired;
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
                result.Message = ErrorMessages.BankCardDoesNotExist;
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
            }

            return result;
        }

        private bool IsValidCard(DateTime validThru)
        {
            return validThru.Date > DateTime.Now;
        }
    }
}
