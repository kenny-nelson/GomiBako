//-----------------------------------------------------------------------
// <copyright file="ScriptEditorToolFactory.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Plugins.Applications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using Dodai.Modules.ScriptEditor;
    using Dodai.ViewModels;

    /// <summary>
    /// スクリプトエディタを追加するためのツールファクトリクラスです。
    /// </summary>
    public sealed class ScriptEditorToolFactory : PluginToolFactory
    {
        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public ScriptEditorToolFactory()
            : base("ScriptEditor", "ScriptEditor")
        {
        }

        /// <summary>
        /// ビューを作成します。
        /// </summary>
        /// <returns>ビュークラスのインスタンスを返します。</returns>
        public override Control CreateView()
        {
            return new ScriptEditorView();
        }

        /// <summary>
        /// ツールビューモデルを作成します。
        /// </summary>
        /// <returns>ツールビューモデルのインスタンスを返します。</returns>
        public override ToolViewModel CreateViewModel()
        {
            var viewModel = new ScriptEditorViewModel();
            viewModel.Title = "ScriptEditor";
            return viewModel;
        }
    }
}
