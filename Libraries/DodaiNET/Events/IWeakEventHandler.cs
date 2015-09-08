//-----------------------------------------------------------------------
// <copyright file="IWeakEventHandler.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Events
{
    using System;

    /// <summary>
    /// 弱参照イベントハンドラーのインターフェースです。
    /// </summary>
    /// <typeparam name="TEventArgs">イベント引数の型です。</typeparam>
    public interface IWeakEventHandler<TEventArgs> where TEventArgs : EventArgs
    {
        /// <summary>
        /// イベントハンドラーを取得します。
        /// </summary>
        EventHandler<TEventArgs> Handler { get; }
    }
}
