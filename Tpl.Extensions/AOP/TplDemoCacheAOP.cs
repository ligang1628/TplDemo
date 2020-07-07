using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TplDemo.Comment.MemoryCache;

namespace TplDemo.Extensions.AOP
{
    /// <summary>
    /// 面向切面的缓存使用
    /// 缓存的拦截
    /// </summary>
    public class TplDemoCacheAOP : CacheAOPbase
    {
        private readonly ICaching cache;

        public TplDemoCacheAOP(ICaching cache)
        {
            this.cache = cache;
        }

        /// <summary>
        /// Intercept：拦截
        /// </summary>
        /// <param name="invocation">翻译：调用</param>
        public override void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            //对当前方法的特性验证
            //如果需要验证
            if (method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(CachingAttribute)) is CachingAttribute qCachingAttribute)
            {
                //获取自定义缓存键
                var cacheKey = CustomerCacheKey(invocation);
                //根据key获取相应的缓存值
                var cacheValue = cache.Get(cacheKey);
                if (cacheValue != null)
                {
                    //将当前获取到的缓存值，赋值给当前执行方法
                    invocation.ReturnValue = cacheValue;
                    return;
                }
                //去执行当前的方法
                invocation.Proceed();
                //存入缓存
                if (!string.IsNullOrWhiteSpace(cacheKey))
                {
                    cache.Set(cacheKey, invocation.ReturnValue);
                }
            }
            else
            {
                invocation.Proceed();//直接执行被拦截方法
            }
        }
    }
}
