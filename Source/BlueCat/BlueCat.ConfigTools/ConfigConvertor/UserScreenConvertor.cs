using BlueCat.Tools.FileTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCat.ConfigTools
{
    /// <summary>
    /// 综合屏配置转换器
    /// </summary>
    public class UserScreenConvertor
    {
        private List<FieldConvertParam> _modifyParams = new List<FieldConvertParam>();
        public List<FieldConvertParam> ModifyParams { get => _modifyParams; set => _modifyParams = value; }

        /// <summary>
        /// 综合屏配置转换器
        /// </summary>
        /// <param name="taskFile"></param>
        public UserScreenConvertor(string taskFile)
        {
            ModifyParams = ExcelHelper.ImputFromExcel<FieldConvertParam>(taskFile, 0, 0, "UserScreen");
        }

        /// <summary>
        /// 综合屏文件修改
        /// </summary>
        /// <param name="originPath"></param>
        /// <param name="desPath"></param>
        public void UserScreenModify(string originPath, string desPath)
        {
            if (!File.Exists(originPath))
            {
                throw new Exception(string.Format("修改配置文件出错，综合屏配置文件不存在【{0}】", originPath));
            }
            if (ModifyParams.Count <= 0)
            {
                return;
            }
            string file = FileConvertor.ReadFile(originPath);
            StringBuilder config = new StringBuilder(file);
            foreach (FieldConvertParam param in ModifyParams)
            {
                config = config.Replace(param.FormerFieldValue, param.CurrentFieldValue);
            }
            FileConvertor.WriteFile(desPath, config.ToString(), false);
        }
    }
}
