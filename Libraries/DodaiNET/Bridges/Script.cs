//-----------------------------------------------------------------------
// <copyright file="Script.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Bridges
{
    using System.Diagnostics.Contracts;
    using Dodai.Logs;

    /// <summary>
    /// スクリプトクラスです。
    /// </summary>
    public sealed class Script
    {
        private readonly Logger logger = new Logger();
        private bool result = false;
        private object returnValue = null;
        private RunScriptOperationDelegate runScriptOperation = null;
        private EchoInfoDelegate echoInfo = null;
        private EchoErrorDelegate echoError = null;

        internal delegate bool RunScriptOperationDelegate(out object returnValue, string factoryKey, params object[] args);

        internal delegate void EchoInfoDelegate(string message);

        internal delegate void EchoErrorDelegate(string message);

        /// <summary>
        /// スクリプトの処理結果を取得します。
        /// </summary>
        public bool Result
        {
            get
            {
                return this.result;
            }
        }

        /// <summary>
        /// スクリプトの処理後の返り値を取得します。
        /// </summary>
        public object ReturnValue
        {
            get
            {
                return this.returnValue;
            }
        }

        internal Logger Logger
        {
            get
            {
                return this.logger;
            }
        }

        /// <summary>
        /// スクリプトを実行します。
        /// </summary>
        /// <param name="factoryKey">スクリプトのキーです。</param>
        /// <param name="args">引数です。</param>
        public void Execute(string factoryKey, params object[] args)
        {
            Contract.Assume(this.runScriptOperation != null);

            try
            {
                this.result &= this.runScriptOperation(out this.returnValue, factoryKey, args);
            }
            catch (System.Exception ex)
            {
                this.logger.WriteError(ex.Message);
                this.logger.WriteError(ex.StackTrace);
            }
        }

        /// <summary>
        /// 情報をエコーします。
        /// </summary>
        /// <param name="message">エコーするメッセージです。</param>
        public void EchoInfo(string message)
        {
            Contract.Assume(this.echoInfo != null);
            this.echoInfo(message);
        }

        /// <summary>
        /// エラーをエコーします。
        /// </summary>
        /// <param name="message">エコーするメッセージです。</param>
        public void EchoError(string message)
        {
            Contract.Assume(this.echoError != null);
            this.echoError(message);
        }

        internal void SetRunScriptOperationDelegate(RunScriptOperationDelegate runScriptManipulation)
        {
            Contract.Requires(runScriptManipulation != null);
            this.runScriptOperation = runScriptManipulation;
        }

        internal void SetEchoInfoDelegate(EchoInfoDelegate echoInfo)
        {
            Contract.Requires(echoInfo != null);
            this.echoInfo = echoInfo;
        }

        internal void SetEchoErrorDelegate(EchoErrorDelegate echoError)
        {
            Contract.Requires(echoError != null);
            this.echoError = echoError;
        }
    }
}
