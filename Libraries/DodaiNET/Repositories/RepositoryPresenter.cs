//-----------------------------------------------------------------------
// <copyright file="RepositoryPresenter.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Dodai.Services;

    /// <summary>
    /// リポジトリプレゼンタークラスです。
    /// </summary>
    public sealed class RepositoryPresenter : Presenter, IRepositoryPresenter
    {
        private readonly Dictionary<Type, Repository> repositories =
            new Dictionary<Type, Repository>();

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public RepositoryPresenter()
            : base(Enum.GetName(typeof(PresenterKind), PresenterKind.Repository))
        {
        }

        /// <summary>
        /// リポジトリを取得します。
        /// </summary>
        /// <typeparam name="TRepository">取得対象のリポジトリの型です。</typeparam>
        /// <returns>指定リポジトリのインスタンスを返します。</returns>
        public TRepository GetRepository<TRepository>() where TRepository : Repository
        {
            lock (this.SyncObj)
            {
                var type = typeof(TRepository);
                Repository repository = null;
                if (this.repositories.TryGetValue(type, out repository))
                {
                    return repository as TRepository;
                }

                return null;
            }
        }

        /// <summary>
        /// リポジトリを追加します。
        /// </summary>
        /// <typeparam name="TRepository">追加するリポジトリの型です。</typeparam>
        /// <param name="repository">追加するリポジトリです。</param>
        public void AddRepository<TRepository>(TRepository repository) where TRepository : Repository
        {
            Contract.Assume(repository != null);

            lock (this.SyncObj)
            {
                var type = typeof(TRepository);
                if (!this.repositories.ContainsKey(type))
                {
                    this.repositories.Add(type, repository);
                }
            }
        }

        /// <summary>
        /// リポジトリを所有しているか判定します。
        /// </summary>
        /// <typeparam name="TRepository">リポジトリの型です。</typeparam>
        /// <returns>所有している場合は、真を返します。</returns>
        public bool HasRepository<TRepository>() where TRepository : Repository
        {
            lock (this.SyncObj)
            {
                var type = typeof(TRepository);
                return this.repositories.ContainsKey(type);
            }
        }

        internal IEnumerable<Repository> GetRepositories()
        {
            lock (this.SyncObj)
            {
                return this.repositories.Values;
            }
        }

        /// <summary>
        /// 廃棄します。内部処理
        /// </summary>
        protected override void DisposeInternal()
        {
            this.repositories.Clear();
        }
    }
}
