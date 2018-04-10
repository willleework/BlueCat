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
    public class ConfigManageEventArgs
    {
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        public ConfigManageEventArgs(string message)
        {
            Message = message;
        }
    }
    public class ConfigManage
    {
        public static EventHandler<ConfigManageEventArgs> MesageEvent;
        #region 变量
        /// <summary>
        /// 数据库
        /// </summary>
        private static MySQL1130C _mySQL = new MySQL1130C();

        /// <summary>
        /// 临时工作目录
        /// </summary>
        private static string _tempWorkPath = "F:\\ConfigTest";

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
            return _mySQL.yh_tclientconfig.Where(p => p.operator_no == operator_no && p.client_config_type == client_config_type && p.sys_version_no == sys_version_no ).ToList();
        }

        /// <summary>
        /// 保存修改到数据库
        /// </summary>
        public static void SaveConfigInfoChanges2DB(List<yh_tclientconfig> datas)
        {
            foreach (yh_tclientconfig config in datas)
            {
                _mySQL.yh_tclientconfig.Attach(config);
                _mySQL.Entry<yh_tclientconfig>(config).State = System.Data.Entity.EntityState.Modified;
            }
            _mySQL.SaveChanges();
        }

        /// <summary>
        /// 保存修改到数据库
        /// </summary>
        public static void AddConfigInfo2DB(List<yh_tclientconfig> datas)
        {
            foreach (yh_tclientconfig config in datas)
            {
                _mySQL.yh_tclientconfig.Add(config);

            }
            _mySQL.SaveChanges();
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
        public static GridLayoutInfo GetGridConfigEntityFromZipFile(string zipFilePath, string decompressPath)
        {
            //string decompressFile = Path.Combine(_tempWorkPath, "Decompress");
            FileConvertor.SevenZipDecompress(zipFilePath, decompressPath);
            decompressPath = Path.Combine(decompressPath, "GridLayoutInfo.xml");
            GridLayoutInfo config = FileConvertor.XmlDeserializeObjectFromFile<GridLayoutInfo>(decompressPath);
            return config;
        }

        /// <summary>
        /// 将配置保存到实体类
        /// </summary>
        private static void SaveConfigInfoToModels(List<yh_tclientconfig> configs, string localZip)
        {
            string cmpFileName;
            for (int i = 0; i < configs.Count; i++)
            {
                cmpFileName = localZip + "." + (i + 1).ToString().PadLeft(3, '0');
                if (!File.Exists(cmpFileName))
                {
                    continue;
                }
                //将压缩文件生成字符串流
                byte[] dbBytes = FileConvertor.File2Bytes(cmpFileName);

                //保存到配置类中
                string dbStr = Convert.ToBase64String(dbBytes);
                configs[i].config_info = dbStr;
                configs[i].sys_version_no = "OPLUS_20171130G";
                configs[i].config_version = 1;
                configs[i].update_date = int.Parse(DateTime.Now.Date.ToString("yyyymmdd"));
                configs[i].update_time = int.Parse(DateTime.Now.ToString("HHMMss"));
                configs[i].serial_no = 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conConfig">数据库连接配置信息</param>
        public static void ModifyServerConfig(string conConfig, int operate_no, string client_config_type, string sys_version_no, string taskConfig)
        {
            try
            {
                string workPath = Path.Combine(Environment.CurrentDirectory, "ConfigManageTempWork");
                string zipPath = Path.Combine(workPath, "Compress");
                string serverZip = Path.Combine(zipPath, "serverData.7z.001");
                string localZip = Path.Combine(zipPath, "localData.7z");
                string deZipPath = Path.Combine(workPath, "Decompress");
                //string taskConfig = Path.Combine(workPath, "TaskConfig", "ConfigTasks.json");

                MesageEvent?.Invoke(null, new ConfigManageEventArgs("配置工作路径成功"));

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

                MesageEvent?.Invoke(null, new ConfigManageEventArgs("创建工作路径成功"));

                _mySQL.Database.Connection.ConnectionString = conConfig;

                //从数据库获取配置信息
                List<yh_tclientconfig> configs = GetServerConfigInfo(operate_no, client_config_type, sys_version_no);
                if (configs.Count <= 0)
                {
                    return;
                }

                MesageEvent?.Invoke(null, new ConfigManageEventArgs("从数据库获取到配置数据"));

                //筛选出最大版本号（config_version）的配置信息并按照文件序号排序（serial_no压缩包顺序）
                int config_version = configs.Max(p => p.config_version);
                configs = configs.Where(p => p.config_version == config_version).ToList().OrderBy(p => p.serial_no).ToList();

                //解析数据库信息生成压缩文件
                string cmpFileName;
                for (int i = 0; i < configs.Count; i++)
                {
                    cmpFileName = "serverData.7z." + (i + 1).ToString().PadLeft(3, '0');
                    GetConfigFileFromDbData(configs[i].config_info, Path.Combine(zipPath, cmpFileName));
                }
                MesageEvent?.Invoke(null, new ConfigManageEventArgs("解析数据库信息生成压缩文件"));

                //解压文件并获取表格配置信息
                GridLayoutInfo config = GetGridConfigEntityFromZipFile(serverZip, deZipPath);
                MesageEvent?.Invoke(null, new ConfigManageEventArgs("解压文件并获取表格配置信息"));

                //从json配置中生成修改任务
                List<GridConfigModifyTask> tasks = GetTask(taskConfig, config);
                //task.TaskParam.GridConfigInfo = config;
                MesageEvent?.Invoke(null, new ConfigManageEventArgs("从json配置中生成修改任务"));

                MesageEvent?.Invoke(null, new ConfigManageEventArgs("执行修改任务"));
                //任务处理
                foreach (GridConfigModifyTask task in tasks)
                {
                    MesageEvent?.Invoke(null, new ConfigManageEventArgs(string.Format("开始执行任务：{0}， 修改视图：{1}，修改主键：{2}", task.TaskID, task.TaskParam.View, task.TaskParam.KeyField)));
                    task.TaskHandle();
                    MesageEvent?.Invoke(null, new ConfigManageEventArgs(string.Format("完成任务：{0}", task.TaskID)));
                }

                MesageEvent?.Invoke(null, new ConfigManageEventArgs("将修改保存到文件中"));
                //将修改保存到文件中
                FileConvertor.ObjectSerializeXmlFile<GridLayoutInfo>(config, Path.Combine(deZipPath, "GridLayoutInfo.xml"));

                MesageEvent?.Invoke(null, new ConfigManageEventArgs("压缩文件"));
                //压缩文件
                FileConvertor.SevenZipCompress(deZipPath, localZip, 11);

                SaveConfigInfoToModels(configs, localZip);

                MesageEvent?.Invoke(null, new ConfigManageEventArgs("保存信息到数据库"));
                //保存到数据库
                AddConfigInfo2DB(configs);
                MesageEvent?.Invoke(null, new ConfigManageEventArgs("修改成功"));
                MesageEvent?.Invoke(null, new ConfigManageEventArgs("------------------------"));
            }
            catch (Exception ex)
            {
                MesageEvent?.Invoke(null, new ConfigManageEventArgs("修改文件发生异常"));
                MesageEvent?.Invoke(null, new ConfigManageEventArgs(ex.Message+";"+ex.Source));
            }
        }

    }
}
