using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlueCat.ConfigTools;
using BlueCat.DAL.MySQL1130C;
using System.IO;
using BlueCat.Tools.FileTools;

namespace BlueCat.Tests
{
    /// <summary>
    /// UnitTest_ConfigTools 的摘要说明
    /// </summary>
    [TestClass]
    public class UnitTest_ConfigTools
    {
        private string _tempWorkPath = "F:\\ConfigTest";

        public UnitTest_ConfigTools()
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
        public void TestMethod_ServerFileModify()
        {
            string desPath = Path.Combine(_tempWorkPath, "Compress.7z");
            string taskPath = Path.Combine(_tempWorkPath, "TaskConfig", "ConfigTasks.json");
            string xmlPath = Path.Combine(_tempWorkPath, "Decompress", "GridLayoutInfo.xml");
            List<yh_tclientconfig> dbconfigs = ConfigManage.GetServerConfigInfo(10003, "0", "OPLUS_20171130C");
            if (dbconfigs.Count <= 0)
            {
                Assert.Fail();
                return;
            }

            ConfigManage.GetConfigFileFromDbData(dbconfigs[0].config_info, desPath);

            GridLayoutInfo info = ConfigManage.GetGridConfigEntityFromZipFile(desPath);

            Assert.IsTrue(info!=null);

            GridConfigModifyTask task = ConfigManage.GetTask(taskPath);
            task.TaskParam.GridConfigInfo = info;
            task.TaskHandle();

            FileConvertor.ObjectSerializeXmlFile<GridLayoutInfo>(info, xmlPath);
        }
    }
}
