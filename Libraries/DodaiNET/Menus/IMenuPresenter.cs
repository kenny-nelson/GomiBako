//-----------------------------------------------------------------------
// <copyright file="IMenuPresenter.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Menus
{
    using System.ComponentModel;
    using Dodai.Services;

    /// <summary>
    /// メニュープレゼンターのインターフェースです。
    /// </summary>
    public interface IMenuPresenter : IPresenter
    {
        /// <summary>
        /// メニューを取得します。
        /// </summary>
        /// <param name="kind">取得するメニューの種類です。</param>
        /// <returns>該当するメニューを返します。</returns>
        ICollectionView GetMenus(MenuKind kind);

        /// <summary>
        /// ビューメニューを取得します。
        /// </summary>
        /// <returns>該当するメニューを返します。</returns>
        ICollectionView GetViewMenus();
    }
}