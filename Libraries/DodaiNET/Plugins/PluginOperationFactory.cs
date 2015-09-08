//-----------------------------------------------------------------------
// <copyright file="PluginOperationFactory.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Plugins
{    
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dodai.Operations;

    /// <summary>
    /// プラグインオペレーションファクトリクラスです。
    /// </summary>
    public abstract class PluginOperationFactory : PluginFactory
    {        
        private CreateMessageDelegate createMessage = null;

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        /// <param name="factoryKey">ファクトリキーです。</param>
        protected PluginOperationFactory(string factoryKey)
            : base(factoryKey)
        {
        }

        internal delegate string CreateMessageDelegate(string factoryKey, params object[] args);

        /// <summary>
        /// スクリプトオペレーションが作成できるか判定します。
        /// </summary>
        /// <param name="args">スクリプトオペレーションの引数です。</param>
        /// <returns>作成可能な場合は、真を返します。</returns>
        public bool CanCreateScriptOperation(params object[] args)
        {
            return this.CanCreateOperation(args);
        }

        /// <summary>
        /// スクリプトオペレーションを作成します。
        /// </summary>
        /// <param name="args">スクリプトオペレーションの引数です。</param>
        /// <returns>オペレーションのインスタンスを返します。</returns>
        public Operation CreateScriptOperation(params object[] args)
        {
            var manipulation = this.CreateOperation(args);
            manipulation.IsScriptable = true;

            Contract.Assume(this.createMessage != null);
            manipulation.ScriptMessage = this.createMessage(this.FactoryKey, args);

            return manipulation;
        }

        /// <summary>
        /// 作成時のメッセージデリゲートを設定します。
        /// </summary>
        /// <param name="createMessage">設定するデリゲートです。</param>
        internal void SetCreateMessageDelegate(CreateMessageDelegate createMessage)
        {
            Contract.Requires(createMessage != null);

            this.createMessage = createMessage;
        }

        /// <summary>
        /// オペレーションが作成できるか判定します。
        /// </summary>
        /// <param name="args">オペレーションの引数です。</param>
        /// <returns>作成可能な場合は、真を返します。</returns>
        protected virtual bool CanCreateOperation(params object[] args)
        {
            return true;
        }

        /// <summary>
        /// オペレーションを作成します。
        /// </summary>
        /// <param name="args">オペレーションの引数です。</param>
        /// <returns>オペレーションのインスタンスを返します。</returns>
        protected abstract Operation CreateOperation(params object[] args);
    }
}
