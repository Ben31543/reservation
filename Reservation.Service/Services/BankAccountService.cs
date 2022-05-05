using Microsoft.EntityFrameworkCore;
using Reservation.Data;
using Reservation.Data.Entities;
using Reservation.Models.BankAccount;
using Reservation.Models.Common;
using Reservation.Resources.Contents;
using Reservation.Service.Interfaces;
using System.Threading.Tasks;

namespace Reservation.Service.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly ApplicationContext _db;

        public BankAccountService(ApplicationContext db)
        {
            _db = db;
        }

        public async Task<RequestResult> AttachBankAccountToServiceMemberAsync(BankAccountAttachModel model)
        {
            RequestResult result = new RequestResult();
            var bankAccount = await _db.BankAccounts.FirstOrDefaultAsync(i => i.AccountNumber == model.AccountNumber && i.Owner == model.Owner);
            if (bankAccount == null)
            {
                result.Message = LocalizationKeys.Errors.BankAccountDoesNotExist;
                return result;
            }

            await _db.SaveChangesAsync();
            result.Succeeded = true;
            result.Value = bankAccount.Id;
            return result;
        }

        public async Task<RequestResult> DetachServiceMemberFromBankAccountAsync(long accountId)
        {
            RequestResult result = new RequestResult();
            var bankAccount = await GetBankAccountInfoAsync(accountId);
            if (bankAccount == null)
            {
                result.Message = LocalizationKeys.Errors.BankAccountDoesNotExist;
                return result;
            }

            result.Succeeded = true;
            return result;
        }

        public async Task<BankAccount> GetBankAccountInfoAsync(long bankAccountId)
        {
            return await _db.BankAccounts.FirstOrDefaultAsync(i => i.Id == bankAccountId);
        }
    }
}
