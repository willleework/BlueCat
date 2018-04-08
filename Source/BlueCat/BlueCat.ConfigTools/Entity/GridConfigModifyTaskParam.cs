using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCat.ConfigTools
{
    /// <summary>
    /// 表格配置文件任务参数
    /// </summary>
    public class GridConfigModifyTaskParam
    {
        /// <summary>
        /// 视图名称
        /// </summary>
        public string View { get; set; }

        /// <summary>
        /// 表格名称
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// 表格名称
        /// </summary>
        public string Column { get; set; }

        /// <summary>
        /// 主键字段
        /// </summary>
        public string KeyField { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public OperateType OperateType { get; set; }

        /// <summary>
        /// 操作列
        /// </summary>
        public string OperateField { get; set; }

        /// <summary>
        /// 操作列目标值
        /// </summary>
        public string OperateFieldValue { get; set; }

        /// <summary>
        /// 配置信息
        /// </summary>
        public GridLayoutInfo GridConfigInfo { get; set; }
    }
}
