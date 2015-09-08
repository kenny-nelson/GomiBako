//-----------------------------------------------------------------------
// <copyright file="Log.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Logs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// ログの種類です。
    /// </summary>
    public enum LogKind
    {
        /// <summary>
        /// 情報
        /// </summary>
        Information,

        /// <summary>
        /// 警告
        /// </summary>
        Warning,

        /// <summary>
        /// デバッグ
        /// </summary>
        Debug,

        /// <summary>
        /// エラー
        /// </summary>
        Error,
    }

    /// <summary>
    /// ログクラスです。
    /// </summary>
    public sealed class Log
    {
        private readonly LogKind kind;
        private readonly string message;
        private readonly string exceptionMessage;
#if DEBUG
        private readonly string fileName;
        private readonly int fileLine;
        private readonly string memberName;
#endif

        internal Log(LogKind kind, string message, [CallerFilePath] string fileName = null, [CallerLineNumber] int fileLine = 0, [CallerMemberName] string memberName = null)
        {
            this.kind = kind;
            this.message = message;
            this.exceptionMessage = string.Empty;
#if DEBUG
            this.fileName = fileName;
            this.fileLine = fileLine;
            this.memberName = memberName;
#endif
        }

        internal Log(LogKind kind, string message, Exception exception, [CallerFilePath] string fileName = null, [CallerLineNumber] int fileLine = 0, [CallerMemberName] string memberName = null)
        {
            this.kind = kind;
            this.message = message;
            this.exceptionMessage = exception.Message;
#if DEBUG
            this.fileName = fileName;
            this.fileLine = fileLine;
            this.memberName = memberName;
#endif
        }

        internal LogKind Kind
        {
            get
            {
                return this.kind;
            }
        }

        internal string Message
        {
            get
            {
                return this.message;
            }
        }

        internal string ExceptionMessage
        {
            get
            {
                return this.exceptionMessage;
            }
        }

#if DEBUG
        internal string FileName
        {
            get
            {
                return this.fileName;
            }
        }

        internal int FileLine
        {
            get
            {
                return this.fileLine;
            }
        }

        internal string MemberName
        {
            get
            {
                return this.memberName;
            }
        }
#endif
    }
}
