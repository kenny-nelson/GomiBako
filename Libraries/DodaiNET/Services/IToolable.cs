//-----------------------------------------------------------------------
// <copyright file="IToolable.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Services
{
    /// <summary>
    /// ツール化可能インターフェースです。
    /// </summary>
    public interface IToolable : IPanelable
    {
        /// <summary>
        /// コンテントIDを取得します。
        /// </summary>
        string ContentId { get; }

        /// <summary>
        /// パネル配置を取得します。
        /// </summary>
        PanelLocation Location { get; }

        /// <summary>
        /// 幅を取得します。
        /// </summary>
        double Width { get; }

        /// <summary>
        /// 高さを取得します。
        /// </summary>
        double Height { get; }

        /// <summary>
        /// 選択状態かどうか取得設定します。
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        /// 可視状態かどうか取得設定します。
        /// </summary>
        bool IsVisible { get; set; }
    }
}
