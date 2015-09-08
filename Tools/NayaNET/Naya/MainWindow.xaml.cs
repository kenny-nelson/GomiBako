//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Naya
{    
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using Dodai.Helpers;
    using Xceed.Wpf.AvalonDock.Layout.Serialization;

    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// レイアウトをロードします。
        /// </summary>
        public void LoadLayout()
        {
            var layoutSerializer = new XmlLayoutSerializer(this.dockManager);
            layoutSerializer.LayoutSerializationCallback += LayoutHelper.CallLayoutSerialization;
            layoutSerializer.Deserialize(LayoutHelper.LayoutConfigFile);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.SaveLayout();
        }

        private void SaveLayout()
        {
            var layoutSerializer = new XmlLayoutSerializer(this.dockManager);
            layoutSerializer.Serialize(LayoutHelper.LayoutConfigFile);
        }
    }
}
