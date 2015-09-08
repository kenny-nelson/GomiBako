//-----------------------------------------------------------------------
// <copyright file="ActiveDocumentChangedEventArgs.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// ドキュメントアクティブ状態変更後イベント引数クラスです。
    /// </summary>
    public class ActiveDocumentChangedEventArgs : EventArgs
    {
        private readonly WeakReference<IDocumentable> weakDocument;

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        /// <param name="document">対象のドキュメントです。</param>
        internal ActiveDocumentChangedEventArgs(IDocumentable document)
        {
            this.weakDocument = new WeakReference<IDocumentable>(document);
        }

        /// <summary>
        /// 対象のドキュメントを弱参照で取得します。
        /// </summary>
        public WeakReference<IDocumentable> WeakDocument
        {
            get
            {
                return this.weakDocument;
            }
        }
    }
}
