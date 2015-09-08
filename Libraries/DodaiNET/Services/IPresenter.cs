//-----------------------------------------------------------------------
// <copyright file="IPresenter.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Services
{
    /// <summary>
    /// プレゼンターの種類です。
    /// </summary>
    public enum PresenterKind
    {
        /// <summary>
        /// オペレーション（コマンド処理みたいなもの）
        /// </summary>
        Operation,

        /// <summary>
        /// ログ
        /// </summary>
        Log,

        /// <summary>
        /// パネル
        /// </summary>
        Panel,

        /// <summary>
        /// メニュー
        /// </summary>
        Menu,

        /// <summary>
        /// コンフィグ
        /// </summary>
        Config,

        /// <summary>
        /// リポジトリ（プロジェクトのファイル群単位）
        /// </summary>
        Repository,

        /// <summary>
        /// スクリプト
        /// </summary>
        Script,

        /// <summary>
        /// プラグイン
        /// </summary>
        Plugin
    }

    /// <summary>
    /// プレゼンターのインターフェースです。
    /// </summary>
    public interface IPresenter
    {
        /// <summary>
        /// プレゼンターキーを取得します。
        /// </summary>
        string PresenterKey
        {
            get;
        }
    }
}