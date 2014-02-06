using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Bearventure.Gameplay.Characters.Skills;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Bearventure.Gameplay.Characters;
using Bearventure.Engine.CollisionDetection;

namespace Bearventure.Gameplay.HUD
{
    public class HeadsUpDisplay
    {
        private ContentManager content;
        private Vector2 logOffset;
        private Vector2 logPosition;
        private List<Enemy> enemies;
        private Player player;
        private GraphicsDeviceManager graphics;
        private Vector2 origin;

        private int frameCounter;
        private double fpsTimer;
        private int framesPerSecond;
        private Vector2 fpsCounterPosition;
        private Vector2 fpsCounterPositionOffset;
        private Vector2 playerLocation;
        private Vector2 playerLocationOffset;
        private Vector2 playerZonesLocation;
        private Vector2 playerZonesLocationOffset;
        private Vector2 cameraZonesLocation;
        private Vector2 cameraZonesOffset;

        private const int DrawDistance = 350;

        private List<StatusBar> enemyHealthBars = new List<StatusBar>();
        private StatusBar playerHealthBar;
        private StatusBar playerRageBar;

        public void Initialize(ContentManager content, GraphicsDeviceManager graphics, List<Enemy> enemies, Player player)
        {
            this.content = content;
            this.logOffset = new Vector2(-graphics.GraphicsDevice.Viewport.Width / 2 + 300, graphics.GraphicsDevice.Viewport.Height / 2 - 250);
            this.fpsCounterPositionOffset = new Vector2(-graphics.GraphicsDevice.Viewport.Width / 2 + 250, -graphics.GraphicsDevice.Viewport.Height / 2 + 250);
            this.playerLocationOffset = new Vector2(-graphics.GraphicsDevice.Viewport.Width / 2 + 250, -graphics.GraphicsDevice.Viewport.Height / 2 + 300);
            this.playerZonesLocationOffset = new Vector2(-graphics.GraphicsDevice.Viewport.Width / 2 + 250, -graphics.GraphicsDevice.Viewport.Height / 2 + 350);
            this.cameraZonesOffset = new Vector2(-graphics.GraphicsDevice.Viewport.Width / 2 + 250, -graphics.GraphicsDevice.Viewport.Height / 2 + 400);
            this.enemies = enemies;
            this.player = player;
            this.graphics = graphics;

            foreach (Enemy e in enemies)
            {
                StatusBar hb = new StatusBar(new Rectangle(0, 0, 100, 8));
                hb.Initialize(graphics, content);
                hb.EdgeColor = Color.Black;
                hb.ShowEdges = true;               
                hb.BarColor = Color.DarkRed;
                hb.EdgeThickness = 1;
                enemyHealthBars.Add(hb);
            }

            playerHealthBar = new StatusBar(new Rectangle(0, 0, 210, 15));
            playerHealthBar.Initialize(graphics, content);
            playerHealthBar.EdgeColor = Color.Black;
            playerHealthBar.ShowEdges = true;
            playerHealthBar.BarColor = Color.DarkRed;
            playerHealthBar.EdgeThickness = 1;
            playerHealthBar.ShowText = true;
            playerHealthBar.TextColor = Color.Yellow;

            playerRageBar = new StatusBar(new Rectangle(0, 0, 210, 15));
            playerRageBar.Initialize(graphics, content);
            playerRageBar.EdgeColor = Color.Black;
            playerRageBar.ShowEdges = true;
            playerRageBar.BarColor = Color.Yellow;
            playerRageBar.EdgeThickness = 1;
            playerRageBar.ShowText = true;
            playerRageBar.TextColor = Color.Black;
        }

        public void Update(GameTime gameTime, Vector2 cameraPosition)
        {
            UpdateBars();

            logPosition = cameraPosition + logOffset;
            fpsCounterPosition = cameraPosition + fpsCounterPositionOffset;
            playerLocation = cameraPosition + playerLocationOffset;
            playerZonesLocation = cameraPosition + playerZonesLocationOffset;
            cameraZonesLocation = cameraPosition + cameraZonesOffset;

            for (int i = 0; i < enemies.Count; i++)
            {
                enemyHealthBars[i].Update(gameTime, enemies[i].Position, 0, enemies[i].maxHealth, enemies[i].health);
            }

            origin = new Vector2(cameraPosition.X - graphics.GraphicsDevice.Viewport.Width / 2, cameraPosition.Y - graphics.GraphicsDevice.Viewport.Height / 2);

            playerHealthBar.Update(gameTime, origin + new Vector2(ResolutionManager.GetVirtualResolution().X *0.1f , ResolutionManager.GetVirtualResolution().Y *0.96f), 0, player.maxHealth, player.health);
            playerRageBar.Update(gameTime, origin + new Vector2(ResolutionManager.GetVirtualResolution().X * 0.9f, ResolutionManager.GetVirtualResolution().Y * 0.96f), 0, player.MaxSkillResource, player.CurrentSkillResource);

            UpdateFPS(gameTime);
        }

        private void UpdateBars()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (!enemies[i].IsActive)
                    enemyHealthBars.RemoveAt(i);
                if (i == enemyHealthBars.Count)
                {
                    StatusBar hb = new StatusBar(new Rectangle(0, 0, 100, 8));
                    hb.Initialize(graphics, content);
                    hb.EdgeColor = Color.Black;
                    hb.ShowEdges = true;
                    hb.BarColor = Color.DarkRed;
                    hb.EdgeThickness = 1;
                    enemyHealthBars.Add(hb);
                }
            }
        }

        /// <summary>
        /// Draws the HUD
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if(Globals.DisplayHealthBars)
                DrawHealthBars(spriteBatch);

            DrawCombatLog(spriteBatch);

            DrawDebugInfo(spriteBatch);
            playerHealthBar.Draw(spriteBatch);
            playerRageBar.Draw(spriteBatch);
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
            foreach (StatusBar hb in enemyHealthBars)
            {
                if (hb.CurrentValueDraw > 0 && Vector2.Distance(player.Position, hb.Position) < DrawDistance)
                    hb.Draw(spriteBatch);
            } 
        }

        private void DrawDebugInfo(SpriteBatch spriteBatch)
        {
            string playerZones = "null";
            string cameraZones = "null";

            Rectangle cameraBox = new Rectangle((int)origin.X,
                (int)origin.Y,
                ResolutionManager.graphicsDevice.GraphicsDevice.Viewport.Width,
                ResolutionManager.graphicsDevice.GraphicsDevice.Viewport.Height);

            spriteBatch.DrawString(content.Load<SpriteFont>(Constants.HudFont), "Fps: " + framesPerSecond, fpsCounterPosition, Color.Yellow);
            spriteBatch.DrawString(content.Load<SpriteFont>(Constants.HudFont), "Player position: " + player.Position, playerLocation, Color.Yellow);

            if(CollisionHandler.OnAdjustedZones(player.BoundingBox).Count == 1)
                playerZones = CollisionHandler.OnAdjustedZones(player.BoundingBox)[0].ToString();
            else if (CollisionHandler.OnAdjustedZones(player.BoundingBox).Count > 1)
                playerZones = CollisionHandler.OnAdjustedZones(player.BoundingBox)[0] + ", " + CollisionHandler.OnAdjustedZones(player.BoundingBox)[1];


            spriteBatch.DrawString(content.Load<SpriteFont>(Constants.HudFont), "Player on zones: " + playerZones, playerZonesLocation, Color.Yellow);

            if (CollisionHandler.OnAdjustedZones(cameraBox).Count == 1)
                cameraZones = CollisionHandler.OnAdjustedZones(cameraBox)[0].ToString();
            else if (CollisionHandler.OnAdjustedZones(cameraBox).Count > 1)
                cameraZones = CollisionHandler.OnAdjustedZones(cameraBox)[0] + ", " + CollisionHandler.OnAdjustedZones(cameraBox)[1];

            spriteBatch.DrawString(content.Load<SpriteFont>(Constants.HudFont), "Camera on zones: " + cameraZones, cameraZonesLocation, Color.Yellow);
        }

        private void UpdateFPS(GameTime gameTime)
        {
            frameCounter++;

            fpsTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (fpsTimer >= 1)
            {            
                framesPerSecond = frameCounter;
                frameCounter = 0;
                fpsTimer = 0;
            }
        }
    }
}
