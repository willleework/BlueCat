using BlueCat.Cache;
using BlueCat.DAL.MySQL1130C;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BlueCat.ConfigTools
{
    /// <summary>
    /// 期货行情配置文件转换工具
    /// </summary>
    public class FutureQuotationConvertor
    {
        private CachePool _Pool = new CachePool();

        /// <summary>
        /// 期货行情配置文件转换工具
        /// </summary>
        /// <param name="pool"></param>
        public FutureQuotationConvertor(CachePool pool)
        {
            _Pool = pool;
        }

        /// <summary>
        /// 期货行情配置文件转换工具
        /// </summary>
        /// <param name="originPath"></param>
        /// <param name="destinationPath"></param>
        public void FutureQuotationModify(string originPath, string destinationPath)
        {
            if (!File.Exists(originPath))
            {
                throw new Exception(string.Format("修改行情配置文件出错，行情文件不存在【{0}】", originPath));
            }
            List<QuotationGroup0224G> groupFor0224G = LoadQuotationGroup0224G(originPath);

            Save(groupFor0224G, destinationPath);
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
                        standardContract = _Pool.Stock.GetPK(item.FirstIntercode);
                    }
                    else
                    {
                        if (item.QuoteType == QuotationType.本地套利)
                        {
                            firstLeg = _Pool.Stock.GetPK(item.FirstIntercode);
                            secondLeg = _Pool.Stock.GetPK(item.SecondIntercode);
                        }
                        else
                        {
                            arbitrage = _Pool.ArbitrageCache.GetPK(item.ArbitrageCode);
                            if (arbitrage.arbitrage_code > 0)
                            {
                                arbiStock = _Pool.Stock.GetPK(item.ArbitrageCode);
                                firstLeg = _Pool.Stock.GetPK((int)arbitrage.fir_intercode);
                                secondLeg = _Pool.Stock.GetPK((int)arbitrage.sec_intercode);
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
        /// 加载行情明细 
        /// </summary>
        /// <param name="groupId">行情组序号</param>
        /// <param name="nodeList">行情明细列表</param>
        /// <returns></returns>
        private List<QuotationGroupDetails0224G> LoadQuotationGroupDetails(int groupId, XmlNodeList nodeList)
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
