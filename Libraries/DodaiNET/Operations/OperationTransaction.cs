//-----------------------------------------------------------------------
// <copyright file="OperationTransaction.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Operations
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dodai.ComponentModel;
    using Dodai.Services;

    /// <summary>
    /// オペレーショントランザクションクラスです。
    /// </summary>
    public sealed class OperationTransaction : DisposableObject
    {
        private readonly OperationGroup operationGroup = new OperationGroup();
        private readonly Dictionary<int, Operation> mergeableOperations = new Dictionary<int, Operation>();
        private readonly IOperationPresenter presenter;
        private bool merge = false;

        internal OperationTransaction(OperationPresenter presenter, bool merge)
        {
            Contract.Requires(presenter != null);
            this.presenter = presenter;
            this.merge = merge;
        }

        internal int Count
        {
            get
            {
                lock (this.SyncObj)
                {
                    return this.operationGroup.Count;
                }
            }
        }

        internal int MergeableCount
        {
            get
            {
                lock (this.SyncObj)
                {
                    return this.mergeableOperations.Values.Count;
                }
            }
        }

        internal OperationGroup OperationGroup
        {
            get
            {
                lock (this.SyncObj)
                {
                    return this.operationGroup;
                }
            }
        }

        /// <summary>
        /// オペレーションを追加します。
        /// </summary>
        /// <param name="operation">追加するオペレーションです。</param>
        public void Add(Operation operation)
        {
            Contract.Requires(operation != null);

            lock (this.SyncObj)
            {
                if (this.merge)
                {
                    if (!operation.IsMergeable)
                    {
                        this.operationGroup.Add(operation);
                        return;
                    }

                    Operation value;
                    if (!this.mergeableOperations.TryGetValue(operation.GetMergeableHashCode(), out value))
                    {
                        this.mergeableOperations.Add(operation.GetMergeableHashCode(), operation);
                    }
                    else
                    {
                        this.mergeableOperations[operation.GetMergeableHashCode()] = operation;
                    }
                }
                else
                {
                    this.operationGroup.Add(operation);
                }
            }
        }

        /// <summary>
        /// 廃棄します。内部処理
        /// </summary>
        protected override void DisposeInternal()
        {
            lock (this.SyncObj)
            {
                if (this.merge)
                {
                    foreach (var operation in this.mergeableOperations.Values)
                    {
                        this.operationGroup.Add(operation);
                    }
                }

                this.presenter.EndTransaction();
            }
        }
    }
}
