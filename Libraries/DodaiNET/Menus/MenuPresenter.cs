//-----------------------------------------------------------------------
// <copyright file="MenuPresenter.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Windows.Data;
    using Dodai.Services;

    /// <summary>
    /// メニュープレゼンタークラスです。
    /// </summary>
    public sealed class MenuPresenter : Presenter, IMenuPresenter
    {
        private readonly Dictionary<MenuKind, ObservableCollection<Menu>> kindToMenus =
            new Dictionary<MenuKind, ObservableCollection<Menu>>();

        private readonly ObservableCollection<Menu> viewMenus =
            new ObservableCollection<Menu>();

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public MenuPresenter()
            : base(Enum.GetName(typeof(PresenterKind), PresenterKind.Menu))
        {
            foreach (var key in Enum.GetValues(typeof(MenuKind)))
            {
                var kind = (MenuKind)key;
                ObservableCollection<Menu> value = new ObservableCollection<Menu>();
                this.kindToMenus.Add(kind, value);
                BindingOperations.EnableCollectionSynchronization(value, this.SyncObj);
            }

            BindingOperations.EnableCollectionSynchronization(this.viewMenus, this.SyncObj);
        }

        /// <summary>
        /// メニューを取得します。
        /// </summary>
        /// <param name="kind">取得するメニューの種類です。</param>
        /// <returns>該当するメニューを返します。</returns>
        public ICollectionView GetMenus(MenuKind kind)
        {
            return CollectionViewSource.GetDefaultView(this.kindToMenus[kind]);
        }

        /// <summary>
        /// ビューメニューを取得します。
        /// </summary>
        /// <returns>該当するメニューを返します。</returns>
        public ICollectionView GetViewMenus()
        {
            return CollectionViewSource.GetDefaultView(this.viewMenus);
        }

        /// <summary>
        /// メニューを追加します。
        /// </summary>
        /// <param name="kind">追加するメニューの種類です。</param>
        /// <param name="menu">追加するメニューです。</param>
        public void AddMenu(MenuKind kind, Menu menu)
        {
            Contract.Requires(menu != null);
            this.kindToMenus[kind].Add(menu);
        }

        /// <summary>
        /// ビューメニューを追加します。
        /// </summary>
        /// <param name="menu">追加するメニューです。</param>
        public void AddViewMenu(Menu menu)
        {
            Contract.Requires(menu != null);
            this.viewMenus.Add(menu);
        }

        /// <summary>
        /// 廃棄します。内部処理
        /// </summary>
        protected override void DisposeInternal()
        {
            foreach (var value in this.kindToMenus.Values)
            {
                value.Clear();
                BindingOperations.DisableCollectionSynchronization(value);
            }

            this.viewMenus.Clear();
            BindingOperations.DisableCollectionSynchronization(this.viewMenus);
        }
    }
}
