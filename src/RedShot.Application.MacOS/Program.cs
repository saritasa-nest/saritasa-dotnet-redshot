﻿using System;
using System.Reflection;
using Eto.Forms;
using Eto.Forms.Controls.SkiaSharp;
using RedShot.Infrastructure;
using RedShot.Infrastructure.Configuration;
using RedShot.Initialization;

namespace RedShot.Application.MacOS
{
    /// <summary>
    /// Main class.
    /// </summary>
    internal class Program
    {
        private const string ApplicationId = "RedShot-01e8516a-42a1-4fde-87ff-71e6e5b32b28";
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Runs the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            using var mutex = new System.Threading.Mutex(false, ApplicationId);
            if (mutex.WaitOne(0, false))
            {
                StartApplication();
            }
        }

        private static void StartApplication()
        {
            logger.Debug("The RedShot application was started!");

            AppInitializer.Initialize();

            var app = new Eto.Forms.Application(Eto.Platforms.Mac64);
            app.UnhandledException += InstanceOnUnhandledException;
            app.Initialized += AppInitialized;
            AppDomain.CurrentDomain.UnhandledException += DomainUnhandledException;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            Eto.Platform.Detect.Add<ISKControl>(() => new Eto.Forms.Controls.SkiaSharp.Mac.SKControlHandler());
            app.Run(ApplicationManager.GetTray());
        }

        private static void AppInitialized(object sender, EventArgs e)
        {
            Shortcut.ShortcutManager.BindShortcuts();
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Shortcut.ShortcutManager.UnbindShortcuts();
            ConfigurationManager.Save();
        }

        private static void InstanceOnUnhandledException(object sender, Eto.UnhandledExceptionEventArgs e)
        {
            ShowException(e.ExceptionObject as Exception);
        }

        private static void DomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ShowException(e.ExceptionObject as Exception);
        }

        /// <summary>
        /// Show exception in message box.
        /// </summary>
        /// <param name="ex">Exception to show.</param>
        private static void ShowException(Exception ex)
        {
            if (ex == null)
            {
                return;
            }

            if (ex is TargetInvocationException tiex)
            {
                MessageBox.Show(tiex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxType.Error);
            }
            else
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxType.Error);
            }

            logger.Fatal(ex);

            Eto.Forms.Application.Instance.Quit();
        }
    }
}
