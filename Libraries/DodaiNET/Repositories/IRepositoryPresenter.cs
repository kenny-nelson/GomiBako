//-----------------------------------------------------------------------
// <copyright file="IRepositoryPresenter.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dodai.Services;

    /// <summary>
    /// リポジトリプレゼンターのインターフェースです。
    /// </summary>
    public interface IRepositoryPresenter : IPresenter
    {
        /// <summary>
        /// リポジトリを取得します。
        /// </summary>
        /// <typeparam name="TRepository">取得対象のリポジトリの型です。</typeparam>
        /// <returns>指定リポジトリのインスタンスを返します。</returns>
        TRepository GetRepository<TRepository>() where TRepository : Repository;

        /// <summary>
        /// リポジトリを追加します。
        /// </summary>
        /// <typeparam name="TRepository">追加するリポジトリの型です。</typeparam>
        /// <param name="repository">追加するリポジトリです。</param>
        void AddRepository<TRepository>(TRepository repository) where TRepository : Repository;

        /// <summary>
        /// リポジトリを所有しているか判定します。
        /// </summary>
        /// <typeparam name="TRepository">リポジトリの型です。</typeparam>
        /// <returns>所有している場合は、真を返します。</returns>
        bool HasRepository<TRepository>() where TRepository : Repository;
    }
}
