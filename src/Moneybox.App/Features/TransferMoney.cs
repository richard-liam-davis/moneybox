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

            fromAccount.TransferOut(transferAmount);
            toAccount.TransferIn(transferAmount);

            // TODO: Implement a transaction to ensure both accounts are updated successfully
            this.accountRepository.Update(fromAccount);
            this.accountRepository.Update(toAccount);

            // send notifications only if the updates were successful
            if (fromAccount.FundsLow)
            {
                this.notificationService.NotifyFundsLow(fromAccount.User.Email);
            }

            if (toAccount.ApproachingPayInLimit)
            {
                this.notificationService.NotifyApproachingPayInLimit(toAccount.User.Email);
            }
        }
    }
}
