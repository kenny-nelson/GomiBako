//-----------------------------------------------------------------------
// <copyright file="OperationFactoryCache.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Plugins
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Dodai.ComponentModel;

    /// <summary>
    /// オペレーションファクトリキャッシュクラスです。
    /// </summary>
    internal sealed class OperationFactoryCache : SynchronizedObject
    {
        private readonly Dictionary<string, PluginOperationFactory> cache =
            new Dictionary<string, PluginOperationFactory>();

        /// <summary>
        /// ファクトリを取得します。
        /// </summary>
        /// <returns>オペレーションファクトリを返します。</returns>
        public IEnumerable<PluginOperationFactory> GetFactories()
        {
            lock (this.SyncObj)
            {
                return this.cache.Values;
            }
        }

        /// <summary>
        /// 指定ファクトリキーのファクトリを取得します。
        /// </summary>
        /// <param name="factoryKey">ファクトリキーです。</param>
        /// <returns>オペレーションファクトリのインスタンスを返します。</returns>
        public PluginOperationFactory GetFactory(string factoryKey)
        {
            lock (this.SyncObj)
            {
                return this.cache[factoryKey];
            }
        }

        /// <summary>
        /// ファクトリを追加します。
        /// </summary>
        /// <param name="factoryKey">追加するファクトリキーです。</param>
        /// <param name="factory">追加するファクトリです。</param>
        public void AddFactory(string factoryKey, PluginOperationFactory factory)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(factoryKey));
            Contract.Requires(factory != null);

            lock (this.SyncObj)
            {
                this.cache.Add(factoryKey, factory);
            }
        }

        /// <summary>
        /// ファクトリを削除します。
        /// </summary>
        /// <param name="factoryKey">削除するファクトリのファクトリキーです。</param>
        public void RemoveFactory(string factoryKey)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(factoryKey));

            lock (this.SyncObj)
            {
                this.cache.Remove(factoryKey);
            }
        }
    }
}
