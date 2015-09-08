//-----------------------------------------------------------------------
// <copyright file="IOperationPresenter.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Operations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dodai.Services;

    /// <summary>
    /// オペレーションプレゼンターのインターフェースです。
    /// </summary>
    public interface IOperationPresenter : IPresenter
    {
        /// <summary>
        /// オペレーションを追加します。
        /// </summary>
        /// <param name="operation">追加するオペレーションです。</param>
        void Add(Operation operation);

        /// <summary>
        /// オペレーションを実行します。
        /// </summary>
        /// <param name="operation">実行するオペレーションです。</param>
        void Run(Operation operation);

        /// <summary>
        /// アンドゥ対象になっているオペレーションなど状態をクリアします。
        /// </summary>
        void Clear();

        /// <summary>
        /// アンドゥが可能か判定します。
        /// </summary>
        /// <returns>アンドゥ可能な場合は真を返します。</returns>
        bool CanUndo();

        /// <summary>
        /// リドゥが可能か判定します。
        /// </summary>
        /// <returns>リドゥ可能な場合は真を返します。</returns>
        bool CanRedo();

        /// <summary>
        /// トランザクションを開始します。
        /// </summary>
        /// <param name="merge">真の場合はオペレーションをマージします。</param>
        /// <returns>使用するオペレーショントランザクションのインスタンスを返します。</returns>
        OperationTransaction BeginTransaction(bool merge);

        /// <summary>
        /// トランザクションを終了します。
        /// </summary>
        void EndTransaction();

        /// <summary>
        /// アンドゥを実行します。
        /// </summary>
        void Undo();

        /// <summary>
        /// リドゥを実行します。
        /// </summary>
        void Redo();
    }
}
