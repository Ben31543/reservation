using Reservation.Data.Entities;
using Reservation.Models.BankAccount;
using Reservation.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Service.Interfaces
{
    public interface IBankAccountService
    {
        Task<RequestResult> AttachBankAccountToServiceMemberAsync(BankAccountAttachModel model);

        Task<RequestResult> DetachServiceMemberFromBankAccountAsync(long accountId);

        Task<BankAccount> GetBankAccountInfoAsync(long bankAccountId);
    }
}
