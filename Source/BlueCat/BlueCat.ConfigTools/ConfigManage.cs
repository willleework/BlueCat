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
    public class ConfigManage
    {
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
        private static GridConfigModifyTaskParam GetTaskParam(string jsonPath)
        {
            GridConfigModifyTaskParam cfm;
            //jsonPath = Path.Combine(Environment.CurrentDirectory, "TaskInfo", "ConfigTasks.json");
            using (StreamReader sr = new StreamReader(jsonPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Ignore;

                //构建Json.net的读取流  
                JsonReader reader = new JsonTextReader(sr);

                //对读取出的Json.net的reader流进行反序列化，并装载到模型中  
                cfm = serializer.Deserialize<GridConfigModifyTaskParam>(reader);
            }
            return cfm;
        }

        /// <summary>
        /// 根据json配置文件获取任务
        /// </summary>
        /// <param name="taskConfigPath"></param>
        /// <returns></returns>
        public static GridConfigModifyTask GetTask(string taskConfigPath)
        {
            GridConfigModifyTask task = new GridConfigModifyTask();
            task.TaskID = Guid.NewGuid();
            task.TaskDescription = "表格配置文件修改";
            task.TaskPriority = TaskManage.Priority.FirstLevel;
            task.TaskParam = GetTaskParam(taskConfigPath);
            return task;
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
            return _mySQL.yh_tclientconfig.Where(p => p.operator_no == operator_no && p.client_config_type == client_config_type && p.sys_version_no == sys_version_no).ToList();
        }

        /// <summary>
        /// 保存修改到数据库
        /// </summary>
        public static void SaveConfigInfoChanges2DB(yh_tclientconfig data)
        {
            _mySQL.yh_tclientconfig.Attach(data);
            _mySQL.Entry(data).State = System.Data.Entity.EntityState.Modified;
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
        /// 
        /// </summary>
        /// <param name="conConfig">数据库连接配置信息</param>
        public static void ModifyServerConfig(string conConfig)
        {
            string workPath = Path.Combine(Environment.CurrentDirectory, "ConfigManageTempWork");
            string zipPath = Path.Combine(workPath, "Compress");
            string serverZip = Path.Combine(zipPath, "serverData.7z");
            string localZip = Path.Combine(zipPath, "localData.7z");
            string deZipPath = Path.Combine(workPath, "Decompress");
            string taskConfig = Path.Combine(workPath, "TaskConfig", "ConfigTasks.json");

            _mySQL.Database.Connection.ConnectionString = conConfig;

            //从数据库获取配置信息
            List<yh_tclientconfig> configs = GetServerConfigInfo(10005, "0", "OPLUS_20171130B");

            //解析数据库信息生成压缩文件
            GetConfigFileFromDbData(configs[0].config_info, Path.Combine(zipPath, "serverData.7z"));

            //解压文件并获取表格配置信息
            GridLayoutInfo config = GetGridConfigEntityFromZipFile(serverZip, deZipPath);

            //从json配置中生成修改任务
            GridConfigModifyTask task = GetTask(taskConfig);
            task.TaskParam.GridConfigInfo = config;

            //执行修改任务
            task.TaskHandle();

            //将修改保存到文件中
            FileConvertor.ObjectSerializeXmlFile<GridLayoutInfo>(config, Path.Combine(deZipPath, "GridLayoutInfo.xml"));

            //压缩文件
            FileConvertor.SevenZipCompress(deZipPath, localZip);

            //将压缩文件生成字符串流
            localZip = localZip + ".001";
            byte[] dbBytes = FileConvertor.File2Bytes(localZip);

            //保存到配置类中
            string dbStr = Convert.ToBase64String(dbBytes);
            configs[0].config_info = dbStr;

            //保存到数据库
            SaveConfigInfoChanges2DB(configs[0]);
        }
    }
}
