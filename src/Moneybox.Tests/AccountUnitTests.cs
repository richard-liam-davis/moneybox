using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moneybox.App;
using System;

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
        
        [TestMethod]
        public void Cannot_Transfer_Out_More_Than_You_Have()
        {
            var account = new Account();

            account.Balance = 1000;

            Assert.ThrowsException<InvalidOperationException>(() => account.TransferOut(10000));
        }

        [TestMethod]
        public void Cannot_Transfer_In_More_Than_You_Are_Allowed()
        {
            var account = new Account();

            Assert.ThrowsException<InvalidOperationException>(() => account.TransferIn(10000));
        }
    }
}
