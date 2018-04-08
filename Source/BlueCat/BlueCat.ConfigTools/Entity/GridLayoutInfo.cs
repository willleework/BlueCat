using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace BlueCat.ConfigTools
{
    /// <summary>
    /// Grid布局信息
    /// </summary>
    [XmlRootAttribute("Layout", Namespace = "Layout", IsNullable = false)]
    public class GridLayoutInfo
    {
        private List<GridLayoutViewInfo> _views = new List<GridLayoutViewInfo>();

        /// <summary>
        /// 表格集合 
        /// </summary>
        [XmlArray("Views"), XmlArrayItem("View")]
        public List<GridLayoutViewInfo> Views
        {
            get { return _views; }
            set { _views = value; }
        }

        /// <summary>
        /// 获取需要保存的布局信息
        /// </summary>
        /// <returns></returns>
        public GridLayoutInfo GetSaveLayoutInfo()
        {
            GridLayoutInfo layout = new GridLayoutInfo();
            layout.Views.AddRange(Views.Where(p => p.Tables.Any(q => q.IsChanged && q.Columns.Count > 0)));
            return layout;
        }
    }
}
