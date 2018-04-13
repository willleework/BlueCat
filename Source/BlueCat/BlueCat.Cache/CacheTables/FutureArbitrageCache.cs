using BlueCat.DAL.MySQL1130C;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCat.Cache
{
    public class FutureArbitrageCache : ICacheTable
    {
        #region 内部变量
        MySQL1130C _database;
        public List<pub_tfuturearbitrageinfo> _stockList = new List<pub_tfuturearbitrageinfo>();
        public Dictionary<int, pub_tfuturearbitrageinfo> _stockDicKeyInterCode = new Dictionary<int, pub_tfuturearbitrageinfo>();
        #endregion

        #region 实例化及数据加载
        /// <summary>
        /// 证券信息缓存
        /// </summary>
        /// <param name="db"></param>
        public FutureArbitrageCache(MySQL1130C db)
        {
            _database = db;
        }

        /// <summary>
        /// 加载缓存
        /// </summary>
        public void Load()
        {
            List<pub_tfuturearbitrageinfo> stocks;
            lock (_database)
            {
                stocks = _database.pub_tfuturearbitrageinfo.ToList();
            }
            foreach (pub_tfuturearbitrageinfo stock in stocks)
            {
                if (_stockDicKeyInterCode.ContainsKey(stock.arbitrage_code))
                {
                    continue;
                }
                lock (_stockList)
                {
                    if (_stockDicKeyInterCode.ContainsKey(stock.arbitrage_code))
                    {
                        continue;
                    }
                    _stockList.Add(stock);
                    _stockDicKeyInterCode.Add(stock.arbitrage_code, stock);
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
        private bool CacheFilter(pub_tfuturearbitrageinfo stock)
        {
            return true;
        }
        #endregion

        #region 查询方法
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public List<pub_tfuturearbitrageinfo> GetAll()
        {
            return _stockList;
        }

        /// <summary>
        /// 获取证券信息 主键：内码
        /// </summary>
        /// <param name="intercode"></param>
        /// <returns></returns>
        public pub_tfuturearbitrageinfo GetPK(int intercode)
        {
            pub_tfuturearbitrageinfo stock;
            if (_stockDicKeyInterCode.TryGetValue(intercode, out stock))
            {
                return stock;
            }
            else
            {
                return new pub_tfuturearbitrageinfo();
            }
        }
        #endregion
    }
}
