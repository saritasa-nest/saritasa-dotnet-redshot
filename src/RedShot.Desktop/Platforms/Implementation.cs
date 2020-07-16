﻿using RedShot.Desktop.Abstractions;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RedShot.Desktop.Platforms
{
    /// <summary>
    /// Provides a central place for accessing platform-specific implementations.
    /// New platform-specific implementations should be registered here.
    /// </summary>
    public static class Implementation
    {
        //private static IBridgeSystem _bridgeSystem { get; set; } = BridgeSystem.Bash;
        //private static ShellConfigurator _shell { get; set; } = new ShellConfigurator(_bridgeSystem);

        /// <summary>
        /// Returns a <c>Type</c> that represents the platform-specific implementation for 
        /// the given type <typeparamref name="T"/>, or <c>null</c> if no implementation
        /// exists for the current platform.
        /// </summary>
        /// <typeparam name="T">The <c>Type</c> (mostly an interface type) to get a platform-
        /// specific implementation for.</typeparam>
        public static Type ForType<T>()
        {

            if (typeof(T) == typeof(INotifyIcon))
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    return typeof(Windows.NotifyIcon);
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    //if (!string.IsNullOrEmpty(response.stdout) &&
                    //    response.stdout.ToLower().Contains("ubuntu"))
                    //{
                    //    return typeof(Linux.NotifyIcon);
                    //}
                }

                return null;
            }

            //if (typeof(T) == typeof(ISystemEventNotifier))
            //{
            //    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            //        return typeof(Win.SystemEventNotifier);
            //    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            //        return typeof(Linux.SystemEventNotifier);
            //    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            //        return typeof(OSX.SystemEventNotifier);
            //    else return null;
            //}

            throw new NotImplementedException(
                String.Format("No platform-specific implementations registered for type {0}!",
                typeof(T).ToString()));
        }
    }
}
