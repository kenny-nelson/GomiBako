//-----------------------------------------------------------------------
// <copyright file="DispatcherHelper.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Helpers
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Threading;

    /// <summary>
    /// ディスパッチャヘルパークラスです。
    /// </summary>
    internal static class DispatcherHelper
    {
        private static Dispatcher uiDispatcher;

        /// <summary>
        /// UIディスパッチャを取得設定します。
        /// </summary>
        public static Dispatcher UIDispatcher
        {
            get
            {
                if ((bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue)
                {
                    uiDispatcher = Dispatcher.CurrentDispatcher;
                }

                return uiDispatcher;
            }

            set
            {
                uiDispatcher = value;
            }
        }
    }
}
