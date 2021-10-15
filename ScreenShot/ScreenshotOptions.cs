using System.Windows.Media;

namespace Screenshot
{
    /// <summary>
    /// 截图界面设置.
    /// </summary>
    public class ScreenshotOptions
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenshotOptions"/> class.
        /// </summary>
        public ScreenshotOptions()
        {
            BackgroundOpacity = 0.5;
            SelectionRectangleBorderBrush = Brushes.LimeGreen;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the BackgroundOpacity
        /// Background opacity when selecting region to capture..
        /// </summary>
        public double BackgroundOpacity { get; set; }

        /// <summary>
        /// Gets or sets the SelectionRectangleBorderBrush
        /// Brush used to draw border of selection rectangle..
        /// </summary>
        public Brush SelectionRectangleBorderBrush { get; set; }

        #endregion Properties
    }
}