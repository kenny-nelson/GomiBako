//-----------------------------------------------------------------------
// <copyright file="NotifiableObject.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.ComponentModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;
    using System.Threading;

    /// <summary>
    /// プロパティ通知オブジェクトクラスです。
    /// </summary>
    public abstract class NotifiableObject : SynchronizedObject, INotifyPropertyChanged
    {
        /// <summary>
        /// プロパティ変更時のイベントハンドラーです。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// プロパティ変更を通知します。
        /// </summary>
        /// <typeparam name="T">通知対象の型です。</typeparam>
        /// <param name="propertyExpression">設定するプロパティ式です。</param>
        protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            Contract.Requires(propertyExpression != null);
            var memberExpression = propertyExpression.Body as MemberExpression;
            Contract.Assume(memberExpression != null);

            this.RaisePropertyChanged(memberExpression.Member.Name);
        }

        /// <summary>
        /// プロパティ変更を通知します。
        /// </summary>
        /// <param name="propertyName">変更対象のプロパティ名です。</param>
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = Interlocked.CompareExchange(ref this.PropertyChanged, null, null);
            if (handler != null)
            {
                PropertyChangedEventArgs args = new PropertyChangedEventArgs(propertyName);
                handler(this, args);
            }
        }

        /// <summary>
        /// プロパティを設定します。値が同一であっても強制的に設定するバージョンです。
        /// </summary>
        /// <typeparam name="T">設定対象の型です。</typeparam>
        /// <param name="item">設定対象のアイテムです。</param>
        /// <param name="value">設定する値です。</param>
        /// <param name="propertyName">プロパティ名です。使用時に自動で入力されます。</param>
        protected void SetPropertyForce<T>(ref T item, T value, [CallerMemberName] string propertyName = "")
        {
            item = value;
            this.RaisePropertyChanged(propertyName);
        }

        /// <summary>
        /// プロパティを設定します。
        /// </summary>
        /// <typeparam name="T">設定対象の型です。</typeparam>
        /// <param name="item">設定対象のアイテムです。</param>
        /// <param name="value">設定する値です。</param>
        /// <param name="propertyName">プロパティ名です。使用時に自動で入力されます。</param>
        protected void SetProperty<T>(ref T item, T value, [CallerMemberName] string propertyName = "")
        {
            if (!EqualityComparer<T>.Default.Equals(item, value))
            {
                this.SetPropertyForce(ref item, value, propertyName);
            }
        }

        /// <summary>
        /// プロパティを設定します。値が同一であっても強制的に設定するバージョンです。
        /// </summary>
        /// <typeparam name="T">設定対象の型です。</typeparam>
        /// <param name="item">設定対象のアイテムです。</param>
        /// <param name="value">設定する値です。</param>
        /// <param name="callback">設定後に実行するコールバックです。</param>
        /// <param name="propertyName">プロパティ名です。使用時に自動で入力されます。</param>
        protected void SetPropertyForce<T>(ref T item, T value, Action callback, [CallerMemberName] string propertyName = "")
        {
            Contract.Requires(callback != null);

            item = value;
            this.RaisePropertyChanged(propertyName);
            callback();
        }

        /// <summary>
        /// プロパティを設定します。
        /// </summary>
        /// <typeparam name="T">設定対象の型です。</typeparam>
        /// <param name="item">設定対象のアイテムです。</param>
        /// <param name="value">設定する値です。</param>
        /// <param name="callback">設定後に実行するコールバックです。</param>
        /// <param name="propertyName">プロパティ名です。使用時に自動で入力されます。</param>
        protected void SetProperty<T>(ref T item, T value, Action callback, [CallerMemberName] string propertyName = "")
        {
            Contract.Requires(callback != null);

            if (!EqualityComparer<T>.Default.Equals(item, value))
            {
                this.SetPropertyForce(ref item, value, propertyName);
                callback();
            }
        }
    }
}
