using System.ComponentModel;
using System.Xml.Serialization;

namespace BlueCat.ConfigTools.Quotation1130
{
    /// <summary>
    /// 期货行情组详情
    /// </summary>
    public class QuotationGroupDetails1130
    {
        #region 属性
        /// <summary>
        /// 套利类型
        /// </summary>
        [DefaultValue('\0')]
        [XmlAttribute("QuoteType")]
        public char QuoteType { get; set; }

        /// <summary>
        /// 标准合约
        /// </summary>
        [DefaultValue("")]
        [XmlAttribute("StandardCode")]
        public string StandardCode { get; set; }

        /// <summary>
        /// 标准合约市场
        /// </summary>
        [DefaultValue(-1)]
        [XmlAttribute("StandardMarket")]
        public int StandardMarket { get; set; }


        /// <summary>
        /// 标准套利单代码
        /// </summary>
        [DefaultValue("")]
        [XmlAttribute("STDArbitrageCode")]
        public string STDArbitrageCode { get; set; }

        /// <summary>
        /// 套利合约市场
        /// </summary>
        [DefaultValue(-1)]
        [XmlAttribute("STDArbitrageMarketNo")]
        public int STDArbitrageMarketNo { get; set; }

        /// <summary>
        /// 第一腿合约代码
        /// </summary>
        [DefaultValue("")]
        [XmlAttribute("FirstLegCode")]
        public string FirstLegCode { get; set; }

        /// <summary>
        /// 第一腿合约市场
        /// </summary>
        [DefaultValue(-1)]
        [XmlAttribute("FirstLegMarketNo")]
        public int FirstLegMarketNo { get; set; }

        /// <summary>
        /// 第二腿合约代码
        /// </summary>
        [DefaultValue("")]
        [XmlAttribute("SecondLegCode")]
        public string SecondLegCode { get; set; }

        /// <summary>
        /// 第二腿合约市场
        /// </summary>
        [DefaultValue(-1)]
        [XmlAttribute("SecondLegMarketNo")]
        public int SecondLegMarketNo { get; set; }

        /// <summary>
        /// 第一腿合约配比
        /// 指在组合单一份合约中，第一腿合约有几手
        /// </summary>
        [DefaultValue(0)]
        [XmlAttribute("FirContractsRatio")]
        public double FirContractsRatio {get;set;}

        /// <summary>
        /// 第二腿合约配比
        /// </summary>
        [XmlAttribute("SecContractsRatio")]
        public double SecContractsRatio { get; set; }

        /// <summary>
        /// 卖出提醒
        /// </summary>
        [DefaultValue(0)]
        [XmlAttribute("SellThreshold")]
        public double SellThreshold { get; set; }

        /// <summary>
        /// 启用卖出提醒，默认不启用
        /// </summary>
        [DefaultValue(false)]
        [XmlAttribute("EnableSellThreshold")]
        public bool EnableSellThreshold { get; set; }

        /// <summary>
        /// 买入提醒
        /// </summary>
        [DefaultValue(0)]
        [XmlAttribute("BuyThreshold")]
        public double BuyThreshold { get;set;}

        /// <summary>
        /// 启用买入提醒，默认不启用
        /// </summary>
        [DefaultValue(false)]
        [XmlAttribute("EnableBuyThreshold")]
        public bool EnableBuyThreshold { get; set; }

        /// <summary>
        /// 默认数量
        /// </summary>
        [DefaultValue(1)]
        [XmlAttribute("DefaultAmount")]
        public double DefaultAmount { get; set; }

        /// <summary>
        /// 触发模式
        /// </summary>
        [XmlAttribute("TriggerMode")]
        public int TriggerMode { get; set; }

        #endregion

    }

}
