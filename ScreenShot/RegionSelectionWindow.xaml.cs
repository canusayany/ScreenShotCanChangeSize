using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Screenshot
{
    /// <summary>
    /// Defines the <see cref="RegionSelectionWindow" />.
    /// </summary>
    public partial class RegionSelectionWindow
    {
        #region Fields

        /// <summary>
        /// Defines the IsCancelShot.
        /// </summary>
        public static bool IsCancelShot = false;

        /// <summary>
        /// Defines the _selectionStartPosForFirst.
        /// </summary>
        private Point? _selectionStartPosForFirst;

        /// <summary>
        /// Defines the isFirstComeIn.
        /// </summary>
        private bool isFirstComeIn = true;

        /// <summary>
        /// Defines the isMouseLeftButtonDown.
        /// </summary>
        private bool isMouseLeftButtonDown = false;

        /// <summary>
        /// Defines the selectBoderRightDownPosition.
        /// </summary>
        private Point? selectBoderRightDownPosition = null;

        /// <summary>
        /// Defines the selectBoderLeftTopPosition.
        /// </summary>
        private Point? selectBoderLeftTopPosition = null;

        /// <summary>
        /// Defines the toolBarLeftTopPosition.
        /// </summary>
        private Point? toolBarLeftTopPosition = null;

        /// <summary>
        /// Defines the mouseDownPosition.
        /// </summary>
        private Point? mouseDownPosition = null;

        /// <summary>
        /// Defines the funType.
        /// </summary>
        private FunType funType = new FunType();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionSelectionWindow"/> class.
        /// </summary>
        public RegionSelectionWindow()
        {
            InitializeComponent();

            Loaded += (s, e) => Activate();
            Cursor = Cursors.Cross;
            funType = FunType.Select;
        }

        #endregion Constructors

        #region Enums

        /// <summary>
        /// Defines the FunType.
        /// </summary>
        private enum FunType
        {
            /// <summary>
            /// Defines the Select.
            /// </summary>
            Select,

            /// <summary>
            /// Defines the MoveAll.
            /// </summary>
            MoveAll,

            /// <summary>
            /// Defines the MoveLeftTop.
            /// </summary>
            MoveLeftTop,

            /// <summary>
            /// Defines the MoveLeftMid.
            /// </summary>
            MoveLeftMid,

            /// <summary>
            /// Defines the MoveLeftDown.
            /// </summary>
            MoveLeftDown,

            /// <summary>
            /// Defines the MoveTopMid.
            /// </summary>
            MoveTopMid,

            /// <summary>
            /// Defines the MoveRightTop.
            /// </summary>
            MoveRightTop,

            /// <summary>
            /// Defines the MoveRightMid.
            /// </summary>
            MoveRightMid,

            /// <summary>
            /// Defines the moveRightDown.
            /// </summary>
            moveRightDown,

            /// <summary>
            /// Defines the MoveDownMid.
            /// </summary>
            MoveDownMid,

            /// <summary>
            /// Defines the Cancle.
            /// </summary>
            Cancle,

            /// <summary>
            /// Defines the Ok.
            /// </summary>
            Ok,

            /// <summary>
            /// Defines the nul.
            /// </summary>
            nul
        }

        #endregion Enums

        #region Properties

        /// <summary>
        /// Gets the SelectedRegion.
        /// </summary>
        public Rect? SelectedRegion { get; private set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// The GetAbsolutePlacement.
        /// </summary>
        /// <param name="element">The element<see cref="FrameworkElement"/>.</param>
        /// <param name="relativeToScreen">The relativeToScreen<see cref="bool"/>.</param>
        /// <returns>The <see cref="Rect"/>.</returns>
        public Rect GetAbsolutePlacement(FrameworkElement element, bool relativeToScreen = true)
        {
            var absolutePos = element.PointToScreen(new System.Windows.Point(0, 0));
            if (relativeToScreen)
            {
                return new Rect(absolutePos.X, absolutePos.Y, element.ActualWidth, element.ActualHeight);
            }
            var posMW = Application.Current.MainWindow.PointToScreen(new System.Windows.Point(0, 0));
            absolutePos = new System.Windows.Point(absolutePos.X - posMW.X, absolutePos.Y - posMW.Y);
            return new Rect(absolutePos.X, absolutePos.Y, element.ActualWidth, element.ActualHeight);
        }

        /// <summary>
        /// The OnKeyDown.
        /// </summary>
        /// <param name="e">The e<see cref="KeyEventArgs"/>.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }

        /// <summary>
        /// The OnMouseLeftButtonDown.
        /// </summary>
        /// <param name="e">The e<see cref="MouseButtonEventArgs"/>.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            isMouseLeftButtonDown = true;
            mouseDownPosition = e.GetPosition(this);
            selectBoderRightDownPosition = new Point(Canvas.GetLeft(SelectionBorder) + SelectionBorder.Width, Canvas.GetTop(SelectionBorder) + SelectionBorder.Height);
            selectBoderLeftTopPosition = new Point(Canvas.GetLeft(SelectionBorder), Canvas.GetTop(SelectionBorder));
            toolBarLeftTopPosition = new Point(Canvas.GetLeft(Bor_Tool), Canvas.GetTop(Bor_Tool));
            Bor_Tool.Visibility = Visibility.Hidden;
            if (isFirstComeIn)
            {
                _selectionStartPosForFirst = e.GetPosition(this);
                Mouse.Capture(this);
                return;
            }

            if (Cursor == Cursors.SizeAll)
            {
            }
        }

        /// <summary>
        /// The OnMouseLeftButtonUp.
        /// </summary>
        /// <param name="e">The e<see cref="MouseButtonEventArgs"/>.</param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            isMouseLeftButtonDown = false;
            isFirstComeIn = false;
            funType = FunType.nul;
            Cursor = Cursors.Arrow;
            Bor_Tool.Visibility = Visibility.Visible;
            if (!Equals(Mouse.Captured, this) || _selectionStartPosForFirst == null)
            {
                return;
            }

            //SelectedRegion = new Rect(_selectionStartPos.Value, e.GetPosition(this));

            _selectionStartPosForFirst = null;
            Cursor = Cursors.Arrow;
            Mouse.Capture(null);
        }

        /// <summary>
        /// The OnMouseMove.
        /// </summary>
        /// <param name="e">The e<see cref="MouseEventArgs"/>.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!isMouseLeftButtonDown)
            {
                return;
            }
            double ellSize = Ell_DownMid.Width;
            switch (funType)
            {
                case FunType.Select:
                    var position = e.GetPosition(this);
                    var left = Math.Min(_selectionStartPosForFirst.Value.X, position.X);
                    var top = Math.Min(_selectionStartPosForFirst.Value.Y, position.Y);
                    Canvas.SetLeft(SelectionImage, -left);
                    Canvas.SetTop(SelectionImage, -top);
                    Canvas.SetLeft(SelectionBorder, left);
                    Canvas.SetTop(SelectionBorder, top);
                    SelectionBorder.Width = Math.Abs(position.X - _selectionStartPosForFirst.Value.X);
                    SelectionBorder.Height = Math.Abs(position.Y - _selectionStartPosForFirst.Value.Y);
                    Canvas.SetLeft(ChangeBoderSize, left - ellSize / 2);
                    Canvas.SetTop(ChangeBoderSize, top - ellSize / 2);
                    ChangeBoderSize.Width = Math.Abs(position.X - _selectionStartPosForFirst.Value.X) + ellSize;
                    ChangeBoderSize.Height = Math.Abs(position.Y - _selectionStartPosForFirst.Value.Y) + ellSize;
                    //ChangeTool bar Position
                    Canvas.SetLeft(Bor_Tool, left + Math.Abs(position.X - _selectionStartPosForFirst.Value.X) - 65);
                    Canvas.SetTop(Bor_Tool, top + Math.Abs(position.Y - _selectionStartPosForFirst.Value.Y) + 5);
                    return;

                case FunType.MoveAll:
                    position = e.GetPosition(this);
                    left = selectBoderLeftTopPosition.Value.X - (mouseDownPosition.Value.X - position.X);
                    top = selectBoderLeftTopPosition.Value.Y - (mouseDownPosition.Value.Y - position.Y);

                    Canvas.SetLeft(SelectionImage, -left);
                    Canvas.SetTop(SelectionImage, -top);

                    Canvas.SetLeft(SelectionBorder, left);
                    Canvas.SetTop(SelectionBorder, top);
                    //ChangeBoderSize
                    Canvas.SetLeft(ChangeBoderSize, left - ellSize / 2);
                    Canvas.SetTop(ChangeBoderSize, top - ellSize / 2);
                    //ChangeTool bar Position
                    Canvas.SetLeft(Bor_Tool, toolBarLeftTopPosition.Value.X - (mouseDownPosition.Value.X - position.X));
                    Canvas.SetTop(Bor_Tool, toolBarLeftTopPosition.Value.Y - (mouseDownPosition.Value.Y - position.Y));
                    return;

                case FunType.MoveLeftTop:

                    position = e.GetPosition(this);

                    left = Math.Min(selectBoderRightDownPosition.Value.X, position.X);
                    top = Math.Min(selectBoderRightDownPosition.Value.Y, position.Y);
                    // text_RightDown.Text = "";
                    // text_RightDown.Text += ("\r" + "lefttop:" + $"left:{left}.top:{top}");
                    Canvas.SetLeft(SelectionImage, -left);
                    Canvas.SetTop(SelectionImage, -top);

                    Canvas.SetLeft(SelectionBorder, left);
                    Canvas.SetTop(SelectionBorder, top);
                    SelectionBorder.Width = Math.Abs(position.X - selectBoderRightDownPosition.Value.X);
                    SelectionBorder.Height = Math.Abs(position.Y - selectBoderRightDownPosition.Value.Y);
                    //ChangeBoderSize
                    Canvas.SetLeft(ChangeBoderSize, left - ellSize / 2);
                    Canvas.SetTop(ChangeBoderSize, top - ellSize / 2);
                    ChangeBoderSize.Width = Math.Abs(position.X - selectBoderRightDownPosition.Value.X) + ellSize;
                    ChangeBoderSize.Height = Math.Abs(position.Y - selectBoderRightDownPosition.Value.Y) + ellSize;
                    //ChangeTool bar Position
                    Canvas.SetLeft(Bor_Tool, left + Math.Abs(position.X - selectBoderRightDownPosition.Value.X) - 65);
                    Canvas.SetTop(Bor_Tool, top + Math.Abs(position.Y - selectBoderRightDownPosition.Value.Y) + 5);
                    return;

                case FunType.MoveLeftMid:
                    position = e.GetPosition(this);

                    left = Math.Min(selectBoderRightDownPosition.Value.X, position.X);

                    Canvas.SetLeft(SelectionImage, -left);

                    Canvas.SetLeft(SelectionBorder, left);
                    SelectionBorder.Width = Math.Abs(position.X - selectBoderRightDownPosition.Value.X);
                    //ChangeBoderSize
                    Canvas.SetLeft(ChangeBoderSize, left - ellSize / 2);
                    ChangeBoderSize.Width = Math.Abs(position.X - selectBoderRightDownPosition.Value.X) + ellSize;
                    //ChangeTool bar Position
                    Canvas.SetLeft(Bor_Tool, left + Math.Abs(position.X - selectBoderRightDownPosition.Value.X) - 65);
                    break;
                //todo 一下代码有问题
                case FunType.MoveLeftDown:
                    OperateLeftDownMove(e, ellSize);
                    break;

                case FunType.MoveTopMid:
                    position = e.GetPosition(this);
                    top = Math.Min(selectBoderRightDownPosition.Value.Y, position.Y);
                    Canvas.SetTop(SelectionImage, -top);
                    Canvas.SetTop(SelectionBorder, top);
                    SelectionBorder.Height = Math.Abs(position.Y - selectBoderRightDownPosition.Value.Y);
                    //ChangeBoderSize
                    Canvas.SetTop(ChangeBoderSize, top - ellSize / 2);
                    ChangeBoderSize.Height = Math.Abs(position.Y - selectBoderRightDownPosition.Value.Y) + ellSize;
                    //ChangeTool bar Position
                    Canvas.SetTop(Bor_Tool, top + Math.Abs(position.Y - selectBoderRightDownPosition.Value.Y) + 5);
                    break;

                case FunType.MoveRightTop:
                    OperateRightTopMove(e, ellSize);
                    break;

                case FunType.MoveRightMid:
                    position = e.GetPosition(this);

                    left = Math.Min(selectBoderLeftTopPosition.Value.X, position.X);

                    Canvas.SetLeft(SelectionImage, -left);

                    Canvas.SetLeft(SelectionBorder, left);
                    SelectionBorder.Width = Math.Abs(position.X - selectBoderRightDownPosition.Value.X + (selectBoderRightDownPosition - selectBoderLeftTopPosition).Value.X);
                    //ChangeBoderSize
                    Canvas.SetLeft(ChangeBoderSize, left - ellSize / 2);
                    ChangeBoderSize.Width = SelectionBorder.Width + ellSize;
                    //ChangeTool bar Position
                    Canvas.SetLeft(Bor_Tool, left + SelectionBorder.Width - 65);
                    break;

                case FunType.moveRightDown:
                    position = e.GetPosition(this);
                    left = Math.Min(selectBoderLeftTopPosition.Value.X, position.X);
                    top = Math.Min(selectBoderLeftTopPosition.Value.Y, position.Y);
                    //text_RightDown.Text = "";
                    //text_RightDown.Text += "\r" + ($"selectBoderLeftTopPosition.Value.X:{selectBoderLeftTopPosition.Value.X},position.X:{position.X} ");
                    //text_RightDown.Text += "\r" + ($"selectBoderLeftTopPosition.Value.Y:{selectBoderLeftTopPosition.Value.Y},position.Y:{position.Y} ");
                    //text_RightDown.Text += "\r" + ($"设置顶点 left:{left}   top:{top} ");
                    //text_RightDown.Text += "\r" + ($"鼠标实时位置 position:{position}");
                    //text_RightDown.Text += "\r" + ($"鼠标按下位置mouseDownPosition:{mouseDownPosition}");
                    //text_RightDown.Text += "\r" + ($"原始宽度:{(selectBoderRightDownPosition - selectBoderLeftTopPosition).Value.X}");
                    //text_RightDown.Text += "\r" + ($"原始高度:{(selectBoderRightDownPosition - selectBoderLeftTopPosition).Value.Y}");
                    //text_RightDown.Text += "\r" + ($"设置前高度{ ChangeBoderSize.Height} 宽度:{  SelectionBorder.Width}");
                    Canvas.SetLeft(SelectionImage, -left);
                    Canvas.SetTop(SelectionImage, -top);
                    Canvas.SetLeft(SelectionBorder, left);
                    Canvas.SetTop(SelectionBorder, top);
                    Console.WriteLine();
                    SelectionBorder.Width = Math.Abs((selectBoderRightDownPosition - selectBoderLeftTopPosition).Value.X - (mouseDownPosition.Value.X - position.X));
                    SelectionBorder.Height = Math.Abs((selectBoderRightDownPosition - selectBoderLeftTopPosition).Value.Y - (mouseDownPosition.Value.Y - position.Y));
                    // text_RightDown.Text += "\r" + $"设置后高度:{ ChangeBoderSize.Height} 宽度:{  SelectionBorder.Width}";
                    //ChangeBoderSize
                    Canvas.SetLeft(ChangeBoderSize, left - ellSize / 2);
                    Canvas.SetTop(ChangeBoderSize, top - ellSize / 2);
                    ChangeBoderSize.Width = SelectionBorder.Width + ellSize;
                    ChangeBoderSize.Height = SelectionBorder.Height + ellSize;
                    //ChangeTool bar Position
                    Canvas.SetLeft(Bor_Tool, left + SelectionBorder.Width - 65);
                    Canvas.SetTop(Bor_Tool, top + SelectionBorder.Height + 5);
                    break;

                case FunType.MoveDownMid:
                    position = e.GetPosition(this);
                    top = Math.Min(selectBoderLeftTopPosition.Value.Y, position.Y);
                    Canvas.SetTop(SelectionImage, -top);
                    Canvas.SetTop(SelectionBorder, top);
                    SelectionBorder.Height = Math.Abs(position.Y - selectBoderRightDownPosition.Value.Y + (selectBoderRightDownPosition - selectBoderLeftTopPosition).Value.Y);
                    //ChangeBoderSize
                    Canvas.SetTop(ChangeBoderSize, top - ellSize / 2);
                    ChangeBoderSize.Height = SelectionBorder.Height + ellSize;
                    //ChangeTool bar Position
                    Canvas.SetTop(Bor_Tool, top + SelectionBorder.Height + 5);
                    break;

                case FunType.Cancle:
                    break;

                case FunType.Ok:
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// The Ell_DownMid_MouseLeftButtonDown.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="MouseButtonEventArgs"/>.</param>
        private void Ell_DownMid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Cursor = Cursors.SizeNS;
            funType = FunType.MoveDownMid;
        }

        /// <summary>
        /// The Ell_LeftDown_MouseLeftButtonDown.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="MouseButtonEventArgs"/>.</param>
        private void Ell_LeftDown_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Cursor = Cursors.SizeNESW;
            funType = FunType.MoveLeftDown;
        }

        /// <summary>
        /// The Ell_LeftMid_MouseLeftButtonDown.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="MouseButtonEventArgs"/>.</param>
        private void Ell_LeftMid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Cursor = Cursors.SizeWE;
            funType = FunType.MoveLeftMid;
        }

        /// <summary>
        /// The Ell_LeftTop_MouseLeftButtonDown.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="MouseButtonEventArgs"/>.</param>
        private void Ell_LeftTop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Cursor = Cursors.SizeNWSE;
            funType = FunType.MoveLeftTop;
        }

        /// <summary>
        /// The Ell_MouseLeftButtonMove.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="MouseEventArgs"/>.</param>
        private void Ell_MouseLeftButtonMove(object sender, MouseEventArgs e)
        {
            if (isMouseLeftButtonDown)
            {
                double ellSize = Ell_DownMid.Width;
                var position = e.GetPosition(this);

                var left = Math.Min(selectBoderRightDownPosition.Value.X, position.X);
                var top = Math.Min(selectBoderRightDownPosition.Value.Y, position.Y);

                Canvas.SetLeft(SelectionImage, -left);
                Canvas.SetTop(SelectionImage, -top);

                Canvas.SetLeft(SelectionBorder, left);
                Canvas.SetTop(SelectionBorder, top);
                SelectionBorder.Width = Math.Abs(position.X - selectBoderRightDownPosition.Value.X);
                SelectionBorder.Height = Math.Abs(position.Y - selectBoderRightDownPosition.Value.Y);
                //ChangeBoderSize
                Canvas.SetLeft(ChangeBoderSize, left - ellSize / 2);
                Canvas.SetTop(ChangeBoderSize, top - ellSize / 2);
                ChangeBoderSize.Width = Math.Abs(position.X - selectBoderRightDownPosition.Value.X) + ellSize;
                ChangeBoderSize.Height = Math.Abs(position.Y - selectBoderRightDownPosition.Value.Y) + ellSize;
                //ChangeTool bar Position
                Canvas.SetLeft(Bor_Tool, left + Math.Abs(position.X - selectBoderRightDownPosition.Value.X) - 65);
                Canvas.SetTop(Bor_Tool, top + Math.Abs(position.Y - selectBoderRightDownPosition.Value.Y) + 5);
            }
        }

        /// <summary>
        /// The Ell_RightDown_MouseLeftButtonDown.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="MouseButtonEventArgs"/>.</param>
        private void Ell_RightDown_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Cursor = Cursors.SizeNWSE;
            funType = FunType.moveRightDown;
        }

        /// <summary>
        /// The Ell_RightMid_MouseLeftButtonDown.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="MouseButtonEventArgs"/>.</param>
        private void Ell_RightMid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Cursor = Cursors.SizeWE;
            funType = FunType.MoveRightMid;
        }

        /// <summary>
        /// The Ell_RightTop_MouseLeftButtonDown.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="MouseButtonEventArgs"/>.</param>
        private void Ell_RightTop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Cursor = Cursors.SizeNESW;
            funType = FunType.MoveRightTop;
        }

        /// <summary>
        /// The Ell_TopMid_MouseLeftButtonDown.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="MouseButtonEventArgs"/>.</param>
        private void Ell_TopMid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Cursor = Cursors.SizeNS;
            funType = FunType.MoveTopMid;
        }

        /// <summary>
        /// The Image_Cancle_MouseLeftButtonDown.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="MouseButtonEventArgs"/>.</param>
        private void Image_Cancle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectedRegion = null;
            Close();
        }

        /// <summary>
        /// The Image_OK_MouseLeftButtonDown.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="MouseButtonEventArgs"/>.</param>
        private void Image_OK_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectedRegion = GetAbsolutePlacement(SelectionBorder);
            Close();
        }

        /// <summary>
        /// The OperateLeftDownMove.
        /// </summary>
        /// <param name="e">The e<see cref="MouseEventArgs"/>.</param>
        /// <param name="ellSize">The ellSize<see cref="double"/>.</param>
        private void OperateLeftDownMove(MouseEventArgs e, double ellSize)
        {
            Point position = e.GetPosition(this);
            Point rightTop = new Point(selectBoderRightDownPosition.Value.X, selectBoderLeftTopPosition.Value.Y);

            double left = Math.Min(rightTop.X, position.X);
            double top = Math.Min(rightTop.Y, position.Y);

            Canvas.SetLeft(SelectionImage, -left);
            Canvas.SetTop(SelectionImage, -top);

            Canvas.SetLeft(SelectionBorder, left);
            Canvas.SetTop(SelectionBorder, top);
            SelectionBorder.Width = Math.Abs((selectBoderRightDownPosition - selectBoderLeftTopPosition).Value.X + (mouseDownPosition.Value.X - position.X));
            SelectionBorder.Height = Math.Abs((selectBoderRightDownPosition - selectBoderLeftTopPosition).Value.Y - (mouseDownPosition.Value.Y - position.Y));
            Canvas.SetLeft(ChangeBoderSize, left - ellSize / 2);
            Canvas.SetTop(ChangeBoderSize, top - ellSize / 2);
            ChangeBoderSize.Width = SelectionBorder.Width + ellSize;
            ChangeBoderSize.Height = SelectionBorder.Height + ellSize;
            //ChangeTool bar Position
            Canvas.SetLeft(Bor_Tool, left + SelectionBorder.Width - 65);
            Canvas.SetTop(Bor_Tool, top + SelectionBorder.Height + 5);
        }

        /// <summary>
        /// The OperateRightTopMove.
        /// </summary>
        /// <param name="e">The e<see cref="MouseEventArgs"/>.</param>
        /// <param name="ellSize">The ellSize<see cref="double"/>.</param>
        private void OperateRightTopMove(MouseEventArgs e, double ellSize)
        {
            var position = e.GetPosition(this);
            Point leftdown = new Point(selectBoderLeftTopPosition.Value.X, selectBoderRightDownPosition.Value.Y);

            var left = Math.Min(position.X, leftdown.X);
            var top = Math.Min(position.Y, leftdown.Y);
            Canvas.SetLeft(SelectionImage, -left);
            Canvas.SetTop(SelectionImage, -top);

            Canvas.SetLeft(SelectionBorder, left);
            Canvas.SetTop(SelectionBorder, top);

            SelectionBorder.Width = Math.Abs((selectBoderRightDownPosition - selectBoderLeftTopPosition).Value.X - (mouseDownPosition.Value.X - position.X));
            SelectionBorder.Height = Math.Abs((selectBoderRightDownPosition - selectBoderLeftTopPosition).Value.Y + (mouseDownPosition.Value.Y - position.Y));
            //ChangeBoderSize
            Canvas.SetLeft(ChangeBoderSize, left - ellSize / 2);
            Canvas.SetTop(ChangeBoderSize, top - ellSize / 2);
            ChangeBoderSize.Width = SelectionBorder.Width + ellSize;
            ChangeBoderSize.Height = SelectionBorder.Height + ellSize;
            //ChangeTool bar Position
            Canvas.SetLeft(Bor_Tool, left + SelectionBorder.Width - 65);
            Canvas.SetTop(Bor_Tool, top + SelectionBorder.Height + 5);
        }

        /// <summary>
        /// The SelectionBorder_MouseLeftButtonDown.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="MouseButtonEventArgs"/>.</param>
        private void SelectionBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Cursor = Cursors.SizeAll;
            funType = FunType.MoveAll;
        }

        #endregion Methods
    }
}