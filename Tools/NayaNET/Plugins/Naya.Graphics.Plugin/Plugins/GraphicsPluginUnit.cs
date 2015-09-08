//-----------------------------------------------------------------------
// <copyright file="GraphicsPluginUnit.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Naya.Graphics.Plugins
{    
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dodai;
    using Dodai.Plugins;
    using Naya.Graphics.Repositories;

    /// <summary>
    /// グラフィックスに関連するプラグインユニットクラスです。
    /// </summary>
    [Export(typeof(PluginUnit))]
    public class GraphicsPluginUnit : PluginUnit
    {
        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public GraphicsPluginUnit()
            : base("Naya.Graphics")
        {
        }

        /// <summary>
        /// ツールファクトリクラスを取得します。
        /// </summary>
        /// <returns>ツールファクトリのインスタンスを返します。</returns>
        public override IEnumerable<PluginToolFactory> GetToolFactories()
        {
            yield return new TextureListToolFactory();            
        }

        /// <summary>
        /// 登録開始時に実行される処理です。
        /// </summary>
        public override void BeginRegistration()
        {
            var repositoryPresenter = GlobalPresenter.GetRepositoryPresenter();
            Contract.Assume(repositoryPresenter != null);

            if (!repositoryPresenter.HasRepository<GraphicsRepository>())
            {
                var repository = new GraphicsRepository();
                repositoryPresenter.AddRepository(repository);
            }
        }

        /// <summary>
        /// 登録終了時に実行される処理です。
        /// </summary>
        public override void EndRegistration()
        {
        }
    }
}
