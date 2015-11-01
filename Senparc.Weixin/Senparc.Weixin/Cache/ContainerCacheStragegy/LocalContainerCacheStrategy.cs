﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using Senparc.Weixin.Containers;

namespace Senparc.Weixin.Cache
{
    public static class LocalCacheHelper
    {

        /// <summary>
        /// 所有数据集合的列表
        /// </summary>
        public static IDictionary<string, IDictionary<string,IBaseContainerBag>> LocalCache { get; set; }

        static LocalCacheHelper()
        {
            LocalCache = new Dictionary<string, IDictionary<string, IBaseContainerBag>>(StringComparer.OrdinalIgnoreCase);
        }

    }

    /// <summary>
    /// 本地容器缓存策略
    /// </summary>
    /// <typeparam name="TContainerBag">容器内</typeparam>
    public class LocalContainerCacheStrategy : IContainerCacheStragegy
        //where TContainerBag : class, IBaseContainerBag, new()
    {
        #region 数据源

        private IDictionary<string, IDictionary<string, IBaseContainerBag>> _cache = LocalCacheHelper.LocalCache;

        #endregion

        #region 单例

        /// <summary>
        /// LocalCacheStrategy的构造函数
        /// </summary>
        LocalContainerCacheStrategy()
        {
        }

        //静态LocalCacheStrategy
        public static IBaseCacheStrategy<string, IDictionary<string, IBaseContainerBag>> Instance
        {
            get
            {
                return Nested.instance;//返回Nested类中的静态成员instance
            }
        }

        class Nested
        {
            static Nested()
            {
            }
            //将instance设为一个初始化的LocalCacheStrategy新实例
            internal static readonly LocalContainerCacheStrategy instance = new LocalContainerCacheStrategy();
        }

        #endregion

        #region ILocalCacheStrategy 成员


        public string CacheSetKey { get; set; }


        public void InsertToCache(string key, IDictionary<string, IBaseContainerBag> value)
        {
            if (key == null || value == null)
            {
                return;
            }

            LocalCacheHelper.LocalCache[key] = value;
        }

        public void RemoveFromCache(string key)
        {
            LocalCacheHelper.LocalCache.Remove(key);
        }

        public IDictionary<string, IBaseContainerBag> Get(string key)
        {
            if (_cache.ContainsKey(key))
            {
                return _cache[key];
            }
            return null;
        }

        public IDictionary<string, IDictionary<string, IBaseContainerBag>> GetAll()
        {
            //var list = _collectionList.Values.ToList();
            return _cache;
        }

        public bool CheckExisted(string key)
        {
            return _cache.ContainsKey(key);
        }

        public long GetCount()
        {
            return _cache.Count;
        }

        #endregion
    }
}
