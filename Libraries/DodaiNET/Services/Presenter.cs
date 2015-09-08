//-----------------------------------------------------------------------
// <copyright file="Presenter.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Services
{
    using System.Diagnostics.Contracts;
    using Dodai.ComponentModel;

    /// <summary>
    /// プレゼンタークラスです。
    /// </summary>
    public abstract class Presenter : DisposableObject, IPresenter
    {
        private readonly string presenterKey;

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        /// <param name="key">プレゼンターキーです。</param>
        protected Presenter(string key)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(key));
            this.presenterKey = key;
        }

        /// <summary>
        /// プレゼンターキーを取得します。
        /// </summary>
        public string PresenterKey
        {
            get
            {
                return this.presenterKey;
            }
        }
    }
}
