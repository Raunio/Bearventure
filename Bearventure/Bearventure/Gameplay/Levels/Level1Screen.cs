#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Bearventure.Gameplay.Levels;
using XmlItems;
using Bearventure.Gameplay;
using Bearventure.Engine;
using Bearventure.Engine.Effects;
using Bearventure.Gameplay.Characters;
using Bearventure.Gameplay.Characters.Skills;
using Bearventure.Gameplay.HUD;
using Bearventure.Engine.CollisionDetection;
using Bearventure.Gameplay.GameObjects;

#endregion

namespace Bearventure
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class Level1Screen : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteFont gameFont;

        LevelBackground background;
        Camera camera;
        CameraController cameraController;
        HeadsUpDisplay hud;

        List<Enemy> enemies;
        List<Platform> platforms;
        List<Ladder> ladders;
        Player player;

        float pauseAlpha;

        InputAction pauseAction;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public Level1Screen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            pauseAction = new InputAction(
                new Buttons[] { Buttons.Start, Buttons.Back },
                new Keys[] { Keys.Escape },
                true);
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                if (content == null)
                    content = new ContentManager(ScreenManager.Game.Services, "Content");
                
                player = new Player(content);
                XmlReader.Initialize(content, player);
                background = new LevelBackground(XmlReader.LevelInformation("Levels/Testilevel2/Testilevel2LevelInfo"), content);
                player.position = XmlReader.StartPoint("Levels/Testilevel2/Items/Testilevel2_StartPoint");
              
                gameFont = content.Load<SpriteFont>(Constants.GameFont);
                
                enemies = XmlReader.EnemyList("Levels/Testilevel2/Items/Testilevel2_Enemies");
                Enemy owl = new Enemy(Constants.EnemyType.DelayOwl, new Vector2(700, 3500), content.Load<Texture2D>("Sprites/paskapollo"), player);
                enemies.Add(owl);

                platforms = new List<Platform>();
                platforms.Add(new Platform(content, Constants.PlatformType.Basic, new Vector2(700, 3600)));

                ladders = new List<Ladder>();
                ladders.Add(new Ladder(content, Constants.LadderType.Wooden, new Vector2(500, 4000)));

                CombatManager.Instance.Initialize(player, enemies);
                hud = new HeadsUpDisplay();
                hud.Initialize(content, ResolutionManager.graphicsDevice, enemies, player);
                cameraController = new CameraController();
                cameraController.AssingTo(player);

                

                Texture2D[] collisionMap = new Texture2D[background.Fractions];

                CollisionHandler.Initialize(new CollisionMap("Levels/Testilevel2/CollisionMap/Testilevel2CollisionMap_", 32, 4), enemies, player, platforms, content);

                CharacterPhysics.Gravity = 1.5f;

                MusicManager.Instance.LoadContent(content);
                camera = new Camera(ScreenManager.GraphicsDevice.Viewport, new Vector2(background.Width, background.Height));
                
                MusicManager.Instance.PlayLevel1Music();
                VisualEffectManager.Instance.Initialize(content, ResolutionManager.graphicsDevice);
                VisualEffectManager.Instance.InitializeTerrainEffects(player, enemies);
                // once the load has finished, we use ResetElapsedTime to tell the game's
                // timing mechanism that we have just finished a very long frame, and that
                // it should not try to catch up.s
                ScreenManager.Game.ResetElapsedTime();
            }

#if WINDOWS_PHONE
            if (Microsoft.Phone.Shell.PhoneApplicationService.Current.State.ContainsKey("PlayerPosition"))
            {
                playerPosition = (Vector2)Microsoft.Phone.Shell.PhoneApplicationService.Current.State["PlayerPosition"];
                enemyPosition = (Vector2)Microsoft.Phone.Shell.PhoneApplicationService.Current.State["EnemyPosition"];
            }
#endif
        }


        public override void Deactivate()
        {
#if WINDOWS_PHONE
            Microsoft.Phone.Shell.PhoneApplicationService.Current.State["PlayerPosition"] = playerPosition;
            Microsoft.Phone.Shell.PhoneApplicationService.Current.State["EnemyPosition"] = enemyPosition;
#endif

            base.Deactivate();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void Unload()
        {
            content.Unload();

#if WINDOWS_PHONE
            Microsoft.Phone.Shell.PhoneApplicationService.Current.State.Remove("PlayerPosition");
            Microsoft.Phone.Shell.PhoneApplicationService.Current.State.Remove("EnemyPosition");
#endif
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                foreach (Enemy enemy in enemies)
                {
                    enemy.Update(gameTime);
                }

                player.Update(gameTime);

                foreach (Platform p in platforms)
                {
                    p.Update(gameTime);
                }

                foreach (Ladder l in ladders)
                {
                    l.Update(gameTime);
                }

                cameraController.Update(gameTime);

                camera.LookAt(cameraController.Position);
                VisualEffectManager.Instance.UpdateEffects(gameTime);
                hud.Update(gameTime, camera.Position);
            }

            
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (pauseAction.Evaluate(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
#if WINDOWS_PHONE
                ScreenManager.AddScreen(new PhonePauseScreen(), ControllingPlayer);
#else
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
#endif
            }
            else
            {

            }
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            ResolutionManager.BeginDraw();


            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;


            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.None,
                    RasterizerState.CullNone, null, camera.GetTransformation(ResolutionManager.graphicsDevice.GraphicsDevice));
            

            background.Draw(spriteBatch);
            background.DrawGrid(spriteBatch);
            
            CollisionHandler.Map.DrawMap(spriteBatch);
            CollisionHandler.Map.DrawGrid(spriteBatch);

            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(spriteBatch);
                //if (enemy.ActiveSkill != null)
                    //enemy.ActiveSkill.DrawHitBox(spriteBatch, content.Load<Texture2D>("Sprites/player"));
            }      

            player.Draw(spriteBatch);

            foreach (Platform p in platforms)
            {
                p.Draw(spriteBatch);
            }

            foreach (Ladder l in ladders)
            {
                l.Draw(spriteBatch);
            }

            VisualEffectManager.Instance.DrawEffects(spriteBatch);

            //if(player.ActiveSkill != null)
                //player.ActiveSkill.DrawHitBox(spriteBatch, content.Load<Texture2D>("Sprites/player"));

            hud.Draw(spriteBatch);
            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }


        #endregion
    }
}
