using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlueCat.Tools.FileTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueCat.ConfigTools;
using System.IO;

namespace BlueCat.Tools.FileTools.Tests
{
    [TestClass()]
    public class UnitTest_FileConvertor
    {
        [TestMethod()]
        public void ImputFromExcelTest()
        {
            List<CustomLayoutParam> param = ExcelHelper.ImputFromExcel<CustomLayoutParam>("D:\\customTest\\task.xlsx", 0, 0, "CustomLayout");
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
    }
}