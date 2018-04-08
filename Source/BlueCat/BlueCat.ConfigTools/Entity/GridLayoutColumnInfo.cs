using System.Xml.Serialization;

namespace BlueCat.ConfigTools
{
    /// <summary>
    /// 列布局信息
    /// </summary>
    public class GridLayoutColumnInfo
    {
        private string _fieldName = string.Empty;

        /// <summary>
        /// 字段名称
        /// </summary>
        [XmlAttribute("FieldName")]
        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        private int _index = -1;

        /// <summary>
        /// 列顺序索引
        /// </summary>
        [XmlAttribute("Index")]
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        private int _width = -1;

        /// <summary>
        /// 列宽
        /// </summary>
        [XmlAttribute("Width")]
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        private bool _visible = true;

        /// <summary>
        /// 是否显示列
        /// </summary>
        [XmlAttribute("Visible")]
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        private int _sortIndex = -1;

        /// <summary>
        /// 排序索引号
        /// </summary>
        [XmlAttribute("SortIndex")]
        public int SortIndex
        {
            get { return _sortIndex; }
            set { _sortIndex = value; }
        }

        private ColumnSortOrder _sortOrder = ColumnSortOrder.None;

        /// <summary>
        /// 排序方式：升序、降序、无排序
        /// </summary>
        [XmlAttribute("SortOrder")]
        public ColumnSortOrder SortOrder
        {
            get { return _sortOrder; }
            set { _sortOrder = value; }
        }

        private FixedStyle _fixed = FixedStyle.None;

        /// <summary>
        /// 是否冻结列
        /// </summary>
        [XmlAttribute("Fixed")]
        public FixedStyle Fixed
        {
            get { return _fixed; }
            set { _fixed = value; }
        }
    }
}
