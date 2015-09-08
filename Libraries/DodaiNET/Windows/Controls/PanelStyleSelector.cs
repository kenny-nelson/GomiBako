//-----------------------------------------------------------------------
// <copyright file="PanelStyleSelector.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Windows.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using Dodai.ViewModels;

    /// <summary>
    /// パネルのスタイルセレクタークラスです。
    /// </summary>
    public sealed class PanelStyleSelector : StyleSelector
    {
        /// <summary>
        /// ツールスタイルを取得設定します。
        /// </summary>
        public Style ToolStyle
        {
            get;
            set;
        }

        /// <summary>
        /// ドキュメントスタイルを取得設定します。
        /// </summary>
        public Style DocumentStyle
        {
            get;
            set;
        }

        /// <summary>
        /// スタイルを返します。
        /// </summary>
        /// <param name="item">コンテンツです。</param>
        /// <param name="container">スタイルの適用対象の要素です。</param>
        /// <returns>該当するスタイルを返します。それ以外は、nullを返します。</returns>
        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is ToolViewModel)
            {
                return this.ToolStyle;
            }

            if (item is DocumentViewModel)
            {
                return this.DocumentStyle;
            }

            return base.SelectStyle(item, container);
        }
    }
}
