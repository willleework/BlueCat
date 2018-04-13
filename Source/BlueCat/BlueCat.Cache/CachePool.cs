using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueCat.DAL.MySQL1130C;

namespace BlueCat.Cache
{
    /// <summary>
    /// 客户端缓存池
    /// </summary>
    public class CachePool
    {
        private bool _inited = false;
        #region 缓存表
        /// <summary>
        /// 证券信息
        /// </summary>
        public StockCache Stock { get; set; }

        /// <summary>
        /// 套利代码
        /// </summary>
        public FutureArbitrageCache ArbitrageCache { get; set; }
        #endregion

        #region 方法
        /// <summary>
        /// 缓存池初始化
        /// </summary>
        /// <param name="conStr"></param>
        public void Init(string conStr)
        {
            lock (this)
            {
                if (!_inited)
                {
                    using (MySQL1130C mysql = new MySQL1130C())
                    {
                        //设置数据连接串
                        mysql.Database.Connection.ConnectionString = conStr;

                        //并行方式，数据越多性能提升越明显，表数据少时可以不用此方案
                        Parallel.Invoke(
                        () =>
                        {
                        //证券表缓存加载
                        Stock = new StockCache(mysql);
                            Stock.Load();
                        },
                        () =>
                        {
                        //套利代码表缓存加载
                        ArbitrageCache = new FutureArbitrageCache(mysql);
                            ArbitrageCache.Load();
                        });
                        _inited = true;
                    }
                }
            }
        } 
        #endregion
    }
}
