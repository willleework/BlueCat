using SevenZip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BlueCat.Tools.FileTools
{
    /// <summary>
    /// 文件转换工具
    /// </summary>
    public class FileConvertor
    {
        public static string ByteToString()
        {
            return string.Empty;
        }

        #region 写文件
        /****************************************
         * 函数名称：WriteFile
         * 功能说明：当文件不存时，则创建文件，并追加文件
         * 参    数：Path:文件路径,Strings:文本内容
         * 调用示列：
         *           string Path = Server.MapPath("Default2.aspx");       
         *           string Strings = "这是我写的内容啊";
         *           Common.Utility.FileOperate.WriteFile(Path,Strings);
        *****************************************/
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="Strings">文件内容</param>
        public static void WriteFile(string Path, string Strings, bool append = true)
        {

            if (!System.IO.File.Exists(Path))
            {
                System.IO.FileStream f = System.IO.File.Create(Path);
                f.Close();
                f.Dispose();
            }
            System.IO.StreamWriter f2 = new System.IO.StreamWriter(Path, append, System.Text.Encoding.UTF8);
            f2.WriteLine(Strings);
            f2.Close();
            f2.Dispose();


        }
        #endregion

        #region 读文件
        /****************************************
         * 函数名称：ReadFile
         * 功能说明：读取文本内容
         * 参    数：Path:文件路径
         * 调用示列：
         *           string Path = Server.MapPath("Default2.aspx");       
         *           string s = Common.Utility.FileOperate.ReadFile(Path);
        *****************************************/
        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <returns></returns>
        public static string ReadFile(string Path)
        {
            string s = "";
            if (!System.IO.File.Exists(Path))
                s = "不存在相应的目录";
            else
            {
                StreamReader f2 = new StreamReader(Path, System.Text.Encoding.UTF8);
                s = f2.ReadToEnd();
                f2.Close();
                f2.Dispose();
            }

            return s;
        }
        #endregion

        #region 序列化

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T">要序列化的对象类型</typeparam>
        /// <param name="obj">要序列化的对象</param>
        /// <returns></returns>
        public static string ObjectSerializeXml<T>(T obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                using (StreamReader reader = new StreamReader(ms, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T">要序列化的对象类型</typeparam>
        /// <param name="obj">要序列化的对象</param>
        /// <returns></returns>
        public static void ObjectSerializeXmlFile<T>(T obj, string fileName)
        {
            string xml = ObjectSerializeXml(obj);
            File.WriteAllText(fileName, xml, Encoding.UTF8);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">反序列化对象类型</typeparam>
        /// <param name="xmlOfObject">反序列化字符串</param>
        /// <returns></returns>
        public static T XmlDeserializeObjectFromString<T>(string xmlOfObject) where T : class
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter sr = new StreamWriter(ms, Encoding.UTF8))
                {
                    sr.Write(xmlOfObject);
                    sr.Flush();
                    ms.Seek(0, SeekOrigin.Begin);
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return serializer.Deserialize(ms) as T;
                }
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlOfObject"></param>
        /// <returns></returns>
        public static T XmlDeserializeObjectFromFile<T>(string xmlOfObject) where T : class
        {
            string fileName = xmlOfObject;
            T obj;
            if (File.Exists(fileName))
            {
                string xml = File.ReadAllText(fileName, Encoding.UTF8);
                obj = XmlDeserializeObjectFromString<T>(xml);
            }
            else
            {
                throw new Exception(string.Format("文件不存在:[{0}]", xmlOfObject));
            }
            return obj;
        }

        /// <summary>
        /// 将byte数组转换为文件并保存到指定地址
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="savepath"></param>
        public static void Bytes2File(byte[] buff, string savepath)
        {
            if (File.Exists(savepath))
            {
                File.Delete(savepath);
            }

            FileStream fs = new FileStream(savepath, FileMode.CreateNew);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(buff, 0, buff.Length);
            bw.Close();
            fs.Close();
            bw.Dispose();
            fs.Dispose();
        }

        /// <summary>
        /// 将文件转换为byte数组
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <returns>转换后的byte数组</returns>
        public static byte[] File2Bytes(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                return new byte[0];
            }

            FileInfo fi = new FileInfo(path);
            byte[] buff = new byte[fi.Length];

            //using (FileStream fs = fi.OpenRead())
            //{
            //    fs.Read(buff, 0, Convert.ToInt32(fs.Length));
            //}
            FileStream fs = fi.OpenRead();
            fs.Read(buff, 0, Convert.ToInt32(fs.Length));
            fs.Close();

            return buff;
        }

        /// <summary>
        /// 7z方式解压缩文件夹
        /// </summary>
        /// <param name="strInFilePath">压缩文件路径</param>
        /// <param name="strOutDirectoryPath">需要带/的解压路径</param>
        public static void SevenZipDecompress(string strInFilePath, string strOutDirectoryPath)
        {
            //设置动态链接库地址
            string library = Path.Combine(System.Environment.CurrentDirectory, "Resource", "SevenZip", "7z.dll");
            SevenZipCompressor.SetLibraryPath(library);

            using (SevenZipExtractor tmp = new SevenZipExtractor(strInFilePath))
            {
                for (int i = 0; i < tmp.ArchiveFileData.Count; i++)
                {
                    tmp.ExtractFiles(strOutDirectoryPath, tmp.ArchiveFileData[i].Index);
                }
            }

        }

        /// <summary>
        /// 分卷压缩方法可设定压缩文件大小
        /// </summary>
        /// <param name="strInDirectoryPath"></param>
        /// <param name="strOutFilePath"></param>
        public static void SevenZipCompress(string strInDirectoryPath, string strOutFilePath, int sizeMaxK = 500)
        {
            string library = Path.Combine(Path.Combine(System.Environment.CurrentDirectory, "Resource", "SevenZip"), "7z.dll");
            SevenZipCompressor.SetLibraryPath(library);

            if (strInDirectoryPath != string.Empty && strOutFilePath != string.Empty)
            {
                SevenZipCompressor compressor = new SevenZipCompressor();
                compressor.ArchiveFormat = OutArchiveFormat.SevenZip;
                compressor.CompressionMode = CompressionMode.Create;
                compressor.TempFolderPath = System.IO.Path.GetTempPath();
                compressor.VolumeSize = Convert.ToInt32(sizeMaxK * 1024);// 输出文件大小
                compressor.CompressDirectory(strInDirectoryPath, strOutFilePath);
            }
        }

        /// <summary>
        /// 获取Excel文件数据表列表
        /// </summary>
        public static ArrayList GetExcelTables(string ExcelFileName)
        {
            DataTable dt = new DataTable();
            ArrayList TablesList = new ArrayList();
            if (File.Exists(ExcelFileName))
            {
                using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties='Excel 2010';Data Source=" + ExcelFileName))
                {
                    try
                    {
                        conn.Open();
                        dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                    }
                    catch (Exception exp)
                    {
                        throw exp;
                    }

                    //获取数据表个数
                    int tablecount = dt.Rows.Count;
                    for (int i = 0; i < tablecount; i++)
                    {
                        string tablename = dt.Rows[i][2].ToString().Trim().TrimEnd('$');
                        if (TablesList.IndexOf(tablename) < 0)
                        {
                            TablesList.Add(tablename);
                        }
                    }
                }
            }
            return TablesList;
        }

        /// <summary>
        /// 将Excel文件导出至DataTable(第一行作为表头)
        /// </summary>
        /// <param name="ExcelFilePath">Excel文件路径</param>
        /// <param name="TableName">数据表名，如果数据表名错误，默认为第一个数据表名</param>
        public static DataTable InputFromExcel(string ExcelFilePath, string TableName)
        {
            if (!File.Exists(ExcelFilePath))
            {
                throw new Exception("Excel文件不存在！");
            }

            //如果数据表名不存在，则数据表名为Excel文件的第一个数据表
            ArrayList TableList = new ArrayList();
            TableList = GetExcelTables(ExcelFilePath);

            if (TableName.IndexOf(TableName) < 0)
            {
                TableName = TableList[0].ToString().Trim();
            }

            DataTable table = new DataTable();
            OleDbConnection dbcon = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ExcelFilePath + ";Extended Properties='Excel 2010'");
            OleDbCommand cmd = new OleDbCommand("select * from [" + TableName + "$]", dbcon);
            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);

            try
            {
                if (dbcon.State == ConnectionState.Closed)
                {
                    dbcon.Open();
                }
                adapter.Fill(table);
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (dbcon.State == ConnectionState.Open)
                {
                    dbcon.Close();
                }
            }
            return table;
        }
        #endregion
    }

}
