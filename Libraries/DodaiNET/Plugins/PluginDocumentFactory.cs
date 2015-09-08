//-----------------------------------------------------------------------
// <copyright file="PluginDocumentFactory.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Plugins
{
    using System.Windows.Controls;
    using Dodai.ViewModels;

    /// <summary>
    /// プラグインドキュメントファクトリクラスです。
    /// </summary>
    public abstract class PluginDocumentFactory : PluginFactory
    {
        /// <summary>
        /// コンストラクタです。
        /// </summary>
        /// <param name="factoryKey">ファクトリキーです。</param>
        protected PluginDocumentFactory(string factoryKey)
            : base(factoryKey)
        {
        }

        /// <summary>
        /// ビューを作成します。
        /// </summary>
        /// <returns>ビュークラスのインスタンスを返します。</returns>
        public abstract Control CreateView();

        /// <summary>
        /// ビューモデルを作成します。
        /// </summary>
        /// <returns>ドキュメントビューモデルのインスタンスを返します。</returns>
        public abstract DocumentViewModel CreateViewModel();

        /// <summary>
        /// ドキュメントを作成します。
        /// </summary>
        /// <returns>ビューとビューモデルを関連付けたビューモデルのインスタンスを返します。</returns>
        internal DocumentViewModel CreateDocument()
        {
            var view = this.CreateView();
            var viewModel = this.CreateViewModel();
            viewModel.PanelView = view;
            return viewModel;
        }
    }
}
