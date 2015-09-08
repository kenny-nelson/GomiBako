//-----------------------------------------------------------------------
// <copyright file="OperationGroup.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Operations
{    
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Dodai.Services;

    /// <summary>
    /// オペレーションを束ねたクラスです。
    /// </summary>
    public sealed class OperationGroup : Operation
    {
        private readonly object syncRoot = new object();
        private readonly List<Operation> operations = new List<Operation>();
        private readonly string label;
        private bool result = false;
        private object returnValue = null;

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public OperationGroup()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        /// <param name="label">オペレーションに設定するラベル名です。</param>
        public OperationGroup(string label)
        {
            Contract.Assume(label != null);
            this.label = label;
        }

        /// <summary>
        /// オペレーションラベルを取得します。
        /// </summary>
        public override string Label
        {
            get
            {
                return this.label;
            }
        }

        /// <summary>
        /// オペレーションの処理結果を取得します。
        /// </summary>
        public override bool Result
        {
            get
            {
                return this.result;
            }
        }

        /// <summary>
        /// オペレーションの返り値を取得します。
        /// </summary>
        public override object ReturnValue
        {
            get
            {
                return this.returnValue;
            }
        }

        /// <summary>
        /// オペレーションがマージ可能かどうか取得します。
        /// </summary>
        public override bool IsMergeable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 束ねたオペレーション数を取得します。
        /// </summary>
        public int Count
        {
            get
            {
                lock (this.syncRoot)
                {
                    return this.operations.Count;
                }
            }
        }        

        /// <summary>
        /// 返り値を設定します。
        /// </summary>
        /// <param name="value">設定する値です。</param>
        public void SetReturnValue(object value)
        {
            this.returnValue = value;
        }

        /// <summary>
        /// オペレーションを実行します。
        /// </summary>
        /// <returns>実行後のオペレーションを返します。</returns>
        public override Operation Run()
        {
            lock (this.syncRoot)
            {
                this.result = false;
                OperationGroup operationGroup = new OperationGroup();
                foreach (var item in this.operations)
                {
                    operationGroup.Add(item.Run());
                }

                operationGroup.Reverse();
                this.result = true;
                return operationGroup;
            }
        }

        /// <summary>
        /// アンドゥ可能かどうか判定します。
        /// </summary>
        /// <returns>アンドゥ可能ならば真を返します。</returns>
        public override bool CanUndoable()
        {
            lock (this.syncRoot)
            {
                if (this.operations.Any(obj => obj.CanUndoable() == false))
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// 束ねるオペレーションを追加します。
        /// </summary>
        /// <param name="operation">追加するオペレーションです。</param>
        public void Add(Operation operation)
        {
            Contract.Requires(operation != null);

            lock (this.syncRoot)
            {
                this.operations.Add(operation);
            }
        }

        /// <summary>
        /// 束ねたオペレーションを逆順にします。
        /// </summary>
        public void Reverse()
        {
            lock (this.syncRoot)
            {
                this.operations.Reverse();
            }
        }

        /// <summary>
        /// マージ処理用のハッシュコードを取得します。
        /// </summary>
        /// <returns>ハッシュコードを返します。</returns>
        public override int GetMergeableHashCode()
        {
            return 0;
        }
    }
}
