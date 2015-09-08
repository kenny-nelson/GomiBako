//-----------------------------------------------------------------------
// <copyright file="GlobalPresenter.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using Dodai.Bridges;
    using Dodai.Helpers;
    using Dodai.Logs;
    using Dodai.Menus;
    using Dodai.Operations;
    using Dodai.Panels;
    using Dodai.Plugins;
    using Dodai.Plugins.Applications;
    using Dodai.Plugins.Generic;
    using Dodai.Repositories;
    using Dodai.Services;
    using Dodai.ViewModels;

    /// <summary>
    /// 拡張側で処理を行う場合の全体を管理するプレゼンタークラスです。
    /// </summary>
    public class GlobalPresenter
    {
        /// <summary>
        /// オペレーションプレゼンターのキー文字列です。
        /// </summary>
        protected static readonly string OperationPresenterKey;

        /// <summary>
        /// ログプレゼンターのキー文字列です。
        /// </summary>
        protected static readonly string LogPresenterKey;

        /// <summary>
        /// プラグインプレゼンターのキー文字列です。
        /// </summary>
        protected static readonly string PluginPresenterKey;

        /// <summary>
        /// パネルプレゼンターのキー文字列です。
        /// </summary>
        protected static readonly string PanelPresenterKey;

        /// <summary>
        /// メニュープレゼンターのキー文字列です。
        /// </summary>
        protected static readonly string MenuPresenterKey;

        /// <summary>
        /// リポジトリプレゼンターのキー文字列です。
        /// </summary>
        protected static readonly string RepositoryPresenterKey;

        /// <summary>
        /// スクリプトプレゼンターのキー文字列です。
        /// </summary>
        protected static readonly string ScriptPresenterKey;

        private static readonly Global Instance = new Global();

        private static readonly object SyncRoot = new object();
        private static readonly Dictionary<string, IPresenter> Presenters =
            new Dictionary<string, IPresenter>();

        /// <summary>
        /// 静的クラスのコンストラクタです。
        /// </summary>
        static GlobalPresenter()
        {
            OperationPresenterKey = Enum.GetName(typeof(PresenterKind), PresenterKind.Operation);
            LogPresenterKey = Enum.GetName(typeof(PresenterKind), PresenterKind.Log);
            PluginPresenterKey = Enum.GetName(typeof(PresenterKind), PresenterKind.Plugin);
            PanelPresenterKey = Enum.GetName(typeof(PresenterKind), PresenterKind.Panel);
            MenuPresenterKey = Enum.GetName(typeof(PresenterKind), PresenterKind.Menu);
            RepositoryPresenterKey = Enum.GetName(typeof(PresenterKind), PresenterKind.Repository);
            ScriptPresenterKey = Enum.GetName(typeof(PresenterKind), PresenterKind.Script);
        }

        /// <summary>
        /// 初期化します。
        /// </summary>
        /// <returns>タスクを返します。</returns>
        public static async Task Initialize()
        {
            var pluginPresenter = Instance.GetPresenter<PluginPresenter>(PluginPresenterKey);
            Contract.Assume(pluginPresenter != null);

            var menuPresenter = Instance.GetPresenter<MenuPresenter>(MenuPresenterKey);
            Contract.Assume(menuPresenter != null);

            var scriptPresenter = Instance.GetPresenter<ScriptPresenter>(ScriptPresenterKey);
            Contract.Assume(scriptPresenter != null);

            await pluginPresenter.Initialize();

            await Task.Run(() =>
            {
                foreach (var factory in pluginPresenter.GetMenuFactories())
                {
                    foreach (var menu in factory.GetMenus())
                    {
                        menuPresenter.AddMenu(menu.Kind, menu);
                    }
                }

                foreach (var menu in CreateViewMenus())
                {
                    menuPresenter.AddViewMenu(menu);
                }
            });

            await Task.Run(() =>
            {
                foreach (var factory in pluginPresenter.GetOperationFactories())
                {
                    factory.SetCreateMessageDelegate(scriptPresenter.Bridge.CreateMessage);
                }
            });

            await Task.Run(() =>
            {
                scriptPresenter.Script.SetRunScriptOperationDelegate(Instance.CallRunScriptOperation);
                scriptPresenter.Script.SetEchoInfoDelegate(Instance.CallEchoInfo);
                scriptPresenter.Script.SetEchoErrorDelegate(Instance.CallEchoError);
            });

            await Task.Run(() =>
            {
                var repositoryPresenter = Instance.GetPresenter<RepositoryPresenter>(RepositoryPresenterKey);
                Contract.Assume(repositoryPresenter != null);

                foreach (var repository in repositoryPresenter.GetRepositories())
                {
                    Instance.RepositoryActiveDocumentChanged += repository.CallActiveDocumentChanged;
                }
            });

            await Task.Run(() =>
            {
                foreach (var factory in pluginPresenter.GetDialogFactories())
                {
                    factory.Owner = Instance.Owner;
                }
            });
        }

        /// <summary>
        /// オーナーウィンドウを設定します。
        /// </summary>
        /// <param name="owner">オーナーになるウィンドウです。</param>
        public static void SetOwner(Window owner)
        {
            Instance.SetOwner(owner);
        }

        /// <summary>
        /// オペレーションプレゼンターを取得します。
        /// </summary>
        /// <returns>オペレーションプレゼンターのインスタンスを返します。</returns>
        public static IOperationPresenter GetOperationPresenter()
        {
            return Instance.GetPresenter<IOperationPresenter>(OperationPresenterKey);
        }

        /// <summary>
        /// ログプレゼンターを取得します。
        /// </summary>
        /// <returns>ログプレゼンターのインスタンスを返します。</returns>
        public static ILogPresenter GetLogPresenter()
        {
            return Instance.GetPresenter<ILogPresenter>(LogPresenterKey);
        }

        /// <summary>
        /// プラグインプレゼンターを取得します。
        /// </summary>
        /// <returns>プラグインプレゼンターのインスタンスを返します。</returns>
        public static IPluginPresenter GetPluginPresenter()
        {
            return Instance.GetPresenter<IPluginPresenter>(PluginPresenterKey);
        }

        /// <summary>
        /// メニュープレゼンターを取得します。
        /// </summary>
        /// <returns>メニュープレゼンターのインスタンスを返します。</returns>
        public static IMenuPresenter GetMenuPresenter()
        {
            return Instance.GetPresenter<IMenuPresenter>(MenuPresenterKey);
        }

        /// <summary>
        /// リポジトリプレゼンターを取得します。
        /// </summary>
        /// <returns>リポジトリプレゼンターのインスタンスを返します。</returns>
        public static IRepositoryPresenter GetRepositoryPresenter()
        {
            return Instance.GetPresenter<IRepositoryPresenter>(RepositoryPresenterKey);
        }

        /// <summary>
        /// 指定されたユニットの中のファクトリからドキュメントを作成します。
        /// </summary>
        /// <param name="unitKey">ユニットキーです。</param>
        /// <param name="factoryKey">ファクトリキーです。</param>
        /// <returns>ドキュメントのインスタンスを返します。</returns>
        public static IDocumentable CreateDocument(string unitKey, string factoryKey)
        {
            return Instance.CreateDocument(unitKey, factoryKey);
        }

        /// <summary>
        /// ドキュメントを追加します。
        /// </summary>
        /// <param name="document">追加するドキュメントです。</param>
        public static void AddDocument(IDocumentable document)
        {
            Contract.Requires(document is DocumentViewModel);

            Instance.AddDocument((DocumentViewModel)document);
        }

        /// <summary>
        /// 指定ファクトリからスクリプトオペレーションを作成します。
        /// </summary>
        /// <param name="factoryKey">ファクトリキーです。</param>
        /// <param name="args">オペレーション作成時に渡す引数です。</param>
        /// <returns>オペレーションのインスタンスを返します。</returns>
        public static Operation CreateScriptOperation(string factoryKey, params object[] args)
        {
            return Instance.CreateScriptOperation(factoryKey, args);
        }

        /// <summary>
        /// 指定オペレーションを実行します。
        /// </summary>
        /// <param name="operation">実行するオペレーションです。</param>
        public static void RunOperation(Operation operation)
        {
            Instance.RunOperation(operation);
        }

        /// <summary>
        /// ロガーをプッシュします。
        /// </summary>
        /// <param name="logger">プッシュするロガーです。</param>
        public static void PushLogger(Logger logger)
        {
            Instance.PushLogger(logger);
        }

        /// <summary>
        /// ログをフラッシュします。
        /// この処理を実行してためているログをすべて出力します。
        /// </summary>
        public static void FlushLogs()
        {
            Instance.FlushLogs();
        }

        /// <summary>
        /// ロガーをプッシュ後フラッシュします。
        /// </summary>
        /// <param name="logger">プッシュするロガーです。</param>
        public static void PushLoggerAndFlush(Logger logger)
        {
            Instance.PushLoggerAndFlush(logger);
        }

        /// <summary>
        /// デフォルトツールを作成します。
        /// </summary>
        public static void MakeDefaultTools()
        {
            var panelPresenter = Instance.GetPresenter<PanelPresenter>(PanelPresenterKey);
            Contract.Assume(panelPresenter != null);

            ToolViewModel outputViewModel = null;
            {
                var factory = GetToolFactory(typeof(OutputToolFactory).FullName);
                outputViewModel = AddTool(factory, panelPresenter);
            }

            {
                var factory = GetToolFactory(typeof(ScriptEditorToolFactory).FullName);
                AddTool(factory, panelPresenter);
            }

            outputViewModel.IsActive = true;
        }

        /// <summary>
        /// ダイアログを表示します。
        /// </summary>
        /// <typeparam name="TDialog">表示するダイアログの型です。</typeparam>
        /// <returns>表示に問題がない場合は、真を返します。</returns>
        public static bool Show<TDialog>()
                where TDialog : class
        {
            return Instance.Show<TDialog>();
        }

        /// <summary>
        /// ダイアログを表示します。
        /// </summary>
        /// <typeparam name="TDialog">表示するダイアログの型です。</typeparam>
        /// <typeparam name="TArgs">ダイアログの引数の型です。</typeparam>
        /// <param name="args">表示時に渡す引数です。</param>
        /// <returns>表示に問題がない場合は、真を返します。</returns>
        public static bool Show<TDialog, TArgs>(TArgs args)
            where TDialog : class
            where TArgs : PluginDialogArgs
        {
            return Instance.Show<TDialog, TArgs>(args);
        }

        internal static void SetActiveDocument(IDocumentable document)
        {
            Instance.SetActiveDocument(document);
        }

        internal static PanelPresenter GetPanelPresenter()
        {
            return Instance.GetPresenter<PanelPresenter>(PanelPresenterKey);
        }

        internal static ScriptPresenter GetScriptPresenter()
        {
            return Instance.GetPresenter<ScriptPresenter>(ScriptPresenterKey);
        }

        internal static PluginToolFactory GetToolFactory(string factoryFullName)
        {
            return Instance.GetPresenter<PluginPresenter>(PluginPresenterKey).GetToolFactory(factoryFullName);
        }

        internal static ToolViewModel AddTool(PluginToolFactory factory, PanelPresenter panelPresenter)
        {
            Contract.Requires(factory != null);
            Contract.Requires(panelPresenter != null);

            var viewModel = factory.CreateTool();
            viewModel.IsVisible = true;
            viewModel.IsActive = true;
            if (!panelPresenter.ContainTool(viewModel))
            {
                Instance.ActiveDocumentChanged += viewModel.CallActiveDocumentChanged;

                foreach (var repository in Instance.GetPresenter<RepositoryPresenter>(RepositoryPresenterKey).GetRepositories())
                {
                    repository.RepositoryChanged += viewModel.CallRepositoryChanged;
                }

                panelPresenter.AddTool(viewModel);
            }

            return viewModel;
        }

        /// <summary>
        /// プレゼンターを追加します。内部処理
        /// </summary>
        /// <param name="presenterKey">追加するプレゼンターのキーです。</param>
        /// <param name="presenter">追加するプレゼンターです。</param>
        protected void AddPresenterInternal(string presenterKey, IPresenter presenter)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(presenterKey));
            Contract.Requires(presenter != null);

            Instance.AddPresenter(presenterKey, presenter);
        }

        /// <summary>
        /// プレゼンターをクリアします。内部処理
        /// </summary>
        protected void ClearPresentersInternal()
        {
            Instance.ClearPresenters();
        }

        private static IEnumerable<Menu> CreateViewMenus()
        {
            var pluginPresenter = Instance.GetPresenter<PluginPresenter>(PluginPresenterKey);
            Contract.Assume(pluginPresenter != null);

            var panelPresenter = Instance.GetPresenter<PanelPresenter>(PanelPresenterKey);
            Contract.Assume(panelPresenter != null);

            foreach (var factory in pluginPresenter.GetToolFactories())
            {
                Menu menu = new Menu(
                    param =>
                    {
                        AddTool(factory, panelPresenter);
                    },
                    param =>
                    {
                        return true;
                    });

                menu.Label = factory.Label;
                yield return menu;
            }
        }        

        /// <summary>
        /// グローバルクラスです。
        /// </summary>
        private class Global : Presenter
        {
            private readonly Dictionary<string, IPresenter> presenters =
                new Dictionary<string, IPresenter>();

            private readonly WeakReference<IDocumentable> activeDocument =
                new WeakReference<IDocumentable>(null);

            private Window owner = null;

            private EventHandler<ActiveDocumentChangedEventArgs> repositoryActiveDocumentChanged;
            private EventHandler<ActiveDocumentChangedEventArgs> activeDocumentChanged;

            /// <summary>
            /// コンストラクタです。
            /// </summary>
            public Global()
                : base("Global")
            {
            }

            internal event EventHandler<ActiveDocumentChangedEventArgs> RepositoryActiveDocumentChanged
            {
                add
                {
                    this.repositoryActiveDocumentChanged += value.MakeWeakEventHandler(eventHandler => this.repositoryActiveDocumentChanged -= eventHandler);
                }

                remove
                {
                }
            }

            internal event EventHandler<ActiveDocumentChangedEventArgs> ActiveDocumentChanged
            {
                add
                {
                    this.activeDocumentChanged += value.MakeWeakEventHandler(eventHandler => this.activeDocumentChanged -= eventHandler);
                }

                remove
                {
                }
            }

            internal Window Owner
            {
                get
                {
                    return this.owner;
                }
            }

            internal Dictionary<string, IPresenter> Presenters
            {
                get
                {
                    return this.presenters;
                }
            }

            internal WeakReference<IDocumentable> ActiveDocument
            {
                get
                {
                    return this.activeDocument;
                }
            }            

            internal TPresenter GetPresenter<TPresenter>(string presenterKey) where TPresenter : class, IPresenter
            {
                return this.presenters[presenterKey] as TPresenter;
            }

            internal void AddPresenter(string presenterKey, IPresenter presenter)
            {
                Contract.Requires(!string.IsNullOrWhiteSpace(presenterKey));
                Contract.Requires(presenter != null);

                lock (this.SyncObj)
                {
                    this.presenters.Add(presenterKey, presenter);
                }
            }

            internal void ClearPresenters()
            {
                lock (this.SyncObj)
                {
                    this.presenters.Clear();
                }
            }

            internal void SetOwner(Window owner)
            {
                Contract.Requires(owner != null);
                this.owner = owner;
            }

            internal void SetActiveDocument(IDocumentable document)
            {
                this.ActiveDocument.SetTarget(document);

                if (this.repositoryActiveDocumentChanged != null)
                {
                    this.repositoryActiveDocumentChanged(this, new ActiveDocumentChangedEventArgs(document));
                }

                if (this.activeDocumentChanged != null)
                {
                    this.activeDocumentChanged(this, new ActiveDocumentChangedEventArgs(document));
                }
            }

            internal bool Show<TDialog>()
                where TDialog : class
            {
                var pluginPresenter = this.GetPresenter<PluginPresenter>(PluginPresenterKey);
                Contract.Assume(pluginPresenter != null);

                var dialog = pluginPresenter.GetDialogFactory<TDialog>();
                Contract.Assume(dialog != null);

                return dialog.Show();
            }

            internal bool Show<TDialog, TArgs>(TArgs args)
                where TDialog : class
                where TArgs : PluginDialogArgs
            {
                var pluginPresenter = this.GetPresenter<PluginPresenter>(PluginPresenterKey);
                Contract.Assume(pluginPresenter != null);

                var dialog = pluginPresenter.GetDialogFactory<TDialog>();
                Contract.Assume(dialog != null);

                return ((PluginDialogFactory<TArgs>)dialog).Show(args);
            }

            internal Operation CreateScriptOperation(string factoryKey, params object[] args)
            {
                var pluginPresenter = this.GetPresenter<PluginPresenter>(PluginPresenterKey);
                Contract.Assume(pluginPresenter != null);

                var factory = pluginPresenter.GetOperationFactory(factoryKey);
                return factory.CreateScriptOperation(args);
            }

            internal void RunOperation(Operation operation)
            {
                Contract.Requires(operation != null);

                var operationPresenter = this.GetPresenter<OperationPresenter>(OperationPresenterKey);
                Contract.Assume(operationPresenter != null);

                operationPresenter.Run(operation);

                if (operation.IsScriptable)
                {
                    Logger logger = new Logger();
                    logger.WriteInformation(operation.ScriptMessage);
                    this.PushLoggerAndFlush(logger);
                }
            }

            internal DocumentViewModel CreateDocument(string unitKey, string factoryKey)
            {
                var pluginPresenter = this.GetPresenter<PluginPresenter>(PluginPresenterKey);
                Contract.Assume(pluginPresenter != null);

                var factory = pluginPresenter.GetDocumentFactory(unitKey, factoryKey);
                Contract.Assume(factory != null);

                var viewModel = factory.CreateDocument();
                Contract.Assume(viewModel != null);

                viewModel.DocumentClosed += this.CallDocumentClosed;

                return viewModel;
            }

            internal void AddDocument(DocumentViewModel document)
            {
                Contract.Requires(document != null);

                var panelPresenter = this.GetPresenter<PanelPresenter>(PanelPresenterKey);
                Contract.Assume(panelPresenter != null);

                panelPresenter.AddDocument(document);
            }

            internal void PushLogger(Logger logger)
            {
                Contract.Assume(logger != null);

                var logPresenter = this.GetPresenter<LogPresenter>(LogPresenterKey);
                Contract.Assume(logPresenter != null);

                logPresenter.PushLogger(logger);
            }

            internal void FlushLogs()
            {
                var logPresenter = this.GetPresenter<LogPresenter>(LogPresenterKey);
                Contract.Assume(logPresenter != null);

                logPresenter.Flush();
            }

            internal void PushLoggerAndFlush(Logger logger)
            {
                Contract.Assume(logger != null);

                var logPresenter = this.GetPresenter<LogPresenter>(LogPresenterKey);
                Contract.Assume(logPresenter != null);

                logPresenter.PushLogger(logger);
                logPresenter.Flush();
            }

            internal bool CallRunScriptOperation(out object returnValue, string factoryKey, params object[] args)
            {
                var operation = this.CreateScriptOperation(factoryKey, args);
                this.RunOperation(operation);
                returnValue = operation.ReturnValue;
                return operation.Result;
            }

            internal void CallEchoInfo(string message)
            {
                Logger logger = new Logger();
                logger.WriteInformation(message);
                this.PushLoggerAndFlush(logger);
            }

            internal void CallEchoError(string message)
            {
                Logger logger = new Logger();
                logger.WriteError(message);
                this.PushLoggerAndFlush(logger);
            }

            protected override void DisposeInternal()
            {
                this.ClearPresenters();
            }

            private void CallDocumentClosed(object sender, DocumentClosedEventArgs e)
            {
                Contract.Assume(sender != null);
                Contract.Assume(e != null);

                var panelPresenter = this.GetPresenter<PanelPresenter>(PanelPresenterKey);
                Contract.Assume(panelPresenter != null);

                IDocumentable document = null;
                if (e.WeakDocument.TryGetTarget(out document))
                {
                    var viewModel = document as DocumentViewModel;
                    Contract.Assume(viewModel != null);
                    Contract.Assume(panelPresenter.ContainDocument(viewModel));

                    viewModel.PanelView.DataContext = null;
                    viewModel.DocumentClosed -= this.CallDocumentClosed;
                    panelPresenter.RemoveDocument(viewModel);
                }
            }
        }
    }
}
