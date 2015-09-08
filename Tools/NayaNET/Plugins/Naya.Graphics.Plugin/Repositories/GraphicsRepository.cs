//-----------------------------------------------------------------------
// <copyright file="GraphicsRepository.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Naya.Graphics.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dodai.Repositories;
    using Dodai.Services;

    /// <summary>
    /// グラフィックスに関するリポジトリクラスです。
    /// </summary>
    public sealed class GraphicsRepository : Repository
    {
        /// <summary>
        /// 状態をクリアします。
        /// </summary>
        public override void Clear()
        {
        }

        /// <summary>
        /// ドキュメントのアクティブ状態変更を通知します。
        /// </summary>
        /// <param name="e">イベント引数です。</param>
        protected override void OnActiveDocumentChanged(ActiveDocumentChangedEventArgs e)
        {
        }
    }
}