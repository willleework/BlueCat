using System;
using System.Collections.Generic;
using System.Data;
using BlueCat.ConfigTools;
using BlueCat.Tools.FileTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlueCat.Tests
{
    [TestClass]
    public class UnitTest_FileConvertor
    {
        [TestMethod]
        public void TestMethod_ExcelRead()
        {
            string excelPath = "F:\\ConfigTest\\task.xlsx";
            string tableName = "Tasks";
            //DataTable table = FileConvertor.InputFromExcel(excelPath, tableName);
            List<GridConfigModifyTaskParam> param = ExcelHelper.ImputFromExcel<GridConfigModifyTaskParam>(excelPath);
            Assert.IsNotNull(param.Count>0);
        }

        [TestMethod]
        public void TestMethod_TaskForExcel()
        {
            //string excelPath = "F:\\ConfigTest\\task.xlsx";
            //GridLayoutConvertor cvt = new GridLayoutConvertor(excelPath);
            //List<GridConfigModifyTaskParam> tasks =  cvt.GetTaskParamFromExcel(excelPath);
            //Assert.IsNotNull(tasks.Count==2);
        }

    }
}
