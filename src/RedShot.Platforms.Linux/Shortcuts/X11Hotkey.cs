﻿using Eto.Forms;
using System;
using System.Runtime.InteropServices;

namespace RedShot.Platforms.Linux.Shortcuts
{
    public class X11Hotkey
    {
        public Gdk.Key Key { get; }

        public Gdk.ModifierType Modifiers { get; }

        private const int KeyPress = 2;
        private const int GrabModeAsync = 1;
        private int keycode;
        private Gdk.Window rootWin = Gdk.Global.DefaultRootWindow;

        public X11Hotkey(Gdk.Key key, Gdk.ModifierType modifiers)
        {
            this.Key = key;
            this.Modifiers = modifiers;


            IntPtr xDisplay = GetXDisplay(rootWin);
            this.keycode = XKeysymToKeycode(xDisplay, (int)this.Key);
            if (Environment.Is64BitProcess)
            {
                rootWin.AddFilter(new Gdk.FilterFunc(FilterFunction64));
            }
            else
            {
                rootWin.AddFilter(new Gdk.FilterFunc(FilterFunction32));
            }
        }

        public event EventHandler Pressed;

        public void Register()
        {
            IntPtr xDisplay = GetXDisplay(rootWin);

            XGrabKey(
                     xDisplay,
                     this.keycode,
                     (uint)this.Modifiers,
                     GetXWindow(rootWin),
                     false,
                     GrabModeAsync,
                     GrabModeAsync);
        }

        public void Unregister()
        {
            IntPtr xDisplay = GetXDisplay(rootWin);

            XUngrabKey(
                     xDisplay,
                     this.keycode,
                     (uint)this.Modifiers,
                     GetXWindow(rootWin));
        }

        public override bool Equals(object obj)
        {
            if (obj is X11Hotkey hotkey)
            {
                return Modifiers.Equals(hotkey.Modifiers) && Key.Equals(hotkey.Key);
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Modifiers.GetHashCode() * 397) ^ Key.GetHashCode();
            }
        }

        private Gdk.FilterReturn FilterFunction32(IntPtr xEvent, Gdk.Event evnt)
        {
            var xKeyEvent = (XKeyEvent32)Marshal.PtrToStructure(
                xEvent,
                typeof(XKeyEvent32));

            if (xKeyEvent.type == KeyPress)
            {
                if (xKeyEvent.keycode == this.keycode
                    && xKeyEvent.state == (uint)this.Modifiers)
                {
                    this.OnPressed(EventArgs.Empty);
                }
            }

            return Gdk.FilterReturn.Continue;
        }

        private Gdk.FilterReturn FilterFunction64(IntPtr xEvent, Gdk.Event evnt)
        {
            var xKeyEvent = (XKeyEvent64)Marshal.PtrToStructure(
                xEvent,
                typeof(XKeyEvent64));

            if (xKeyEvent.type == KeyPress)
            {
                if (xKeyEvent.keycode == this.keycode
                    && xKeyEvent.state == (uint)this.Modifiers)
                {
                    this.OnPressed(EventArgs.Empty);
                }
            }

            return Gdk.FilterReturn.Continue;
        }

        protected virtual void OnPressed(EventArgs e)
        {
            var handler = this.Pressed;
            handler?.Invoke(this, e);
        }

        private static IntPtr GetXWindow(Gdk.Window window)
        {
            return gdk_x11_drawable_get_xid(window.Handle);
        }

        private static IntPtr GetXDisplay(Gdk.Window window)
        {
            return gdk_x11_drawable_get_xdisplay(
                gdk_x11_window_get_drawable_impl(window.Handle));
        }

        [DllImport("libgtk-x11-2.0")]
        private static extern IntPtr gdk_x11_drawable_get_xid(IntPtr gdkWindow);

        [DllImport("libgtk-x11-2.0")]
        private static extern IntPtr gdk_x11_drawable_get_xdisplay(IntPtr gdkDrawable);

        [DllImport("libgtk-x11-2.0")]
        private static extern IntPtr gdk_x11_window_get_drawable_impl(IntPtr gdkWindow);

        [DllImport("libX11")]
        private static extern int XKeysymToKeycode(IntPtr display, int key);

        [DllImport("libX11")]
        private static extern int XGrabKey(
            IntPtr display,
            int keycode,
            uint modifiers,
            IntPtr grab_window,
            bool owner_events,
            int pointer_mode,
            int keyboard_mode);

        [DllImport("libX11")]
        private static extern int XUngrabKey(
            IntPtr display,
            int keycode,
            uint modifiers,
            IntPtr grab_window);

        [StructLayout(LayoutKind.Sequential)]
        internal struct XKeyEvent32
        {
            public short type;
            public uint serial;
            public short send_event;
            public IntPtr display;
            public uint window;
            public uint root;
            public uint subwindow;
            public uint time;
            public int x, y;
            public int x_root, y_root;
            public uint state;
            public uint keycode;
            public short same_screen;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct XKeyEvent64
        {
            public int type;
            public ulong serial;
            public int send_event;
            public IntPtr display;
            public ulong window;
            public ulong root;
            public ulong subwindow;
            public ulong time;
            public int x, y;
            public int x_root, y_root;
            public uint state;
            public uint keycode;
            public int same_screen;
        }
    }
}
