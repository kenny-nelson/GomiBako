//-----------------------------------------------------------------------
// <copyright file="DocumentClosedEventArgs.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Services
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// ドキュメントクローズ時のイベント引数です。
    /// </summary>
    internal class DocumentClosedEventArgs : EventArgs
    {
        private readonly WeakReference<IDocumentable> weakDocument;

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        /// <param name="document">対象のドキュメントです。</param>
        internal DocumentClosedEventArgs(IDocumentable document)
        {
            Contract.Assume(document != null);

            this.weakDocument = new WeakReference<IDocumentable>(document);
        }

        /// <summary>
        /// 弱参照で対象のドキュメントを取得します。
        /// </summary>
        internal WeakReference<IDocumentable> WeakDocument
        {
            get
            {
                return this.weakDocument;
            }
        }
    }
}
