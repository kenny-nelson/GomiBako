//-----------------------------------------------------------------------
// <copyright file="RepositoryChangedEventArgs.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Repositories
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// リポジトリ変更イベント引数クラスです。
    /// </summary>
    public sealed class RepositoryChangedEventArgs : EventArgs
    {
        private readonly Repository target;
        private readonly object changeObject;

        internal RepositoryChangedEventArgs(Repository target, object changeObject)
        {
            Contract.Requires(target != null);
            Contract.Requires(changeObject != null);

            this.target = target;
            this.changeObject = changeObject;
        }

        /// <summary>
        /// 対象のリポジトリを取得します。
        /// </summary>
        public Repository Target
        {
            get
            {
                return this.target;
            }
        }

        /// <summary>
        /// 変更オブジェクトを取得します。
        /// </summary>
        public object ChangeObject
        {
            get
            {
                return this.changeObject;
            }
        }
    }
}
