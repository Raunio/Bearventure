using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bearventure.Gameplay
{
    class Camera
    {
        #region Members
        private float zoom;       // Kameran zoom
        public Matrix Transform;    // Matriisimuutos
        public Vector2 Position;    // Kameran Position
        private float Rotation;   // Kameran Rotation
        public Vector2 Origin { get; set; }
        private Viewport viewPort;
        private Vector2 levelSize;

        #endregion

        public Camera(Viewport ViewPort, Vector2 levelSize)
        {
            zoom = 1f;
            Rotation = 0f;
            Position = Vector2.Zero;
            Origin = new Vector2(ViewPort.Width / 2.0f, ViewPort.Height / 2.0f);
            this.levelSize = levelSize;
            viewPort = ViewPort;

            ViewPortRectangle = new Rectangle(0, 0, ViewPort.Width, ViewPort.Height);
        }

        public Rectangle ViewPortRectangle
        {
            get;
            private set;
        }

        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; } // Negatiivinen zoom kääntää kuvan
        }

        public float rotation
        {
            get { return Rotation; }
            set { Rotation = value; }
        }

        // Liikutus funktio kameralle
        public void Move(Vector2 amount)
        {
            Position += amount;
        }

        public Vector2 Pos
        {
            get 
            { 
                return new Vector2(Position.X - viewPort.Width / 2, Position.Y - viewPort.Height / 2);
            }
        }

        public void LookAt(Vector2 position, Vector2 levelSize)
        {
            if (position.X < viewPort.Width / 2)
                position.X = viewPort.Width / 2;
            else if (position.X > levelSize.X - viewPort.Width / 2)
                position.X = levelSize.X - viewPort.Width / 2;
            
            Position = position;

            ViewPortRectangle = new Rectangle((int)Pos.X, (int)Pos.Y, ViewPortRectangle.Width, ViewPortRectangle.Height);
        }

        public Matrix GetTransformation(GraphicsDevice graphicsDevice)
        {
            Transform =
                Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0f)) *
                    Matrix.CreateRotationZ(rotation) *
                    Matrix.CreateScale((float)graphicsDevice.Viewport.Width / ResolutionManager.GetVirtualResolution().X * zoom,
                           (float)graphicsDevice.Viewport.Height / ResolutionManager.GetVirtualResolution().Y * zoom,
                           zoom) *
                    Matrix.CreateTranslation(new Vector3(Origin, 0));
            return Transform;
        }
    }
}
