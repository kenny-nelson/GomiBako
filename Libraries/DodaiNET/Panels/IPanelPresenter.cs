//-----------------------------------------------------------------------
// <copyright file="IPanelPresenter.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Panels
{
    using System.ComponentModel;
    using Dodai.Services;
    using Dodai.ViewModels;

    /// <summary>
    /// パネルプレゼンターのインターフェースです。
    /// </summary>
    public interface IPanelPresenter : IPresenter
    {
        /// <summary>
        /// ツールを取得します。
        /// </summary>
        /// <returns>対象のインスタンスを返します。</returns>
        ICollectionView GetTools();

        /// <summary>
        /// ドキュメントを取得します。
        /// </summary>
        /// <returns>対象のインスタンスを返します。</returns>
        ICollectionView GetDocuments();

        /// <summary>
        /// 指定のツールが含まれているか判定します。
        /// </summary>
        /// <param name="tool">判定対象のツールです。</param>
        /// <returns>含まれていれば、真を返します。</returns>
        bool ContainTool(ToolViewModel tool);

        /// <summary>
        /// 指定のドキュメントが含まれているか判定します。
        /// </summary>
        /// <param name="document">判定対象のドキュメントです。</param>
        /// <returns>含まれていれば、真を返します。</returns>
        bool ContainDocument(DocumentViewModel document);
    }
}