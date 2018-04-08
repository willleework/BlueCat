using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCat.ConfigTools
{
    public enum FixedStyle
    {
        None,
        Left,
        Right
    }

    public enum ColumnSortOrder
    {
        None,
        Ascending,
        Descending
    }

    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperateType
    {
        Insert,
        Delete,
        Modify,
        Query
    }
}
