//-----------------------------------------------------------------------
// <copyright file="ILogPresenter.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Logs
{
    using Dodai.Services;

    /// <summary>
    /// ログプレゼンターのインターフェースです。
    /// </summary>
    public interface ILogPresenter : IPresenter
    {
        /// <summary>
        /// ロガーをプッシュします。
        /// </summary>
        /// <param name="logger">プッシュ対象のロガーです。</param>
        void PushLogger(Logger logger);

        /// <summary>
        /// ログをフラッシュします。
        /// </summary>
        void Flush();
    }
}