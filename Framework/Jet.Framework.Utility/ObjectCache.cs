using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.Utility
{
    /// <summary>
    /// 对象缓存基类
    /// </summary>
    public abstract class ObjectCache<T>
        where T : class
    {
        protected T ObjCache = null;

        /// <summary>
        /// 获得一个最新的内容
        /// </summary>
        protected abstract T GetNewObject();

        protected virtual void AfterGetNewObject()
        {
        }

        /// <summary>
        /// 获取内容
        /// </summary>
        public virtual T GetObject()
        {
            if (this.ObjCache == null)
                this.RefreshCache();

            return this.ObjCache;
        }

        public void RefreshCache()
        {
            lock (this)
            {
                this.ObjCache = this.GetNewObject();
                this.AfterGetNewObject();
            }
        }
    }

    /// <summary>
    /// 对象自动刷新缓存基类
    /// </summary>
    public abstract class AutoFreshObjectCache<T> : ObjectCache<T>
        where T : class
    {
        protected abstract bool IsNeedUpdateCache();

        /// <summary>
        /// 获取内容
        /// </summary>
        public override T GetObject()
        {
            if (this.IsNeedUpdateCache())
            {
                DebugHelper.WriteLine("RefreshCache");
                this.RefreshCache();
            }

            return this.ObjCache;
        }
    }

    /// <summary>
    /// 每天更新一次缓存
    /// </summary>
    public abstract class DailyCache<T> : AutoFreshObjectCache<T>
        where T : class
    {
        private DateTime UpdateDate;

        protected override bool IsNeedUpdateCache()
        {
            if (this.ObjCache == null)
                return true;
            else
                return this.UpdateDate != DateTime.Now.Date;
        }

        protected override void AfterGetNewObject()
        {
            this.UpdateDate = DateTime.Now.Date;
        }
    }

    /// <summary>
    /// 定时更新缓存(分钟)
    /// </summary>
    public abstract class FixedTimeCache<T> : AutoFreshObjectCache<T>
        where T : class
    {
        public FixedTimeCache(int updateMinitues)
        {
            this.UpdateMinitues = updateMinitues;
        }

        /// <summary>
        /// 更新时间间隔(分钟)
        /// </summary>
        public int UpdateMinitues { get; private set; }

        private DateTime UpdateTime { get; set; }

        protected override bool IsNeedUpdateCache()
        {
            if (this.ObjCache == null)
                return true;
            else
                return Math.Abs((int)(this.UpdateTime - DateTime.Now).TotalMinutes) >= this.UpdateMinitues;
        }

        protected override void AfterGetNewObject()
        {
            this.UpdateTime = DateTime.Now;
        }
    }
}
