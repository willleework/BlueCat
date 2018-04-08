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
        /// 从压缩文件中解压并反序列化配置类
        /// </summary>
        /// <param name="zipFilePath"></param>
        /// <returns></returns>
        public static GridLayoutInfo GetGridConfigEntityFromZipFile(string zipFilePath)
        {
            string decompressFile = Path.Combine(_tempWorkPath, "Decompress");
            FileConvertor.SevenZipDecompress(zipFilePath, decompressFile);
             decompressFile = Path.Combine(decompressFile, "GridLayoutInfo.xml");
            GridLayoutInfo config = FileConvertor.XmlDeserializeObjectFromFile<GridLayoutInfo>(decompressFile);
            return config;
        }
    }
}
