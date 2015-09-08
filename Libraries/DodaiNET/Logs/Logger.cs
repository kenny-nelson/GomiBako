//-----------------------------------------------------------------------
// <copyright file="Logger.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Logs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;
    using Dodai.ComponentModel;

    /// <summary>
    /// ロガークラスです。
    /// </summary>
    public sealed class Logger : DisposableObject
    {
        private readonly List<Log> logs = new List<Log>();
        private readonly object syncRoot = new object();

        internal IList<Log> Logs
        {
            get
            {
                return this.logs;
            }
        }

        /// <summary>
        /// デバッグログを出力します。
        /// </summary>
        /// <param name="message">出力するメッセージです。</param>
        /// <param name="fileName">ファイル名です。使用時に指定する必要はありません。</param>
        /// <param name="fileLine">ファイル行です。使用時に指定する必要はありません。</param>
        /// <param name="memberName">メンバー名です。使用時に指定する必要はありません。</param>
        [Conditional("DEBUG")]
        public void WriteDebug(string message, [CallerFilePath] string fileName = null, [CallerLineNumber] int fileLine = 0, [CallerMemberName] string memberName = null)
        {
            Contract.Requires(message != null);
            this.WriteLog(LogKind.Debug, message, fileName, fileLine, memberName);
        }

        /// <summary>
        /// デバッグログを出力します。
        /// </summary>
        /// <param name="message">出力するメッセージです。</param>
        /// <param name="exception">出力する例外です。</param>
        /// <param name="fileName">ファイル名です。使用時に指定する必要はありません。</param>
        /// <param name="fileLine">ファイル行です。使用時に指定する必要はありません。</param>
        /// <param name="memberName">メンバー名です。使用時に指定する必要はありません。</param>
        [Conditional("DEBUG")]
        public void WriteDebug(string message, Exception exception, [CallerFilePath] string fileName = null, [CallerLineNumber] int fileLine = 0, [CallerMemberName] string memberName = null)
        {
            Contract.Requires(message != null);
            this.WriteLog(LogKind.Debug, message, exception, fileName, fileLine, memberName);
        }

        /// <summary>
        /// 情報ログを出力します。
        /// </summary>
        /// <param name="message">出力するメッセージです。</param>
        /// <param name="fileName">ファイル名です。使用時に指定する必要はありません。</param>
        /// <param name="fileLine">ファイル行です。使用時に指定する必要はありません。</param>
        /// <param name="memberName">メンバー名です。使用時に指定する必要はありません。</param>
        public void WriteInformation(string message, [CallerFilePath] string fileName = null, [CallerLineNumber] int fileLine = 0, [CallerMemberName] string memberName = null)
        {
            Contract.Requires(message != null);
            this.WriteLog(LogKind.Information, message, fileName, fileLine, memberName);
        }

        /// <summary>
        /// 情報ログを出力します。
        /// </summary>
        /// <param name="message">出力するメッセージです。</param>
        /// <param name="exception">出力する例外です。</param>
        /// <param name="fileName">ファイル名です。使用時に指定する必要はありません。</param>
        /// <param name="fileLine">ファイル行です。使用時に指定する必要はありません。</param>
        /// <param name="memberName">メンバー名です。使用時に指定する必要はありません。</param>
        public void WriteInformation(string message, Exception exception, [CallerFilePath] string fileName = null, [CallerLineNumber] int fileLine = 0, [CallerMemberName] string memberName = null)
        {
            Contract.Requires(message != null);
            this.WriteLog(LogKind.Information, message, exception, fileName, fileLine, memberName);
        }

        /// <summary>
        /// 警告ログを出力します。
        /// </summary>
        /// <param name="message">出力するメッセージです。</param>
        /// <param name="fileName">ファイル名です。使用時に指定する必要はありません。</param>
        /// <param name="fileLine">ファイル行です。使用時に指定する必要はありません。</param>
        /// <param name="memberName">メンバー名です。使用時に指定する必要はありません。</param>
        public void WriteWarning(string message, [CallerFilePath] string fileName = null, [CallerLineNumber] int fileLine = 0, [CallerMemberName] string memberName = null)
        {
            Contract.Requires(message != null);
            this.WriteLog(LogKind.Warning, message, fileName, fileLine, memberName);
        }

        /// <summary>
        /// 警告ログを出力します。
        /// </summary>
        /// <param name="message">出力するメッセージです。</param>
        /// <param name="exception">出力する例外です。</param>
        /// <param name="fileName">ファイル名です。使用時に指定する必要はありません。</param>
        /// <param name="fileLine">ファイル行です。使用時に指定する必要はありません。</param>
        /// <param name="memberName">メンバー名です。使用時に指定する必要はありません。</param>
        public void WriteWarning(string message, Exception exception, [CallerFilePath] string fileName = null, [CallerLineNumber] int fileLine = 0, [CallerMemberName] string memberName = null)
        {
            Contract.Requires(message != null);
            this.WriteLog(LogKind.Warning, message, exception, fileName, fileLine, memberName);
        }

        /// <summary>
        /// エラーログを出力します。
        /// </summary>
        /// <param name="message">出力するメッセージです。</param>
        /// <param name="fileName">ファイル名です。使用時に指定する必要はありません。</param>
        /// <param name="fileLine">ファイル行です。使用時に指定する必要はありません。</param>
        /// <param name="memberName">メンバー名です。使用時に指定する必要はありません。</param>
        public void WriteError(string message, [CallerFilePath] string fileName = null, [CallerLineNumber] int fileLine = 0, [CallerMemberName] string memberName = null)
        {
            Contract.Requires(message != null);
            this.WriteLog(LogKind.Error, message, fileName, fileLine, memberName);
        }

        /// <summary>
        /// エラーログを出力します。
        /// </summary>
        /// <param name="message">出力するメッセージです。</param>
        /// <param name="exception">出力する例外です。</param>
        /// <param name="fileName">ファイル名です。使用時に指定する必要はありません。</param>
        /// <param name="fileLine">ファイル行です。使用時に指定する必要はありません。</param>
        /// <param name="memberName">メンバー名です。使用時に指定する必要はありません。</param>
        public void WriteError(string message, Exception exception, [CallerFilePath] string fileName = null, [CallerLineNumber] int fileLine = 0, [CallerMemberName] string memberName = null)
        {
            Contract.Requires(message != null);
            this.WriteLog(LogKind.Error, message, exception, fileName, fileLine, memberName);
        }

        /// <summary>
        /// ログ情報をクリアします。
        /// </summary>
        public void Clear()
        {
            this.logs.Clear();
        }

        /// <summary>
        /// 廃棄します。内部処理です。
        /// </summary>
        protected override void DisposeInternal()
        {
            this.Clear();
        }

        private void WriteLog(LogKind kind, string message, string fileName, int fileLine, string memberName)
        {
            lock (this.syncRoot)
            {
                this.logs.Add(new Log(kind, message, fileName, fileLine, memberName));
            }
        }

        private void WriteLog(LogKind kind, string message, Exception exception, string fileName, int fileLine, string memberName)
        {
            lock (this.syncRoot)
            {
                this.logs.Add(new Log(kind, message, exception, fileName, fileLine, memberName));
            }
        }
    }
}
