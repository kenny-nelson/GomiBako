//-----------------------------------------------------------------------
// <copyright file="DocumentViewModel.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Dodai.Commands;
    using Dodai.Services;

    /// <summary>
    /// ドキュメントのビューモデルクラスです。
    /// </summary>
    public abstract class DocumentViewModel : PanelViewModel, IDocumentable
    {
        private readonly ICommand closeCommand;
        private bool isForceClose = false;        

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        protected DocumentViewModel()
        {
            this.closeCommand =
                new ViewReceiverCommand<object>(param => this.Close(), param => { return true; });
        }

        internal event EventHandler<ActiveChangedEventArgs> ActiveChanged;

        internal event EventHandler<DocumentClosedEventArgs> DocumentClosed;
        
        /// <summary>
        /// クローズコマンドを取得します。
        /// </summary>
        public ICommand CloseCommand
        {
            get
            {
                return this.closeCommand;
            }
        }

        /// <summary>
        /// 強制クローズをするか状態を設定します。
        /// </summary>
        internal bool IsForceClose
        {
            set
            {
                this.isForceClose = value;
            }
        }

        /// <summary>
        /// ドキュメントをクローズします。
        /// </summary>
        public void Close()
        {
            if (this.isForceClose || this.TryClose())
            {
                if (this.DocumentClosed != null)
                {
                    this.DocumentClosed(this, new DocumentClosedEventArgs(this));
                }
            }
        }

        /// <summary>
        /// アクティブ状態変更を通知します。
        /// </summary>
        /// <param name="e">イベント引数です。</param>
        internal override void OnActiveChanged(ActiveChangedEventArgs e)
        {
            if (this.ActiveChanged != null)
            {
                this.ActiveChanged(this, e);
            }
        }

        /// <summary>
        /// クローズ処理を試みます。
        /// </summary>
        /// <returns>クローズ処理が可能な場合は真を返します。</returns>
        protected virtual bool TryClose()
        {
            return true;
        }
    }
}
