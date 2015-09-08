//-----------------------------------------------------------------------
// <copyright file="RedoOperationFactory.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Plugins.Applications
{    
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dodai;
    using Dodai.Operations;
    using Dodai.Operations.Applications;

    /// <summary>
    /// リドゥオペレーションを追加するためのオペレーションファクトリクラスです。
    /// </summary>
    public sealed class RedoOperationFactory : PluginOperationFactory
    {
        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public RedoOperationFactory()
            : base("Redo")
        {
        }

        /// <summary>
        /// オペレーションが作成できるか判定します。
        /// </summary>
        /// <param name="args">オペレーションの引数です。</param>
        /// <returns>作成可能な場合は、真を返します。</returns>
        protected override bool CanCreateOperation(params object[] args)
        {
            var presenter = GlobalPresenter.GetOperationPresenter();
            Contract.Assume(presenter != null);

            return presenter.CanRedo();
        }

        /// <summary>
        /// オペレーションを作成します。
        /// </summary>
        /// <param name="args">オペレーションの引数です。</param>
        /// <returns>オペレーションのインスタンスを返します。</returns>
        protected override Operation CreateOperation(params object[] args)
        {
            return new RedoOperation();
        }
    }
}
