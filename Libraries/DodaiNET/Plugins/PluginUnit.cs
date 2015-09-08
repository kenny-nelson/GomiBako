//-----------------------------------------------------------------------
// <copyright file="PluginUnit.cs" company="none">
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

    /// <summary>
    /// プラグイン読み込みの一単位のクラスです。
    /// </summary>
    public abstract class PluginUnit
    {
        private readonly string unitKey;

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        /// <param name="unitKey">ユニットキーです。</param>
        protected PluginUnit(string unitKey)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(unitKey));
            this.unitKey = unitKey;
        }

        /// <summary>
        /// ユニットキーを取得します。
        /// </summary>
        internal string UnitKey
        {
            get
            {
                return this.unitKey;
            }
        }

        /// <summary>
        /// メニューファクトリクラスを取得します。
        /// </summary>
        /// <returns>メニューファクトリのインスタンスを返します。</returns>
        public virtual IEnumerable<PluginMenuFactory> GetMenuFactories()
        {
            yield break;
        }

        /// <summary>
        /// ドキュメントファクトリクラスを取得します。
        /// </summary>
        /// <returns>ドキュメントファクトリのインスタンスを返します。</returns>
        public virtual IEnumerable<PluginDocumentFactory> GetDocumentFactories()
        {
            yield break;
        }

        /// <summary>
        /// ツールファクトリクラスを取得します。
        /// </summary>
        /// <returns>ツールファクトリのインスタンスを返します。</returns>
        public virtual IEnumerable<PluginToolFactory> GetToolFactories()
        {
            yield break;
        }

        /// <summary>
        /// オペレーションファクトリを取得します。
        /// </summary>
        /// <returns>オペレーションファクトリのインスタンスを返します。</returns>
        public virtual IEnumerable<PluginOperationFactory> GetOperationFactories()
        {
            yield break;
        }

        /// <summary>
        /// ダイアログファクトリを取得します。
        /// </summary>
        /// <returns>ダイアログファクトリのインスタンスを返します。</returns>
        public virtual IEnumerable<PluginDialogFactory> GetDialogFactories()
        {
            yield break;
        }

        /// <summary>
        /// 登録開始時に実行される処理です。
        /// </summary>
        public virtual void BeginRegistration()
        {
        }

        /// <summary>
        /// 登録終了時に実行される処理です。
        /// </summary>
        public virtual void EndRegistration()
        {
        }
    }
}
