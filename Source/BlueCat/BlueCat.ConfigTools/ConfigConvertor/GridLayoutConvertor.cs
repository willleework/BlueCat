using BlueCat.DAL.MySQL1130C;
using BlueCat.Tools.FileTools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCat.ConfigTools
{
    /// <summary>
    /// 表格配置文件转换
    /// </summary>
    public class GridLayoutConvertor
    {
        private TaskConfigFileType _taskConfigSourceType = TaskConfigFileType.EXCEL;
        private string _configPath;
        /// <summary>
        /// 表格配置文件转换
        /// </summary>
        /// <param name="taskConfig"></param>
        public GridLayoutConvertor(string taskConfig)
        {
            _configPath = taskConfig;
        }

        #region 表格配置文件操作函数
        /// <summary>
        /// 表格配置文件修改
        /// </summary>
        public void GridLayoutConfigModify(string originPath, string desPath)
        {
            if (!File.Exists(originPath))
            {
                throw new Exception(string.Format("修改表格配置文件出错，行情文件不存在【{0}】", originPath));
            }
            GridLayoutInfo config = GetGridConfigEntityFromFile(originPath);
            if (config == null)
            {
                throw new Exception("从配置文件生成表格配置信息失败");
            }
            List<GridConfigModifyTask> tasks = new List<GridConfigModifyTask>();
            if (_taskConfigSourceType == TaskConfigFileType.JSON)
            {
                //从json配置中生成修改任务
                if (!File.Exists(_configPath))
                {
                    throw new Exception(string.Format("表格配置文件转换器找不到任务信息，【{0}】", _configPath));
                }
                tasks = GetTask(_configPath, config);
            }
            else if (_taskConfigSourceType == TaskConfigFileType.EXCEL)
            {
                //从excel配置中生成修改任务
                if (!File.Exists(_configPath))
                {
                    throw new Exception(string.Format("表格配置文件转换器找不到任务信息，【{0}】", _configPath));
                }
                tasks = GetTask(_configPath, config);
            }

            //任务分级别执行
            List<GridConfigModifyTask> tasksFirst = tasks.FindAll(p => p.TaskPriority == TaskManage.Priority.FirstLevel);
            List<GridConfigModifyTask> tasksSecond = tasks.FindAll(p => p.TaskPriority == TaskManage.Priority.SecondLevel);
            List<GridConfigModifyTask> tasksThird = tasks.FindAll(p => p.TaskPriority == TaskManage.Priority.ThirdLevel);

            //本地套利单独处理
            LctConfigHandle(config);

            //委托面板单独处理
            EntConfigHandle(config);

            //任务处理
            foreach (GridConfigModifyTask task in tasksFirst)
            {
                task.TaskHandle();
            }
            foreach (GridConfigModifyTask task in tasksSecond)
            {
                task.TaskHandle();
            }
            foreach (GridConfigModifyTask task in tasksThird)
            {
                task.TaskHandle();
            }

            //将修改保存到文件中
            FileConvertor.ObjectSerializeXmlFile<GridLayoutInfo>(config, desPath);
        }

        /// <summary>
        /// 单独处理本地套利面板配置
        /// 本地套利面板0224版本与埋单面板ViewName相同，实体类不同
        /// 故此处在1130版本单独添加本地套利分支
        /// </summary>
        /// <param name="config"></param>
        private void LctConfigHandle(GridLayoutInfo config)
        {
            //期货单产品本地套利面板单独处理
            GridLayoutViewInfo view = config.Views.Find(p => p.Name.Equals("O.Menu.Trade.Future_Single_AdvPanel"));
            if (view != null)
            {
                GridLayoutTableInfo table = view.Tables.Find(p => p.Name.Equals("O.BizControl.Future.GridLocalAdvEntrustModel"));
                if (table != null)
                {
                    GridLayoutViewInfo lctView = config.Views.Find(p => p.Name.Equals("FutureSingleLocalAdv"));
                    if (lctView == null)
                    {
                        lctView = new GridLayoutViewInfo()
                        {
                            Name = "FutureSingleLocalAdv"
                        };
                    }
                    GridLayoutTableInfo lctTable = lctView.Tables == null ? null : lctView.Tables.Find(p => p.Name.Equals("O.BizControl.Derivative.TableRowLocalAdvEntrustModel"));
                    if (lctTable == null)
                    {
                        lctTable = new GridLayoutTableInfo()
                        {
                            Name = "O.BizControl.Derivative.TableRowLocalAdvEntrustModel",
                            Columns = table.Columns
                        };
                    }
                    else
                    {
                        lctTable.Columns = table.Columns;
                    }
                    lctView.Tables = new List<GridLayoutTableInfo>();
                    lctView.Tables.Add(lctTable);
                    config.Views.Add(lctView);
                    view.Tables.Remove(table);
                }
            }
        }

        /// <summary>
        /// 委托面板单独处理
        /// 委托面板1130版本增加了保证金占用字段，会导致列顺序错乱，故默认隐藏掉
        /// </summary>
        /// <param name="config"></param>
        private void EntConfigHandle(GridLayoutInfo config)
        {
            GridLayoutColumnInfo column = new GridLayoutColumnInfo()
            {
                FieldName = "PendingMargin",
                Fixed = FixedStyle.None,
                Visible = false,
                Index = 100,
                SortIndex = 100,
                SortOrder = ColumnSortOrder.None,
                Width = 75
            };
            //期货自由交易单产品委托面板
            GridLayoutViewInfo view1 = config.Views.Find(p => p.Name.Equals("O.Menu.Trade.Future_Single_EntrustPanel"));
            if (view1 != null)
            {
                GridLayoutTableInfo table = view1.Tables.Find(p => p.Name.Equals("O.Menu.Trade.Future.TableRowEntrustModel"));
                if (table != null)
                {
                    if (table.Columns != null)
                    {
                        table.Columns.Add(column);
                    }
                }
            }

            //期货自由交易多产品委托面板
            GridLayoutViewInfo view2 = config.Views.Find(p => p.Name.Equals("FutureMultiTrade_FutureEntrustPanel"));
            if (view2 != null)
            {
                GridLayoutTableInfo table1 = view1.Tables.Find(p => p.Name.Equals("O.Menu.Trade.Future.TableRowEntrustModel"));
                if (table1 != null)
                {
                    if (table1.Columns != null)
                    {
                        table1.Columns.Add(column);
                    }
                }
                GridLayoutTableInfo table2 = view2.Tables.Find(p => p.Name.Equals("O.Menu.Trade.Future.TableRowEntrustByBatchModel"));
                if (table2 != null)
                {
                    if (table2.Columns != null)
                    {
                        table2.Columns.Add(column);
                    }
                }
            }

            //综合屏综合交易模块委托面板
            GridLayoutViewInfo view3 = config.Views.Find(p => p.Name.Equals("ScreenTradeModel_Entrust"));
            if (view3 != null)
            {
                GridLayoutTableInfo table = view3.Tables.Find(p => p.Name.Equals("O.BizControl.Future.GridEntrustViewModel"));
                if (table != null)
                {
                    if (table.Columns != null)
                    {
                        table.Columns.Add(column);
                    }
                }
            }

            //综合屏委托面板
            GridLayoutViewInfo view4 = config.Views.Find(p => p.Name.Equals("O.BizControl.FutureTrade_Single_EntrustPanel"));
            if (view4 != null)
            {
                GridLayoutTableInfo table = view4.Tables.Find(p => p.Name.Equals("O.BizControl.Future.GridEntrustViewModel"));
                if (table != null)
                {
                    if (table.Columns != null)
                    {
                        table.Columns.Add(column);
                    }
                }
            }
        }

        /// <summary>
        /// 获取表格配置文件
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public GridLayoutInfo GetGidLayoutConfig(string xmlPath)
        {
            GridLayoutInfo frameLayoutInfo;
            if (File.Exists(xmlPath))
            {
                string xml = File.ReadAllText(xmlPath, Encoding.UTF8);
                frameLayoutInfo = FileConvertor.XmlDeserializeObjectFromFile<GridLayoutInfo>(xml);
            }
            else
            {
                frameLayoutInfo = new GridLayoutInfo();
            }
            return frameLayoutInfo;
        }

        /// <summary>
        /// 从压缩文件中解压并反序列化配置类
        /// </summary>
        /// <param name="zipFilePath"></param>
        /// <returns></returns>
        public GridLayoutInfo GetGridConfigEntityFromFile(string decompressPath)
        {
            GridLayoutInfo config = FileConvertor.XmlDeserializeObjectFromFile<GridLayoutInfo>(decompressPath);
            return config;
        }

        /// <summary>
        /// 将配置保存到实体类
        /// </summary>
        private void SaveConfigInfoToModels(List<yh_tclientconfig> configs, string localZip, string sys_version_no_new)
        {
            string cmpFileName;
            int serial_no = 1;
            for (int i = 0; i < configs.Count; i++)
            {
                cmpFileName = localZip + "." + serial_no.ToString().PadLeft(3, '0');
                if (!File.Exists(cmpFileName))
                {
                    configs.RemoveAt(i);
                    continue;
                }
                //将压缩文件生成字符串流
                byte[] dbBytes = FileConvertor.File2Bytes(cmpFileName);

                //保存到配置类中
                string dbStr = Convert.ToBase64String(dbBytes);
                configs[i].config_info = dbStr;
                configs[i].sys_version_no = sys_version_no_new;
                configs[i].config_version = 1;
                configs[i].update_date = int.Parse(DateTime.Now.Date.ToString("yyyymmdd"));
                configs[i].update_time = int.Parse(DateTime.Now.ToString("HHMMss"));
                configs[i].serial_no = serial_no++;
            }
        }
        #endregion

        #region 任务生成函数
        /// <summary>
        /// 从Json文件获取任务配置参数
        /// </summary>
        /// <param name="jsonPath"></param>
        /// <returns></returns>
        private List<GridConfigModifyTaskParam> GetTaskParamFromJSON(string jsonPath)
        {
            List<GridConfigModifyTaskParam> cfm;
            using (StreamReader sr = new StreamReader(jsonPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Ignore;

                //构建Json.net的读取流  
                JsonReader reader = new JsonTextReader(sr);

                //对读取出的Json.net的reader流进行反序列化，并装载到模型中  
                cfm = serializer.Deserialize<List<GridConfigModifyTaskParam>>(reader);
            }
            return cfm;
        }

        /// <summary>
        /// 从Excel文件获取任务配置参数
        /// </summary>
        /// <param name="jsonPath"></param>
        /// <returns></returns>
        private List<GridConfigModifyTaskParam> GetTaskParamFromExcel(string excelPath)
        {
            List<GridConfigModifyTaskParam> taskParams = new List<GridConfigModifyTaskParam>();
            List<GridConfigModifyTaskParamForExcel> taskParamsForExcel = new List<GridConfigModifyTaskParamForExcel>();

            taskParamsForExcel = ExcelHelper.ImputFromExcel<GridConfigModifyTaskParamForExcel>(excelPath);
            Dictionary<String, GridConfigModifyTaskParam> taskParamDic = new Dictionary<string, GridConfigModifyTaskParam>();
            foreach (var param in taskParamsForExcel)
            {
                string key = string.Format("{0}-{1}-{2}-{3}-{4}", string.IsNullOrEmpty(param.View) ? "@View@" : param.View, string.IsNullOrEmpty(param.Table) ? "@Table@" : param.Table, string.IsNullOrEmpty(param.Column) ? "@Column@" : param.Column, param.KeyField, param.OperateType);
                if (!taskParamDic.ContainsKey(key))
                {

                    GridConfigModifyTaskParam taskP = new GridConfigModifyTaskParam()
                    {
                        View = param.View,
                        Table = param.Table,
                        Column = param.Column,
                        KeyField = param.KeyField,
                        OperateType = param.OperateType,
                        Operates = new List<Operate>()
                    };
                    taskP.Operates.Add(new Operate() { OperateField = param.OperateField, OperateFieldValue = param.OperateFieldValue });
                    taskParamDic.Add(key, taskP);
                }
                else
                {
                    taskParamDic[key].Operates.Add(new Operate() { OperateField = param.OperateField, OperateFieldValue = param.OperateFieldValue });
                }
            }
            return taskParamDic.Values.ToList();
        }

        /// <summary>
        /// 根据json配置文件获取任务
        /// </summary>
        /// <param name="taskConfigPath"></param>
        /// <returns></returns>
        public List<GridConfigModifyTask> GetTask(string taskConfigPath, GridLayoutInfo config)
        {
            List<GridConfigModifyTask> tasks = new List<GridConfigModifyTask>();
            List<GridConfigModifyTaskParam> taskParams = new List<GridConfigModifyTaskParam>();
            if (_taskConfigSourceType == TaskConfigFileType.JSON)
            {
                taskParams = GetTaskParamFromJSON(taskConfigPath);
            }
            else if(_taskConfigSourceType == TaskConfigFileType.EXCEL)
            {
                taskParams = GetTaskParamFromExcel(taskConfigPath);
            }
            foreach (GridConfigModifyTaskParam param in taskParams)
            {
                GridConfigModifyTask task = new GridConfigModifyTask();
                task.TaskID = Guid.NewGuid();
                task.TaskDescription = "表格配置文件修改";
                if (param.KeyField == "Column")
                {
                    task.TaskPriority = TaskManage.Priority.FirstLevel;
                }
                else if (param.KeyField == "Table")
                {
                    task.TaskPriority = TaskManage.Priority.SecondLevel;
                }
                else
                {
                    task.TaskPriority = TaskManage.Priority.ThirdLevel;
                }
                task.TaskParam = param;
                task.TaskParam.GridConfigInfo = config;
                tasks.Add(task);
            }
            return tasks;
        }
        #endregion
    }
}
