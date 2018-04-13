using BlueCat.Cache;
using BlueCat.DAL.MySQL1130C;
using BlueCat.Tools.FileTools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCat.ConfigTools
{
    public class ConfigManage
    {
        #region 变量
        /// <summary>
        /// 数据库
        /// </summary>
        private static string _dbConnect = string.Empty;

        /// <summary>
        /// 消息事件
        /// </summary>
        public static EventHandler<ConfigManageEventArgs> MesageEvent;

        /// <summary>
        /// 表格配置文件
        /// </summary>
        private static string gridLayoutConfig = "GridLayoutInfo.xml";

        /// <summary>
        /// 行情配置文件
        /// </summary>
        private static string quotationGroup = "QuotationGroup.xml";
        #endregion

        /// <summary>
        /// 获取表格配置文件
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public static GridLayoutInfo GetGidLayoutConfig(string xmlPath)
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
        /// 从Json文件获取任务配置参数
        /// </summary>
        /// <param name="jsonPath"></param>
        /// <returns></returns>
        private static List<GridConfigModifyTaskParam> GetTaskParam(string jsonPath)
        {
            List<GridConfigModifyTaskParam> cfm;
            //jsonPath = Path.Combine(Environment.CurrentDirectory, "TaskInfo", "ConfigTasks.json");
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
        /// 根据json配置文件获取任务
        /// </summary>
        /// <param name="taskConfigPath"></param>
        /// <returns></returns>
        public static List<GridConfigModifyTask> GetTask(string taskConfigPath, GridLayoutInfo config)
        {
            List<GridConfigModifyTask> tasks = new List<GridConfigModifyTask>();
            List<GridConfigModifyTaskParam> taskParams = GetTaskParam(taskConfigPath);
            foreach (GridConfigModifyTaskParam param in taskParams)
            {
                GridConfigModifyTask task = new GridConfigModifyTask();
                task.TaskID = Guid.NewGuid();
                task.TaskDescription = "表格配置文件修改";
                task.TaskPriority = TaskManage.Priority.FirstLevel;
                task.TaskParam = param;
                task.TaskParam.GridConfigInfo = config;
                tasks.Add(task);
            }
            return tasks;
        }

        /// <summary>
        /// 获取服务端配置数据
        /// </summary>
        /// <param name="operator_no"></param>
        /// <param name="client_config_type"></param>
        /// <param name="sys_version_no"></param>
        /// <returns></returns>
        public static List<yh_tclientconfig> GetServerConfigInfo(int operator_no, string client_config_type, string sys_version_no)
        {
            using (MySQL1130C _mySQL = new MySQL1130C())
            {
                _mySQL.Database.Connection.ConnectionString = _dbConnect;
                return _mySQL.yh_tclientconfig.Where(p => p.operator_no == operator_no && p.client_config_type == client_config_type && p.sys_version_no == sys_version_no).ToList();
            }
        }

        /// <summary>
        /// 保存修改到数据库
        /// </summary>
        public static void SaveConfigInfoChanges2DB(List<yh_tclientconfig> datas)
        {
            using (MySQL1130C _mySQL = new MySQL1130C())
            {
                _mySQL.Database.Connection.ConnectionString = _dbConnect;
                foreach (yh_tclientconfig config in datas)
                {
                    _mySQL.yh_tclientconfig.Attach(config);
                    _mySQL.Entry<yh_tclientconfig>(config).State = System.Data.Entity.EntityState.Modified;
                }

                _mySQL.SaveChanges();
            }
        }

        /// <summary>
        /// 保存修改到数据库
        /// </summary>
        public static void AddConfigInfo2DB(List<yh_tclientconfig> datas)
        {
            using (MySQL1130C _mySQL = new MySQL1130C())
            {
                _mySQL.Database.Connection.ConnectionString = _dbConnect;
                foreach (yh_tclientconfig config in datas)
                {
                    _mySQL.yh_tclientconfig.Add(config);
                }
                _mySQL.SaveChanges();
            }
        }

        /// <summary>
        /// 从数据字符串获取文件
        /// </summary>
        /// <param name="dbData"></param>
        public static void GetConfigFileFromDbData(string dbData, string filseSavePath)
        {
            //string desPath = Path.Combine(_tempWorkPath, "Compress.7z");
            byte[] dbBytes = Convert.FromBase64String(dbData);
            FileConvertor.Bytes2File(dbBytes, filseSavePath);
        }

        /// <summary>
        /// 从配置文件获取字符串
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetDbDataFromConfigFile(string filePath)
        {
            byte[] dbBytes = FileConvertor.File2Bytes(filePath);
            return Convert.ToBase64String(dbBytes);
        }

        /// <summary>
        /// 从压缩文件中解压并反序列化配置类
        /// </summary>
        /// <param name="zipFilePath"></param>
        /// <returns></returns>
        public static GridLayoutInfo GetGridConfigEntityFromFile(string decompressPath)
        {
            GridLayoutInfo config = FileConvertor.XmlDeserializeObjectFromFile<GridLayoutInfo>(decompressPath);
            return config;
        }

        /// <summary>
        /// 将配置保存到实体类
        /// </summary>
        private static void SaveConfigInfoToModels(List<yh_tclientconfig> configs, string localZip, string sys_version_no_new)
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

        /// <summary>
        /// 潜复制
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static yh_tclientconfig ShallowCoty(yh_tclientconfig config)
        {
            return new yh_tclientconfig()
            {
                company_id = config.company_id,
                config_version = config.config_version,
                client_config_type = config.client_config_type,
                config_info = config.config_info,
                operator_no = config.operator_no,
                serial_no = config.serial_no,
                sys_version_no = config.sys_version_no,
                update_date = config.update_date,
                update_time = config.update_time
            };
        }

        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string ExceptionHandle(Exception ex)
        {
            StringBuilder innerExp = new StringBuilder();
            Exception temp = ex;
            while (temp.InnerException != null)
            {
                temp = temp.InnerException;
                innerExp.Append(string.Format("->{0}", temp.Message));
            }
            return string.Format("{0}[异常信息]：{1};\r\n[错误源]:{2};\r\n[内部错误]:{3};\r\n[堆栈信息]:{4}", Environment.NewLine, ex.Message, ex.Source, innerExp.ToString(), ex.StackTrace);
        }

        /// <summary>
        /// 输出错误结束符
        /// </summary>
        /// <param name="info"></param>
        /// <param name="process"></param>
        private static void PrintErrEndLine(string info, int process)
        {
            MesageEvent?.Invoke(null, new ConfigManageEventArgs(info, process));
            MesageEvent?.Invoke(null, new ConfigManageEventArgs("------------------------------------------------"));
            MesageEvent?.Invoke(null, new ConfigManageEventArgs(""));
        }


        /// <summary>
        /// 配置文件更新
        /// </summary>
        /// <param name="conConfig">数据库连接配置信息</param>
        public static void ModifyServerConfig(string conConfig, int operate_no, string client_config_type, string sys_version_no, string sys_version_no_new, string taskConfig, CachePool pool)
        {
            try
            {
                MesageEvent?.Invoke(null, new ConfigManageEventArgs(string.Format("开始执行修改：操作员序号【{0}】;系统版本【{1}】；待更新系统版本【{2}】", operate_no, sys_version_no, sys_version_no_new)));
                string workPath = Path.Combine(Environment.CurrentDirectory, "ConfigManageTempWork");
                string zipPath = Path.Combine(workPath, "Compress");
                string serverZip = Path.Combine(zipPath, "serverData.7z.001");
                string localZip = Path.Combine(zipPath, "localData.7z");
                string deZipPath = Path.Combine(workPath, "Decompress");
                string[] configFile = new string[] { };
                bool needModify = false;
                //string taskConfig = Path.Combine(workPath, "TaskConfig", "ConfigTasks.json");
                _dbConnect = conConfig;

                MesageEvent?.Invoke(null, new ConfigManageEventArgs("开始配置工作路径成功", 2));

                if (Directory.Exists(zipPath))
                {
                    Directory.Delete(zipPath, true);
                }
                Directory.CreateDirectory(zipPath);

                if (Directory.Exists(deZipPath))
                {
                    Directory.Delete(deZipPath, true);
                }
                Directory.CreateDirectory(deZipPath);
                MesageEvent?.Invoke(null, new ConfigManageEventArgs("工作路径配置成功", 5));

                MesageEvent?.Invoke(null, new ConfigManageEventArgs("开始从数据库查询用户配置信息", 6));
                //从数据库获取配置信息
                List<yh_tclientconfig> configs = GetServerConfigInfo(operate_no, client_config_type, sys_version_no);
                if (configs.Count <= 0)
                {
                    PrintErrEndLine("数据库查询不到相关配置信息", 0);
                    return;
                }
                MesageEvent?.Invoke(null, new ConfigManageEventArgs("查询配置信息成功", 20));

                MesageEvent?.Invoke(null, new ConfigManageEventArgs("开始解析数据库配置信息", 21));
                //筛选出最大版本号（config_version）的配置信息并按照文件序号排序（serial_no压缩包顺序）
                int config_version = configs.Max(p => p.config_version);
                configs = configs.Where(p => p.config_version == config_version).ToList().OrderBy(p => p.serial_no).ToList();
                //后续要做新增操作，此处拷贝一份作为插入数据
                configs = configs.Select(p => ShallowCoty(p)).ToList();
                //解析数据库信息生成压缩文件
                string cmpFileName;
                for (int i = 0; i < configs.Count; i++)
                {
                    cmpFileName = "serverData.7z." + (i + 1).ToString().PadLeft(3, '0');
                    GetConfigFileFromDbData(configs[i].config_info, Path.Combine(zipPath, cmpFileName));
                }
                MesageEvent?.Invoke(null, new ConfigManageEventArgs("解析数据库信息生成压缩文件成功", 30));

                MesageEvent?.Invoke(null, new ConfigManageEventArgs("开始解压文件", 31));
                //解压文件并获取表格配置信息
                FileConvertor.SevenZipDecompress(serverZip, deZipPath);
                MesageEvent?.Invoke(null, new ConfigManageEventArgs("解压文件成功", 40));

                //生成配置文件路径
                string gridLayoutPath = Path.Combine(deZipPath, gridLayoutConfig);
                string quotationPath = Path.Combine(deZipPath, quotationGroup);
                #region 修改GridConfig
                if (File.Exists(gridLayoutPath))
                {
                    MesageEvent?.Invoke(null, new ConfigManageEventArgs("开始修改GridLayoutInfo文件", 41));
                    GridLayoutInfo config = GetGridConfigEntityFromFile(gridLayoutPath);
                    if (config == null)
                    {
                        PrintErrEndLine("从配置文件生成表格配置信息失败", 0);
                        return;
                    }

                    //从json配置中生成修改任务
                    List<GridConfigModifyTask> tasks = GetTask(taskConfig, config);
                    MesageEvent?.Invoke(null, new ConfigManageEventArgs("从json配置中生成修改任务", 45));

                    MesageEvent?.Invoke(null, new ConfigManageEventArgs("执行修改任务"));
                    //任务处理
                    foreach (GridConfigModifyTask task in tasks)
                    {
                        MesageEvent?.Invoke(null, new ConfigManageEventArgs(string.Format("开始执行任务：{0}， 修改视图：{1}，修改主键：{2}", task.TaskID, task.TaskParam.View, task.TaskParam.KeyField)));
                        task.TaskHandle();
                        MesageEvent?.Invoke(null, new ConfigManageEventArgs(string.Format("完成任务：{0}", task.TaskID)));
                    }

                    MesageEvent?.Invoke(null, new ConfigManageEventArgs("将修改保存到文件中", 50));
                    //将修改保存到文件中
                    FileConvertor.ObjectSerializeXmlFile<GridLayoutInfo>(config, gridLayoutConfig);
                    needModify = true;
                    MesageEvent?.Invoke(null, new ConfigManageEventArgs("修改GridLayoutInfo文件成功", 55));
                }
                else
                {
                    PrintErrEndLine("该用户尚未生成GridLayoutInfo配置文件", 55);
                }
                #endregion

                #region 修改行情文件
                if (File.Exists(quotationPath))
                {
                    MesageEvent?.Invoke(null, new ConfigManageEventArgs("开始修改QuotationGroup文件", 56));
                    FutureQuotationConvertor convertor = new FutureQuotationConvertor(pool);
                    convertor.FutureQuotationModify(quotationPath, quotationPath);
                    needModify = true;
                    MesageEvent?.Invoke(null, new ConfigManageEventArgs("用户QuotationGroup文件处理成功", 70));
                }
                else
                {
                    PrintErrEndLine("该用户尚未生成QuotationGroup配置文件", 70);
                }
                #endregion

                if (needModify)
                {
                    MesageEvent?.Invoke(null, new ConfigManageEventArgs("压缩文件", 71));
                    //压缩文件
                    FileConvertor.SevenZipCompress(deZipPath, localZip, 11);

                    SaveConfigInfoToModels(configs, localZip, sys_version_no_new);

                    if (configs.Count <= 0)
                    {
                        PrintErrEndLine("没有需要保存的数据", 0);
                        return;
                    }

                    MesageEvent?.Invoke(null, new ConfigManageEventArgs("开始保存信息到数据库", 75));
                    //保存到数据库
                    AddConfigInfo2DB(configs);
                    MesageEvent?.Invoke(null, new ConfigManageEventArgs("修改成功", 100));
                    MesageEvent?.Invoke(null, new ConfigManageEventArgs("------------------------------------------------"));
                    MesageEvent?.Invoke(null, new ConfigManageEventArgs(Environment.NewLine));
                }
                else
                {
                    MesageEvent?.Invoke(null, new ConfigManageEventArgs("没有需要修改的文件", 100));
                    MesageEvent?.Invoke(null, new ConfigManageEventArgs("------------------------------------------------"));
                    MesageEvent?.Invoke(null, new ConfigManageEventArgs(Environment.NewLine));
                }
            }
            catch (Exception ex)
            {
                MesageEvent?.Invoke(null, new ConfigManageEventArgs("修改文件发生异常，错误信息如下："));
                PrintErrEndLine(ExceptionHandle(ex), 0);
            }
        }

    }

    public class ConfigManageEventArgs
    {
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 进度
        /// </summary>
        [DefaultValue(-1)]
        public int ProgressIndex { get; set; }

        public ConfigManageEventArgs(string message, int progress = -1)
        {
            Message = message;
            ProgressIndex = progress;
        }
    }
}
