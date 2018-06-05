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
            string configText = MultiTradeModify(config.ToString());

            FileConvertor.WriteFile(desPath, configText, false);
        }

        /// <summary>
        /// 多产品tabcontrol节点特殊处理
        /// </summary>
        /// <param name="text"></param>
        private string MultiTradeModify(string text)
        {
            int beginIndex = text.IndexOf("<MenuName>FutureMultiTrade</MenuName>");
            if (beginIndex < 0)
            {
                return text;
            }
            int endIndex = text.IndexOf("</SaveParamItem>", beginIndex);
            if (endIndex < beginIndex || endIndex >= text.Length)
            {
                return text;
            }

            text = TextModifyByIndexScope(beginIndex, endIndex, text);
            return text;
        }

        private string TextModifyByIndexScope(int beginIndex, int endInex, string text)
        {
            if (beginIndex <= 0 || text.Length <= endInex)
            {
                return text;
            }
            string textForEdit = text.Substring(beginIndex, endInex - beginIndex);
            string modifyText = textForEdit.Replace("\"oTabControl1\"", "\"tabQurey\"");
            modifyText = modifyText.Replace("\"pankou\"", "\"panKou\"");
            return text.Replace(textForEdit, modifyText);
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
