//-----------------------------------------------------------------------
// <copyright file="LogFactory.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Logs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// ログファクトリクラスです。
    /// </summary>
    public abstract class LogFactory
    {
        /// <summary>
        /// コンストラクタです。
        /// </summary>
        protected LogFactory()
        { 
        }

        /// <summary>
        /// ログをフラッシュします。
        /// </summary>
        /// <param name="logs">フラッシュ対象のログです。</param>
        /// <returns>フラッシュ成功ならば真を返します。</returns>
        public abstract bool Flush(IReadOnlyCollection<Log> logs);
    }
}
