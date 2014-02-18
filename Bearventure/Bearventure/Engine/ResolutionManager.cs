//////////////////////////////////////////////////////////////////////////
////License:  The MIT License (MIT)
////Copyright (c) 2010 David Amador (http://www.david-amador.com)
////
////Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
////
////The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
////
////THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//////////////////////////////////////////////////////////////////////////

// HAD TO REFRACTOR THE CODE A LITTLE. WAS UNREADABLE. -Huemac 2012
// Added also nice enumerated constant values for resolutions

// Added getter for current screen mode. - Raunio

using System;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bearventure
{
    static class ResolutionManager
    {
        
        private static readonly ReadOnlyCollection<Point> resolutions =
            new ReadOnlyCollection<Point>(new[]
        {
            new Point(1920,1080),
            new Point(1280,720),
            new Point(720,480),
            new Point(1440,900),
            new Point(1680,1050)
            
        });


        static public GraphicsDeviceManager graphicsDevice
        {
            get;
            private set;
        }

        public static Constants.ScreenMode CurrentScreenMode
        {
            get;
            private set;
        }

        static private int width = 800;
        static private int height = 600;
        static private int virtualWidth = 1280;
        static private int virtualHeight = 720;
        static private Matrix scaleMatrix;
        static private bool isFullScreen = false;
        static private bool isDirtyMatrix = true;

        //static public bool FullScreen
        //{
        //    get { return isFullScreen; }
        //}

        static public void Initialize(ref GraphicsDeviceManager graphicsDeviceManager)
        {
            width = graphicsDeviceManager.PreferredBackBufferWidth;
            height = graphicsDeviceManager.PreferredBackBufferHeight;
            graphicsDevice = graphicsDeviceManager;
            
            isDirtyMatrix = true;
            ApplyResolutionSettings();
        }


        static public Matrix GetScaleMatrix()
        {
            if (isDirtyMatrix) RecreateScaleMatrix();

            return scaleMatrix;
        }

        static public void SetResolution(int screenWidth, int screenHeight, bool fullScreen)
        {
            width = screenWidth;
            height = screenHeight;

            isFullScreen = fullScreen;

            ApplyResolutionSettings();
        }


        static public void SetResolution(Constants.ScreenMode screenMode, bool fullScreen)
        {
            CurrentScreenMode = screenMode;

            width = resolutions[(int)screenMode].X;
            height = resolutions[(int)screenMode].Y;

            isFullScreen = fullScreen;

            ApplyResolutionSettings();
        }

        static public void SetVirtualResolution(int width, int height)
        {
            virtualWidth = width;
            virtualHeight = height;

            isDirtyMatrix = true;
        }

        // Added by Huemac
        static public Point GetVirtualResolution()
        {
            //Console.WriteLine(virtualWidth + "X" + virtualHeight);
            return new Point(virtualWidth, virtualHeight);
        }

        static public Point GetResolution()
        {
            return new Point(width, height);
        }
        static public void SetVirtualResolution(Constants.ScreenMode screenMode)
        {
            CurrentScreenMode = screenMode;

            virtualWidth = resolutions[(int)screenMode].X;
            virtualHeight = resolutions[(int)screenMode].Y;

            isDirtyMatrix = true;
        }

        static private void ApplyResolutionSettings()
        {

#if XBOX360
           _FullScreen = true;
#endif

            // If we aren't using a full screen mode, the height and width of the window can
            // be set to anything equal to or smaller than the actual screen size.
            if (isFullScreen == false)
            {
                if ((width <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
                    && (height <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height))
                {
                    graphicsDevice.PreferredBackBufferWidth = width;
                    graphicsDevice.PreferredBackBufferHeight = height;
                    graphicsDevice.IsFullScreen = isFullScreen;
                    graphicsDevice.ApplyChanges();
                }
            }
            else
            {
                // If we are using full screen mode, we should check to make sure that the display
                // adapter can handle the video mode we are trying to set.  To do this, we will
                // iterate through the display modes supported by the adapter and check them against
                // the mode we want to set.
                foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                {
                    // Check the width and height of each mode against the passed values
                    if ((dm.Width == width) && (dm.Height == height))
                    {
                        // The mode is supported, so set the buffer formats, apply changes and return
                        graphicsDevice.PreferredBackBufferWidth = width;
                        graphicsDevice.PreferredBackBufferHeight = height;
                        graphicsDevice.IsFullScreen = isFullScreen;
                        graphicsDevice.ApplyChanges();
                    }
                }
            }

            isDirtyMatrix = true;

            width = graphicsDevice.PreferredBackBufferWidth;
            height = graphicsDevice.PreferredBackBufferHeight;
        }

        /// <summary>
        /// Sets the device to use the draw pump
        /// Sets correct aspect ratio
        /// </summary>
        static public void BeginDraw()
        {
            // Start by reseting viewport to (0,0,1,1)
            FullViewport();
            // Clear to Black
            graphicsDevice.GraphicsDevice.Clear(Color.Transparent);
            // Calculate Proper Viewport according to Aspect Ratio
            ResetViewport();
            // and clear that
            // This way we are gonna have black bars if aspect ratio requires it and
            // the clear color on the rest
            graphicsDevice.GraphicsDevice.Clear(Color.Blue);
        }

        static private void RecreateScaleMatrix()
        {
            isDirtyMatrix = false;
            scaleMatrix = Matrix.CreateScale(
                           (float)graphicsDevice.GraphicsDevice.Viewport.Width / virtualWidth,
                           (float)graphicsDevice.GraphicsDevice.Viewport.Width / virtualWidth,
                           1f);
        }


        static public void FullViewport()
        {
            Viewport viewPort = new Viewport();
            viewPort.X = viewPort.Y = 0;
            viewPort.Width = width;
            viewPort.Height = height;
            graphicsDevice.GraphicsDevice.Viewport = viewPort;
        }

        /// <summary>
        /// Get virtual Mode Aspect Ratio
        /// </summary>
        /// <returns>Aspect ratio</returns>
        static public float GetVirtualAspectRatio()
        {
            return (float)virtualWidth / (float)virtualHeight;
        }

        static public float GetAspectRatio()
        {
            return (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        }

        static public void ResetViewport()
        {
            float targetAspectRatio = GetVirtualAspectRatio();
            // Figure out the largest area that fits in this resolution at the desired aspect ratio
            int newWidth = graphicsDevice.PreferredBackBufferWidth;
            int newHeight = (int)(newWidth / targetAspectRatio + .5f);
            bool changed = false;

            if (newHeight > graphicsDevice.PreferredBackBufferHeight)
            {
                newHeight = graphicsDevice.PreferredBackBufferHeight;
                // PillarBox
                newWidth = (int)(newHeight * targetAspectRatio + .5f);
                changed = true;
            }

            // Set up the new viewport centered in the backbuffer
            Viewport viewport = new Viewport();

            viewport.X = (graphicsDevice.PreferredBackBufferWidth / 2) - (newWidth / 2);
            viewport.Y = (graphicsDevice.PreferredBackBufferHeight / 2) - (newHeight / 2);
            viewport.Width = newWidth;
            viewport.Height = newHeight;
            viewport.MinDepth = 0;
            viewport.MaxDepth = 1;

            if (changed)
                isDirtyMatrix = true;

            graphicsDevice.GraphicsDevice.Viewport = viewport;
        }

    }
}