//-----------------------------------------------------------------------
// <copyright file="IDocumentable.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Services
{
    /// <summary>
    /// ドキュメント化可能インターフェースです。
    /// </summary>
    public interface IDocumentable : IPanelable
    {
        /// <summary>
        /// ドキュメントをクローズします。
        /// </summary>
        void Close();
    }
}