﻿using SkiaSharp;
using SkiaSharp.Views.UWP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace RedShot.Desktop.Views.Screenshots
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ScreenshotView : Page
    {
        public ScreenshotView()
        {
            this.InitializeComponent();
        }

		//private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
		//{
		//	// the the canvas and properties
		//	var canvas = e.Surface.Canvas;

		//	// get the screen density for scaling
		//	var display = DisplayInformation.GetForCurrentView();
		//	var scale = display.LogicalDpi / 96.0f;
		//	var scaledSize = new SKSize(e.Info.Width / scale, e.Info.Height / scale);

		//	// handle the device screen density
		//	canvas.Scale(scale);

		//	// make sure the canvas is blank
		//	canvas.Clear(SKColors.White);

		//	// draw some text
		//	var paint = new SKPaint
		//	{
		//		Color = SKColors.Black,
		//		IsAntialias = true,
		//		Style = SKPaintStyle.Fill,
		//		TextAlign = SKTextAlign.Center,
		//		TextSize = 24
		//	};
		//	var coord = new SKPoint(scaledSize.Width / 2, (scaledSize.Height + paint.TextSize) / 2);
		//	canvas.DrawText("SkiaSharp", coord, paint);
		//}
	}
}
