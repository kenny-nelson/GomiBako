//-----------------------------------------------------------------------
// <copyright file="PluginDialogFactory.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Plugins.Generic
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    /// <summary>
    /// プラグインダイアログファクトリクラスです。
    /// </summary>
    /// <typeparam name="TArgs">プラグインダイアログ引数を継承した型です。</typeparam>
    public abstract class PluginDialogFactory<TArgs> : PluginDialogFactory where TArgs : PluginDialogArgs
    {
        /// <summary>
        /// コンストラクタです。
        /// </summary>
        /// <param name="dialogType">ダイアログタイプです。</param>
        protected PluginDialogFactory(Type dialogType)
            : base(dialogType, true)
        {
        }

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        /// <param name="dialogType">ダイアログタイプです。</param>
        /// <param name="isModal">真の設定の場合、モーダルダイアログとして使用します。</param>
        protected PluginDialogFactory(Type dialogType, bool isModal)
            : base(dialogType, isModal)
        {
        }

        /// <summary>
        /// ダイアログを表示します。
        /// </summary>
        /// <returns>表示処理に問題なければ、真を返します。</returns>
        internal override bool Show()
        {
            return this.Show(null);
        }

        /// <summary>
        /// ダイアログを表示します。
        /// </summary>
        /// <param name="args">ダイアログ表示時に渡す引数です。</param>
        /// <returns>表示処理に問題がなければ、真を返します。</returns>
        internal bool Show(TArgs args)
        {
            var dialog = this.CreateDialog(args);
            Contract.Assume(dialog != null);

            dialog.Owner = this.Owner;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            if (this.IsModal)
            {
                bool? result = dialog.ShowDialog();
                if (result.HasValue)
                {
                    return (bool)result;
                }
            }
            else
            {
                dialog.Show();
            }

            return true;
        }

        /// <summary>
        /// ダイアログを作成します。
        /// </summary>
        /// <param name="args">ダイアログ作成時に渡す引数です。</param>
        /// <returns>ウィンドウクラスを継承したダイアログのインスタンスを返します。</returns>
        protected abstract Window CreateDialog(TArgs args);
    }
}