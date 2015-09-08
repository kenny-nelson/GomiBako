//-----------------------------------------------------------------------
// <copyright file="PluginPresenter.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Plugins
{    
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Data;
    using Dodai.Menus;
    using Dodai.Services;
    using Dodai.ViewModels;

    /// <summary>
    /// プラグインプレゼンタークラスです。
    /// </summary>
    public class PluginPresenter : Presenter, IPluginPresenter
    {        
        private readonly ObservableCollection<Menu> panelMenus =
            new ObservableCollection<Menu>();

        private readonly FactoryCache<PluginDocumentFactory> documentFactoryCache =
            new FactoryCache<PluginDocumentFactory>();

        private readonly ToolFactoryCache toolFactoryCache =
            new ToolFactoryCache();

        private readonly FactoryCache<PluginMenuFactory> menuFactoryCache =
            new FactoryCache<PluginMenuFactory>();

        private readonly OperationFactoryCache operationFactoryCache =
            new OperationFactoryCache();

        private readonly Dictionary<Type, PluginDialogFactory> dialogFactoryCache =
            new Dictionary<Type, PluginDialogFactory>();

        private CompositionContainer container = null;
        private PluginImporter importer = null;

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public PluginPresenter()
            : base(Enum.GetName(typeof(PresenterKind), PresenterKind.Plugin))
        {
            BindingOperations.EnableCollectionSynchronization(this.panelMenus, this.SyncObj);
        }

        /// <summary>
        /// プラグインを設定します。
        /// </summary>
        /// <param name="path">プラグインのパスです。</param>
        public void Setup(string path)
        {
            Contract.Requires(!string.IsNullOrEmpty(path));
            DirectoryCatalog pluginCatalog = new DirectoryCatalog(path);
            CatalogExportProvider pluginCatalogProvider = new CatalogExportProvider(pluginCatalog);

            AssemblyCatalog defaultCatalog = new AssemblyCatalog(Assembly.GetAssembly(typeof(PluginPresenter)));
            CatalogExportProvider defaultCatalogProvider = new CatalogExportProvider(defaultCatalog);
            this.container = new CompositionContainer(pluginCatalogProvider, defaultCatalogProvider);
            pluginCatalogProvider.SourceProvider = this.container;
            defaultCatalogProvider.SourceProvider = this.container;
            this.importer = new PluginImporter();
        }

        /// <summary>
        /// 初期化します。
        /// </summary>
        /// <returns>タスクを返します。</returns>
        public async Task Initialize()
        {
            Contract.Assume(this.importer != null);

            await Task.Run(() =>
            {
                container.ComposeParts(this.importer);
            });

            await this.InitializeFactoriesAsync();
        }        

        internal PluginOperationFactory GetOperationFactory(string factoryKey)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(factoryKey));
            return this.operationFactoryCache.GetFactory(factoryKey);
        }

        internal PluginDocumentFactory GetDocumentFactory(string unitKey, string factoryKey)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(unitKey));
            Contract.Requires(!string.IsNullOrWhiteSpace(factoryKey));

            return this.documentFactoryCache.GetValue(unitKey, factoryKey);
        }

        internal IEnumerable<PluginOperationFactory> GetOperationFactories()
        {
            return this.operationFactoryCache.GetFactories();
        }

        internal IEnumerable<PluginToolFactory> GetToolFactories()
        {
            foreach (var unitKey in this.toolFactoryCache.GetUnitKeys())
            {
                foreach (var factoryKey in this.toolFactoryCache.GetFactoryKeys(unitKey))
                {
                    yield return this.toolFactoryCache.GetValue(unitKey, factoryKey);
                }
            }
        }

        internal IEnumerable<PluginDialogFactory> GetDialogFactories()
        {
            return this.dialogFactoryCache.Values;
        }

        internal PluginToolFactory GetToolFactory(string factoryFullName)
        {
            return this.toolFactoryCache.GetFactory(factoryFullName);
        }

        internal IEnumerable<PluginMenuFactory> GetMenuFactories()
        {
            foreach (var unitKey in this.menuFactoryCache.GetUnitKeys())
            {
                foreach (var factoryKey in this.menuFactoryCache.GetFactoryKeys(unitKey))
                {
                    yield return this.menuFactoryCache.GetValue(unitKey, factoryKey);
                }
            }
        }

        internal PluginDialogFactory GetDialogFactory<TDialogType>() where TDialogType : class
        {
            return this.dialogFactoryCache[typeof(TDialogType)];
        }

        /// <summary>
        /// 廃棄します。内部処理
        /// </summary>
        protected override void DisposeInternal()
        {
            BindingOperations.DisableCollectionSynchronization(this.panelMenus);
        }

        /// <summary>
        /// 非同期でファクトリを初期化します。
        /// </summary>
        /// <returns>タスクを返します。</returns>
        private Task InitializeFactoriesAsync()
        {
            Contract.Assume(this.importer.Plugins != null);

            return Task.Run(() =>
            {
                foreach (var unit in this.importer.Plugins)
                {
                    unit.Value.BeginRegistration();

                    foreach (var menuFactory in unit.Value.GetMenuFactories())
                    {
                        this.menuFactoryCache.AddValue(unit.Value.UnitKey, menuFactory.FactoryKey, menuFactory);
                    }

                    foreach (var documentFactory in unit.Value.GetDocumentFactories())
                    {
                        this.documentFactoryCache.AddValue(unit.Value.UnitKey, documentFactory.FactoryKey, documentFactory);
                    }

                    foreach (var toolFactory in unit.Value.GetToolFactories())
                    {
                        this.toolFactoryCache.AddValue(unit.Value.UnitKey, toolFactory.FactoryKey, toolFactory);
                    }

                    foreach (var operationFactory in unit.Value.GetOperationFactories())
                    {
                        this.operationFactoryCache.AddFactory(operationFactory.FactoryKey, operationFactory);
                    }

                    foreach (var dialogFactory in unit.Value.GetDialogFactories())
                    {
                        this.dialogFactoryCache.Add(dialogFactory.DialogType, dialogFactory);
                    }
                }

                foreach (var unit in this.importer.Plugins)
                {
                    unit.Value.EndRegistration();
                }
            });
        }        
    }
}
