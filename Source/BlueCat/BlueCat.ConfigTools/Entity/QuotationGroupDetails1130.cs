using System.ComponentModel;
using System.Xml.Serialization;

namespace BlueCat.ConfigTools.Quotation1130
{
    /// <summary>
    /// �ڻ�����������
    /// </summary>
    public class QuotationGroupDetails1130
    {
        #region ����
        /// <summary>
        /// ��������
        /// </summary>
        [DefaultValue('\0')]
        [XmlAttribute("QuoteType")]
        public char QuoteType { get; set; }

        /// <summary>
        /// ��׼��Լ
        /// </summary>
        [DefaultValue("")]
        [XmlAttribute("StandardCode")]
        public string StandardCode { get; set; }

        /// <summary>
        /// ��׼��Լ�г�
        /// </summary>
        [DefaultValue(-1)]
        [XmlAttribute("StandardMarket")]
        public int StandardMarket { get; set; }


        /// <summary>
        /// ��׼����������
        /// </summary>
        [DefaultValue("")]
        [XmlAttribute("STDArbitrageCode")]
        public string STDArbitrageCode { get; set; }

        /// <summary>
        /// ������Լ�г�
        /// </summary>
        [DefaultValue(-1)]
        [XmlAttribute("STDArbitrageMarketNo")]
        public int STDArbitrageMarketNo { get; set; }

        /// <summary>
        /// ��һ�Ⱥ�Լ����
        /// </summary>
        [DefaultValue("")]
        [XmlAttribute("FirstLegCode")]
        public string FirstLegCode { get; set; }

        /// <summary>
        /// ��һ�Ⱥ�Լ�г�
        /// </summary>
        [DefaultValue(-1)]
        [XmlAttribute("FirstLegMarketNo")]
        public int FirstLegMarketNo { get; set; }

        /// <summary>
        /// �ڶ��Ⱥ�Լ����
        /// </summary>
        [DefaultValue("")]
        [XmlAttribute("SecondLegCode")]
        public string SecondLegCode { get; set; }

        /// <summary>
        /// �ڶ��Ⱥ�Լ�г�
        /// </summary>
        [DefaultValue(-1)]
        [XmlAttribute("SecondLegMarketNo")]
        public int SecondLegMarketNo { get; set; }

        /// <summary>
        /// ��һ�Ⱥ�Լ���
        /// ָ����ϵ�һ�ݺ�Լ�У���һ�Ⱥ�Լ�м���
        /// </summary>
        [DefaultValue(0)]
        [XmlAttribute("FirContractsRatio")]
        public double FirContractsRatio {get;set;}

        /// <summary>
        /// �ڶ��Ⱥ�Լ���
        /// </summary>
        [XmlAttribute("SecContractsRatio")]
        public double SecContractsRatio { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [DefaultValue(0)]
        [XmlAttribute("SellThreshold")]
        public double SellThreshold { get; set; }

        /// <summary>
        /// �����������ѣ�Ĭ�ϲ�����
        /// </summary>
        [DefaultValue(false)]
        [XmlAttribute("EnableSellThreshold")]
        public bool EnableSellThreshold { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [DefaultValue(0)]
        [XmlAttribute("BuyThreshold")]
        public double BuyThreshold { get;set;}

        /// <summary>
        /// �����������ѣ�Ĭ�ϲ�����
        /// </summary>
        [DefaultValue(false)]
        [XmlAttribute("EnableBuyThreshold")]
        public bool EnableBuyThreshold { get; set; }

        /// <summary>
        /// Ĭ������
        /// </summary>
        [DefaultValue(1)]
        [XmlAttribute("DefaultAmount")]
        public double DefaultAmount { get; set; }

        /// <summary>
        /// ����ģʽ
        /// </summary>
        [XmlAttribute("TriggerMode")]
        public int TriggerMode { get; set; }

        #endregion

    }

}
