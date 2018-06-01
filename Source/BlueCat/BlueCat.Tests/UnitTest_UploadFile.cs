using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlueCat.DAL.MySQL1130C;
using BlueCat.Tools.FileTools;
using System.IO;
using System.Linq;

namespace BlueCat.Tests
{
    /// <summary>
    /// UnitTest_UploadFile 的摘要说明
    /// </summary>
    [TestClass]
    public class UnitTest_UploadFile
    {
        string workPath = "F:\\ConfigTest";
            MySQL1130C dbtrade = new MySQL1130C();
        public UnitTest_UploadFile()
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

        /// <summary>
        /// 文件上传测试
        /// </summary>
        [TestMethod]
        public void TestMethod_Upload7zFile()
        {
            int companyid = 300001;
            int operateno = 10005;
            string sysversion = "OPLUS_20171130Y";
            int configversion = 1;
            int serialno = 1;
            string zipFilePath = Path.Combine(workPath, "Config.7z");
            byte[] datas = FileConvertor.File2Bytes(zipFilePath);
            string dataStream = Convert.ToBase64String(datas);
            yh_tclientconfig config = dbtrade.yh_tclientconfig.Where(p => p.client_config_type == "0" && p.company_id == companyid && p.config_version == configversion && p.operator_no == operateno && p.sys_version_no == sysversion&&p.serial_no == serialno).First();

            config.config_info = dataStream;
            dbtrade.yh_tclientconfig.Attach(config);
            dbtrade.Entry<yh_tclientconfig>(config).State = System.Data.Entity.EntityState.Modified;
            dbtrade.SaveChanges();
        }

        /// <summary>
        /// 文件下载测试
        /// </summary>
        [TestMethod]
        public void TestMethod_Download7zFile()
        {
            string sysversion = "OPLUS_20170224I";
            string zipPath = Path.Combine(workPath, "Compress", DateTime.Now.ToString(sysversion + "-yyyyMMdd-HHmmssfff") + ".7z.");
            int companyid = 302000;
            int operateno = 1214;
            int configversion = 2;
            int serialno = 1;
            //yh_tclientconfig config = dbtrade.yh_tclientconfig.Where(p => p.client_config_type == "0" && p.company_id == companyid && p.config_version == configversion && p.operator_no == operateno && p.sys_version_no == sysversion && p.serial_no == serialno).First();

            List<yh_tclientconfig> configs = dbtrade.yh_tclientconfig.Where(p => p.client_config_type == "1" && p.company_id == companyid && p.config_version == configversion && p.operator_no == operateno && p.sys_version_no == sysversion ).ToList();
            foreach (yh_tclientconfig config in configs)
            {
                byte[] datas = Convert.FromBase64String(config.config_info);
                string ext = string.Format("{0:000}", config.serial_no);
                FileConvertor.Bytes2File(datas, zipPath+ext);
            }
        }
    }
}
