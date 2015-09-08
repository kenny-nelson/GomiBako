//-----------------------------------------------------------------------
// <copyright file="WeakEventHandler.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 弱参照イベントハンドラークラスです。
    /// </summary>
    /// <typeparam name="TTarget">対象の型です。</typeparam>
    /// <typeparam name="TEventArgs">イベント引数の型です。</typeparam>
    public class WeakEventHandler<TTarget, TEventArgs> : IWeakEventHandler<TEventArgs>
        where TTarget : class
        where TEventArgs : EventArgs
    {            
        private WeakReference<TTarget> targetRef;
        private OpenEventHandler openHandler;
        private EventHandler<TEventArgs> handler;
        private Action<EventHandler<TEventArgs>> unregisterCallback;       

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        /// <param name="handler">イベントハンドらです。</param>
        /// <param name="unregisterCallback">登録解除時のコールバックです。</param>
        public WeakEventHandler(EventHandler<TEventArgs> handler, Action<EventHandler<TEventArgs>> unregisterCallback)
        {
            this.targetRef = new WeakReference<TTarget>((TTarget)handler.Target);
            this.openHandler = (OpenEventHandler)Delegate.CreateDelegate(typeof(OpenEventHandler), null, handler.Method);
            this.handler = this.Invoke;
            this.unregisterCallback = unregisterCallback;
        }

        private delegate void OpenEventHandler(TTarget @this, object sender, TEventArgs e);

        /// <summary>
        /// イベントハンドラーを取得します。
        /// </summary>
        public EventHandler<TEventArgs> Handler
        {
            get
            {
                return this.handler;
            }
        }

        /// <summary>
        /// 弱参照イベントハンドラーからイベントハンドラーへのオペレータです。
        /// </summary>
        /// <param name="weakHandler">変換する弱参照イベントハンドラーです。</param>
        /// <returns>イベントハンドラーを返します。</returns>
        public static implicit operator EventHandler<TEventArgs>(WeakEventHandler<TTarget, TEventArgs> weakHandler)
        {
            return weakHandler.handler;
        }

        /// <summary>
        /// イベントを起動します。
        /// </summary>
        /// <param name="sender">送信者です。</param>
        /// <param name="e">イベント引数です。</param>
        public void Invoke(object sender, TEventArgs e)
        {
            TTarget target = null;
            if (this.targetRef.TryGetTarget(out target))
            {
                this.openHandler.Invoke(target, sender, e);
            }
            else if (this.unregisterCallback != null)
            {
                this.unregisterCallback(this.handler);
                this.unregisterCallback = null;
            }
        }        
    }
}
