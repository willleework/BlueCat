using System.Collections.Generic;
using System.Xml;

namespace BlueCat.ConfigTools
{
    /// <summary>
    /// 期货行情组
    /// </summary>
    public class QuotationGroup0224G
    {
        #region 属性
        private bool _is_default = false;

        /// <summary>
        /// 是否为默认行情组
        /// </summary>
        public bool IsDefault
        {
            get { return _is_default; }
            set { _is_default = value; }
        }

        private int _quote_group_id = 0;
        /// <summary>
        /// 行情组序号
        /// </summary>
        public int QuotationGroupId
        {
            get
            {
                return _quote_group_id;
            }
            set
            {
                _quote_group_id = value;
            }
        }

        private int _order_id = 0;
        /// <summary>
        /// 排序序号
        /// </summary>
        public int OrderId
        {
            get { return _order_id; }
            set { _order_id = value; }
        }

        private string _quote_group_name = " ";
        /// <summary>
        /// 行情组名称
        /// </summary>
        public string QuotationGroupName
        {
            get
            {
                return _quote_group_name;
            }
            set
            {
                if (value == null) { value = ""; }
                _quote_group_name = value;
            }
        }

        private char _quote_group_type = ' ';
        /// <summary>
        /// 行情组类型
        /// </summary>
        public char QuotationGroupType
        {
            get
            {
                return _quote_group_type;
            }
            set
            {
                //暂不实现
                //字典:400003 的值域有效性校验
                _quote_group_type = value;
            }
        } 

        private List<QuotationGroupDetails0224G> _FutureQuotationGroupdetails = new List<QuotationGroupDetails0224G>();
        /// <summary>
        /// 期货行情组详情
        /// </summary>
        public List<QuotationGroupDetails0224G> JyOfutureGroupdetails
        {
            get
            {
                return _FutureQuotationGroupdetails;
            }
            set
            {
                if (_FutureQuotationGroupdetails != null)
                {
                    _FutureQuotationGroupdetails = value;
                }
            }
        }
        #endregion


        /// <summary>
        /// 用于保存行情明细的节点列表
        /// </summary>
        public XmlNodeList NodeList { get; set; }
    }

}
