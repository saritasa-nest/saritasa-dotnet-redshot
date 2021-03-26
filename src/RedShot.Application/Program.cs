using System;
using System.Reflection;
using System.IO;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using NLog.Extensions.Logging;
using Eto.Forms;
using Eto.Forms.Controls.SkiaSharp;
using RedShot.Infrastructure;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Configuration.Models;
using RedShot.Infrastructure.Settings;
using RedShot.Infrastructure.Formatting;
using RedShot.Resources;
using AutoMapper;
using RedShot.Infrastructure.Configuration.Models.General;
using RedShot.Infrastructure.Common;
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
                CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
                CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

                StartApplication();
            }
        }

        private static void StartApplication()
        {
            var config = GetConfiguration();
            ConfigureLogging(config);
            ConfigureApplication(config);

            logger.Debug("The RedShot application was started!");

            AppDomain.CurrentDomain.UnhandledException += (o, e) => ShowException(e.ExceptionObject as Exception);
            AppDomain.CurrentDomain.ProcessExit += CurrentDomainProcessExit;

            var app = new Eto.Forms.Application(Eto.Platform.Detect);
            app.UnhandledException += (o, e) => ShowException(e.ExceptionObject as Exception);
            app.Initialized += AppInitialized;

            AddAreaControl();
            AddStyles();
            app.Run(new ApplicationTray("RedShot", Icons.RedCircle));
        }

        private static void ConfigureLogging(IConfiguration configuration)
        {
            var nlogSection = configuration.GetSection("NLog");
            NLog.LogManager.Configuration = new NLogLoggingConfiguration(nlogSection);
        }

        private static void ConfigureApplication(IConfiguration configuration)
        {
            AppSettings.Instance = configuration.GetSection("AppSettings").Get<AppSettings>();

            var mappingConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile<Infrastructure.Uploading.MappingProfile>();
                config.AddProfile<Infrastructure.Formatting.MappingProfile>();
                config.AddProfile<Infrastructure.Recording.Common.MappingProfile>();
            });
            mappingConfiguration.CompileMappings();
            Mapping.Mapper = mappingConfiguration.CreateMapper();

            ConfigureAutostart();
        }

        private static void ConfigureAutostart()
        {
            var generalConfigurationModel = Infrastructure.Configuration.ConfigurationProvider.Instance.GetConfiguration<GeneralConfiguration>();
            var generalConfiguration = Mapping.Mapper.Map<GeneralOptions>(generalConfigurationModel);
            if (generalConfiguration.LaunchAtSystemStart)
            {
                var autostart = new Autostart();
                autostart.EnableAutostart();
            }
        }

        private static void AppInitialized(object sender, EventArgs e)
        {
            Shortcut.ShortcutManager.Instance.BindShortcuts();
        }

        private static void CurrentDomainProcessExit(object sender, EventArgs e)
        {
            Shortcut.ShortcutManager.Instance.UnbindShortcuts();
            Infrastructure.Configuration.ConfigurationProvider.Instance.Save();
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

        private static IConfiguration GetConfiguration()
        {
            var applicationFolder = Directory.GetCurrentDirectory();
            Environment.SetEnvironmentVariable(RedShotBinaryFolderVariable, applicationFolder);
            var nlogConfig = new MemoryStream(Properties.Resources.NLogConfiguration);
            var appConfig = new MemoryStream(Properties.Resources.AppConfiguration);

            return new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonStream(nlogConfig)
                .AddJsonStream(appConfig)
                .Build();
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
