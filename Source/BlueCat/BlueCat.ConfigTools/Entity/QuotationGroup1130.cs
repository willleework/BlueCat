using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;
using BlueCat.ConfigTools.Quotation1130;

namespace BlueCat.ConfigTools
{
    /// <summary>
    /// �ڻ�������
    /// </summary>
    public class QuotationGroup1130
    {
        #region ����
        /// <summary>
        /// ���������
        /// </summary>
        [DefaultValue("0")]
        [XmlAttribute("QuotationGroupId")]
        public string QuotationGroupId { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        [DefaultValue("0")]
        [XmlAttribute("OrderId")]
        public string OrderId { get; set; }

        /// <summary>
        /// ����������
        /// </summary>
        [DefaultValue(" ")]
        [XmlAttribute("QuotationGroupName")]
        public string QuotationGroupName { get; set; }


        /// <summary>
        /// ����������
        /// </summary>
        [DefaultValue("1")]
        [XmlAttribute("QuotationGroupType")]
        public string QuotationGroupType { get; set; }

        /// <summary>
        /// �Ƿ�ΪĬ��������
        /// </summary>
        [DefaultValue("False")]
        [XmlAttribute("IsDefault")]
        public string IsDefault { get; set;}

        private List<QuotationGroupDetails1130> _FutureQuotationGroupdetails = new List<QuotationGroupDetails1130>();
        /// <summary>
        /// �ڻ�����������
        /// </summary>
        [XmlElement("item")]
        public List<QuotationGroupDetails1130> Quoatations
        {
            get
            {
                return _FutureQuotationGroupdetails;
            }
        }
        #endregion

    }

}
