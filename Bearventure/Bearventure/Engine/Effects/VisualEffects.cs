using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ProjectMercury;
using Microsoft.Xna.Framework.Content;
using ProjectMercury.Emitters;

namespace Bearventure.Engine.Effects
{
    class VisualEffects
    {
        /// <summary>
        /// Small dust effect.
        /// </summary>
        public static VisualEffect Dust = new VisualEffect();
        public static VisualEffect Test = new VisualEffect();
        /// <summary>
        /// Load all effects.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="graphics"></param>
        public static void Load(ContentManager content, GraphicsDeviceManager graphics)
        {
            Dust.Initialize("VisualEffects/Dust", content, graphics);

            Test.Initialize("VisualEffects/testi123", content, graphics);
            Test.Lifetime = 2000;
        }
    }
}
