//-----------------------------------------------------------------------
// <copyright file="Repository.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Repositories
{
    using System;
    using System.Diagnostics.Contracts;
    using Dodai.Helpers;
    using Dodai.Services;

    /// <summary>
    /// リポジトリの抽象クラスです。
    /// </summary>
    public abstract class Repository
    {
        private EventHandler<RepositoryChangedEventArgs> repositoryChanged;

        internal event EventHandler<RepositoryChangedEventArgs> RepositoryChanged
        {
            add
            {
                this.repositoryChanged += value.MakeWeakEventHandler(eventHandler => this.repositoryChanged -= eventHandler);
            }

            remove
            {
            }
        }

        /// <summary>
        /// 状態をクリアします。
        /// </summary>
        public abstract void Clear();

        internal void CallActiveDocumentChanged(object sender, ActiveDocumentChangedEventArgs e)
        {
            Contract.Assume(e != null);

            this.OnActiveDocumentChanged(e);
        }

        /// <summary>
        /// リポジトリ変更を通知します。
        /// </summary>
        /// <param name="changeObject">変更オブジェクトです。</param>
        protected void RaiseRepositoryChanged(object changeObject)
        {
            if (this.repositoryChanged != null)
            {
                this.repositoryChanged(this, new RepositoryChangedEventArgs(this, changeObject));
            }
        }

        /// <summary>
        /// ドキュメントのアクティブ状態変更を通知します。
        /// </summary>
        /// <param name="e">イベント引数です。</param>
        protected virtual void OnActiveDocumentChanged(ActiveDocumentChangedEventArgs e)
        {
        }
    }
}
