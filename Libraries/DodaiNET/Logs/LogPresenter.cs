//-----------------------------------------------------------------------
// <copyright file="LogPresenter.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Logs
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using Dodai.Services;

    /// <summary>
    /// ログプレゼンタークラスです。
    /// </summary>
    public class LogPresenter : Presenter, ILogPresenter
    {        
        private readonly List<Log> logs = new List<Log>();
        private readonly object syncRoot = new object();
        private LogFactory factory = null;

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public LogPresenter()
            : base(Enum.GetName(typeof(PresenterKind), PresenterKind.Log))
        {
            this.factory = new NullLogFactory();
        }

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        /// <param name="factory">使用するログファクトリです。</param>
        public LogPresenter(LogFactory factory)
            : base(Enum.GetName(typeof(PresenterKind), PresenterKind.Log))
        {
            Contract.Assume(factory != null);
            this.factory = factory;
        }

        internal LogFactory Factory
        {
            get
            {
                return this.factory;
            }
        }

        /// <summary>
        /// ロガーをプッシュします。
        /// </summary>
        /// <param name="logger">プッシュ対象のロガーです。</param>
        public void PushLogger(Logger logger)
        {
            Contract.Assume(logger != null);
            lock (this.syncRoot)
            {
                foreach (var log in logger.Logs)
                {
                    this.logs.Add(log);
                }
            }
        }

        /// <summary>
        /// ログをフラッシュします。
        /// </summary>
        public void Flush()
        {
            lock (this.syncRoot)
            {
                if (this.factory.Flush(new ReadOnlyCollection<Log>(this.logs)))
                {
                    this.logs.Clear();
                }
            }
        }

        /// <summary>
        /// 廃棄します。内部処理
        /// </summary>
        protected override void DisposeInternal()
        {
            this.logs.Clear();
            this.factory = null;
        }
    }
}