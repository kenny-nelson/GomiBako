//-----------------------------------------------------------------------
// <copyright file="EventHandlerHelper.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Helpers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;
    using System.Reflection;
    using Dodai.Events;

    /// <summary>
    /// イベントハンドラーヘルパークラスです。
    /// </summary>
    public static class EventHandlerHelper
    {
        /// <summary>
        /// 弱参照イベントハンドラーを作成します。
        /// </summary>
        /// <typeparam name="TEventArgs">イベント引数の型です。</typeparam>
        /// <param name="handler">イベントハンドラーです。</param>
        /// <param name="unregisterCallback">登録解除時のコールバックです。</param>
        /// <returns>イベントハンドラーのインスタンスを返します。</returns>
        public static EventHandler<TEventArgs> MakeWeakEventHandler<TEventArgs>(this EventHandler<TEventArgs> handler, Action<EventHandler<TEventArgs>> unregisterCallback)
            where TEventArgs : EventArgs
        {
            Contract.Requires(handler != null);
            Contract.Assume(!handler.Method.IsStatic && handler.Target != null);

            Type type = typeof(WeakEventHandler<,>).MakeGenericType(handler.Method.DeclaringType, typeof(TEventArgs));
            ConstructorInfo constructor = type.GetConstructor(new Type[] { typeof(EventHandler<TEventArgs>), typeof(Action<EventHandler<TEventArgs>>) });

            IWeakEventHandler<TEventArgs> weakHandler = (IWeakEventHandler<TEventArgs>)constructor.Invoke(new object[] { handler, unregisterCallback });
            return weakHandler.Handler;
        }

        /// <summary>
        /// イベント名を取得します。
        /// </summary>
        /// <typeparam name="T">対象の型です。</typeparam>
        /// <param name="expression">イベントへの式です。</param>
        /// <returns>イベント名を返します。</returns>
        internal static string GetEventName<T>(Expression<Func<T>> expression)
        {
            return (expression.Body as MemberExpression).Member.Name;
        }
    }
}
