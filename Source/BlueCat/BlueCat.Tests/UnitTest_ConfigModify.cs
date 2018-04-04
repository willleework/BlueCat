using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using BlueCat.DAL.MySQL1130C;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SevenZip;

namespace BlueCat.Tests
{
    [TestClass]
    public class UnitTest_ConfigModify
    {
        MySQL1130C dbtrade = new MySQL1130C();

        [TestMethod]
        public void TestMethod_DBRead()
        {
            //MySQL1130C dbtrade = new MySQL1130C();
            //dbtrade.Database.Connection.ConnectionString = string.Empty;
            //dbtrade.Database.Connection.ConnectionString = "server=10.20.31.42;user id=root;password=ziguan.123;persistsecurityinfo=True;database=dbtrade";
            IQueryable<yh_tclientconfig> configs = dbtrade.yh_tclientconfig.Where(p => p.client_config_type.Equals("2"));
            List<yh_tclientconfig> configList = configs.ToList();
            string savePath = Path.Combine(Environment.CurrentDirectory, "userConfig.7z");
            string filePath = Path.Combine(Environment.CurrentDirectory, "Unzip");
            byte[] configInfo = Convert.FromBase64String(configList[0].config_info);
            //string text = System.Text.Encoding.Default.GetString(configInfo);
            Bytes2File(configInfo, savePath);
            DecompressFile(savePath, filePath);
            Assert.IsTrue(configs.Count() > 0);
        }

        /// <summary>
        /// 将byte数组转换为文件并保存到指定地址
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="savepath"></param>
        private void Bytes2File(byte[] buff, string savepath)
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
        /// 7z方式解压缩文件夹
        /// </summary>
        /// <param name="strInFilePath">压缩文件路径</param>
        /// <param name="strOutDirectoryPath">需要带/的解压路径</param>
        public static void DecompressFile(string strInFilePath, string strOutDirectoryPath)
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
    }
}
