using System.Collections.Generic;
using System.Xml;

namespace BlueCat.ConfigTools
{
    /// <summary>
    /// �ڻ�������
    /// </summary>
    public class QuotationGroup0224G
    {
        #region ����
        private bool _is_default = false;

        /// <summary>
        /// �Ƿ�ΪĬ��������
        /// </summary>
        public bool IsDefault
        {
            get { return _is_default; }
            set { _is_default = value; }
        }

        private int _quote_group_id = 0;
        /// <summary>
        /// ���������
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
        /// �������
        /// </summary>
        public int OrderId
        {
            get { return _order_id; }
            set { _order_id = value; }
        }

        private string _quote_group_name = " ";
        /// <summary>
        /// ����������
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
        /// ����������
        /// </summary>
        public char QuotationGroupType
        {
            get
            {
                return _quote_group_type;
            }
            set
            {
                //�ݲ�ʵ��
                //�ֵ�:400003 ��ֵ����Ч��У��
                _quote_group_type = value;
            }
        } 

        private List<QuotationGroupDetails0224G> _FutureQuotationGroupdetails = new List<QuotationGroupDetails0224G>();
        /// <summary>
        /// �ڻ�����������
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
        /// ���ڱ���������ϸ�Ľڵ��б�
        /// </summary>
        public XmlNodeList NodeList { get; set; }
    }

}
