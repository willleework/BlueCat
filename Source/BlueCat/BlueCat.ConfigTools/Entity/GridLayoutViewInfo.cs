using System.Collections.Generic;
using System.Xml.Serialization;

namespace BlueCat.ConfigTools
{
    /// <summary>
    /// 表格视图布局信息
    /// 一个View可以绑定不同类型的数据源，不同数据源展示的列不一样
    /// </summary>
    public class GridLayoutViewInfo
    {
        private string _name = string.Empty;

        /// <summary>
        /// 视图名称，命名格式：菜单名_表格名
        /// </summary>
        [XmlAttribute("Name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private List<GridLayoutTableInfo> _tables = new List<GridLayoutTableInfo>();

        /// <summary>
        /// 表格集合 
        /// </summary>
        [XmlArray("Tables"), XmlArrayItem("Table")]
        public List<GridLayoutTableInfo> Tables
        {
            get { return _tables; }
            set { _tables = value; }
        }
    }
}
