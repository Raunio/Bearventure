using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectMercury;
using ProjectMercury.Emitters;
using ProjectMercury.Modifiers;
using ProjectMercury.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Bearventure.Engine.Effects
{
    public class VisualEffect : ICloneable
    {
        private ParticleEffect particleEffect;
        private Renderer renderer;
        private float timer;

        public Vector2 Position
        {
            get
            {
                if (position == null)
                    return Vector2.Zero + PositionOffset;
                else
                    return position + PositionOffset;
            }

            set
            {
                position = value;
            }
        }
        private Vector2 position;
        /// <summary>
        /// Determines the maximum amount of particles on this effect.
        /// </summary>
        public int Budget
        {
            get
            {
                if (budget == 0)
                    return 500;
                else
                    return budget;
            }
            set
            {
                budget = value;
            }
        }
        private int budget;
        /// <summary>
        /// Determines how long individual particles live in seconds.
        /// </summary>
        public float Term
        {
            get
            {
                if (term == 0)
                    return 2f;
                else
                    return term;
            }
            set
            {
                term = value;
            }
        }
        private float term;
        /// <summary>
        /// Release rotation of particles.
        /// </summary>
        public VariableFloat ReleaseRotation
        {
            get
            {
                if (releaseRotation == 0)
                    return new VariableFloat();
                else
                    return releaseRotation;
            }
            set
            {
                releaseRotation = value;
            }
        }
        private VariableFloat releaseRotation;
        /// <summary>
        /// Quantity of released particles.
        /// </summary>
        public int ReleaseQuantity
        {
            get
            {
                if (releaseQuantity == 0)
                    return 2;
                else
                    return releaseQuantity;
            }
            set
            {
                releaseQuantity = value;
            }
        }
        private int releaseQuantity;
        /// <summary>
        /// The size of an individual particle.
        /// </summary>
        public float ReleaseScale
        {
            get
            {
                if (releaseScale == 0)
                    return 32f;
                else
                    return releaseScale;
            }
            set
            {
                releaseScale = value;
            }
        }
        private float releaseScale;
        /// <summary>
        /// Gets or sets the Ultimate scale of an individual particle.
        /// </summary>
        public float UltimateScale
        {
            get
            {
                if (ultimateScale < 0)
                    return ReleaseScale;
                else
                    return ultimateScale;
            }
            set
            {
                ultimateScale = value;
            }
            
        }
        private float ultimateScale = -1;
        /// <summary>
        /// The velocity and variation of particle velocities.
        /// </summary>
        public VariableFloat ReleaseSpeed
        {
            get
            {
                if (releaseSpeed == 0)
                    return new VariableFloat();
                else
                    return releaseSpeed;
            }
            set
            {
                releaseSpeed = value;
            }
        }
        private VariableFloat releaseSpeed;
        /// <summary>
        /// The initial opacity of a particle.
        /// </summary>
        public float InitialOpacity
        {
            get
            {
                if (initialOpacity < 0)
                    return 1f;
                else
                    return initialOpacity;
            }
            set
            {
                initialOpacity = value;
            }
        }
        private float initialOpacity = -1;
        public float MiddleOpacity
        {
            get
            {
                if (middleOpacity < 0)
                    return InitialOpacity * MiddlePosition;
                else
                    return middleOpacity;
            }
            set
            {
                middleOpacity = value;
            }
        }
        /// <summary>
        /// Gets or sets the middle position for modifiers. Value between 0 and 1.
        /// </summary>
        public float MiddlePosition
        {
            get
            {
                if (middlePosition < 0)
                    return 0.5f;
                else
                    return middlePosition;
            }
            set
            {
                if (value > 1)
                    middlePosition = 1;
                else
                    middlePosition = value;
            }
        }
        private float middlePosition = -1;
        private float middleOpacity = -1;
        /// <summary>
        /// The final opacity of a particle.
        /// </summary>
        public float UltimateOpacity
        {
            get
            {
                if (ultimateOpacity < 0)
                    return 1f;
                else
                    return ultimateOpacity;
            }
            set
            {
                ultimateOpacity = value;
            }
        }
        private float ultimateOpacity;
        /// <summary>
        /// The initial colour of a particle.
        /// </summary>
        public Color InitialColor
        {
            get
            {
                if (initialColor == null)
                    return Color.White;
                else
                    return initialColor;
            }
            set
            {
                initialColor = value;
            }
        }
        private Color initialColor;
        /// <summary>
        /// The final colour of a particle.
        /// </summary>
        public Color UltimateColor
        {
            get
            {
                if (ultimateColor == null)
                    return Color.White;
                else
                    return ultimateColor;
            }
            set
            {
                ultimateColor = value;
            }
        }
        private Color ultimateColor;
        /// <summary>
        /// The lifetime of the visual effect in milliseconds.
        /// </summary>
        public float Lifetime
        {
            get;
            set;
        }
        /// <summary>
        /// Gets wether the effect is alive or not.
        /// </summary>
        public bool IsAlive
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets or sets the offset for the trigger position of the effect.
        /// </summary>
        public Vector2 PositionOffset
        {
            get
            {
                if (positionOffset == null)
                    return Vector2.Zero;
                else
                    return positionOffset;
            }
            set
            {
                positionOffset = value;
            }
        }
        private Vector2 positionOffset;
        public VisualEffect()
        {
        }

        public void Initialize(string asset, ContentManager content, GraphicsDeviceManager graphics)
        {
            renderer = new SpriteBatchRenderer
            {
                GraphicsDeviceService = graphics,
            };

            this.particleEffect = content.Load<ParticleEffect>(asset);

            particleEffect.Initialise();
            renderer.LoadContent(content);
            particleEffect.LoadContent(content);
        }

        public void Trigger(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer < Lifetime || Lifetime == 0)
            {
                IsAlive = true;
                particleEffect.Trigger(Position);
            }

            else if (timer >= Lifetime + Term * 1000)
            {
                IsAlive = false;
            }
            
        }

        public void Update(GameTime gameTime)
        {
            particleEffect.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteBatchRenderer rend = this.renderer as SpriteBatchRenderer;
            rend.RenderEffect(particleEffect, spriteBatch);
        }

        public object Clone()
        {
            return (VisualEffect)this.MemberwiseClone();
        }


    }
}
