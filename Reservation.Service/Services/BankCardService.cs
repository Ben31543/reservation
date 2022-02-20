using Microsoft.EntityFrameworkCore;
using Reservation.Data;
using Reservation.Models.BankCard;
using Reservation.Models.Common;
using Reservation.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Service.Services
{
    public class BankCardService : IBankCardService
    {
        private readonly IMemberService _member;
        private readonly ApplicationContext _db;
        public BankCardService(ApplicationContext db, IMemberService member)
        {
            _db = db;
            _member = member;
        }

        public async Task<RequestResult> AttachCardToMemberAsync(AttachCardToMemberModel model)
        {
            var result = new RequestResult();

            var bankCard = await _db.BankCards.FirstOrDefaultAsync(i => i.Number == model.CardNumber
             && i.ValidThru == model.ValidThru
             && i.CVV == model.CVV
             && i.Owner == model.Owner);
            if (bankCard == null)
            {
                result.Message = "CardDoesNotExist";
                return result;
            }

            if (!IsValidCard(bankCard.ValidThru))
            {
                result.Message = "CardHasExpired";
                return result;
            }

            var member = await _member.GetMemberByIdAsync(model.MemberId);
            if (member == null)
            {
                result.Message = "MemberNotFound";
                return result;
            }

            member.BankCardId = bankCard.Id;
            bankCard.IsAttached = true;
            bankCard.MemberId = member.Id;

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

        public async Task<RequestResult> DetachCardFromMemberAsync(long bankCardId)
        {
            var result = new RequestResult();

            var card = await _db.BankCards
                .Include(i => i.Member)
                .FirstOrDefaultAsync(i => i.Id == bankCardId);

            if (card == null)
            {
                result.Message = "CardDoesNotExist";
                return result;
            }

            var member = await _member.GetMemberByIdAsync(card.Member.Id);
            if (member == null)
            {
                result.Message = "MemberNotFound";
                return result;
            }

            member.BankCardId = null;
            card.MemberId = null;
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
