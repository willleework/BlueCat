using BlueCat.Tools.FileTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCat.ConfigTools
{
    public class CustomLayoutConvertor
    {
        private List<FieldConvertParam> _modifyParams = new List<FieldConvertParam>();
        public List<FieldConvertParam> ModifyParams { get => _modifyParams; set => _modifyParams = value; }

        /// <summary>
        /// 自由布局配置转换器
        /// </summary>
        public CustomLayoutConvertor(string taskFile)
        {
            ModifyParams = ExcelHelper.ImputFromExcel<FieldConvertParam>(taskFile, 0, 0, "CustomLayout");
        }


        /// <summary>
        /// 自由布局文件修改
        /// </summary>
        /// <param name="originPath"></param>
        /// <param name="desPath"></param>
        public void CustomLayoutModify(string originPath, string desPath)
        {
            if (!File.Exists(originPath))
            {
                throw new Exception(string.Format("修改配置文件出错，自由布局文件不存在【{0}】", originPath));
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
            FileConvertor.WriteFile(desPath, config.ToString());
        }
    }

    /// <summary>
    /// 自由布局修改配置参数
    /// </summary>
    public class FieldConvertParam
    {
        /// <summary>
        /// 上一版本值
        /// </summary>
        public string FormerFieldValue
        {
            get;set;
        }

        /// <summary>
        /// 当前版本值
        /// </summary>
        public string CurrentFieldValue
        {
            get;set;
        }
    }
}
