using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commission;

namespace Commission.UnitTests
{
    [TestClass]
    public class CommissionLoggerTests
    {
        [TestMethod]
        public void GetBonus_CaseIsOver100000_ReturnsBonus()
        {
            CommissionLogger c = new CommissionLogger();
        }
 
        public void GetBonus_CaseIsOver250000_ReturnsBonus()
        {

        }
    }
}
