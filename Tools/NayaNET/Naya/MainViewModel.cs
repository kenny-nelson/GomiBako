//-----------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Naya
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using Dodai;
    using Dodai.Commands;
    using Dodai.Menus;
    using Dodai.Panels;
    using Dodai.Plugins;
    using Dodai.ViewModels;
    using Naya.Modules.About;

    /// <summary>
    /// メインビューのビューモデルクラスです。
    /// </summary>
    public class MainViewModel : WorkspaceViewModel
    {
        private readonly ICommand exitCommand;
        private readonly ICommand aboutCommand;

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public MainViewModel()
        {
            this.exitCommand =
                new ViewReceiverCommand<object>(param => this.ExecuteExit(param), param => { return this.CanExecuteExit(param); });
            this.aboutCommand =
                new ViewReceiverCommand<object>(param => this.ExecuteAbout(param), param => { return this.CanExecuteAbout(param); });
        }

        /// <summary>
        /// Exit処理を行うコマンドを取得します。
        /// </summary>
        public ICommand ExitCommand
        {
            get
            {
                return this.exitCommand;
            }
        }

        /// <summary>
        /// About 処理を行うコマンドを取得します。
        /// </summary>
        public ICommand AboutCommand
        {
            get
            {
                return this.aboutCommand;
            }
        }

        private void ExecuteExit(object parameter)
        {
        }

        private bool CanExecuteExit(object parameter)
        {
            return true;
        }

        private void ExecuteAbout(object parameter)
        {
            GlobalPresenter.Show<AboutDialog>();
        }

        private bool CanExecuteAbout(object parameter)
        {
            return true;
        }
    }
}
