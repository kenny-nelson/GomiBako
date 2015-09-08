//-----------------------------------------------------------------------
// <copyright file="ScriptBridge.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Bridges
{
    using System.Diagnostics.Contracts;
    using Dodai.Logs;

    /// <summary>
    /// スクリプトと処理をつなぐクラスです。
    /// </summary>
    public abstract class ScriptBridge
    {
        private readonly string bridgeVariableName;
        private Script script = null;

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        /// <param name="bridgeVariableName">スクリプト処理時のブリッジするための変数名です。</param>
        protected ScriptBridge(string bridgeVariableName)
        {
            Contract.Assume(!string.IsNullOrEmpty(bridgeVariableName));
            this.bridgeVariableName = bridgeVariableName;
        }

        /// <summary>
        /// スクリプトを取得します。
        /// </summary>
        protected Script Script
        {
            get
            {
                return this.script;
            }
        }

        /// <summary>
        /// ブリッジ用の変数名を取得します。
        /// </summary>
        protected string BridgeVariableName
        {
            get
            {
                return this.bridgeVariableName;
            }
        }

        /// <summary>
        /// スクリプトを実行します。
        /// </summary>
        /// <param name="expression">スクリプトの式文字列です。</param>
        /// <returns>処理が成功した場合は、真を返します。</returns>
        public abstract bool Execute(string expression);

        /// <summary>
        /// スクリプトをファイルから実行します。
        /// </summary>
        /// <param name="fileName">実行するスクリプトファイルです。</param>
        /// <returns>処理が成功した場合は、真を返します。</returns>
        public abstract bool ExecuteFromFile(string fileName);

        /// <summary>
        /// スクリプト実行時のメッセージを作成します。
        /// </summary>
        /// <param name="scriptKey">スクリプトキーです。</param>
        /// <param name="args">スクリプトの引数です。</param>
        /// <returns>作成したメッセージを返します。</returns>
        public abstract string CreateMessage(string scriptKey, params object[] args);

        internal void SetScript(Script script)
        {
            Contract.Assume(script != null);
            this.script = script;
        }

        /// <summary>
        /// ロガーを取得します。
        /// </summary>
        /// <returns>ロガーのインスタンスを返します。</returns>
        protected Logger GetLogger()
        {
            return this.Script.Logger;
        }        
    }
}
