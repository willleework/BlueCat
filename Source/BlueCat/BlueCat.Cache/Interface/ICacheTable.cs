using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCat.Cache
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICacheTable
    {
        /// <summary>
        /// 加载缓存数据
        /// </summary>
        void Load();
    }
}
