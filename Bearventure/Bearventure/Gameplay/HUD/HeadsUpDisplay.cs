using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Bearventure.Gameplay.Characters.Skills;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Bearventure.Gameplay.Characters;

namespace Bearventure.Gameplay.HUD
{
    public class HeadsUpDisplay
    {
        private ContentManager content;
        private Vector2 logOffset;
        private Vector2 logPosition;
        private List<Enemy> enemies;
        private Player player;

        private const int DrawDistance = 350;

        private List<HealthBar> healthBars = new List<HealthBar>();

        public void Initialize(ContentManager content, GraphicsDeviceManager graphics, List<Enemy> enemies, Player player)
        {
            this.content = content;
            this.logOffset = new Vector2(-graphics.GraphicsDevice.Viewport.Width / 2 + 300, graphics.GraphicsDevice.Viewport.Height / 2 - 250);
            this.enemies = enemies;
            this.player = player;

            foreach (Enemy e in enemies)
            {
                HealthBar hb = new HealthBar(new Rectangle(0, 0, 100, 8), e, new Vector2(0, -e.BoundingBox.Height), false);
                hb.Initialize(graphics, content);
                hb.EdgeColor = Color.Black;
                hb.ShowEdges = true;               
                hb.BarColor = Color.DarkRed;
                hb.EdgeThickness = 1;
                healthBars.Add(hb);
            }

            HealthBar playerBar = new HealthBar(new Rectangle(0, 0, 210, 15), player, new Vector2(135, 15), true);
            playerBar.Initialize(graphics, content);
            playerBar.EdgeColor = Color.Black;
            playerBar.ShowEdges = true;
            playerBar.BarColor = Color.DarkRed;
            playerBar.EdgeThickness = 1;
            playerBar.ShowText = true;
            playerBar.TextColor = Color.Yellow;
            healthBars.Add(playerBar);
        }

        public void Update(GameTime gameTime, Vector2 cameraPosition)
        {
            logPosition = cameraPosition + logOffset;

            foreach (HealthBar hb in healthBars)
                hb.Update(gameTime, cameraPosition);
        }
        /// <summary>
        /// Draws the HUD
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            DrawHealthBars(spriteBatch);

            DrawCombatLog(spriteBatch);
        }

        private void DrawCombatLog(SpriteBatch spriteBatch)
        {
            int rows = 5;
            int rowHeight = 20;

            if (CombatManager.Instance.CombatLog.Count < rows)
            {
                int counter = 0;
                foreach (string s in CombatManager.Instance.CombatLog)
                {
                    spriteBatch.DrawString(content.Load<SpriteFont>(Constants.HudFont), CombatManager.Instance.CombatLog[counter],
                        new Vector2(logPosition.X, logPosition.Y + (rowHeight * counter)), Color.Yellow);

                    counter++;
                }
            }
            else
            {
                int i = CombatManager.Instance.CombatLog.Count - rows;
                int j = 0;

                while (i < CombatManager.Instance.CombatLog.Count)
                {
                    spriteBatch.DrawString(content.Load<SpriteFont>(Constants.HudFont), CombatManager.Instance.CombatLog[i],
                        new Vector2(logPosition.X, logPosition.Y + (rowHeight * j)), Color.Yellow);

                    i++;
                    j++;
                }
            }
        }

        private void DrawHealthBars(SpriteBatch spriteBatch)
        {
            foreach (HealthBar hb in healthBars)
            {
                if (!hb.FixedPosition)
                {
                    if (hb.CurrentValue > 0 && Vector2.Distance(player.position, hb.Position) < DrawDistance && Globals.DisplayHealthBars)
                        hb.Draw(spriteBatch);
                }
                else
                    hb.Draw(spriteBatch);
            } 
        }
    }
}
