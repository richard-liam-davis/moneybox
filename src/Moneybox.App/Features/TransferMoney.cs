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

            try
            {
                this.accountRepository.Update(fromAccount);
                this.accountRepository.Update(toAccount);
            }
            catch(Exception ex)
            {
                // TODO: Some form of rollback here
            }

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
