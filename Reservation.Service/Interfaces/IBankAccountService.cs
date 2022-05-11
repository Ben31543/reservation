using Reservation.Data.Entities;
using Reservation.Models.BankAccount;
using Reservation.Models.Common;
using System.Threading.Tasks;

namespace Reservation.Service.Interfaces
{
    public interface IBankAccountService
    {
        Task<RequestResult> AttachBankAccountToServiceMemberAsync(BankAccountAttachModel model);

        Task<RequestResult> DetachServiceMemberFromBankAccountAsync(long accountId);

        Task<BankAccount> GetBankAccountInfoAsync(long bankAccountId);

        Task<string> GetBankAccountNumberAsync(long bankAccountId);
    }
}
