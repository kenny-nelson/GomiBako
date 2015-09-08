//-----------------------------------------------------------------------
// <copyright file="PluginMenuFactory.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Plugins
{
    using System.Collections.Generic;
    using Dodai.Menus;

    /// <summary>
    /// プラグインメニューファクトリクラスです。
    /// </summary>
    public abstract class PluginMenuFactory : PluginFactory
    {
        /// <summary>
        /// コンストラクタです。
        /// </summary>
        /// <param name="factoryKey">ファクトリキーです。</param>
        protected PluginMenuFactory(string factoryKey)
            : base(factoryKey)
        {
        }

        /// <summary>
        /// メニューを取得します。
        /// </summary>
        /// <returns>メニューのインスタンスを返します。</returns>
        public virtual IEnumerable<Menu> GetMenus()
        {
            yield break;
        }
    }
}
