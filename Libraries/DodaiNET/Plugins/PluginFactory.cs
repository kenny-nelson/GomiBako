//-----------------------------------------------------------------------
// <copyright file="PluginFactory.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Plugins
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// プラグインファクトリの抽象クラスです。
    /// </summary>
    public abstract class PluginFactory
    {
        private readonly string factoryKey;

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        /// <param name="factoryKey">ファクトリキーです。</param>
        protected PluginFactory(string factoryKey)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(factoryKey));
            this.factoryKey = factoryKey;
        }

        /// <summary>
        /// ファクトリキーを取得します。
        /// </summary>
        internal string FactoryKey
        {
            get
            {
                return this.factoryKey;
            }
        }
    }
}
