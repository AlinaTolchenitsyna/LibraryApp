using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryApp.BL
{
    internal class TTextbox
    {
        /// <summary>
        /// Метод проверки символа на соответствие цифре
        /// </summary>
        /// <param name="e"></param>
        internal static void CheckSymbol(TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c))
                {
                    e.Handled = true; // Блокировать ввод символа, если это не цифра
                    return;
                }
            }
        }

        /// <summary>
        /// Метод проверки вставленного из буфера обмена текста на соответствие цифре
        /// </summary>
        /// <param name="e"></param>
        internal static void CheckPasting(DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                string text = (string)e.DataObject.GetData(DataFormats.Text);
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        /// <summary>
        /// Проверка текста на то, что он является цифрой
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static bool IsTextAllowed(string text)
        {
            foreach (char c in text)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Метод блокировки ввода пробела
        /// </summary>
        /// <param name="e"></param>
        internal static void BlockSpace(KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true; // Блокировать ввод
            }
        }
    }
}
