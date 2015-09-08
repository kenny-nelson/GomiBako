//-----------------------------------------------------------------------
// <copyright file="WorkspaceViewModel.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.ViewModels
{    
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Dodai;
    using Dodai.Menus;
    using Dodai.Panels;

    /// <summary>
    /// ワークスペースビューモデルクラスです。
    /// </summary>
    public class WorkspaceViewModel : ViewModel
    {
        private DocumentViewModel activeDocument = null;
        private ICollectionView fileNewMenus = null;
        private ICollectionView fileOpenMenus = null;
        private ICollectionView fileSaveMenus = null;
        private ICollectionView toolMenus = null;

        private ICollectionView documents = null;
        private ICollectionView tools = null;
        private ICollectionView viewMenus = null;

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public WorkspaceViewModel()
        {
        }

        /// <summary>
        /// アクティブドキュメントを取得設定します。
        /// </summary>
        public DocumentViewModel ActiveDocument
        {
            get
            {
                return this.activeDocument;
            }

            set
            {
                this.SetProperty(ref this.activeDocument, value);
                GlobalPresenter.SetActiveDocument(value);
            }
        }

        /// <summary>
        /// ファイルニューメニューを取得します。
        /// </summary>
        public ICollectionView FileNewMenus
        {
            get
            {
                if (this.fileNewMenus == null)
                {
                    IMenuPresenter presenter = GlobalPresenter.GetMenuPresenter();
                    Contract.Assume(presenter != null);
                    this.fileNewMenus = presenter.GetMenus(MenuKind.FileNew);
                }

                return this.fileNewMenus;
            }
        }

        /// <summary>
        /// ファイルオープンメニューを取得します。
        /// </summary>
        public ICollectionView FileOpenMenus
        {
            get
            {
                if (this.fileOpenMenus == null)
                {
                    IMenuPresenter presenter = GlobalPresenter.GetMenuPresenter();
                    Contract.Assume(presenter != null);
                    this.fileOpenMenus = presenter.GetMenus(MenuKind.FileOpen);
                }

                return this.fileOpenMenus;
            }
        }

        /// <summary>
        /// ファイルセーブメニューを取得します。
        /// </summary>
        public ICollectionView FileSaveMenus
        {
            get
            {
                if (this.fileSaveMenus == null)
                {
                    IMenuPresenter presenter = GlobalPresenter.GetMenuPresenter();
                    Contract.Assume(presenter != null);
                    this.fileSaveMenus = presenter.GetMenus(MenuKind.FileSave);
                }

                return this.fileSaveMenus;
            }
        }

        /// <summary>
        /// ツールメニューを取得します。
        /// </summary>
        public ICollectionView ToolMenus
        {
            get
            {
                if (this.toolMenus == null)
                {
                    IMenuPresenter presenter = GlobalPresenter.GetMenuPresenter();
                    Contract.Assume(presenter != null);
                    this.toolMenus = presenter.GetMenus(MenuKind.Tool);
                }

                return this.toolMenus;
            }
        }

        /// <summary>
        /// ドキュメントを取得します。
        /// </summary>
        public ICollectionView Documents
        {
            get
            {
                if (this.documents == null)
                {
                    IPanelPresenter presenter = GlobalPresenter.GetPanelPresenter();
                    Contract.Assume(presenter != null);
                    this.documents = presenter.GetDocuments();
                }

                return this.documents;
            }
        }

        /// <summary>
        /// ツールを取得します。
        /// </summary>
        public ICollectionView Tools
        {
            get
            {
                if (this.tools == null)
                {
                    IPanelPresenter presenter = GlobalPresenter.GetPanelPresenter();
                    Contract.Assume(presenter != null);
                    this.tools = presenter.GetTools();
                }

                return this.tools;
            }
        }

        /// <summary>
        /// ビューを取得します。
        /// </summary>
        public ICollectionView ViewMenus
        {
            get
            {
                if (this.viewMenus == null)
                {
                    IMenuPresenter presenter = GlobalPresenter.GetMenuPresenter();
                    Contract.Assume(presenter != null);
                    this.viewMenus = presenter.GetViewMenus();
                }

                return this.viewMenus;
            }
        }

        /// <summary>
        /// 廃棄します。内部処理
        /// </summary>
        protected override void DisposeInternal()
        {
        }
    }
}
