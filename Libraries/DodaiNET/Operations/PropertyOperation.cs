//-----------------------------------------------------------------------
// <copyright file="PropertyOperation.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Operations
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// プロパティ編集用のオペレーションクラスです。
    /// </summary>
    /// <typeparam name="TTarget">編集対象の型です。</typeparam>
    /// <typeparam name="TObject">編集する値の型です。</typeparam>
    public sealed class PropertyOperation<TTarget, TObject> : Operation where TTarget : class
    {
        private readonly TTarget target;
        private readonly PropertyInfo propertyInfo;
        private readonly string label;
        private int mergeableHashCode;
        private TObject value;
        private bool result = false;

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        /// <param name="target">編集対象です。</param>
        /// <param name="propertyName">編集対象のプロパティ名です。</param>
        /// <param name="value">設定する値です。</param>
        public PropertyOperation(TTarget target, string propertyName, TObject value)
            : this(target, target.GetType().GetProperty(propertyName), value)
        {
        }

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        /// <param name="target">編集対象です。</param>
        /// <param name="propertyInfo">編集対象のプロパティ情報です。</param>
        /// <param name="value">設定する値です。</param>
        public PropertyOperation(TTarget target, PropertyInfo propertyInfo, TObject value)
        {
            Contract.Requires(target != null);
            Contract.Requires(propertyInfo != null);

            this.target = target;
            this.propertyInfo = propertyInfo;

            int hashCode = target.GetHashCode();
            this.mergeableHashCode = hashCode ^ propertyInfo.GetHashCode();
            this.value = value;
            this.label = this.CreateLabel();
        }

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        /// <param name="target">編集対象です。</param>
        /// <param name="propertyExpression">編集対象のプロパティ式です。</param>
        /// <param name="value">設定する値です。</param>
        public PropertyOperation(TTarget target, Expression<Func<TTarget, TObject>> propertyExpression, TObject value)
        {
            Contract.Requires(target != null);
            Contract.Requires(propertyExpression != null);

            var memberExpression = propertyExpression.Body as MemberExpression;
            Contract.Assume(memberExpression != null);

            this.target = target;
            this.propertyInfo = target.GetType().GetProperty(memberExpression.Member.Name);

            int hashCode = target.GetHashCode();
            this.mergeableHashCode = hashCode ^ this.propertyInfo.GetHashCode();
            this.value = value;
            this.label = this.CreateLabel();
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
                return null;
            }
        }

        /// <summary>
        /// オペレーションがマージ可能かどうか取得します。
        /// </summary>
        public override bool IsMergeable
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// オペレーションを実行します。
        /// </summary>
        /// <returns>実行後のオペレーションを返します。</returns>
        public override Operation Run()
        {
            this.result = false;
            TObject temp = (TObject)this.propertyInfo.GetValue(this.target);
            this.propertyInfo.SetValue(this.target, this.value);
            this.value = temp;
            this.result = true;
            return this;
        }

        /// <summary>
        /// アンドゥ可能かどうか判定します。
        /// </summary>
        /// <returns>アンドゥ可能ならば真を返します。</returns>
        public override bool CanUndoable()
        {
            return true;
        }

        /// <summary>
        /// マージ処理用のハッシュコードを取得します。
        /// </summary>
        /// <returns>ハッシュコードを返します。</returns>
        public override int GetMergeableHashCode()
        {
            return this.mergeableHashCode;
        }

        private string CreateLabel()
        {
            StringBuilder builder = new StringBuilder();

            TObject beforeValue = (TObject)this.propertyInfo.GetValue(this.target);
            builder.AppendFormat(
                "{0}.{1} was changed into {2} from {3}.",
                this.target.ToString(),
                this.propertyInfo.ToString(),
                this.value.ToString(),
                beforeValue.ToString());

            return builder.ToString();
        }
    }
}
