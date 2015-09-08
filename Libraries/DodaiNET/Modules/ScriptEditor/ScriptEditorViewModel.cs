//-----------------------------------------------------------------------
// <copyright file="ScriptEditorViewModel.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Modules.ScriptEditor
{    
    using System.Diagnostics.Contracts;
    using System.Windows.Input;
    using Dodai;
    using Dodai.Commands;
    using Dodai.Services;
    using Dodai.ViewModels;

    /// <summary>
    /// スクリプトエディタのビューモデルクラスです。
    /// </summary>
    public sealed class ScriptEditorViewModel : ToolViewModel
    {
        private readonly ICommand executeCommand;
        private string expression = string.Empty;

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public ScriptEditorViewModel()
        {
            this.executeCommand =
                new ViewReceiverCommand<object>(param => this.Execute(param), param => { return this.CanExecute(param); });
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
        /// 実行コマンドを取得します。
        /// </summary>
        public ICommand ExecuteCommand
        {
            get
            {
                return this.executeCommand;
            }
        }

        /// <summary>
        /// スクリプト式の文字列を取得設定します。
        /// </summary>
        public string Expression
        {
            get
            {
                return this.expression;
            }

            set
            {
                this.SetPropertyForce(ref this.expression, value);
            }
        }

        private void Execute(object parameter)
        {
            var scriptPresenter = GlobalPresenter.GetScriptPresenter();
            Contract.Assume(scriptPresenter != null);

            scriptPresenter.ExecuteFromString(this.Expression);
            GlobalPresenter.PushLoggerAndFlush(scriptPresenter.Script.Logger);            
        }

        private bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
