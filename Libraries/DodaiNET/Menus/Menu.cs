//-----------------------------------------------------------------------
// <copyright file="Menu.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Menus
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Windows.Data;
    using System.Windows.Input;
    using Dodai.Commands;
    using Dodai.ComponentModel;

    /// <summary>
    /// メニューの種類です。
    /// </summary>
    public enum MenuKind
    {
        /// <summary>
        /// 新規ファイル
        /// </summary>
        FileNew,

        /// <summary>
        /// ファイルオープン
        /// </summary>
        FileOpen,

        /// <summary>
        /// ファイル保存
        /// </summary>
        FileSave,

        /// <summary>
        /// ツール
        /// </summary>
        Tool,

        /// <summary>
        /// 分類なし
        /// </summary>
        None
    }

    /// <summary>
    /// メニュークラスです。
    /// </summary>
    public class Menu : DisposableNotifiableObject
    {        
        private readonly object syncRoot = new object();
        private readonly ICommand command;
        private readonly ObservableCollection<Menu> subMenus = new ObservableCollection<Menu>();
        private readonly ICollectionView subMenusView;
        private string label = string.Empty;

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        /// <param name="execute">メニュー選択時に実行するコールバックです。</param>
        /// <param name="canExecute">メニュー選択時に実行可能か確認するコールバックです。</param>
        public Menu(Action<object> execute, Func<object, bool> canExecute)
        {
            Contract.Requires(execute != null);
            Contract.Requires(canExecute != null);

            this.command = new ViewReceiverCommand<object>(execute, canExecute);
            BindingOperations.EnableCollectionSynchronization(this.subMenus, this.syncRoot);
            this.subMenusView = CollectionViewSource.GetDefaultView(this.subMenus);
        }

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public Menu()
        {
            this.command = new ViewReceiverCommand<object>(param => this.Execute(param), param => this.CanExecute(param));
            BindingOperations.EnableCollectionSynchronization(this.subMenus, this.syncRoot);
            this.subMenusView = CollectionViewSource.GetDefaultView(this.subMenus);
        }

        /// <summary>
        /// メニューの種類を取得します。
        /// </summary>
        public virtual MenuKind Kind
        {
            get
            {
                return MenuKind.None;
            }
        }

        /// <summary>
        /// メニューのラベルを取得設定します。
        /// </summary>
        public string Label
        {
            get
            {
                return this.label;
            }

            set
            {
                this.SetProperty(ref this.label, value);
            }
        }

        /// <summary>
        /// サブメニューを取得します。
        /// </summary>
        public ICollectionView SubMenus
        {
            get
            {
                return this.subMenusView;
            }
        }

        /// <summary>
        /// コマンドを取得します。
        /// </summary>
        public ICommand Command
        {
            get
            {
                return this.command;
            }
        }

        /// <summary>
        /// 廃棄します。内部処理
        /// </summary>
        protected override void DisposeInternal()
        {
            BindingOperations.DisableCollectionSynchronization(this.subMenus);
            this.subMenus.Clear();
        }

        /// <summary>
        /// 実行します。内部処理　継承先で内部処理を記述します。
        /// </summary>
        /// <param name="parameter">実行時に渡されるパラメータです。</param>
        protected virtual void ExecuteInternal(object parameter)
        {
        }

        /// <summary>
        /// 実行可能か判定します。内部処理　継承先で内部処理を記述します。
        /// </summary>
        /// <param name="parameter">実行時に渡されるパラメータです。</param>
        /// <returns>実行可能ならば真を返します。</returns>
        protected virtual bool CanExecuteInternal(object parameter)
        {
            return true;
        }

        private void Execute(object parameter)
        {
            this.ExecuteInternal(parameter);
        }

        private bool CanExecute(object parameter)
        {
            return this.CanExecuteInternal(parameter);
        }
    }
}
