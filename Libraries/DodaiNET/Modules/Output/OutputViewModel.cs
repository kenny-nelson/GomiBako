//-----------------------------------------------------------------------
// <copyright file="OutputViewModel.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Modules.Output
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Data;
    using System.Windows.Input;
    using Dodai.Commands;
    using Dodai.Services;
    using Dodai.ViewModels;

    /// <summary>
    /// アウトプットビューのビューモデルクラスです。
    /// </summary>
    public class OutputViewModel : ToolViewModel
    {
        private readonly ObservableCollection<string> messages =
            new ObservableCollection<string>();

        private readonly ICollectionView messagesView;

        private readonly ICommand clearCommand;

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public OutputViewModel()
        {
            this.messagesView = CollectionViewSource.GetDefaultView(this.messages);

            this.clearCommand =
                new ViewReceiverCommand<object>(this.ExecuteClear, this.ExecuteCanClear);

            BindingOperations.EnableCollectionSynchronization(this.messages, this.SyncObj);
        }

        /// <summary>
        /// パネル配置を取得します。
        /// </summary>
        public override PanelLocation Location
        {
            get
            {
                return PanelLocation.Bottom;
            }
        }

        /// <summary>
        /// メッセージを取得します。
        /// </summary>
        public ICollectionView Messages
        {
            get
            {
                return this.messagesView;
            }
        }

        /// <summary>
        /// クリアコマンドを取得します。
        /// </summary>
        public ICommand ClearCommand
        {
            get
            {
                return this.clearCommand;
            }
        }        

        internal void AddMessageLine(string message)
        {
            this.messages.Add(message);
        }

        /// <summary>
        /// 廃棄します。内部処理
        /// </summary>
        protected override void DisposeInternal()
        {
            BindingOperations.DisableCollectionSynchronization(this.messages);
        }

        private void ExecuteClear(object parameter)
        {
            this.messages.Clear();
        }

        private bool ExecuteCanClear(object parameter)
        {
            return true;
        }
    }
}
