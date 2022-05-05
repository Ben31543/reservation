using Microsoft.Extensions.Logging;
using Reservation.Data;
using Reservation.Data.Entities;
using Reservation.Models.Common;
using Reservation.Models.Reserving;
using Reservation.Resources.Contents;
using Reservation.Service.Interfaces;
using System;
using System.Threading.Tasks;

namespace Reservation.Service.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationContext _db;
        private readonly IBankCardService _bankCard;
        private readonly IBankAccountService _bankAccount;
        private readonly ILogger _logger;

        public PaymentService(
            ApplicationContext db,
            IBankCardService bankcard,
            IBankAccountService bankAccount,
            ILogger<PaymentService> logger)
        {
            _db = db;
            _bankCard = bankcard;
            _bankAccount = bankAccount;
            _logger = logger;
        }

        public async Task<RequestResult> AddPaymentDataAsync(PaymentDataModel model)
        {
            RequestResult result = new RequestResult();

            var paymentData = new Payment
            {
                Amount = model.Amount,
                BankAcountIdTo = model.BankAcountTo,
                BankCardIdFrom = model.BankCardAccountFrom,
                PaymentDate = DateTime.Now
            };

            await _db.PaymentDatas.AddAsync(paymentData);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                result.Message = e.Message;
                return result;
            }

            result = await RequestPaymentAsync(model.BankCardId, model.BankAccountId, paymentData);
            return result;
        }

        public async Task<RequestResult> RequestRefundAsync(long fromId, long toId, decimal amount)
        {
            RequestResult result = new RequestResult();

            var bankCard = await _bankCard.GetBankCardByIdAsync(fromId);
            if (bankCard == null)
            {
                result.Message = LocalizationKeys.Errors.BankCardDoesNotExist;
                return result;
            }

            var bankAccount = await _bankAccount.GetBankAccountInfoAsync(toId);
            if (bankAccount == null)
            {
                result.Message = LocalizationKeys.Errors.BankAccountDoesNotExist;
                return result;
            }

            if (bankAccount.Balance < amount)
            {
                result.Message = LocalizationKeys.Errors.InsufficientBalance;
                return result;
            }

            try
            {
                bankAccount.Balance -= amount;
                bankCard.CurrentBalance += amount;
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                result.Message = e.Message;
                return result;
            }

            result.Succeeded = true;
            return result;
        }
        
        private async Task<RequestResult> RequestPaymentAsync(long fromId, long toId, Payment paymentData)
        {
            RequestResult result = new RequestResult();

            var bankCard = await _bankCard.GetBankCardByIdAsync(fromId);
            if (bankCard == null)
            {
                result.Message = LocalizationKeys.Errors.BankCardDoesNotExist;
                return result;
            }

            var bankAccount = await _bankAccount.GetBankAccountInfoAsync(toId);
            if (bankAccount == null)
            {
                result.Message = LocalizationKeys.Errors.BankAccountDoesNotExist;
                return result;
            }

            if (bankCard.CurrentBalance < paymentData.Amount)
            {
                result.Message = LocalizationKeys.Errors.InsufficientBalance;
                return result;
            }

            try
            {
                bankCard.CurrentBalance -= paymentData.Amount;
                bankAccount.Balance += paymentData.Amount;
                paymentData.Approved = true;
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                result.Message = e.Message;
                return result;
            }

            result.Succeeded = true;
            return result;
        }
    }
}
