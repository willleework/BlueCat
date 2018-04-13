using BlueCat.DAL.MySQL1130C;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCat.Cache
{
    /// <summary>
    /// 证券信息表
    /// </summary>
    public class StockCache : ICacheTable
    {
        #region 内部变量
        MySQL1130C _database;
        public List<pub_tstockinfo> _stockList = new List<pub_tstockinfo>();
        public Dictionary<int, pub_tstockinfo> _stockDicKeyInterCode = new Dictionary<int, pub_tstockinfo>();
        public Dictionary<string, pub_tstockinfo> _stockDicKeyCodeMarket = new Dictionary<string, pub_tstockinfo>(); 
        #endregion

        #region 实例化及数据加载
        /// <summary>
        /// 证券信息缓存
        /// </summary>
        /// <param name="db"></param>
        public StockCache(MySQL1130C db)
        {
            _database = db;
        }

        /// <summary>
        /// 加载缓存
        /// </summary>
        public void Load()
        {
            List<pub_tstockinfo> stocks;
            int[] marketLimit = new int[] { 3, 4, 7, 9, 34 };
            lock (_database)
            {
                stocks = _database.pub_tstockinfo.Where(p => marketLimit.Contains(p.market_no)).ToList();
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
                }
            }
        } 
        #endregion

        #region 内部方法
        /// <summary>
        /// 缓存过滤方法
        /// </summary>
        /// <param name="stock"></param>
        /// <returns></returns>
        private bool CacheFilter(pub_tstockinfo stock)
        {
            int[] marketLimit = new int[] { 3, 4, 7, 9, 34 };
            return marketLimit.Contains(stock.market_no);
        }

        /// <summary>
        /// 获取证券主键
        /// </summary>
        /// <param name="stock"></param>
        /// <returns></returns>
        private string GetStockKey(pub_tstockinfo stock)
        {
            return string.Format("{0}_{1}", stock.report_code, stock.market_no);
        } 
        #endregion

        #region 查询方法
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public List<pub_tstockinfo> GetAll()
        {
            return _stockList;
        }

        /// <summary>
        /// 获取证券信息 主键：合约代码，市场
        /// </summary>
        /// <param name="reportCode"></param>
        /// <param name="marketNo"></param>
        /// <returns></returns>
        public pub_tstockinfo GetPK(string reportCode, int marketNo)
        {
            string key = string.Format("{0}_{1}", reportCode.ToLower(), marketNo);
            pub_tstockinfo stock;
            if (_stockDicKeyCodeMarket.TryGetValue(key, out stock))
            {
                return stock;
            }
            else
            {
                return new pub_tstockinfo();
            }
        }

        /// <summary>
        /// 获取证券信息 主键：内码
        /// </summary>
        /// <param name="intercode"></param>
        /// <returns></returns>
        public pub_tstockinfo GetPK(int intercode)
        {
            pub_tstockinfo stock;
            if (_stockDicKeyInterCode.TryGetValue(intercode, out stock))
            {
                return stock;
            }
            else
            {
                return new pub_tstockinfo();
            }
        } 
        #endregion
    }
}
