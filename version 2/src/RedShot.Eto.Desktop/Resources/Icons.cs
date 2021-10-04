using Eto.Drawing;
using RedShot.Eto.Desktop.Helpers;
using Images = RedShot.Eto.Desktop.Properties.Resources;

namespace RedShot.Eto.Desktop.Resources
{
    /// <summary>
    /// Class for providing icons.
    /// </summary>
    public static class Icons
    {
        /// <summary>
        /// Arrow icon.
        /// </summary>
        public static Bitmap Arrow => GetImage(Images.Arrow);

        /// <summary>
        /// Back arrow icon.
        /// </summary>
        public static Bitmap Back => GetImage(Images.Back);

        /// <summary>
        /// The close icon.
        /// </summary>
        public static Bitmap Close => GetImage(Images.Close);

        /// <summary>
        /// The download icon.
        /// </summary>
        public static Bitmap Download => GetImage(Images.Download);

        /// <summary>
        /// Eraser icon.
        /// </summary>
        public static Bitmap EraseIcon => GetImage(Images.EraseIcon);

        /// <summary>
        /// Eraser pointer icon.
        /// </summary>
        public static Bitmap EraserPointer => GetImage(Images.EraserPointer);

        /// <summary>
        /// Failed icon.
        /// </summary>
        public static Bitmap Failed => GetImage(Images.Failed);

        /// <summary>
        /// Folder icon.
        /// </summary>
        public static Bitmap Folder => GetImage(Images.Folder);

        /// <summary>
        /// Form icon.
        /// </summary>
        public static Bitmap Form => GetImage(Images.Form);

        /// <summary>
        /// FTP icon.
        /// </summary>
        public static Bitmap Ftp => GetImage(Images.Ftp);

        /// <summary>
        /// The open icon.
        /// </summary>
        public static Bitmap Open => GetImage(Images.Open);

        /// <summary>
        /// Palette icon.
        /// </summary>
        public static Bitmap Paint => GetImage(Images.Paint);

        /// <summary>
        /// Paint brush icon.
        /// </summary>
        public static Bitmap PaintBrush => GetImage(Images.Paintbrush);

        /// <summary>
        /// The play icon.
        /// </summary>
        public static Bitmap Play => GetImage(Images.Play);

        /// <summary>
        /// Pointer icon.
        /// </summary>
        public static Bitmap Pointer => GetImage(Images.Pointer);

        /// <summary>
        /// The record icon.
        /// </summary>
        public static Bitmap Record => GetImage(Images.Record);

        /// <summary>
        /// Rectangle icon.
        /// </summary>
        public static Bitmap Rectangle => GetImage(Images.Rectangle);

        /// <summary>
        /// Red circle icon.
        /// </summary>
        public static Bitmap RedCircle => GetImage(Images.Redcircle);

        /// <summary>
        /// The stop icon.
        /// </summary>
        public static Bitmap Stop => GetImage(Images.Stop);

        /// <summary>
        /// Success icon.
        /// </summary>
        public static Bitmap Success => GetImage(Images.Success);

        /// <summary>
        /// The upload icon.
        /// </summary>
        public static Bitmap Upload => GetImage(Images.Upload);

        /// <summary>
        /// Video icon.
        /// </summary>
        public static Bitmap Video => GetImage(Images.Video);

        /// <summary>
        /// Text size icon.
        /// </summary>
        public static Bitmap Text => GetImage(Images.Text);

        /// <summary>
        /// Plus icon.
        /// </summary>
        public static Bitmap Add => GetImage(Images.Add);

        /// <summary>
        /// Minus icon.
        /// </summary>
        public static Bitmap Remove => GetImage(Images.Remove);

        /// <summary>
        /// The copy icon.
        /// </summary>
        public static Bitmap Copy => GetImage(Images.Copy);

        /// <summary>
        /// Down Arrow.
        /// </summary>
        public static Bitmap DownArrow => GetImage(Images.DownArrow);

        /// <summary>
        /// Gear icon.
        /// </summary>
        public static Bitmap Gear => GetImage(Images.Gear);

        private static Bitmap GetImage(System.Drawing.Bitmap image)
        {
            return EtoDrawingHelper.GetEtoBitmapFromDrawing(image);
        }
    }
}
