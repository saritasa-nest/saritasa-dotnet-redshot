﻿using System;
using System.IO;
using Eto.Forms;
using RedShot.Upload;

namespace RedShot.App
{
    /// <summary>
    /// Manages application views.
    /// </summary>
    public static class ApplicationManager
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static TrayIcon trayIcon;

        /// <summary>
        /// Gives tray icon form.
        /// </summary>
        /// <returns></returns>
        public static Form GetTrayApp()
        {
            try
            {
                trayIcon = new TrayIcon("RedShot", Path.Combine(Directory.GetCurrentDirectory(), "Resources", "red-circle.png"));
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, "Error occured in creating tray icon form");
                throw;
            }
            return trayIcon;
        }

        /// <summary>
        /// Runs screenshot editor.
        /// </summary>
        public static void RunScreenShotEditorDrawing()
        {
            try
            {
                trayIcon.Tray.Visible = false;
                UploadManager.CloseUploaderView();

                var view = new EditorViewDrawingSkiaSharp();
                view.Closed += View_Closed;
                view.Show();
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, "Error occured in creating tray screen editor form");
                throw;
            }
        }

        private static void View_Closed(object sender, System.EventArgs e)
        {
            trayIcon.Tray.Visible = true;
        }
    }
}
