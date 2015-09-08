//-----------------------------------------------------------------------
// <copyright file="LayoutUpdater.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Layout
{    
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using Dodai.Services;
    using Xceed.Wpf.AvalonDock.Layout;

    /// <summary>
    /// レイアウト更新クラスです。
    /// </summary>
    public sealed class LayoutUpdater : ILayoutUpdateStrategy
    {
        private enum InsertPosition
        {
            Start,
            End
        }

        /// <summary>
        /// アンカー挿入前の処理です。
        /// </summary>
        /// <param name="layout">レイアウトです。</param>
        /// <param name="anchorableToShow">アンカーの表示状態です。</param>
        /// <param name="destinationContainer">送り先のコンテナです。</param>
        /// <returns>挿入可能ならば、真を返します。</returns>
        public bool BeforeInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableToShow, ILayoutContainer destinationContainer)
        {
            var tool = anchorableToShow.Content as IToolable;
            if (tool != null)
            {
                var location = tool.Location;
                string paneName = GetPaneName(location);
                var toolsPane = layout.Descendents().OfType<LayoutAnchorablePane>().FirstOrDefault(d => d.Name == paneName);
                if (toolsPane == null)
                {
                    switch (location)
                    {
                        case PanelLocation.Left:
                            toolsPane = CreateAnchorablePane(layout, Orientation.Horizontal, paneName, InsertPosition.Start);
                            break;
                        case PanelLocation.Right:
                            toolsPane = CreateAnchorablePane(layout, Orientation.Horizontal, paneName, InsertPosition.End);
                            break;
                        case PanelLocation.Bottom:
                            toolsPane = CreateAnchorablePane(layout, Orientation.Vertical, paneName, InsertPosition.End);
                            break;
                        default:
                            toolsPane = CreateAnchorablePane(layout, Orientation.Horizontal, paneName, InsertPosition.End);
                            break;
                    }
                }

                toolsPane.Children.Add(anchorableToShow);
                return true;
            }

            return false;
        }

        /// <summary>
        /// アンカー挿入後の処理です。
        /// </summary>
        /// <param name="layout">レイアウトです。</param>
        /// <param name="anchorableShown">アンカーの表示状態です。</param>
        public void AfterInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableShown)
        {
            var tool = anchorableShown.Content as IToolable;
            if (tool != null)
            {
                var anchorablePane = anchorableShown.Parent as LayoutAnchorablePane;
                if (anchorablePane != null && anchorablePane.ChildrenCount == 1)
                {
                    switch (tool.Location)
                    {
                        case PanelLocation.Left:
                        case PanelLocation.Right:
                            anchorablePane.DockWidth = new GridLength(tool.Width, GridUnitType.Pixel);
                            break;
                        case PanelLocation.Bottom:
                            anchorablePane.DockHeight = new GridLength(tool.Height, GridUnitType.Pixel);
                            break;
                        default:
                            anchorablePane.DockWidth = new GridLength(tool.Width, GridUnitType.Pixel);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// ドキュメント挿入前の処理です。
        /// </summary>
        /// <param name="layout">レイアウトです。</param>
        /// <param name="anchorableToShow">アンカーの表示状態です。</param>
        /// <param name="destinationContainer">送り先のコンテナです。</param>
        /// <returns>挿入可能ならば、真を返します。</returns>
        public bool BeforeInsertDocument(LayoutRoot layout, LayoutDocument anchorableToShow, ILayoutContainer destinationContainer)
        {
            return false;
        }

        /// <summary>
        /// ドキュメント挿入後の処理です。
        /// </summary>
        /// <param name="layout">レイアウトです。</param>
        /// <param name="anchorableShown">アンカーの表示状態です。</param>
        public void AfterInsertDocument(LayoutRoot layout, LayoutDocument anchorableShown)
        {
        }

        private static string GetPaneName(PanelLocation location)
        {
            switch (location)
            {
                case PanelLocation.Left:
                    return "LeftPane";
                case PanelLocation.Right:
                    return "RightPane";
                case PanelLocation.Bottom:
                    return "BottomPane";
                default:
                    return "RightPane";
            }
        }

        private static LayoutAnchorablePane CreateAnchorablePane(
            LayoutRoot layout,
            Orientation orientation,
            string paneName,
            InsertPosition position)
        {
            var parent = layout.Descendents().OfType<LayoutPanel>().First();
            foreach (var panel in layout.Descendents().OfType<LayoutPanel>())
            {
                if (panel.Orientation == orientation)
                {
                    parent = panel;
                    break;
                }
            }

            var toolsPane = new LayoutAnchorablePane { Name = paneName };
            if (position == InsertPosition.Start)
            {
                parent.InsertChildAt(0, toolsPane);
            }
            else
            {
                parent.Children.Add(toolsPane);
            }

            return toolsPane;
        }
    }
}
