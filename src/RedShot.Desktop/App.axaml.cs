using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using RedShot.Desktop.ViewModels;
using RedShot.Desktop.Views;
using System;
using System.Collections.Generic;
using Avalonia.FreeDesktop;
using ReactiveUI;
using System.Runtime.InteropServices;
using Eto.Forms;
using RedShot.Desktop.Abstractions;
using RedShot.Desktop.Platforms;

namespace RedShot.Desktop
{
    public class App : Avalonia.Application
    {
        public INotifyIcon NotifyIcon { get; private set; }
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            
            base.OnFrameworkInitializationCompleted();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();


                //// Set up and configure the notification icon
                //// Get the type of the platform-specific implementation
                //Type type = Implementation.ForType<INotifyIcon>();
                //if (type != null)
                //{
                //    // If we have one, create an instance for it
                //    NotifyIcon = (INotifyIcon)Activator.CreateInstance(type);
                //}

                //if (NotifyIcon != null)
                //{
                //    NotifyIcon.ToolTipText = "SQRL .NET Client";
                //    //NotifyIcon.IconPath = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ?
                //    //                      "resm:SQRLDotNetClientUI.Assets.SQRL_icon_normal_16.png" :
                //    //                      RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
                //    //                      @"resm:SQRLDotNetClientUI.Assets.sqrl_icon_normal_256.ico" :
                //    //                      @"resm:SQRLDotNetClientUI.Assets.sqrl_icon_normal_256_32_icon.ico";


                //    //NotifyIcon.DoubleClick += (s, e) =>
                //    //{
                //    //    RestoreMainWindow();
                //    //};

                //    var notifyIconContextMenu = new Avalonia.Controls.ContextMenu();
                //    List<object> menuItems = new List<object>();
                //    menuItems.AddRange(new[] {
                //    new Avalonia.Controls.MenuItem() {
                //        Header = "NotifyIconContextMenuItemHeaderRestore" },
                //    new Avalonia.Controls.MenuItem() {
                //        Header = "NotifyIconContextMenuItemHeaderExit"}
                //    });
                //    notifyIconContextMenu.Items = menuItems;
                //    NotifyIcon.ContextMenu = notifyIconContextMenu;
                //    NotifyIcon.Visible = true;
                //}


                //////// Set up the app's main window, if we aren't staring minimized to tray
                //////if (!AppSettings.Instance.StartMinimized || NotifyIcon == null)
                //////{
                //////    desktop.MainWindow = _mainWindow;
                //////}
            }
        }
    }
}
