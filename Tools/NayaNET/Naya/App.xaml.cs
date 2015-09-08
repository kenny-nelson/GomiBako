//-----------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Naya
{    
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;
    using Dodai;
    using Dodai.Bridges;
    using Dodai.Helpers;
    using Dodai.Logs;
    using Dodai.Menus;
    using Dodai.Modules.Output;
    using Dodai.Operations;
    using Dodai.Panels;
    using Dodai.Plugins;
    using Dodai.Repositories;

    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private readonly NayaPresenter presenter = new NayaPresenter();

        /// <summary>
        /// System.Windows.Application.Startup イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している System.Windows.StartupEventArgsです。</param>
        protected override async void OnStartup(StartupEventArgs e)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            ProfileOptimization.SetProfileRoot(path);
            ProfileOptimization.StartProfile("Naya.profile");

            if (!Environment.Is64BitOperatingSystem || !Environment.Is64BitProcess)
            {
                MessageBox.Show("Not Support 32BitOS.");
                Environment.Exit(-1);
            }

            AppDomain.CurrentDomain.UnhandledException +=
                new UnhandledExceptionEventHandler(this.CurrentDomain_UnhandledException);

            // Setup PluginPresenter.
            {
                PluginPresenter pluginPresenter = new PluginPresenter();
                string pluginPath = Path.Combine(path, "Plugins");
                pluginPresenter.Setup(pluginPath);

                this.presenter.AddPluginPresenter(pluginPresenter);
            }

            // Setup LogPresenter
            {
                OutputLogFactory factory = new OutputLogFactory();
                LogPresenter logPresenter = new LogPresenter(factory);
                this.presenter.AddLogPresenter(logPresenter);
            }

            // Setup OperationPresenter
            {
                OperationPresenter operationPresenter = new OperationPresenter();
                this.presenter.AddOperationPresenter(operationPresenter);
            }

            // Setup MenuPresenter
            {
                MenuPresenter menuPresenter = new MenuPresenter();
                this.presenter.AddMenuPresenter(menuPresenter);
            }

            // Setup PanelPresenter
            {
                PanelPresenter panelPresenter = new PanelPresenter();
                this.presenter.AddPanelPresenter(panelPresenter);
            }

            // Setup RepositoryPresenter
            {
                RepositoryPresenter repositoryPresenter = new RepositoryPresenter();
                this.presenter.AddRepositoryPresenter(repositoryPresenter);
            }

            // Setup ScriptPresenter
            {
                PythonScriptBridge scriptBridge = new PythonScriptBridge("Naya");
                ScriptPresenter scriptPresenter = new ScriptPresenter(scriptBridge);
                this.presenter.AddScriptPresenter(scriptPresenter);
            }

            MainWindow window = new MainWindow();
            MainViewModel viewModel = new MainViewModel();

            GlobalPresenter.SetOwner(window);

            window.DataContext = viewModel;
            window.Show();

            window.IsEnabled = false;
            await NayaPresenter.Initialize();

            if (LayoutHelper.HasAvalonDockConfigFile())
            {
                window.LoadLayout();
            }
            else
            {
                GlobalPresenter.MakeDefaultTools();
            }

            window.IsEnabled = true;
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString());
            Environment.Exit(-1);
            ////e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(((Exception)e.ExceptionObject).ToString());
            Environment.Exit(-1);
        }
    }
}
