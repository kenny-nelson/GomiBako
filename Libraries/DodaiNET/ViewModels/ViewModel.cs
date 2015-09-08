//-----------------------------------------------------------------------
// <copyright file="ViewModel.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dodai.ComponentModel;

    /// <summary>
    /// ビューモデルの抽象クラスです。
    /// </summary>
    public abstract class ViewModel : DisposableNotifiableObject
    {
        /// <summary>
        /// コンストラクタです。
        /// </summary>
        protected ViewModel()
        {
        }

        /// <summary>
        /// 廃棄します。内部処理
        /// </summary>
        protected override void DisposeInternal()
        {
        }
    }
}