//-----------------------------------------------------------------------
// <copyright file="TextureListViewModel.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Naya.Graphics.Modules.TextureList
{    
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dodai.Services;
    using Dodai.ViewModels;

    /// <summary>
    /// テクスチャリストのビューモデルクラスです。
    /// </summary>
    public class TextureListViewModel : ToolViewModel
    {
        private readonly DataTable dataTable =
            new DataTable();

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public TextureListViewModel()
        {
            this.dataTable.Columns.Add("Name", Type.GetType("System.String"));
        }

        /// <summary>
        /// データテーブルを取得します。
        /// </summary>
        public DataTable DataTable
        {
            get
            {
                return this.dataTable;
            }
        }

        /// <summary>
        /// ドキュメントのアクティブ状態変更を通知します。
        /// </summary>
        /// <param name="e">イベント引数です。</param>
        protected override void OnActiveDocumentChanged(ActiveDocumentChangedEventArgs e)
        {
        }
    }
}
