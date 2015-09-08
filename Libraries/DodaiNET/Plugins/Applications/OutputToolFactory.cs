//-----------------------------------------------------------------------
// <copyright file="OutputToolFactory.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Plugins.Applications
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using Dodai;
    using Dodai.Logs;
    using Dodai.Modules.Output;
    using Dodai.ViewModels;

    /// <summary>
    /// アウトプットツールを追加するためのツールファクトリクラスです。
    /// </summary>
    public sealed class OutputToolFactory : PluginToolFactory
    {
        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public OutputToolFactory()
            : base("Output", "Output")
        {
        }

        /// <summary>
        /// ビューを作成します。
        /// </summary>
        /// <returns>ビュークラスのインスタンスを返します。</returns>
        public override Control CreateView()
        {
            return new OutputView();
        }

        /// <summary>
        /// ツールビューモデルを作成します。
        /// </summary>
        /// <returns>ツールビューモデルのインスタンスを返します。</returns>
        public override ToolViewModel CreateViewModel()
        {
            var logPresenter = GlobalPresenter.GetLogPresenter() as LogPresenter;
            Contract.Assume(logPresenter != null);

            var factory = logPresenter.Factory as OutputLogFactory;
            Contract.Assume(factory != null);

            var viewModel = new OutputViewModel();
            factory.SetViewModel(viewModel);
            viewModel.Title = "Output";
            return viewModel;
        }
    }
}
