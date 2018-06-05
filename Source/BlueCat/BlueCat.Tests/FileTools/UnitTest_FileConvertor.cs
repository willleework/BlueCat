using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlueCat.Tools.FileTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueCat.ConfigTools;
using System.IO;
using BlueCat.Cache;

namespace BlueCat.Tools.FileTools.Tests
{
    [TestClass()]
    public class UnitTest_FileConvertor
    {
        [TestMethod()]
        public void ImputFromExcelTest()
        {
            List<FieldConvertParam> param = ExcelHelper.ImputFromExcel<FieldConvertParam>("D:\\customTest\\task.xlsx", 0, 0, "CustomLayout");
            Assert.IsTrue(param != null && param.Count > 0);
        }

        [TestMethod]
        public void CustomlayoutModifyTest()
        {
            CustomLayoutConvertor convertor = new CustomLayoutConvertor("D:\\customTest\\task.xlsx");
            string newFile = string.Format("D:\\customTest\\{0}.xml", DateTime.Now.ToString("yyyyMMdd-HHmmss-fff"));
            convertor.CustomLayoutModify("D:\\customTest\\CustomLayoutConfig.xml", newFile);
            Assert.IsTrue(File.Exists(newFile));
        }

        [TestMethod]
        public void CustomlayoutModify1_Test()
        {
            string configStr = FileConvertor.ReadFile("D:\\customTest\\CustomLayoutConfig.xml");
            int beginIndex = configStr.IndexOf("<MenuName>FutureMultiTrade</MenuName>");
            if (beginIndex >= 0)
            {
                int endIndex = configStr.IndexOf("</SaveParamItem>", beginIndex);

                string multstr = configStr.Substring(beginIndex, endIndex);

                string newStr = multstr.Replace("tabQurey", "oTabControl1");

                configStr = configStr.Replace(multstr, newStr);
            }
        }

        [TestMethod]
        public void UserScreenModifyTest()
        {
            string excelPath = "F:\\ConfigTest\\task.xlsx";
            string configFile = "F:\\ConfigTest\\UserScreenInfo.xml";
            string configFile1 = "F:\\ConfigTest\\newUserScreenInfo.xml";
            UserScreenConvertor screen = new UserScreenConvertor(excelPath);
            screen.UserScreenModify(configFile, configFile1);
        }

        [TestMethod]
        public void FutureLocalParamModify_Test()
        {
            string excelPath = "F:\\ConfigTest\\task.xlsx";
            string configFile = "F:\\ConfigTest\\FutureLocalParam.xml";
            string newFile = string.Format("F:\\ConfigTest\\FutureLocalParam{0}.xml", DateTime.Now.ToString("yyyyMMdd-HHmmss-fff"));
            string conn = "server=192.168.117.101;user id=root;password=ggcjdss2017;persistsecurityinfo=True;database=hdback";
            CachePool pool = new CachePool();
            pool.Init(conn);
            FutureLocalParamConvertor local = new FutureLocalParamConvertor(excelPath, pool);
            local.FutureLocalParamModify(configFile, configFile);
        }
    }
}