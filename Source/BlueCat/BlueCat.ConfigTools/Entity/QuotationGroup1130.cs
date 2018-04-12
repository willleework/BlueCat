using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;
using BlueCat.ConfigTools.Quotation1130;

namespace BlueCat.ConfigTools
{
    /// <summary>
    /// 期货行情组
    /// </summary>
    public class QuotationGroup1130
    {
        #region 属性
        /// <summary>
        /// 行情组序号
        /// </summary>
        [DefaultValue("0")]
        [XmlAttribute("QuotationGroupId")]
        public string QuotationGroupId { get; set; }

        /// <summary>
        /// 排序序号
        /// </summary>
        [DefaultValue("0")]
        [XmlAttribute("OrderId")]
        public string OrderId { get; set; }

        /// <summary>
        /// 行情组名称
        /// </summary>
        [DefaultValue(" ")]
        [XmlAttribute("QuotationGroupName")]
        public string QuotationGroupName { get; set; }


        /// <summary>
        /// 行情组类型
        /// </summary>
        [DefaultValue("1")]
        [XmlAttribute("QuotationGroupType")]
        public string QuotationGroupType { get; set; }

        /// <summary>
        /// 是否为默认行情组
        /// </summary>
        [DefaultValue("False")]
        [XmlAttribute("IsDefault")]
        public string IsDefault { get; set;}

        private List<item> _FutureQuotationGroupdetails = new List<item>();
        /// <summary>
        /// 期货行情组详情
        /// </summary>
        [XmlElement("item")]
        public List<item> Quoatations
        {
            get
            {
                return _FutureQuotationGroupdetails;
            }
        }
        #endregion
    }

}
