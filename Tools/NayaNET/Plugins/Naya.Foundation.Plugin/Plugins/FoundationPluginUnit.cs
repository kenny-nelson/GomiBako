//-----------------------------------------------------------------------
// <copyright file="FoundationPluginUnit.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Naya.Plugins
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dodai;
    using Dodai.Plugins;

    /// <summary>
    /// Foundation 部分のプラグインユニットクラスです。
    /// </summary>
    [Export(typeof(PluginUnit))]
    public class FoundationPluginUnit : PluginUnit
    {
        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public FoundationPluginUnit()
            : base("Foundation")
        {
        }

        /// <summary>
        /// ダイアログファクトリを取得します。
        /// </summary>
        /// <returns>ダイアログファクトリのインスタンスを返します。</returns>
        public override IEnumerable<PluginDialogFactory> GetDialogFactories()
        {
            yield return new AboutDialogFactory();
        }
    }
}
