//-----------------------------------------------------------------------
// <copyright file="IScriptPresenter.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Bridges
{
    using Dodai.Services;

    /// <summary>
    /// スクリプトプレゼンターのインタフェースです。
    /// </summary>
    public interface IScriptPresenter : IPresenter
    {
        /// <summary>
        /// 文字列からスクリプトを実行します。
        /// </summary>
        /// <param name="expression">実行するスクリプト式です。</param>
        /// <returns>実行成功ならば真を返します。</returns>
        bool ExecuteFromString(string expression);

        /// <summary>
        /// ファイルからスクリプトを実行します。
        /// </summary>
        /// <param name="fileName">実行するファイル名です。</param>
        /// <returns>実行成功ならば真を返します。</returns>
        bool ExecuteFromFile(string fileName);
    }
}
