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
            IQueryable<yh_tclientconfig> configs = dbtrade.yh_tclientconfig.Where(p => p.client_config_type.Equals("0") && p.operator_no == 10003);
            List<yh_tclientconfig> configList = configs.ToList();
            string savePath = Path.Combine(Environment.CurrentDirectory, "userConfig.7z");
            string filePath = Path.Combine(Environment.CurrentDirectory, "Unzip");
            byte[] configInfo = Convert.FromBase64String(configList[0].config_info);
            //string text = System.Text.Encoding.Default.GetString(configInfo);
            Bytes2File(configInfo, savePath);
            DecompressFile(savePath, filePath);
            Assert.IsTrue(configs.Count() > 0);
        }

        [TestMethod]
        public void TestMethod_FileCompress()
        {
            string oriPath = Path.Combine(Environment.CurrentDirectory, "Orig");
            string desPath = Path.Combine(Environment.CurrentDirectory, "Compress", "dest.7z");
            SevenZipCompress(oriPath, desPath);
        }

        [TestMethod]
        public void TestMethod_DBData()
        {
            string deCmpPath = Path.Combine(Environment.CurrentDirectory, "DeCompress.7z");
            string dbData = "N3q8ryccAANR6lqWRwkAAAAAAAAjAAAAAAAAAN9Q/hkAd67T46P5ge+2M7BhYLUXzO7HZsD6vCdE1glMZ/+B+6/3lxqbJvgZ3wle+yHYHZZskaTE9XBaZsdcuvJMxSNRETgpn9/XjHfhqIIHvHMGjXV12PDOsh90hSHrAzsaRB4PgJqA67FWTpqkAcn8LL3ZhnJ1s7ZF9x2UaihYucxqOP7fjMG2JdO/JQnOOwyYvOABILlGwcL8GQVHnrvYuKb3Me+ZhbgmvfUx/7DrXltpnFznDVY76cfkbv7oFvhpKu1jU9FhnCpw7TSK1jjun30u7AVkw7xy4Xn1vM1VpxqhP6I/WSME1jwNti4+sYdLfYZ43aTryq3fI3xfdFHtVmvER6h5pD51PHDIPa7kv2Ngy15E/ZddZ5xnjj0xZ3SPa8mSSNCRSl6IY+qEyuFAMVSLvIG4b6eBYw2TEF6ptPoGphhtLncqo+veN4vS3GJyb//Ih1jxK1WNShUOOFb2g7BljO23QZvdb1l6pkmTMwBCP4WJj8wnN39dqNGGQ6jMsAYCWZXWMbyK0Qe1sI+pPfFPxgQxHfQTIpQsqosk5W3B0kGinXmWCc8e/MQgv3Gw7fY0vbnF7uDz87nCZewfZi2K+Cu+4dzXLmBu590fidaFWFDa8ifJEuVMoGhVHE0S9mnbyBKZxxpsBYZIJAuH5R2YXfQ+e98zM8M4Uc14KGM6sC3gY1e9cQ2jwPy5RXekhR0fze4MMh8DriJDhl/RKQglReY29wyqiGl1TyKxXeu0dEMBlvZxKUp/jNtiizORL0SONK7ERb+PQyZV2WDCuP7gZW5EXgLA/n34VfkvGN9C5dCW7XPA/6O/nrdZCkMGl8rYln20MAcsTLa/IBWw39Kq2EaDJfDs3aernNwMDAqcmMMFfvuUNUn0JMlgKRMZSik/htLZ2UDplrp9UCJjM9rqyik4HNDixs3218Sb43M2sVlnhjfrxy8vOTw+Ll+sYfu/W2W2Yx9X6uSc0NWAFeT6negzeMny75fFfDjN7nWAYvlx/k4bJpIRWfc98fhE6jzjam32EA328KM7FcvPlp5FWLXf9HnDWEckI3+HHi9DnQ8GOM4oOv+CaV1WUroQTu/Ll81EMj06umuxYKehE6a9c6rqDNUThFqtTlXxZduLJSZrqWJ3RqaBlG1k3kFOIM2aOqpk3VZ8h8xv1B/KsVMOXKRVrIINN7dIszOFni8Dzl8UE18MbJJq3cnmqQgsN8Wzg2icpcv8FkmsJ+w4LM9+HOeHycDdUM4qOIcLnXKIeG/pfeqhHk7lwCimkSFdLOzAxEYOugQi+QhvISAomVR+g+DcCxSPLa4LCeS8+n4Cy9SMqur4llM57WB87BNo4DUBb+4D4XwyZ07OUc1JWUKXgchS/OQCUSjrSdqOa+Nar0Qf1tyFnRv4xIII0OGmsH3Jm+wAgur9BcHlTiD2w+/3Z6g/8EQQ/FyEyxZ1MgUEcdlLxGksW3HOYoAhlOlheN8RMpbBEdH8B7KwDHd2SJo4PMBlrfQvbsrNmZl5fYVl54YgdWODiUrw/96AW3zVTmlF75vJw0BK/nMdueDwAYaX5GcGPe/s7i5BtBn2i7gfV4k5KYa3hs5OI6h0cNlIWvQi07K2N5o65aWsnFjCENaQ7wtAJ3lSKZ2Axl3rxscERWIahtrNvfcs50K9KM56YBFHseyDWJ0LDL9NTHFL71UsXprS9oFF5sBu7X7j3JtRjH+h/ZuBGNsDTgnXfSbimQk9Gzq6sfHerDks5WkGvEMRJxEjCEKyOnczYwQdo0hFmSmAHsODdcZHbRsTWkF1kC+nNhf3/zRykWDoZDmpcoHAFxoyK8QeGyAma+kwG5D4mJdDlLfXx8w9ed8Usk726QXzwvU1/Q4SOflG/NmfUfQG2GZfiSID5HJAJhqZMWMwIp9Rfc+dnaTk2NGR365owBhmi5+Jntp7dfZ0pWHeETn0QcMxJt29bZqMEoVkRkuDV0n4z8FhKDIwvp6qdQ0IVTyWU93VLJCD2+Rhq7w/wfxPWAbJvHGYrMGZYLbnOTWYoa7IDdzUmasBfXFsDyQTaFgr1jHCjmMNjCns9H6uh8gPBomwWw4bZM5+zau+6DbK1Imt+kGGGK4jJSYqGpaRg/ZeRq6REqClDBAGsJLpCXqNGgiyaL+CaSI1x4NW62BiW81BC5+YPeGdn+RyPeFDZ2vjIU6n9tBOSfzQ4GmNv8oWg5Ptd1+5xuaKwBQZpYqgAfta4w7GG+f8soQzc3dqCTQhjTDOD4gUlfyFIkhIUNElj5QbamQmI9tlh17dFSaljhY9aiB3xmbdmeq4LOAw1e92Qs0CsNt8Y1l2/x+frvITYWGM7sLbXf5B+LTCsPj3QGK8LYIXiwAIb7oYdLRPLa/nN50dPda6+tqZwgSoxSmL3RrUeBbmnz88vTvuQQruczBII+Q9cyUNQV3muvNkbpY8ZCA/SMw0u9oCQok1URVLWP6bo1oODsx1eZzq6bEB85qXAtlTDBw+bZs8rREf9nmkZMd8Hqaad1aKpvQYHNV4AFOG8nTmGpEdIquaUXhg8NhFF4u4lVfMvdy0UWmXinyMtPuMFJQ/p7BeXew28363bGmw/vJXBMunrE+5+LjwLa1Go7aUrPTVeYCzUpTixi9yhSes3b5GtsYHbqW1Psr55XYHS0qvFToIaLZjeltEWWkfY4/SiZBwMucHv93ePqaZb92kMtDhYKDKlCNxUwjLLWr9NKZ+NO8Gdu9XgDtOJhFUqzdG94dZ1WtPNlM4WMGNl+mbfbvwMec8es1p62UIZkzBrh/ec5GXeLV+La2PtFZyd3W1EfiGHfk5MGFBhNdx9pZcFycoR5tTw8Qe3EHe8h9+gj9FmuLr5+ZIYpKSkMWqIvyYAY27uiKE4WKtl3nnvtuQ/GhhTsiMq+e0dtBno2KwB4TUh/gC0f+Yc99IUt8XXU132KHhwEUQoOoVCU/LRE46EI3ZP+OM70Zc0DfvzBqzF+rKAAAAgTMHrg/ViONy1yTT/rN+L4mSvr49CrGQ4YRp9rt62DhoJ5ebPZnZ1QD7jZigmRoBlnds57j38Kmy7yexZgS1fV3ScGGCTWtdEszMKHKOtmgBtOPNUHhQC+7wH4ujzQfpKK2tPO7VrNxBuspr24HK0zzECtf08BjkLq5okGNJvP0zt0+ADOIqvUGyFYv0ABcGiLcBCYCQAAcLAQABIwMBAQVdABAAAAyA4QoBxMU9gQAAAA==";
            byte[] deFileBytes = Convert.FromBase64String(dbData);
            Bytes2File(deFileBytes, deCmpPath);
        }

        [TestMethod]
        public void TestMethod_ByteConvertFile()
        {
            string desPath = Path.Combine(Environment.CurrentDirectory, "Compress", "dest.7z");
            string deCmpPath = Path.Combine(Environment.CurrentDirectory, "DeCompress", "dest1.7z");
            byte[] fileBytes = File2Bytes(desPath);
            string byteStr = Convert.ToBase64String(fileBytes);

            byte[] deFileBytes = Convert.FromBase64String(byteStr);
            Bytes2File(deFileBytes, deCmpPath);
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
    }
}
