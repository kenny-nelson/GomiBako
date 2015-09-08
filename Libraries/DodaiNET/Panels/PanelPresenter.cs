//-----------------------------------------------------------------------
// <copyright file="PanelPresenter.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Panels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Windows.Data;
    using Dodai.Services;
    using Dodai.ViewModels;

    /// <summary>
    /// パネルプレゼンタークラスです。
    /// </summary>
    public class PanelPresenter : Presenter, IPanelPresenter
    {
        private readonly ObservableCollection<ToolViewModel> tools =
            new ObservableCollection<ToolViewModel>();

        private readonly ObservableCollection<DocumentViewModel> documents =
            new ObservableCollection<DocumentViewModel>();

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public PanelPresenter()
            : base(Enum.GetName(typeof(PresenterKind), PresenterKind.Panel))
        {
            BindingOperations.EnableCollectionSynchronization(this.tools, this.SyncObj);
            BindingOperations.EnableCollectionSynchronization(this.documents, this.SyncObj);
        }

        /// <summary>
        /// ツールを取得します。
        /// </summary>
        /// <returns>対象のインスタンスを返します。</returns>
        public ICollectionView GetTools()
        {
            return CollectionViewSource.GetDefaultView(this.tools);
        }

        /// <summary>
        /// ドキュメントを取得します。
        /// </summary>
        /// <returns>対象のインスタンスを返します。</returns>
        public ICollectionView GetDocuments()
        {
            return CollectionViewSource.GetDefaultView(this.documents);
        }

        /// <summary>
        /// ツールを追加します。
        /// </summary>
        /// <param name="tool">追加するツールです。</param>
        public void AddTool(ToolViewModel tool)
        {
            Contract.Assume(tool != null);
            if (!this.tools.Contains(tool))
            {
                this.tools.Add(tool);
            }
        }

        /// <summary>
        /// ドキュメントを追加します。
        /// </summary>
        /// <param name="document">追加するドキュメントです。</param>
        public void AddDocument(DocumentViewModel document)
        {
            Contract.Assume(document != null);
            if (!this.documents.Contains(document))
            {
                this.documents.Add(document);
            }
        }

        /// <summary>
        /// ツールを削除します。
        /// </summary>
        /// <param name="tool">削除するツールです。</param>
        public void RemoveTool(ToolViewModel tool)
        {
            Contract.Assume(tool != null);
            if (this.tools.Contains(tool))
            {
                this.tools.Remove(tool);
            }
        }

        /// <summary>
        /// ドキュメントを削除します。
        /// </summary>
        /// <param name="document">追加するドキュメントです。</param>
        public void RemoveDocument(DocumentViewModel document)
        {
            Contract.Assume(document != null);
            if (this.documents.Contains(document))
            {
                this.documents.Remove(document);
            }
        }

        /// <summary>
        /// 指定のツールが含まれているか判定します。
        /// </summary>
        /// <param name="tool">判定対象のツールです。</param>
        /// <returns>含まれていれば、真を返します。</returns>
        public bool ContainTool(ToolViewModel tool)
        {
            Contract.Assume(tool != null);
            return this.tools.Contains(tool);
        }

        /// <summary>
        /// 指定のドキュメントが含まれているか判定します。
        /// </summary>
        /// <param name="document">判定対象のドキュメントです。</param>
        /// <returns>含まれていれば、真を返します。</returns>
        public bool ContainDocument(DocumentViewModel document)
        {
            Contract.Assume(document != null);
            return this.documents.Contains(document);
        }

        /// <summary>
        /// 廃棄します。内部処理
        /// </summary>
        protected override void DisposeInternal()
        {
            this.documents.Clear();
            this.tools.Clear();
            BindingOperations.DisableCollectionSynchronization(this.documents);
            BindingOperations.DisableCollectionSynchronization(this.tools);
        }
    }
}
