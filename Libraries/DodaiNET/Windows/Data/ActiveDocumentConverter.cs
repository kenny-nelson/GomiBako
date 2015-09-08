//-----------------------------------------------------------------------
// <copyright file="ActiveDocumentConverter.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dodai.Windows.Data
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using Dodai.ViewModels;

    /// <summary>
    /// アクティブドキュメント通知するためのコンバータークラスです。
    /// </summary>
    public sealed class ActiveDocumentConverter : IValueConverter
    {
        /// <summary>
        /// 値を変換します。
        /// </summary>
        /// <param name="value">バインディング ソースによって生成された値です。</param>
        /// <param name="targetType">バインディング ターゲット プロパティの型です。</param>
        /// <param name="parameter">使用するコンバーター パラメーターです。</param>
        /// <param name="culture">コンバーターで使用するカルチャです。</param>
        /// <returns>変換された値を返します。</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DocumentViewModel)
            {
                return value;
            }

            return Binding.DoNothing;
        }

        /// <summary>
        /// 値を変換します。
        /// </summary>
        /// <param name="value">バインディング ターゲットによって生成される値です。</param>
        /// <param name="targetType">変換後の型です。</param>
        /// <param name="parameter">使用するコンバーター パラメーターです。</param>
        /// <param name="culture">コンバーターで使用するカルチャです。</param>
        /// <returns>変換された値を返します。</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DocumentViewModel)
            {
                return value;
            }

            return Binding.DoNothing;
        }
    }
}
