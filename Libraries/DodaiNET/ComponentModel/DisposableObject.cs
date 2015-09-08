//-----------------------------------------------------------------------
// <copyright file="DisposableObject.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.ComponentModel
{
    using System;

    /// <summary>
    /// 廃棄可能オブジェクトクラスです。
    /// </summary>
    public abstract class DisposableObject : SynchronizedObject, IDisposable
    {
        private bool disposed = false;
        
        /// <summary>
        /// 廃棄します。
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 廃棄します。
        /// </summary>
        /// <param name="disposing">廃棄フラグです。true 時に内部処理の廃棄メソッドを実行します。</param>
        protected void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.DisposeInternal();
            }

            this.disposed = true;
        }

        /// <summary>
        /// 廃棄します。内部処理
        /// </summary>
        protected abstract void DisposeInternal();
    }
}