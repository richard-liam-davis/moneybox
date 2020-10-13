using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using System;

namespace Moneybox.App.Features
{
    public class TransferMoney
    {
        private IAccountRepository accountRepository;
        private INotificationService notificationService;

        public TransferMoney(IAccountRepository accountRepository, INotificationService notificationService)
        {
            this.accountRepository = accountRepository;
            this.notificationService = notificationService;
        }

        public void Execute(Guid fromAccountId, Guid toAccountId, decimal transferAmount)
        {
            var fromAccount = this.accountRepository.GetAccountById(fromAccountId);
            var toAccount = this.accountRepository.GetAccountById(toAccountId);

            var paidIn = toAccount.PaidIn + transferAmount;
            if (paidIn > Account.PayInLimit)
            {
                throw new InvalidOperationException("Account pay in limit reached");
            }

            if (Account.PayInLimit - paidIn < 500m)
            {
                this.notificationService.NotifyApproachingPayInLimit(toAccount.User.Email);
            }

            fromAccount.Balance = fromAccount.Balance - transferAmount;
            fromAccount.Withdrawn = fromAccount.Withdrawn - transferAmount;

            toAccount.Balance = toAccount.Balance + transferAmount;
            toAccount.PaidIn = toAccount.PaidIn + transferAmount;

            this.accountRepository.Update(fromAccount);
            this.accountRepository.Update(toAccount);
        }
    }
}
