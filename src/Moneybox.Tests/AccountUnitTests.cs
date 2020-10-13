using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moneybox.App;

namespace Moneybox.Tests
{
    [TestClass]
    public class AccountUnitTests
    {
        [TestMethod]
        public void Can_Transfer_In()
        {
            var account = new Account();

            account.Balance = 100;

            account.TransferIn(200);

            Assert.AreEqual(300, account.Balance);
        }

        [TestMethod]
        public void Can_Transfer_Out()
        {
            var account = new Account();

            account.Balance = 1000;

            account.TransferOut(100);

            Assert.AreEqual(900, account.Balance);
        }
    }
}
