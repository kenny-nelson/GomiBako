//-----------------------------------------------------------------------
// <copyright file="ActiveChangedEventArgs.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// アクティブ変更後イベントの引数クラスです。
    /// </summary>
    internal class ActiveChangedEventArgs : EventArgs
    {
        private bool isActive;

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        /// <param name="active">アクティブな状態を渡します。</param>
        internal ActiveChangedEventArgs(bool active)
        {
            this.isActive = active;
        }

        /// <summary>
        /// アクティブ状態かどうか状態を取得します。
        /// </summary>
        internal bool IsActive
        {
            get
            {
                return this.isActive;
            }
        }
    }
}
