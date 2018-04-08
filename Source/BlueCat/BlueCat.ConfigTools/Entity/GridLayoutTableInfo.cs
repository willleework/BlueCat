using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace BlueCat.ConfigTools
{
    /// <summary>
    /// 表格布局信息
    /// </summary>
    public class GridLayoutTableInfo
    {
        private bool _isChanged = false;

        /// <summary>
        /// 是否发生变化
        /// </summary>
        [XmlIgnore]
        public bool IsChanged
        {
            get { return _isChanged; }
            set { _isChanged = value; }
        }

        private string _name = string.Empty;

        /// <summary>
        /// 表格名称，一般取数据源实体类型全称
        /// </summary>
        [XmlAttribute("Name")]
        public string Name
        {
            get { return _name; }
            set 
            {
                _isChanged = true;
                _name = value; 
            }
        }

        private List<GridLayoutColumnInfo> _columns = new List<GridLayoutColumnInfo>();

        /// <summary>
        /// 列集合 
        /// </summary>
        [XmlArray("Columns"), XmlArrayItem("Column")]
        public List<GridLayoutColumnInfo> Columns
        {
            get { return _columns; }
            set 
            {
                //反序列化的时候会设置这个属性，反序列化出来的布局信息，相对于默认布局，也是发生了变化的，在下次保存时，需要继续保存到文件中
                _isChanged = true;
                _columns = value;
            }
        }

        /// <summary>
        /// 获取列
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        public GridLayoutColumnInfo GetColumn(string fieldName)
        {
            return Columns.FirstOrDefault(p => p.FieldName == fieldName);
        }
    }
}
