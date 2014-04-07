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
        private List<Enemy> enemies;
        private Player player;
        private GraphicsDeviceManager graphics;
        private Vector2 origin;

        private Vector2 playerLocation;
        private Vector2 playerLocationOffset;
        private Vector2 playerZonesLocation;
        private Vector2 playerZonesLocationOffset;

        private Point resolution;

        private const int DrawDistance = 350;

        private StatusBar playerHealthBar;
        private StatusBar playerRageBar;

        private SkillBar skillBar;

        public void Initialize(ContentManager content, GraphicsDeviceManager graphics, List<Enemy> enemies, Player player)
        {
            this.content = content;
            this.playerLocationOffset = new Vector2(-graphics.GraphicsDevice.Viewport.Width / 2 + 250, -graphics.GraphicsDevice.Viewport.Height / 2 + 300);
            this.playerZonesLocationOffset = new Vector2(-graphics.GraphicsDevice.Viewport.Width / 2 + 250, -graphics.GraphicsDevice.Viewport.Height / 2 + 350);
            this.enemies = enemies;
            this.player = player;
            this.graphics = graphics;


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

            skillBar = new SkillBar(player);
        }

        public void Update(GameTime gameTime)
        {
            resolution = ResolutionManager.GetResolution();

            /*playerLocation = resolution + playerLocationOffset;
            playerZonesLocation = resolution + playerZonesLocationOffset;*/


            origin = new Vector2(resolution.X - graphics.GraphicsDevice.Viewport.Width / 2, resolution.Y - graphics.GraphicsDevice.Viewport.Height / 2);

            playerHealthBar.Update(gameTime, new Vector2(resolution.X *0.1f , resolution.Y *0.96f), 0, player.maxHealth, player.health);
            playerRageBar.Update(gameTime, new Vector2(resolution.X * 0.9f, resolution.Y * 0.96f), 0, player.MaxSkillResource, player.CurrentSkillResource);

            skillBar.Update(gameTime, new Vector2(resolution.X * 0.5f - skillBar.BarWidth / 2, resolution.Y * 0.9f));
        }


        /// <summary>
        /// Draws the HUD
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            DrawDebugInfo(spriteBatch);
            playerHealthBar.Draw(spriteBatch);
            playerRageBar.Draw(spriteBatch);
            skillBar.Draw(spriteBatch);
        }


        private void DrawDebugInfo(SpriteBatch spriteBatch)
        {
            string playerZones = "null";
            string cameraZones = "null";

            Rectangle cameraBox = new Rectangle((int)origin.X,
                (int)origin.Y,
                ResolutionManager.graphicsDevice.GraphicsDevice.Viewport.Width,
                ResolutionManager.graphicsDevice.GraphicsDevice.Viewport.Height);

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


        }

    }
}
