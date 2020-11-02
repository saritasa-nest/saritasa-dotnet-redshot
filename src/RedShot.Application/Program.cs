﻿using System;
using System.Reflection;
using System.IO;
using Microsoft.Extensions.Configuration;
using NLog.Extensions.Logging;
using Eto.Forms;
using Eto.Forms.Controls.SkiaSharp;
using RedShot.Infrastructure;
using RedShot.Infrastructure.Configuration;
using RedShot.Initialization;
#if _WINDOWS
using Eto.WinForms.Forms;
#elif _UNIX
using System.Runtime.InteropServices;
#endif

namespace RedShot.Application
{
    /// <summary>
    /// Main class.
    /// </summary>
    internal class Program
    {
        private const string RedShotBinaryFolderVariable = "NLOG__VARIABLES__RedShotBinaryFolder";
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
            ConfigureLogging();
            logger.Debug("The RedShot application was started!");

            AppInitializer.Initialize();

            AppDomain.CurrentDomain.UnhandledException += (o, e) => ShowException(e.ExceptionObject as Exception);
            AppDomain.CurrentDomain.ProcessExit += CurrentDomainProcessExit;

            var app = new Eto.Forms.Application(Eto.Platform.Detect);
            app.UnhandledException += (o, e) => ShowException(e.ExceptionObject as Exception);
            app.Initialized += AppInitialized;

            AddAreaControl();
            AddStyles();
            app.Run(ApplicationManager.GetTrayApp());
        }

        private static void ConfigureLogging()
        {
            var applicationFolder = Directory.GetCurrentDirectory();

            Environment.SetEnvironmentVariable(RedShotBinaryFolderVariable, applicationFolder);
            var jsonStream = new MemoryStream(Properties.Resources.NLogConfiguration);

            var root = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonStream(jsonStream)
                .Build();

            var nlogSection = root.GetSection("NLog");
            NLog.LogManager.Configuration = new NLogLoggingConfiguration(nlogSection);
        }

        private static void AppInitialized(object sender, EventArgs e)
        {
            Shortcut.ShortcutManager.BindShortcuts();
        }

        private static void CurrentDomainProcessExit(object sender, EventArgs e)
        {
            Shortcut.ShortcutManager.UnbindShortcuts();
            ConfigurationManager.Save();
        }

        private static void AddAreaControl()
        {
#if _WINDOWS
            Eto.Platform.Detect.Add<ISKControl>(() => new Eto.Forms.Controls.SkiaSharp.WinForms.SKControlHandler());
#elif _UNIX
            Eto.Platform.Detect.Add<ISKControl>(() => new Eto.Forms.Controls.SkiaSharp.GTK.SKControlHandler());
#else
            throw new NotImplementedException();
#endif
        }

        private static void AddStyles()
        {
#if _WINDOWS
            Eto.Style.Add<NotificationHandler>("FailedNotification", h => h.NotificationIcon = NotificationIcon.Error);
            Eto.Style.Add<NotificationHandler>("SucceedNotification", h => h.NotificationIcon = NotificationIcon.Info);
#endif
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

            Eto.Forms.Application.Instance.Dispose();
        }
    }
}
