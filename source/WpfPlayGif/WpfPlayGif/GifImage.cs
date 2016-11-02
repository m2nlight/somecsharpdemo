// ----------------------------------------
// GifImage.cs
// Created by Bob 2011-11-22 14:51
// ----------------------------------------
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfPlayGif
{
    public class GifImage : Image
    {
        public Uri GifImageUri
        {
            get { return (Uri)GetValue(GifImageUriProperty); }
            set { SetValue(GifImageUriProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GifImageUri.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GifImageUriProperty =
            DependencyProperty.Register("GifImageUri", typeof(Uri), typeof(GifImage),
                                        new FrameworkPropertyMetadata(null,
                                                                      FrameworkPropertyMetadataOptions.AffectsRender,
                                                                      GifImageUriChanged, null));

        private static void GifImageUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GifImage)d).UpdateGif((Uri)e.NewValue);
        }

        private void UpdateGif(Uri uri)
        {
            if (this._playGifThread != null)
            {
                try
                {
                    this._playGifThread.Abort();
                }
                catch
                {
                }
            }

            this._gifBitmapDecoder = new GifBitmapDecoder(uri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
            var frame = this._gifBitmapDecoder.Frames[0];
            frame.Freeze();
            this.Source = frame;
            this._frameIndex = 0;
            this._frameCount = this._gifBitmapDecoder.Frames.Count;
            this._delay = 0;
            if (_frameCount > 0)
            {
                this._playGifThread = new Thread(Play)
                                   {
                                       IsBackground = true,
                                       Name = "PlayGifThread"
                                   };
                this._playGifThread.Start();
            }

        }

        private Thread _playGifThread;
        private GifBitmapDecoder _gifBitmapDecoder;
        private int _frameIndex;
        private int _frameCount;
        private int _delay;

        public GifImage()
        {
        }

        public GifImage(Uri uri)
        {
            GifImageUri = uri;
        }


        private void Play(object state)
        {
            while (true)
            {
                this.Dispatcher.Invoke(
                    new Action(() =>
                                   {
                                       var frame = this._gifBitmapDecoder.Frames[_frameIndex];
                                       frame.Freeze();
                                       this.Source = frame;
                                       this.InvalidateVisual();
                                       var bitmapMetaData = frame.Metadata as BitmapMetadata;
                                       if (bitmapMetaData != null)
                                       {
                                           const string query = "/grctlext/Delay";
                                           if (bitmapMetaData.ContainsQuery(query))
                                           {
                                               _delay = Convert.ToInt32(bitmapMetaData.GetQuery(query));
                                           }
                                       }
                                       else
                                       {
                                           Trace.WriteLine("Cann't query /grctlext/Delay");
                                           _delay = 10; //无法得到帧数，给定默认值（.NET Framework 4.0 in Windows XP BUG）
                                       }
                                   }));

                _frameIndex = (_frameIndex + 1) % _frameCount;
                Thread.Sleep(_delay * 10);
            }
        }

    }
}
