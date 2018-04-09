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

            //获取数据库数据
            List<yh_tclientconfig> dbconfigs = ConfigManage.GetServerConfigInfo(10005, "0", "OPLUS_20171130B");
            if (dbconfigs.Count <= 0)
            {
                Assert.Fail();
                return;
            }

            //从字符串获取压缩文件
            ConfigManage.GetConfigFileFromDbData(dbconfigs[0].config_info, desPath);

            //解压文件，并获取配置信息
            GridLayoutInfo info = ConfigManage.GetGridConfigEntityFromZipFile(desPath, Path.Combine(_tempWorkPath, "Decompress"));

            //Assert.IsTrue(info != null);

            ////修改配置
            //GridConfigModifyTask task = ConfigManage.GetTask(taskPath);
            //task.TaskParam.GridConfigInfo = info;
            //task.TaskHandle();

            ////保存修改到配置文件
            //FileConvertor.ObjectSerializeXmlFile<GridLayoutInfo>(info, xmlPath);

            ////压缩文件
            //FileConvertor.SevenZipCompress(Path.Combine(_tempWorkPath, "Decompress"), Path.Combine(_tempWorkPath, "10005.7z"));

            ////从压缩文件获取字符串
            //byte[] dbBytes = FileConvertor.File2Bytes(Path.Combine(_tempWorkPath, "10005.7z.001"));
            //string dbStr = Convert.ToBase64String(dbBytes);
            //dbconfigs[0].config_info = dbStr;

            //ConfigManage.SaveConfigInfoChanges2DB(dbconfigs[0]);

        }

        [TestMethod]
        public void TestMethod_ModifyServerConfig()
        {
            ConfigManage.ModifyServerConfig("server=10.20.31.42;user id=root;password=ziguan.123;persistsecurityinfo=True;database=dbtrade");
        }
    }
}
