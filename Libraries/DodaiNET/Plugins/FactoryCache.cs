//-----------------------------------------------------------------------
// <copyright file="FactoryCache.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Plugins
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Dodai.ComponentModel;

    /// <summary>
    /// ファクトリキャッシュクラスです。
    /// </summary>
    /// <typeparam name="TValue">キャッシュ対象の型です。</typeparam>
    internal class FactoryCache<TValue> : SynchronizedObject where TValue : class
    {
        private readonly Dictionary<string, Dictionary<string, TValue>> cache =
            new Dictionary<string, Dictionary<string, TValue>>();

        /// <summary>
        /// ユニットキーを取得します。
        /// </summary>
        /// <returns>ユニットキーを返します。</returns>
        public IEnumerable<string> GetUnitKeys()
        {
            return this.cache.Keys;
        }

        /// <summary>
        /// 指定ユニットキーのファクトリキーを取得します。
        /// </summary>
        /// <param name="unitKey">取得対象のユニットキーです。</param>
        /// <returns>ファクトリキーを返します。</returns>
        public IEnumerable<string> GetFactoryKeys(string unitKey)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(unitKey));

            Dictionary<string, TValue> value = null;
            if (this.cache.TryGetValue(unitKey, out value))
            {
                return value.Keys;
            }

            return null;
        }

        /// <summary>
        /// 指定ユニットキーの中のファクトリキーの値を取得します。
        /// </summary>
        /// <param name="unitKey">ユニットキーです。</param>
        /// <param name="factoryKey">ファクトリキーです。</param>
        /// <returns>指定キーの値を返します。</returns>
        public TValue GetValue(string unitKey, string factoryKey)
        {
            Dictionary<string, TValue> factoryValue = null;
            if (this.cache.TryGetValue(unitKey, out factoryValue))
            {
                return factoryValue[factoryKey];
            }

            return null;
        }

        /// <summary>
        /// 値を追加します。
        /// </summary>
        /// <param name="unitKey">追加対象のユニットキーです。</param>
        /// <param name="factoryKey">追加対象のファクトリキーです。</param>
        /// <param name="value">追加する値です。</param>
        public void AddValue(string unitKey, string factoryKey, TValue value)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(unitKey));
            Contract.Requires(!string.IsNullOrWhiteSpace(factoryKey));
            Contract.Requires(value != null);

            lock (this.SyncObj)
            {
                Dictionary<string, TValue> factoryValue = null;
                if (!this.cache.TryGetValue(unitKey, out factoryValue))
                {
                    factoryValue = new Dictionary<string, TValue>();
                    this.cache.Add(unitKey, factoryValue);
                }

                if (!factoryValue.ContainsKey(factoryKey))
                {
                    factoryValue.Add(factoryKey, value);
                }

                this.AddValueInternal(value);
            }
        }

        /// <summary>
        /// 値を削除します。
        /// </summary>
        /// <param name="unitKey">追加対象のユニットキーです。</param>
        /// <param name="factoryKey">追加対象のファクトリキーです。</param>
        public void RemoveValue(string unitKey, string factoryKey)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(unitKey));
            Contract.Requires(!string.IsNullOrWhiteSpace(factoryKey));

            Dictionary<string, TValue> factoryValue = null;
            if (this.cache.TryGetValue(unitKey, out factoryValue))
            {
                factoryValue.Remove(factoryKey);
            }
        }

        /// <summary>
        /// 値を追加します。内部処理
        /// </summary>
        /// <param name="value">追加する値です。</param>
        protected virtual void AddValueInternal(TValue value)
        {
        }
    }
}
