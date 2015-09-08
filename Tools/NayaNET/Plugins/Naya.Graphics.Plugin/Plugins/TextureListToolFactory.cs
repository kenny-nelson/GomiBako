//-----------------------------------------------------------------------
// <copyright file="TextureListToolFactory.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Naya.Graphics.Plugins
{    
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using Dodai.Plugins;
    using Dodai.ViewModels;
    using Naya.Graphics.Modules.TextureList;

    /// <summary>
    /// テクスチャリストのツールファクトリクラスです。
    /// </summary>
    public sealed class TextureListToolFactory : PluginToolFactory
    {
        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public TextureListToolFactory()
            : base("TextureList", "TextureList")
        {
        }

        /// <summary>
        /// ビューを作成します。
        /// </summary>
        /// <returns>ビュークラスのインスタンスを返します。</returns>
        public override Control CreateView()
        {
            return new TextureListView();
        }

        /// <summary>
        /// ツールを作成します。
        /// </summary>
        /// <returns>ツールのビューとビューモデルを関連付けてビューモデルのインスタンスを返します。</returns>
        public override ToolViewModel CreateViewModel()
        {
            var viewModel = new TextureListViewModel();
            viewModel.Title = "TextureList";
            return viewModel;
        }
    }
}
