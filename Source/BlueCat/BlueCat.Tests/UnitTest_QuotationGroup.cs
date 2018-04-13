using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using BlueCat.ConfigTools;
using BlueCat.Tools.FileTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using BlueCat.DAL.MySQL1130C;
using BlueCat.Cache;

namespace BlueCat.Tests
{
    [TestClass]
    public class UnitTest_QuotationGroup
    {
        public List<pub_tstockinfo> _stockList = new List<pub_tstockinfo>();
        public Dictionary<int, pub_tstockinfo> _stockDicKeyInterCode = new Dictionary<int, pub_tstockinfo>();
        public Dictionary<string, pub_tstockinfo> _stockDicKeyCodeMarket = new Dictionary<string, pub_tstockinfo>();
        public CachePool Pool = new CachePool();
        string conn = "server=10.20.31.42;user id=root;password=ziguan.123;persistsecurityinfo=True;database=dbtrade";

        [TestMethod]
        public void TestMethod_LoadStockData()
        {
            CachePool pool = new CachePool();
            pool.Init(conn);
            Assert.IsTrue(pool.Stock.GetAll().Count > 0);
        }

        [TestMethod]
        public void TestMethod_Read0224Quotation()
        {
            string quotationPath = Path.Combine(Environment.CurrentDirectory, "Resource", "Quotation", "QuotationGroup.xml");
            List<QuotationGroup0224G>  quotationGroup0224G = LoadQuotationGroup0224G(quotationPath);
            Assert.IsTrue(quotationGroup0224G.Count>0);
        }

        [TestMethod]
        public void TestMethod_SaveQuotation()
        {
            string quotationPath = Path.Combine(Environment.CurrentDirectory, "Resource", "Quotation", "QuotationGroup.xml");
            string savePath = Path.Combine(Environment.CurrentDirectory, "Resource", "Quotation", string.Format("QuotationGroup{0}.xml",DateTime.Now.ToString("HHMMssfff")));
            Pool.Init("server=10.20.31.42;user id=root;password=ziguan.123;persistsecurityinfo=True;database=dbtrade");
            List<QuotationGroup0224G> quotationGroup0224G = LoadQuotationGroup0224G(quotationPath);
            Save(quotationGroup0224G, savePath);
        }

        /// <summary>
        /// 从文件加载行情信息（0224G）
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public List<QuotationGroup0224G> LoadQuotationGroup0224G(string fileName)
        {
            Dictionary<int, QuotationGroup0224G> quoteDic = new Dictionary<int, QuotationGroup0224G>();
            List<QuotationGroup0224G> quoteList = new List<QuotationGroup0224G>();
            Dictionary<string, QuotationGroup0224G> quoteDicByName = new Dictionary<string, QuotationGroup0224G>();

            XmlDocument xd = new XmlDocument();
            //string fileName = Path.Combine(Environment.CurrentDirectory, "Resource", "Quotation", "QuotationGroup.xml");
            if (File.Exists(fileName))
            {
                xd.Load(fileName);

                foreach (XmlNode root in xd.ChildNodes)
                {
                    foreach (XmlNode xn in root.ChildNodes)
                    {
                        QuotationGroup0224G QuotationGroup0224GItem = new QuotationGroup0224G();
                        QuotationGroup0224GItem.QuotationGroupId = int.Parse(xn.Attributes["QuotationGroupId"].Value);
                        QuotationGroup0224GItem.QuotationGroupName = xn.Attributes["QuotationGroupName"].Value;
                        QuotationGroup0224GItem.QuotationGroupType = xn.Attributes["QuotationGroupType"].Value[0];
                        QuotationGroup0224GItem.IsDefault = bool.Parse(xn.Attributes["IsDefault"].Value);

                        QuotationGroup0224GItem.NodeList = xn.ChildNodes;
                        if (!quoteDic.ContainsKey(QuotationGroup0224GItem.QuotationGroupId) && !quoteDicByName.ContainsKey(QuotationGroup0224GItem.QuotationGroupName))
                        {
                            quoteDic.Add(QuotationGroup0224GItem.QuotationGroupId, QuotationGroup0224GItem);
                            quoteList.Add(QuotationGroup0224GItem);
                            quoteDicByName.Add(QuotationGroup0224GItem.QuotationGroupName, QuotationGroup0224GItem);
                        }
                        QuotationGroup0224GItem.JyOfutureGroupdetails = LoadQuotationGroupDetails(QuotationGroup0224GItem.QuotationGroupId, QuotationGroup0224GItem.NodeList);
                    }
                }
            }
            return quoteList;
        }

        /// <summary>
        /// 加载行情明细 
        /// </summary>
        /// <param name="groupId">行情组序号</param>
        /// <param name="nodeList">行情明细列表</param>
        /// <returns></returns>
        public List<QuotationGroupDetails0224G> LoadQuotationGroupDetails(int groupId, XmlNodeList nodeList)
        {
            List<QuotationGroupDetails0224G> items = new List<QuotationGroupDetails0224G>();
            if (nodeList == null)
                return items;
            foreach (XmlNode sxn in nodeList)
            {
                QuotationGroupDetails0224G item = new QuotationGroupDetails0224G();
                item.QuotationGroupId = groupId;
                int attCount = sxn.Attributes.Count;
                XmlAttribute[] xmlAttributes = new XmlAttribute[attCount];
                sxn.Attributes.CopyTo(xmlAttributes, 0);
                List<XmlAttribute> xmlAttrList = xmlAttributes.ToList();
                if (xmlAttrList.Exists(p => p.Name.Equals("ArbitrageCode")))
                {
                    item.ArbitrageCode = int.Parse(sxn.Attributes["ArbitrageCode"].Value);
                }
                else
                {
                    item.ArbitrageCode = 0;
                }
                if (xmlAttrList.Exists(p => p.Name.Equals("FirIntercode")))
                {
                    item.FirstIntercode = int.Parse(sxn.Attributes["FirIntercode"].Value);
                }
                else
                {
                    item.FirstIntercode = 0;
                }
                if (xmlAttrList.Exists(p => p.Name.Equals("SecIntercode")))
                {
                    item.SecondIntercode = int.Parse(sxn.Attributes["SecIntercode"].Value);
                }
                else
                {
                    item.FirstIntercode = 0;
                }
                if (xmlAttrList.Exists(p => p.Name.Equals("FirContractsRatio")))
                {
                    item.FirstContractsRatio = double.Parse(sxn.Attributes["FirContractsRatio"].Value);
                }
                else
                {
                    item.FirstIntercode = 0;
                }
                if (xmlAttrList.Exists(p => p.Name.Equals("SecContractsRatio")))
                {
                    item.SecondContractsRatio = double.Parse(sxn.Attributes["SecContractsRatio"].Value);
                }
                else
                {
                    item.FirstIntercode = 0;
                }
                if (xmlAttrList.Exists(p => p.Name.Equals("SellThreshold")))
                {
                    item.SellThreshold = double.Parse(sxn.Attributes["SellThreshold"].Value);
                }
                else
                {
                    item.SellThreshold = 0;
                }
                if (xmlAttrList.Exists(p => p.Name.Equals("BuyThreshold")))
                {
                    item.BuyThreshold = double.Parse(sxn.Attributes["BuyThreshold"].Value);
                }
                else
                {
                    item.BuyThreshold = 0;
                }
                if (xmlAttrList.Exists(p => p.Name.Equals("EnableSellThreshold")))
                {
                    item.EnableSellThreshold = bool.Parse(sxn.Attributes["EnableSellThreshold"].Value);
                }
                else
                {
                    item.EnableSellThreshold = false;
                }
                if (xmlAttrList.Exists(p => p.Name.Equals("EnableBuyThreshold")))
                {
                    item.EnableBuyThreshold = bool.Parse(sxn.Attributes["EnableBuyThreshold"].Value);
                }
                else
                {
                    item.EnableBuyThreshold = false;
                }
                if (xmlAttrList.Exists(p => p.Name.Equals("DefaultAmount")))
                {
                    item.DefaultAmount = int.Parse(sxn.Attributes["DefaultAmount"].Value);
                }
                else
                {
                    item.FirstIntercode = 0;
                }
                if (xmlAttrList.Exists(p => p.Name.Equals("TriggerMode")))
                {
                    item.TriggerMode = (int)sxn.Attributes["TriggerMode"].Value[0];
                }
                else
                {
                    item.TriggerMode = 2;
                }
                if (sxn.Attributes["QuoteType"] != null && !string.IsNullOrWhiteSpace(sxn.Attributes["QuoteType"].Value))
                {
                    item.QuoteType = sxn.Attributes["QuoteType"].Value[0];
                }
                else if (sxn.Attributes["ArbiType"] != null && !string.IsNullOrWhiteSpace(sxn.Attributes["ArbiType"].Value))
                {
                    //这里是为了兼容之前版本的配置文件
                    item.QuoteType = sxn.Attributes["ArbiType"].Value[0];
                }
                else
                {
                    continue;
                }


                items.Add(item);
            }

            return items;
        }

        /// <summary>
        /// 保存行情设置
        /// </summary>
        public void Save(List<QuotationGroup0224G> quoteList, string savePath)
        {
            XmlDocument xd = new XmlDocument();
            xd.AppendChild(xd.CreateXmlDeclaration("1.0", "utf-8", string.Empty));
            XmlElement root = xd.CreateElement("root");
            xd.AppendChild(root);
            XmlNode xn = null;
            XmlNode sxn = null;
            foreach (QuotationGroup0224G group in quoteList)
            {
                xn = xd.CreateElement("group");
                xn.Attributes.Append(xd.CreateAttribute("QuotationGroupId"));
                xn.Attributes.Append(xd.CreateAttribute("OrderId"));
                xn.Attributes.Append(xd.CreateAttribute("QuotationGroupName"));
                xn.Attributes.Append(xd.CreateAttribute("QuotationGroupType"));
                xn.Attributes.Append(xd.CreateAttribute("IsDefault"));
                xn.Attributes["QuotationGroupId"].Value = group.QuotationGroupId.ToString();
                xn.Attributes["OrderId"].Value = group.OrderId.ToString();
                xn.Attributes["QuotationGroupName"].Value = group.QuotationGroupName.ToString();
                xn.Attributes["QuotationGroupType"].Value = group.QuotationGroupType.ToString();
                xn.Attributes["IsDefault"].Value = group.IsDefault.ToString();

                foreach (QuotationGroupDetails0224G item in group.JyOfutureGroupdetails)
                {
                    pub_tstockinfo standardContract = new pub_tstockinfo();
                    pub_tfuturearbitrageinfo arbitrage = new pub_tfuturearbitrageinfo();
                    pub_tstockinfo arbiStock = new pub_tstockinfo();
                    pub_tstockinfo firstLeg = new pub_tstockinfo();
                    pub_tstockinfo secondLeg = new pub_tstockinfo();
                    if (item.QuoteType == QuotationType.标准合约)
                    {
                        standardContract = Pool.Stock.GetPK(item.FirstIntercode);
                    }
                    else
                    {
                        if (item.QuoteType == QuotationType.本地套利)
                        {
                            firstLeg = Pool.Stock.GetPK(item.FirstIntercode);
                            secondLeg = Pool.Stock.GetPK(item.SecondIntercode);
                        }
                        else
                        {
                            arbitrage = Pool.ArbitrageCache.GetPK(item.ArbitrageCode);
                            if (arbitrage.arbitrage_code>0)
                            {
                                arbiStock = Pool.Stock.GetPK(item.ArbitrageCode);
                                firstLeg = Pool.Stock.GetPK((int)arbitrage.fir_intercode);
                                secondLeg = Pool.Stock.GetPK((int)arbitrage.sec_intercode);
                            }
                        }
                    }
                    sxn = xd.CreateElement("item");
                    sxn.Attributes.Append(xd.CreateAttribute("QuoteType"));
                    sxn.Attributes.Append(xd.CreateAttribute("StandardCode"));
                    sxn.Attributes.Append(xd.CreateAttribute("StandardMarket"));
                    sxn.Attributes.Append(xd.CreateAttribute("STDArbitrageCode"));
                    sxn.Attributes.Append(xd.CreateAttribute("STDArbitrageMarketNo"));
                    sxn.Attributes.Append(xd.CreateAttribute("FirstLegCode"));
                    sxn.Attributes.Append(xd.CreateAttribute("FirstLegMarketNo"));
                    sxn.Attributes.Append(xd.CreateAttribute("SecondLegCode"));
                    sxn.Attributes.Append(xd.CreateAttribute("SecondLegMarketNo"));
                    sxn.Attributes.Append(xd.CreateAttribute("FirContractsRatio"));
                    sxn.Attributes.Append(xd.CreateAttribute("SecContractsRatio"));
                    sxn.Attributes.Append(xd.CreateAttribute("SellThreshold"));
                    sxn.Attributes.Append(xd.CreateAttribute("EnableSellThreshold"));
                    sxn.Attributes.Append(xd.CreateAttribute("BuyThreshold"));
                    sxn.Attributes.Append(xd.CreateAttribute("EnableBuyThreshold"));
                    sxn.Attributes.Append(xd.CreateAttribute("DefaultAmount"));
                    sxn.Attributes.Append(xd.CreateAttribute("TriggerMode"));
                    sxn.Attributes["QuoteType"].Value = item.QuoteType.ToString();
                    sxn.Attributes["StandardCode"].Value = string.IsNullOrEmpty(standardContract.report_code) ? string.Empty : standardContract.report_code;
                    sxn.Attributes["StandardMarket"].Value = standardContract.market_no.ToString();
                    sxn.Attributes["STDArbitrageCode"].Value = string.IsNullOrEmpty(arbiStock.report_code) ? string.Empty : arbiStock.report_code.ToString();
                    sxn.Attributes["STDArbitrageMarketNo"].Value = arbiStock.market_no.ToString();
                    sxn.Attributes["FirstLegCode"].Value = string.IsNullOrEmpty(firstLeg.report_code) ? string.Empty : firstLeg.report_code;
                    sxn.Attributes["FirstLegMarketNo"].Value = firstLeg.market_no.ToString();
                    sxn.Attributes["SecondLegCode"].Value = string.IsNullOrEmpty(secondLeg.report_code) ? string.Empty : secondLeg.report_code.ToString();
                    sxn.Attributes["SecondLegMarketNo"].Value = secondLeg.market_no.ToString();
                    sxn.Attributes["FirContractsRatio"].Value = item.FirstContractsRatio.ToString();
                    sxn.Attributes["SecContractsRatio"].Value = item.SecondContractsRatio.ToString();
                    sxn.Attributes["SellThreshold"].Value = item.SellThreshold.ToString();
                    sxn.Attributes["EnableSellThreshold"].Value = item.EnableSellThreshold.ToString();
                    sxn.Attributes["BuyThreshold"].Value = item.BuyThreshold.ToString();
                    sxn.Attributes["EnableBuyThreshold"].Value = item.EnableBuyThreshold.ToString();
                    sxn.Attributes["DefaultAmount"].Value = item.DefaultAmount.ToString();
                    sxn.Attributes["TriggerMode"].Value = item.TriggerMode.ToString();

                    xn.AppendChild(sxn);
                }

                root.AppendChild(xn);
            }

            using (StreamWriter sw = File.CreateText(savePath))
            {
                xd.Save(sw);
            }
        }

        /// <summary>
        /// 加载证券信息数据 
        /// </summary>
        public void LoadStockData()
        {
            int[] futureMarketLimit = new int[] { 3, 4, 7, 9, 34 };
            List<pub_tstockinfo> stocks;
            using (MySQL1130C context = new MySQL1130C())
            {
                stocks = context.pub_tstockinfo.Where(p => futureMarketLimit.Contains(p.market_no)).ToList();
            }
            foreach (pub_tstockinfo stock in stocks)
            {
                if (_stockDicKeyInterCode.ContainsKey(stock.inter_code))
                {
                    continue;
                }
                lock (_stockList)
                {
                    if (_stockDicKeyInterCode.ContainsKey(stock.inter_code))
                    {
                        continue;
                    }
                    _stockList.Add(stock);
                    _stockDicKeyInterCode.Add(stock.inter_code, stock);
                    _stockDicKeyCodeMarket.Add(GetStockKey(stock), stock);
                    ;
                }
            }
        }

        /// <summary>
        /// 获取证券主键
        /// </summary>
        /// <param name="stock"></param>
        /// <returns></returns>
        public string GetStockKey(pub_tstockinfo stock)
        {
            return string.Format("{0}_{1}", stock.report_code, stock.market_no);
        }
    }



    /// <summary>
    /// 行情类型
    /// </summary>
    public class QuotationType
    {
        public const char 标准合约 = '1';
        public const char 标准跨期套利 = '2';
        public const char 标准跨品种套利 = '3';
        public const char 标准跨市场套利 = '4';
        public const char 本地套利 = '5';
    }
}
