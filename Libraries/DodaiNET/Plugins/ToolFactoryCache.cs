//-----------------------------------------------------------------------
// <copyright file="ToolFactoryCache.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Plugins
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// ツールファクトリキャッシュクラスです。
    /// </summary>
    internal sealed class ToolFactoryCache : FactoryCache<PluginToolFactory>
    {
        private readonly Dictionary<string, PluginToolFactory> fullNameToFactory =
            new Dictionary<string, PluginToolFactory>();

        /// <summary>
        /// ファクトリを取得します。
        /// </summary>
        /// <param name="factoryFullName">ファクトリ名です。</param>
        /// <returns>ツールファクトリのインスタンスを返します。</returns>
        public PluginToolFactory GetFactory(string factoryFullName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(factoryFullName));

            return this.fullNameToFactory[factoryFullName];
        }

        /// <summary>
        /// 値を追加します。内部処理
        /// </summary>
        /// <param name="value">追加する値です。</param>
        protected override void AddValueInternal(PluginToolFactory value)
        {
            this.fullNameToFactory.Add(value.GetType().FullName, value);
        }
    }
}
