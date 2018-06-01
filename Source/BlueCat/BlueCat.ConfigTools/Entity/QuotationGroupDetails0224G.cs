namespace BlueCat.ConfigTools
{
    /// <summary>
    /// 期货行情组详情
    /// </summary>
    public class QuotationGroupDetails0224G
    {
        #region 属性
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

        private int _arbitrage_code = 0;
        /// <summary>
        /// 标准套利单代码
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
        /// 第一腿合约内码
        /// 期货套利单的第一腿合约内码
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
        /// 第二腿合约内码
        /// 期货套利单的第二腿合约内码
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
        /// 第一腿合约配比
        /// 指在组合单一份合约中，第一腿合约有几手
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
        /// 第二腿合约配比
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
        /// 卖出提醒
        /// </summary>
        public double SellThreshold
        {
            get { return _sellThreshold; }
            set { _sellThreshold = value; }
        }

        private bool _enableSellThreshold = false;
        /// <summary>
        /// 启用卖出提醒，默认不启用
        /// </summary>
        public bool EnableSellThreshold
        {
            get { return _enableSellThreshold; }
            set { _enableSellThreshold = value; }
        }

        private double _buyThreshold = 0;
        /// <summary>
        /// 买入提醒
        /// </summary>
        public double BuyThreshold
        {
            get { return _buyThreshold; }
            set { _buyThreshold = value; }
        }

        private bool _enableBuyThreshold = false;
        /// <summary>
        /// 启用买入提醒，默认不启用
        /// </summary>
        public bool EnableBuyThreshold
        {
            get { return _enableBuyThreshold; }
            set { _enableBuyThreshold = value; }
        }

        private double _defaultAmount = 1;
        /// <summary>
        /// 默认数量
        /// </summary>
        public double DefaultAmount
        {
            get { return _defaultAmount; }
            set { _defaultAmount = value; }
        }

        private int _triggerMode = 2;
        /// <summary>
        /// 触发模式
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
        /// 套利类型
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
