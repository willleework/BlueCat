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

        /// <summary>
        /// 自由布局文件
        /// </summary>
        private static string customLayout = "CustomLayoutConfig.xml";

        /// <summary>
        /// 综合屏配置文件
        /// </summary>
        private static string userScreen = "UserScreenInfo.xml";

        /// <summary>
        /// 期货本地参数
        /// </summary>
        private static string futurelocalParam = "FutureLocalParam.xml";
        #endregion

        #region 数据库操作
        /// <summary>
        /// 获取服务端配置数据
        /// </summary>
        /// <param name="operator_no"></param>
        /// <param name="client_config_type"></param>
        /// <param name="sys_version_no"></param>
        /// <returns></returns>
        public static List<yh_tclientconfig> GetServerConfigInfo(int operator_no, string sys_version_no)
        {
            using (MySQL1130C _mySQL = new MySQL1130C())
            {
                _mySQL.Database.Connection.ConnectionString = _dbConnect;
                return _mySQL.yh_tclientconfig.ToList().Where(p => p.operator_no == operator_no  && ReplaceErrChar(p.sys_version_no) == sys_version_no).ToList();
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
        #endregion

        #region 文件及流处理
        /// <summary>
        /// 从数据字符串获取文件
        /// </summary>
        /// <param name="dbData"></param>
        public static void GetConfigFileFromDbData(string dbData, string filseSavePath)
        {
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
        /// 替换换行符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceErrChar(string str)
        {
            return str.Replace("\n\r", "").Replace("\n", "").Replace("\r", "").Replace("\r\n", "");
        }

        /// <summary>
        /// 将配置保存到实体类
        /// </summary>
        private static void SaveConfigInfoToModels(List<yh_tclientconfig> configs, string localZip, string sys_version_no_new,int config_version, string client_config_type)
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
                configs[i].config_version = config_version;
                configs[i].update_date = int.Parse(DateTime.Now.Date.ToString("yyyyMMdd"));
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
        #endregion

        #region 外部消息事件处理
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
        #endregion

        /// <summary>
        /// 配置文件更新
        /// </summary>
        /// <param name="conConfig">数据库连接配置信息</param>
        public static void ModifyServerConfig(string conConfig, int operate_no, string sys_version_no, string sys_version_no_new, int config_version, string taskConfig, CachePool pool)
        {
            try
            {
                int process = 0;
                MesageEvent?.Invoke(null, new ConfigManageEventArgs(string.Format("开始执行修改：操作员序号【{0}】;系统版本【{1}】；待更新系统版本【{2}】", operate_no, sys_version_no, sys_version_no_new)));
                string workPath = Path.Combine(Environment.CurrentDirectory, "ConfigManageTempWork");
                string zipPath = Path.Combine(workPath, "Compress");
                string serverZip = Path.Combine(zipPath, "serverData.7z.001");
                string localZip = Path.Combine(zipPath, "localData.7z");
                string deZipPath = Path.Combine(workPath, "Decompress");
                string[] configFile = new string[] { };
                bool needModify = false;
                //生成配置文件路径
                string gridLayoutPath = Path.Combine(deZipPath, gridLayoutConfig);
                string quotationPath = Path.Combine(deZipPath, quotationGroup);
                string customLayoutPath = Path.Combine(deZipPath, customLayout);
                string userScreenPath = Path.Combine(deZipPath, userScreen);
                string futurelocalParamPath = Path.Combine(deZipPath, futurelocalParam);

                _dbConnect = conConfig;

                #region 从数据库获取配置信息
                process = 6;
                MesageEvent?.Invoke(null, new ConfigManageEventArgs("开始从数据库查询用户配置信息", process));
                //从数据库获取配置信息
                List<yh_tclientconfig> configs = GetServerConfigInfo(operate_no, sys_version_no);
                if (configs.Count <= 0)
                {
                    process = 0;
                    PrintErrEndLine("数据库查询不到相关配置信息", process);
                    return;
                }
                process = 10;
                MesageEvent?.Invoke(null, new ConfigManageEventArgs("查询配置信息成功", process));
                MesageEvent?.Invoke(null, new ConfigManageEventArgs("------------------------------------------------"));
                #endregion
                List<string> client_config_types = new List<string>() { "0", "1", "2" };
                int processPer = 90 / client_config_types.Count;
                if (processPer <= 0)
                {
                    processPer = 1;
                }
                for (int j = 0; j < client_config_types.Count; j++)
                {
                    int processBegin = process;
                    string config_type = client_config_types[j];
                    MesageEvent?.Invoke(null, new ConfigManageEventArgs(string.Format("开始修改类型【{0}】的配置数据", config_type), process));
                    List<yh_tclientconfig> config_curType = configs.Where(p => ReplaceErrChar(p.client_config_type) == config_type).ToList();
                    if (config_curType.Count <= 0)
                    {
                        PrintErrEndLine(string.Format("找不到类型【{0}】配置文件", config_type), process);
                        continue;
                    }
                    process = (int)(process + processPer * 0.1);
                    #region 创建工作目录
                    MesageEvent?.Invoke(null, new ConfigManageEventArgs("开始配置工作路径", process));

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
                    MesageEvent?.Invoke(null, new ConfigManageEventArgs("工作路径配置成功", process));
                    #endregion

                    process = (int)(process + processPer * 0.1);//10%
                    #region 解析数据库数据
                    MesageEvent?.Invoke(null, new ConfigManageEventArgs("开始解析数据库配置信息", process));
                    //筛选出最大版本号（config_version）的配置信息并按照文件序号排序（serial_no压缩包顺序）
                    int max_config_version = config_curType.Max(p => p.config_version);
                    config_curType = config_curType.Where(p => p.config_version == max_config_version).ToList().OrderBy(p => p.serial_no).ToList();
                    //后续要做新增操作，此处拷贝一份作为插入数据
                    config_curType = config_curType.Select(p => ShallowCoty(p)).ToList();
                    //解析数据库信息生成压缩文件
                    string cmpFileName;
                    for (int i = 0; i < config_curType.Count; i++)
                    {
                        cmpFileName = "serverData.7z." + (i + 1).ToString().PadLeft(3, '0');
                        GetConfigFileFromDbData(config_curType[i].config_info, Path.Combine(zipPath, cmpFileName));
                    }
                    process = (int)(process + processPer * 0.2);//30%
                    MesageEvent?.Invoke(null, new ConfigManageEventArgs("解析数据库信息生成压缩文件成功", process));

                    MesageEvent?.Invoke(null, new ConfigManageEventArgs("开始解压文件", process));
                    //解压文件并获取表格配置信息
                    FileConvertor.SevenZipDecompress(serverZip, deZipPath);
                    process = (int)(process + processPer * 0.1);//40%
                    MesageEvent?.Invoke(null, new ConfigManageEventArgs("解压文件成功", process));
                    #endregion

                    #region 修改类型0的配置文件
                    if (config_type.Equals("0"))
                    {
                        //修改GridConfig
                        if (File.Exists(gridLayoutPath))
                        {
                            MesageEvent?.Invoke(null, new ConfigManageEventArgs("开始修改GridLayoutInfo文件", process));
                            GridLayoutConvertor gridConvertor = new GridLayoutConvertor(taskConfig);
                            gridConvertor.GridLayoutConfigModify(gridLayoutPath, gridLayoutPath);
                            needModify = true;
                            process = (int)(process + processPer * 0.3);
                            MesageEvent?.Invoke(null, new ConfigManageEventArgs("修改GridLayoutInfo文件成功", process));//70%
                        }
                        else
                        {
                            process = 70;
                            PrintErrEndLine("该用户尚未生成GridLayoutInfo配置文件", process);
                        }
                    }
                    #endregion

                    #region 修改类型1的配置文件
                    if (config_type.Equals("1"))
                    {
                        //修改UserScreenInfo.xml
                        if (File.Exists(userScreenPath))
                        {
                            MesageEvent?.Invoke(null, new ConfigManageEventArgs("开始修改UserScreenInfo文件", process));
                            UserScreenConvertor userScreenConvertor = new UserScreenConvertor(taskConfig);
                            userScreenConvertor.UserScreenModify(userScreenPath, userScreenPath);
                            needModify = true;
                            process = (int)(process + processPer * 0.3);
                            MesageEvent?.Invoke(null, new ConfigManageEventArgs("修改UserScreenInfo文件成功", process));//70%
                        }
                        else
                        {
                            process = (int)(process + processPer * 0.3);
                            PrintErrEndLine("该用户尚未生成UserScreenInfo配置文件", process);
                        }
                    }
                    #endregion

                    #region 修改类型2的配置文件
                    if (config_type.Equals("2"))
                    {
                        if (File.Exists(quotationPath))
                        {
                            MesageEvent?.Invoke(null, new ConfigManageEventArgs("开始修改QuotationGroup文件", process));
                            FutureQuotationConvertor convertor = new FutureQuotationConvertor(pool);
                            convertor.FutureQuotationModify(quotationPath, quotationPath);
                            needModify = true;
                            process = (int)(process + processPer * 0.15);
                            MesageEvent?.Invoke(null, new ConfigManageEventArgs("用户QuotationGroup文件处理成功", process));
                        }
                        else
                        {
                            process = (int)(process + processPer * 0.15);
                            PrintErrEndLine("该用户尚未生成QuotationGroup配置文件", process);
                        }

                        if (File.Exists(customLayoutPath))
                        {
                            MesageEvent?.Invoke(null, new ConfigManageEventArgs("开始修改CustomLayoutConfig文件", process));
                            CustomLayoutConvertor layoutConvertor = new CustomLayoutConvertor(taskConfig);
                            if (layoutConvertor.ModifyParams.Count <= 0)
                            {
                                process = (int)(process + processPer * 0.1);
                                MesageEvent?.Invoke(null, new ConfigManageEventArgs("没有检测到需要修改的项目", process));
                            }
                            else
                            {
                                layoutConvertor.CustomLayoutModify(customLayoutPath, customLayoutPath);
                                needModify = true;
                                process = (int)(process + processPer * 0.1);
                                MesageEvent?.Invoke(null, new ConfigManageEventArgs("修改CustomLayoutConfig文件成功", process));//70%
                            }
                        }
                        else
                        {
                            process = (int)(process + processPer * 0.1);
                            PrintErrEndLine("该用户尚未生成CustomLayoutConfig配置文件", process);
                        }

                        if (File.Exists(futurelocalParamPath))
                        {
                            MesageEvent?.Invoke(null, new ConfigManageEventArgs("开始修改FutureLocalParam文件", process));
                            FutureLocalParamConvertor futureLocalParamConvertor = new FutureLocalParamConvertor(taskConfig, pool);
                            if (futureLocalParamConvertor.ModifyParams.Count <= 0)
                            {
                                process = (int)(process + processPer * 0.1);
                                MesageEvent?.Invoke(null, new ConfigManageEventArgs("没有检测到需要修改的项目", process));
                            }
                            else
                            {
                                futureLocalParamConvertor.FutureLocalParamModify(futurelocalParamPath, futurelocalParamPath);
                                needModify = true;
                                process = (int)(process + processPer * 0.1);
                                MesageEvent?.Invoke(null, new ConfigManageEventArgs("修改FutureLocalParam文件成功", process));//70%
                            }
                        }
                        else
                        {
                            process = (int)(process + processPer * 0.1);
                            PrintErrEndLine("该用户尚未生成FutureLocalParam配置文件", process);
                        }
                    }
                    #endregion

                    #region 保存数据到数据库
                    if (needModify)
                    {
                        MesageEvent?.Invoke(null, new ConfigManageEventArgs("压缩文件", process));
                        //压缩文件
                        FileConvertor.SevenZipCompress(deZipPath, localZip, 11);

                        SaveConfigInfoToModels(config_curType, localZip, sys_version_no_new, config_version, config_type);

                        if (config_curType.Count <= 0)
                        {
                            PrintErrEndLine("没有需要保存的数据", 0);
                            return;
                        }

                        process = (int)(process + processPer * 0.1);
                        MesageEvent?.Invoke(null, new ConfigManageEventArgs("开始保存信息到数据库", process));//80%
                        //保存到数据库
                        AddConfigInfo2DB(config_curType);
                        process = (int)(process + processPer * 0.2);
                        MesageEvent?.Invoke(null, new ConfigManageEventArgs("修改成功", process));//100%
                        MesageEvent?.Invoke(null, new ConfigManageEventArgs("------------------------------------------------"));
                    }
                    else
                    {
                        process = processBegin + processPer;//100%
                        MesageEvent?.Invoke(null, new ConfigManageEventArgs("没有需要修改的文件", process));
                    }
                    #endregion
                }
                MesageEvent?.Invoke(null, new ConfigManageEventArgs(string.Format("!!!修改完成!!!操作员序号【{0}】;系统版本【{1}】；更新系统版本【{2}】", operate_no, sys_version_no, sys_version_no_new), 100));
                MesageEvent?.Invoke(null, new ConfigManageEventArgs("------------------------------------------------"));
                MesageEvent?.Invoke(null, new ConfigManageEventArgs(Environment.NewLine));
            }
            catch (Exception ex)
            {
                MesageEvent?.Invoke(null, new ConfigManageEventArgs("修改文件发生异常，错误信息如下："));
                PrintErrEndLine(ExceptionHandle(ex), 0);
            }
        }

    }

    /// <summary>
    /// 外部消息
    /// </summary>
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
