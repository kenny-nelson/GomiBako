//-----------------------------------------------------------------------
// <copyright file="ViewReceiverCommand.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;

    /// <summary>
    /// ビュー応答コマンドクラスです。
    /// </summary>
    /// <typeparam name="T">応答対象の型です。</typeparam>
    public sealed class ViewReceiverCommand<T> : CommandBase, ICommand
    {
        private Action<T> execute;
        private Func<T, bool> canExecute;

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        /// <param name="execute">実行するコールバックです。</param>
        public ViewReceiverCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        /// <param name="execute">実行するコールバックです。</param>
        /// <param name="canExecute">実行することが可能か確認するコールバックです。</param>
        public ViewReceiverCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            Contract.Requires(execute != null);
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// 実行可能か判定します。
        /// </summary>
        /// <param name="parameter">パラメータです。</param>
        /// <returns>実行可能ならば真を返します。</returns>
        public bool CanExecute(T parameter)
        {
            return this.canExecute == null ? true : this.canExecute(parameter);
        }

        /// <summary>
        /// 実行します。
        /// </summary>
        /// <param name="parameter">パラメータです。</param>
        public void Execute(T parameter)
        {
            this.execute(parameter);
        }

        void ICommand.Execute(object parameter)
        {
            if (parameter == null)
            {
                this.Execute(default(T));
            }
            else
            {
                this.Execute((T)parameter);
            }
        }

        bool ICommand.CanExecute(object parameter)
        {
            if (parameter == null)
            {
                return this.CanExecute(default(T));
            }
            else
            {
                return this.CanExecute((T)parameter);
            }
        }
    }
}
