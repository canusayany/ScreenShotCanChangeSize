using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace Screenshot
{
    /// <summary>
    /// Defines the <see cref="BitmapExtensions" />.
    /// </summary>
    public static class BitmapExtensions
    {
        #region Methods

        /// <summary>
        /// The ToBitmapSource.
        /// </summary>
        /// <param name="bitmap">The bitmap<see cref="Bitmap"/>.</param>
        /// <returns>The <see cref="BitmapSource"/>.</returns>
        public static BitmapSource ToBitmapSource(this Bitmap bitmap)
        {
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream(stream.ToArray());
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        #endregion Methods
    }
}