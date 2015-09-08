//-----------------------------------------------------------------------
// <copyright file="ConsoleLogFactory.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Logs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// コンソールログファクトリクラスです。
    /// </summary>
    public sealed class ConsoleLogFactory : LogFactory
    {
        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public ConsoleLogFactory()
            : base()
        {
        }

        /// <summary>
        /// ログをフラッシュします。
        /// </summary>
        /// <param name="logs">フラッシュ対象のログです。</param>
        /// <returns>フラッシュ成功ならば真を返します。</returns>
        public override bool Flush(IReadOnlyCollection<Log> logs)
        {
            Contract.Assume(logs != null);

            foreach (var log in logs)
            {
#if DEBUG
                Console.WriteLine("{0} : {1}, {2}({3})", log.Message, log.MemberName, log.FileName, log.FileLine);
                Trace.WriteLine(string.Format("{0} : {1}, {2}({3})", log.Message, log.MemberName, log.FileName, log.FileLine));
#else
                Console.WriteLine("{0}", log.Message);
                Trace.WriteLine(log.Message);
#endif
            }

            return true;
        }
    }
}
