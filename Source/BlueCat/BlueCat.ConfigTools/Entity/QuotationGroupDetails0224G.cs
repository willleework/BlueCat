namespace BlueCat.ConfigTools
{
    /// <summary>
    /// �ڻ�����������
    /// </summary>
    public class QuotationGroupDetails0224G
    {
        #region ����
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

        private int _arbitrage_code = 0;
        /// <summary>
        /// ��׼����������
        /// </summary>
        public int ArbitrageCode
        {
            get
            {
                return _arbitrage_code;
            }
            set
            {
                _arbitrage_code = value;
            }
        }

        private int _fir_intercode = 0;
        /// <summary>
        /// ��һ�Ⱥ�Լ����
        /// �ڻ��������ĵ�һ�Ⱥ�Լ����
        /// </summary>
        public int FirstIntercode
        {
            get
            {
                return _fir_intercode;
            }
            set
            {
                _fir_intercode = value;
            }
        }

        private int _sec_intercode = 0;
        /// <summary>
        /// �ڶ��Ⱥ�Լ����
        /// �ڻ��������ĵڶ��Ⱥ�Լ����
        /// </summary>
        public int SecondIntercode
        {
            get
            {
                return _sec_intercode;
            }
            set
            {
                _sec_intercode = value;
            }
        }

        private double _fir_contracts_ratio = 0;
        /// <summary>
        /// ��һ�Ⱥ�Լ���
        /// ָ����ϵ�һ�ݺ�Լ�У���һ�Ⱥ�Լ�м���
        /// </summary>
        public double FirstContractsRatio
        {
            get
            {
                return _fir_contracts_ratio;
            }
            set
            {
                _fir_contracts_ratio = value;
            }
        }

        private double _sec_contracts_ratio = 0;
        /// <summary>
        /// �ڶ��Ⱥ�Լ���
        /// </summary>
        public double SecondContractsRatio
        {
            get
            {
                return _sec_contracts_ratio;
            }
            set
            {
                _sec_contracts_ratio = value;
            }
        }

        private double _sellThreshold = 0;
        /// <summary>
        /// ��������
        /// </summary>
        public double SellThreshold
        {
            get { return _sellThreshold; }
            set { _sellThreshold = value; }
        }

        private bool _enableSellThreshold = false;
        /// <summary>
        /// �����������ѣ�Ĭ�ϲ�����
        /// </summary>
        public bool EnableSellThreshold
        {
            get { return _enableSellThreshold; }
            set { _enableSellThreshold = value; }
        }

        private double _buyThreshold = 0;
        /// <summary>
        /// ��������
        /// </summary>
        public double BuyThreshold
        {
            get { return _buyThreshold; }
            set { _buyThreshold = value; }
        }

        private bool _enableBuyThreshold = false;
        /// <summary>
        /// �����������ѣ�Ĭ�ϲ�����
        /// </summary>
        public bool EnableBuyThreshold
        {
            get { return _enableBuyThreshold; }
            set { _enableBuyThreshold = value; }
        }

        private double _defaultAmount = 1;
        /// <summary>
        /// Ĭ������
        /// </summary>
        public double DefaultAmount
        {
            get { return _defaultAmount; }
            set { _defaultAmount = value; }
        }

        private int _triggerMode = 2;
        /// <summary>
        /// ����ģʽ
        /// </summary>
        public int TriggerMode
        {
            get { return _triggerMode; }
            set
            {
                if (_triggerMode == value)
                    return;
                _triggerMode = value;
            }
        }

        private char _arbi_type = ' ';
        /// <summary>
        /// ��������
        /// </summary>
        public char QuoteType
        {
            get
            {
                return _arbi_type;
            }
            set
            {
                _arbi_type = value;
            }
        } 
        #endregion

    }

}
