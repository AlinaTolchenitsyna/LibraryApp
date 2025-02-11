using LibraryApp.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LibraryApp.BL
{
    internal class TLabel
    {
        internal static void UpdateLabel(int content, System.Windows.Controls.Label LblNoResults)
        {
            if (content == 0)
            {
                LblNoResults.Visibility = Visibility.Visible;
            }
            else
            {
                LblNoResults.Visibility = Visibility.Hidden;
            }
            return;
        }
    }
}
