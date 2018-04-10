using System;
using System.Collections.Generic;
using System.IO;
using BlueCat.ConfigTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace BlueCat.Tests
{
    [TestClass]
    public class UnitTest_GridConfigLocalReadTest
    {
        [TestMethod]
        public void Test_SerializeTest()
        {
            string xmlPath = Path.Combine(Environment.CurrentDirectory, "Unzip", "GridLayoutInfo.xml");
            GridLayoutInfo layout = ConfigManage.GetGidLayoutConfig(xmlPath);
            Assert.IsTrue(layout.Views.Exists(p => p.Name == "SpotTrade_MultiDeal"));
        }

        [TestMethod]
        public void Test_JsonSerialize()
        {
            GridConfigModifyTaskParam param1 = new GridConfigModifyTaskParam()
            {
                View = "挂单",
                Table = "O.BizControl.Derivative.TableRowEntrustModel",
                Column = "BatchSerialNo",
                KeyField = "Column",
                Operates = new List<Operate>()
                {
                    new Operate(){ OperateField = "1", OperateFieldValue = "1.2"},
                    new Operate(){ OperateField = "2", OperateFieldValue ="2.1"}
                },
                OperateType = OperateType.Modify
            };
            GridConfigModifyTaskParam param2 = new GridConfigModifyTaskParam()
            {
                View = "挂单",
                Table = "O.BizControl.Derivative.TableRowEntrustModel",
                Column = "BatchSerialNo",
                KeyField = "Column",
                Operates = new List<Operate>()
                {
                    new Operate(){ OperateField = "1", OperateFieldValue = "1.2"},
                    new Operate(){ OperateField = "2", OperateFieldValue ="2.1"}
                },
                OperateType = OperateType.Modify
            };
            List<GridConfigModifyTaskParam> param = new List<GridConfigModifyTaskParam>();
            param.Add(param1);
            param.Add(param2);
            string obj = JsonConvert.SerializeObject(param);
        }

        [TestMethod]
        public void Test_ReadLocalJsonAndDeserialize()
        {
            string xmlPath = Path.Combine(Environment.CurrentDirectory, "TaskInfo", "ConfigTasks.json");
            using (StreamReader sr = new StreamReader(xmlPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Ignore;

                //构建Json.net的读取流  
                JsonReader reader = new JsonTextReader(sr);

                //对读取出的Json.net的reader流进行反序列化，并装载到模型中  
                GridConfigModifyTaskParam cfm = serializer.Deserialize<GridConfigModifyTaskParam>(reader);
            }
        }
    }
}
