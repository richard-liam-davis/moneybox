using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using System;

namespace Moneybox.App.Features
{
    public class WithdrawMoney
    {
        private IAccountRepository accountRepository;
        private INotificationService notificationService;

        public WithdrawMoney(IAccountRepository accountRepository, INotificationService notificationService)
        {
            this.accountRepository = accountRepository;
            this.notificationService = notificationService;
        }

        public void Execute(Guid fromAccountId, decimal withdrawAmount)
        {
            var fromAccount = this.accountRepository.GetAccountById(fromAccountId);

            fromAccount.TransferOut(withdrawAmount);

            this.accountRepository.Update(fromAccount);

            // send notification only if the updates were successful
            if (fromAccount.FundsLow)
            {
                this.notificationService.NotifyFundsLow(fromAccount.User.Email);
            }
        }
    }
}
