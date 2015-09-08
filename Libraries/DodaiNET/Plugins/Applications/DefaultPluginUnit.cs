//-----------------------------------------------------------------------
// <copyright file="DefaultPluginUnit.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Plugins.Applications
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// デフォルトで使用できる処理をまとめたプラグインユニットクラスです。
    /// </summary>
    [Export(typeof(PluginUnit))]
    public sealed class DefaultPluginUnit : PluginUnit
    {
        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public DefaultPluginUnit()
            : base("Default")
        { 
        }

        /// <summary>
        /// オペレーションファクトリを取得します。
        /// </summary>
        /// <returns>オペレーションファクトリのインスタンスを返します。</returns>
        public override IEnumerable<PluginOperationFactory> GetOperationFactories()
        {
            yield return new UndoOperationFactory();
            yield return new RedoOperationFactory();
        }

        /// <summary>
        /// ツールファクトリクラスを取得します。
        /// </summary>
        /// <returns>ツールファクトリのインスタンスを返します。</returns>
        public override IEnumerable<PluginToolFactory> GetToolFactories()
        {
            yield return new OutputToolFactory();
            yield return new ScriptEditorToolFactory();
        }
    }
}
