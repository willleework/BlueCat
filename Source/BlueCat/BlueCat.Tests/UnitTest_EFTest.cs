using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlueCat.DAL.MySQL1130C;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace BlueCat.Tests
{
    /// <summary>
    /// UnitTest_EFTest 的摘要说明
    /// </summary>
    [TestClass]
    public class UnitTest_EFTest
    {
        MySQL1130C dbtrade = new MySQL1130C();

        public UnitTest_EFTest()
        {
            //
            //TODO:  在此处添加构造函数逻辑
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性: 
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod_StockTable()
        {
            int[] markets = new int[] { 3, 4, 7, 9, 34 };
            Dictionary<int, List<pub_tstockinfo>> dic_stocks = new Dictionary<int, List<pub_tstockinfo>>();
            foreach (int market in markets)
            {
                dic_stocks.Add(market, new List<pub_tstockinfo>());
            }
            List<pub_tstockinfo> stocks = dbtrade.pub_tstockinfo.Where(p=>markets.Contains(p.market_no)).ToList();
            int count = stocks.Count();
            Parallel.ForEach(Partitioner.Create(0, count, 1000), p =>
            {
                for (int m = p.Item1; m < p.Item2; m++)
                {
                    try
                    {
                        dic_stocks[stocks[m].market_no].Add(stocks[m]);
                    }
                    catch (Exception ex)
                    {
                        ;
                    }
                }
            });
            //for (int j = 0; j < count; j++)
            //{
            //    dic_stocks[stocks[j].market_no].Add(stocks[j]);
            //}
        }
    }
}
