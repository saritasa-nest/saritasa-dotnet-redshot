using System.Diagnostics;
using Eto.Drawing;
using Eto.Forms;
using SkiaSharp;
using RedShot.Helpers.EditorView;
using RedShot.Helpers;
using Eto.Forms.Controls.SkiaSharp;

namespace RedShot.Recording.Forms
{
    /// <summary>
    /// Area selecting view.
    /// Functions: Select, Move, Resize.
    /// </summary>
    internal partial class AreaSelectingView : Dialog
    {
        public DialogResult DialogResult { get; private set; }

        private bool disposed;

        /// <summary>
        /// Timer for rendering.
        /// </summary>
        private UITimer timer;

        /// <summary>
        /// Control for rendering image of the editor.
        /// </summary>
        private SKControl skcontrol;

        /// <summary>
        /// User's screen snapshot in SkiaSharp format.
        /// </summary>
        private SKBitmap skScreenImage;

        /// <summary>
        /// User's screen snapshot in Eto format.
        /// </summary>
        private Bitmap etoScreenImage;

        /// <summary>
        /// Start location of selecting.
        /// </summary>
        private PointF startLocation;

        /// <summary>
        /// End location of selecting.
        /// </summary>
        private PointF endLocation;

        /// <summary>
        /// State when user selects region.
        /// </summary>
        private bool capturing;

        /// <summary>
        /// State when user has selected region.
        /// </summary>
        private bool captured;

        /// <summary>
        /// Selection region size and location.
        /// </summary>
        public RectangleF SelectionRectangle;

        /// <summary>
        /// Size of editor screen.
        /// </summary>
        private Rectangle screenRectangle;

        /// <summary>
        /// For beauty.
        /// </summary>
        #region Styles
        private Stopwatch penTimer;
        private float[] dash = new float[] { 5, 5 };
        #endregion Styles

        #region Movingfields
        private bool moving;
        private float relativeX;
        private float relativeY;
        #endregion Movingfields

        /// <summary>
        /// Fileds for resizing selected area.
        /// </summary>
        #region ResizingFields
        private bool resizing;
        private ResizePart resizePart;
        private LineF oppositeBorder;
        private PointF oppositeAngle;
        #endregion Resizingfields

        /// <summary>
        /// Initializes whole view.
        /// </summary>
        void InitializeComponent()
        {
            screenRectangle = new Rectangle(ScreenHelper.GetMainWindowSize());
            var size = new Size(screenRectangle.Width, screenRectangle.Height);
            Size = size;

            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;

            etoScreenImage = ScreenHelper.TakeScreenshot();
            skScreenImage = SkiaSharpHelper.ConvertFromEtoBitmap(etoScreenImage);

            penTimer = Stopwatch.StartNew();

            timer = new UITimer();
            timer.Elapsed += RenderFrame;
            timer.Interval = renderFrameTime / 1000;

            skcontrol = new SKControl();
            Content = skcontrol;

            Shown += EditorView_Shown;
            UnLoad += EditorViewDrawingSkiaSharp_UnLoad;
        }

        /// <summary>
        /// Disposes UI elements.
        /// </summary>
        public new void Dispose()
        {
            if (disposed == false)
            {
                disposed = true;
                timer?.Dispose();
                etoScreenImage?.Dispose();
                skScreenImage?.Dispose();
                skcontrol?.Dispose();
            }
        }
    }
}
