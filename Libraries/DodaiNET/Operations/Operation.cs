//-----------------------------------------------------------------------
// <copyright file="Operation.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Operations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// オペレーションの抽象クラスです。
    /// </summary>
    public abstract class Operation
    {
        private bool isScriptable = false;
        private string scriptMessage = string.Empty;

        /// <summary>
        /// オペレーションラベルを取得します。
        /// </summary>
        public abstract string Label
        {
            get;
        }

        /// <summary>
        /// オペレーションの処理結果を取得します。
        /// </summary>
        public abstract bool Result
        {
            get;
        }

        /// <summary>
        /// オペレーションの返り値を取得します。
        /// </summary>
        public abstract object ReturnValue
        {
            get;
        }

        /// <summary>
        /// オペレーションがマージ可能かどうか取得します。
        /// </summary>
        public virtual bool IsMergeable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// スクリプト可能化どうかを取得設定します。
        /// </summary>
        internal bool IsScriptable
        {
            get
            {
                return this.isScriptable;
            }

            set
            {
                this.isScriptable = value;
            }
        }

        /// <summary>
        /// スクリプト処理時のメッセージを取得設定します。
        /// </summary>
        internal string ScriptMessage
        {
            get
            {
                return this.scriptMessage;
            }

            set
            {
                this.scriptMessage = value;
            }
        }

        /// <summary>
        /// オペレーションを実行します。
        /// </summary>
        /// <returns>実行後のオペレーションを返します。</returns>
        public abstract Operation Run();

        /// <summary>
        /// アンドゥ可能かどうか判定します。
        /// </summary>
        /// <returns>アンドゥ可能ならば真を返します。</returns>
        public virtual bool CanUndoable()
        {
            return false;
        }    

        /// <summary>
        /// マージ処理用のハッシュコードを取得します。
        /// </summary>
        /// <returns>ハッシュコードを返します。</returns>
        public virtual int GetMergeableHashCode()
        {
            return 0;
        }        
    }
}
