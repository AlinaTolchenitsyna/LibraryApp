using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace LibraryApp.BL
{
    internal class TImage
    {

        /// <summary>
        /// Конвертирование изображения из массива байтов для отображения 
        /// </summary>
        /// <param name="imageData"></param>
        /// <returns></returns>
        internal static BitmapImage LoadBitmapImage(byte[] imageData)
        {
            if (imageData == null)
            {
                // Если изображение отсутствует, вернуть null
                return null; 
            }

            using (var ms = new MemoryStream(imageData))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                bitmap.Freeze(); 
                return bitmap;
            }
        }

        /// <summary>
        /// Метод выбора изображения с помощью проводника
        /// </summary>
        /// <param name="currentImageData"></param>
        /// <param name="BookImage"></param>
        internal static void SelectImage(ref byte[] currentImageData, System.Windows.Controls.Image BookImage)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            if (openFileDialog.ShowDialog() == true)
            {
                currentImageData = File.ReadAllBytes(openFileDialog.FileName);
                BookImage.Source = BL.TImage.LoadBitmapImage(currentImageData);
            }
        }

        

    }
}
