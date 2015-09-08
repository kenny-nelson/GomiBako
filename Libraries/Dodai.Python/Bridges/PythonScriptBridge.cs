//-----------------------------------------------------------------------
// <copyright file="PythonScriptBridge.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Bridges
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dodai.Bridges;
    using Microsoft.Scripting.Hosting;

    /// <summary>
    /// Pythonスクリプトと処理をつなぐクラスです。
    /// </summary>
    public sealed class PythonScriptBridge : ScriptBridge
    {
        private readonly ScriptEngine engine;
        private readonly ScriptScope scope;

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        /// <param name="bridgeVariableName">スクリプト処理時のブリッジするための変数名です。</param>
        public PythonScriptBridge(string bridgeVariableName)
            : base(bridgeVariableName)
        {
            this.engine = IronPython.Hosting.Python.CreateEngine();
            Contract.Assume(this.engine != null);

            this.scope = this.engine.CreateScope();
            Contract.Assume(this.scope != null);
        }

        /// <summary>
        /// スクリプトを実行します。
        /// </summary>
        /// <param name="expression">スクリプトの式文字列です。</param>
        /// <returns>処理が成功した場合は、真を返します。</returns>
        public override bool Execute(string expression)
        {
            Contract.Assume(expression != null);

            this.scope.SetVariable(this.BridgeVariableName, this.Script);

            ScriptSource source = this.engine.CreateScriptSourceFromString(expression);

            if (source != null)
            {
                source.Execute(this.scope);
                return this.Script.Result;
            }

            return false;
        }

        /// <summary>
        /// スクリプトをファイルから実行します。
        /// </summary>
        /// <param name="fileName">実行するスクリプトファイルです。</param>
        /// <returns>処理が成功した場合は、真を返します。</returns>
        public override bool ExecuteFromFile(string fileName)
        {
            Contract.Assume(!string.IsNullOrEmpty(fileName));

            if (!File.Exists(fileName))
            {
                return false;
            }

            this.scope.SetVariable(this.BridgeVariableName, this.Script);

            ScriptSource source = this.engine.CreateScriptSourceFromFile(fileName, Encoding.UTF8);
            if (source != null)
            {
                source.Execute(this.scope);
                return this.Script.Result;
            }

            return false;
        }

        /// <summary>
        /// スクリプト実行時のメッセージを作成します。
        /// </summary>
        /// <param name="scriptKey">スクリプトキーです。</param>
        /// <param name="args">スクリプトの引数です。</param>
        /// <returns>作成したメッセージを返します。</returns>
        public override string CreateMessage(string scriptKey, params object[] args)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0}.Execute(\"{1}\"", this.BridgeVariableName, scriptKey);

            if (args != null)
            {
                StringBuilder argsBuilder = new StringBuilder();
                foreach (var arg in args)
                {
                    argsBuilder.AppendFormat(", {0}", arg.ToString());
                }

                builder.AppendFormat("{0}", argsBuilder.ToString());
            }

            builder.Append(")");

            return builder.ToString();
        }
    }
}
