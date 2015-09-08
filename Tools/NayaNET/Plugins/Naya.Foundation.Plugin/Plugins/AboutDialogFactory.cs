//-----------------------------------------------------------------------
// <copyright file="AboutDialogFactory.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Naya.Plugins
{    
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using Dodai.Plugins;
    using Dodai.Plugins.Generic;
    using Naya.Modules.About;

    /// <summary>
    /// アバウトダイアログのファクトリクラスです。
    /// </summary>
    public sealed class AboutDialogFactory : PluginDialogFactory<PluginDialogArgs>
    {
        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public AboutDialogFactory()
            : base(typeof(AboutDialog))
        {
        }

        /// <summary>
        /// ダイアログを作成します。
        /// </summary>
        /// <param name="args">ダイアログ作成時に渡す引数です。</param>
        /// <returns>ウィンドウクラスを継承したダイアログのインスタンスを返します。</returns>
        protected override Window CreateDialog(PluginDialogArgs args)
        {
            return new AboutDialog();
        }
    }
}
