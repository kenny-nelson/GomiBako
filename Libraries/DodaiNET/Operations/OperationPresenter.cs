//-----------------------------------------------------------------------
// <copyright file="OperationPresenter.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Operations
{
    using System;
    using System.Collections.Generic;
    using Dodai.Services;

    /// <summary>
    /// オペレーションプレゼンタークラスです。
    /// </summary>
    public sealed class OperationPresenter : Presenter, IOperationPresenter
    {
        private readonly List<Operation> undoList = new List<Operation>();
        private readonly List<Operation> redoList = new List<Operation>();
        private int undoMaxCount = 30;
        private OperationTransaction transaction = null;

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public OperationPresenter()
            : base(Enum.GetName(typeof(PresenterKind), PresenterKind.Operation))
        {
        }

        /// <summary>
        /// オペレーションを追加します。
        /// </summary>
        /// <param name="operation">追加するオペレーションです。</param>
        public void Add(Operation operation)
        {
            if (!operation.CanUndoable())
            {
                return;
            }

            lock (this.SyncObj)
            {
                this.redoList.Clear();
                this.undoList.Add(operation);
                
                while (this.undoMaxCount < this.undoList.Count)
                {
                    this.undoList.RemoveAt(0);
                }
            }
        }

        /// <summary>
        /// オペレーションを実行します。
        /// </summary>
        /// <param name="operation">実行するオペレーションです。</param>
        public void Run(Operation operation)
        {
            if (!operation.CanUndoable())
            {
                return;
            }

            lock (this.SyncObj)
            {
                if (this.transaction != null)
                {
                    this.transaction.Add(operation.Run());
                }
                else
                {
                    this.Add(operation.Run());
                }
            }
        }

        /// <summary>
        /// アンドゥ対象になっているオペレーションなど状態をクリアします。
        /// </summary>
        public void Clear()
        {
            lock (this.SyncObj)
            {
                this.undoList.Clear();
                this.redoList.Clear();
                this.transaction = null;
            }
        }

        /// <summary>
        /// アンドゥが可能か判定します。
        /// </summary>
        /// <returns>アンドゥ可能な場合は真を返します。</returns>
        public bool CanUndo()
        {
            lock (this.SyncObj)
            {
                return this.undoList.Count > 0;
            }
        }

        /// <summary>
        /// リドゥが可能か判定します。
        /// </summary>
        /// <returns>リドゥ可能な場合は真を返します。</returns>
        public bool CanRedo()
        {
            lock (this.SyncObj)
            {
                return this.redoList.Count > 0;
            }
        }

        /// <summary>
        /// トランザクションを開始します。
        /// </summary>
        /// <param name="merge">真の場合はオペレーションをマージします。</param>
        /// <returns>使用するオペレーショントランザクションのインスタンスを返します。</returns>
        public OperationTransaction BeginTransaction(bool merge)
        {
            lock (this.SyncObj)
            {
                if (this.transaction == null)
                {
                    this.transaction = new OperationTransaction(this, merge);
                }

                return this.transaction;
            }
        }

        /// <summary>
        /// トランザクションを終了します。
        /// </summary>
        public void EndTransaction()
        {
            lock (this.SyncObj)
            {
                if (this.transaction != null)
                {
                    OperationGroup operationGroup = this.transaction.OperationGroup;
                    this.transaction = null;
                    this.Run(operationGroup);
                }
            }
        }

        /// <summary>
        /// アンドゥを実行します。
        /// </summary>
        public void Undo()
        {
            lock (this.SyncObj)
            {
                int lastIndex = this.undoList.Count - 1;
                Operation operation = this.undoList[lastIndex];
                this.undoList.RemoveAt(lastIndex);
                this.redoList.Add(operation.Run());
            }
        }

        /// <summary>
        /// リドゥを実行します。
        /// </summary>
        public void Redo()
        {
            lock (this.SyncObj)
            {
                int lastIndex = this.redoList.Count - 1;
                Operation operation = this.redoList[lastIndex];
                this.redoList.RemoveAt(lastIndex);
                this.undoList.Add(operation.Run());
            }
        }

        /// <summary>
        /// 廃棄します。内部処理
        /// </summary>
        protected override void DisposeInternal()
        {
            this.Clear();
        }
    }
}
