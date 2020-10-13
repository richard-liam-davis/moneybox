using System;

namespace Moneybox.App
{
    public class Account
    {
        public const decimal PayInLimit = 4000m;

        public Guid Id { get; set; }

        public User User { get; set; }

        public decimal Balance { get; set; }

        public decimal Withdrawn { get; set; }

        public decimal PaidIn { get; set; }

        public bool FundsLow { get; set; }

        public bool ApproachingPayInLimit { get; set; }

        public void TransferOut(decimal transferAmount)
        {
            var newBalance = Balance - transferAmount;
            
            if (newBalance < 0m)
            {
                throw new InvalidOperationException("Insufficient funds to make transfer");
            }

            if (newBalance < 500m)
            {
                FundsLow = true;
            }

            Balance = newBalance;
            Withdrawn = Withdrawn - transferAmount;
        }

        public void TransferIn(decimal transferAmount)
        {
            var paidIn = PaidIn + transferAmount;
            if (paidIn > PayInLimit)
            {
                throw new InvalidOperationException("Account pay in limit reached");
            }

            if (PayInLimit - paidIn < 500m)
            {
                ApproachingPayInLimit = true;
            }

            Balance = Balance + transferAmount;
            PaidIn = PaidIn + transferAmount;
        }
    }
}
