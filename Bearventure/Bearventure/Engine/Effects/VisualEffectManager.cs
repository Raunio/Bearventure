using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bearventure.Engine.Effects;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ProjectMercury;
using Bearventure.Gameplay.Characters;

namespace Bearventure.Engine.Effects
{
    class VisualEffectManager
    {
        private ContentManager content;
        private GraphicsDeviceManager graphics;

        private List<VisualEffect> Effects = new List<VisualEffect>();
        private VisualEffect[] terrainEffects;

        private static VisualEffectManager instance;
        private List<Enemy> enemies;
        private Player player;

        public static VisualEffectManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new VisualEffectManager();

                return instance;
            }
        }
        /// <summary>
        /// Initialization by passing the contentmanager and graphics object.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="graphics"></param>
        public void Initialize(ContentManager content, GraphicsDeviceManager graphics)
        {
            this.content = content;
            this.graphics = graphics;
        }
        /// <summary>
        /// Initializes the terrain effects by giving pointers to player and enemy positions.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="enemies"></param>
        public void InitializeTerrainEffects(Player player, List<Enemy> enemies)
        {
            this.player = player;
            this.enemies = enemies;

            terrainEffects = new VisualEffect[enemies.Count + 1];

            for (int i = 0; i < enemies.Count + 1; i++)
            {
                terrainEffects[i] = new VisualEffect(Constants.DustEffect);
                terrainEffects[i].Initialize(content, graphics);
            }
        }
        /// <summary>
        /// Update all effects.
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdateEffects(GameTime gameTime)
        {
            foreach (VisualEffect e in Effects)
            {
                e.Trigger(gameTime);
                e.Update(gameTime);
            }

            UpdateTerrainEffects(gameTime);
            CleanEffects();
        }
        /// <summary>
        /// Update all terrain effects.
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateTerrainEffects(GameTime gameTime)
        {
            if (CharacterPhysics.OnTerrain(player) == Constants.Solid && player.state == Constants.CharacterState.Walking)
            {
                terrainEffects[0].Position = new Vector2(player.position.X, player.BoundingBox.Bottom);
                terrainEffects[0].Trigger(gameTime);
            }

            for (int i = 0; i < enemies.Count; i++)
                if (CharacterPhysics.OnTerrain(enemies[i]) == Constants.Solid && enemies[i].state == Constants.CharacterState.Walking)
                {
                    terrainEffects[i + 1].Position = new Vector2(enemies[i].position.X, enemies[i].BoundingBox.Bottom - 5);
                    terrainEffects[i + 1].Trigger(gameTime);
                }

            for (int i = 0; i < terrainEffects.Length; i++)
                terrainEffects[i].Update(gameTime);
            
        }
        /// <summary>
        /// Draws all effetcs.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawEffects(SpriteBatch spriteBatch)
        {
            foreach (VisualEffect e in Effects)
                e.Draw(spriteBatch);
            foreach (VisualEffect e in terrainEffects)
                e.Draw(spriteBatch);
        }
        /// <summary>
        /// Search for dead effects and remove them.
        /// </summary>
        private void CleanEffects()
        {
            for (int i = 0; i < Effects.Count; i++)
                if (!Effects[i].IsAlive)
                    Effects.RemoveAt(i);
        }

        public void CreateEffect(string asset, Vector2 position, int Lifetime)
        {
            VisualEffect e = new VisualEffect(asset);
            e.Initialize(content, graphics);
            e.Position = position;
            e.Lifetime = Lifetime;
            Effects.Add(e);
        }

        public VisualEffect GetTopEffect()
        {
            return Effects.Last();
        }
        
    }
}
