//-----------------------------------------------------------------------
// <copyright file="LayoutHelper.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Helpers
{    
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dodai;
    using Xceed.Wpf.AvalonDock.Layout;
    using Xceed.Wpf.AvalonDock.Layout.Serialization;

    /// <summary>
    /// レイアウトヘルパークラスです。
    /// </summary>
    public static class LayoutHelper
    {
        /// <summary>
        /// レイアウトのコンフィグファイル名です。
        /// </summary>
        public const string LayoutConfigFile = @".\Dodai.AvalonDock.config";

        /// <summary>
        /// アヴァロンドックのコンフィグファイルが存在するか判定します。
        /// </summary>
        /// <returns>存在すれば、真を返します。</returns>
        public static bool HasAvalonDockConfigFile()
        {
            return File.Exists(LayoutConfigFile);
        }

        /// <summary>
        /// レイアウトのシリアライズ処理を呼び出します。
        /// </summary>
        /// <param name="sender">送信元のオブジェクトです。</param>
        /// <param name="e">イベント引数です。</param>
        public static void CallLayoutSerialization(object sender, LayoutSerializationCallbackEventArgs e)
        {
            var anchorable = e.Model as LayoutAnchorable;
            if (anchorable != null)
            {
                var panelPresenter = GlobalPresenter.GetPanelPresenter();
                var factory = GlobalPresenter.GetToolFactory(e.Model.ContentId);
                if (factory != null)
                {
                    var tool = GlobalPresenter.AddTool(factory, panelPresenter);
                    e.Content = tool;
                    tool.IsVisible = anchorable.IsVisible;
                    if (anchorable.IsActive)
                    {
                        tool.IsActive = true;
                    }

                    tool.IsSelected = anchorable.IsSelected;
                }
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
