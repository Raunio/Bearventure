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

        Texture2D yellow;
        Texture2D red;

        LayeredLevelBackground layeredBackground;
        Camera camera;
        CameraController cameraController;

        List<Enemy> enemies;
        List<Platform> platforms;
        List<Ladder> ladders;
        Player _player;
        HUDScreen hudScreen;

        List<GameObjectSpawner> spawners;

        InputHandler inputHandler;

        float pauseAlpha;

        InputAction pauseAction;
        InputAction skillScreenAction;

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
            skillScreenAction = new InputAction(
                new Buttons[] { Buttons.Back },
                new Keys[] { Keys.Back },
                true);
        }

        public void DestroyScreen()
        {
            ScreenManager.RemoveScreen(this);
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
                
                _player = new Player(content);
                XmlReader.Initialize(content, _player);

                inputHandler = new InputHandler();
                inputHandler.InitControls();

                #region Layered Background TESTING

                layeredBackground = new LayeredLevelBackground();

                LevelBackground treeLayer = new LevelBackground("Testilevel2/Background/PuutJaOksat", "Testilevel2PuuJaOksat", 32, content);
                LevelBackground platformLayer = new LevelBackground("Testilevel2/Background/Platform", "Testilevel2Platform", 32, content);
                LevelBackground static1Layer = new LevelBackground("Testilevel2/Background/Static1", "Testilevel2Static1", 32, content);
                LevelBackground static2Layer = new LevelBackground("Testilevel2/Background/Static1Takana", "Testilevel2Static1Takana", 32, content);
                LevelBackground Layer2 = new LevelBackground("Testilevel2/Background/2", "Testilevel2-2", 32, content);
                LevelBackground Layer3 = new LevelBackground("Testilevel2/Background/3", "Testilevel2-3", 32, content);
                LevelBackground Layer4 = new LevelBackground("Testilevel2/Background/4", "Testilevel2-4", 32, content);
                LevelBackground skyLayer = new LevelBackground("Testilevel2/Background/Sky", "Testilevel2Sky", 32, content);

                
                layeredBackground.AddLayer(platformLayer, 1, new Vector2(0, 0));
                layeredBackground.AddLayer(treeLayer, 3, Vector2.Zero);
                layeredBackground.AddLayer(static2Layer, 2, new Vector2(0, 0));
                layeredBackground.AddLayer(static1Layer, 0, new Vector2(0, 0)); 
                layeredBackground.AddLayer(Layer2, 4, new Vector2(0.2f, 0));
                layeredBackground.AddLayer(Layer3, 5, new Vector2(0.3f, 0));
                layeredBackground.AddLayer(Layer4, 6, new Vector2(0.55f, 0));           
                layeredBackground.AddLayer(skyLayer, 7, new Vector2(1f, 0f));

                layeredBackground.Initialize();

                #endregion

                yellow = content.Load<Texture2D>("Sprites/tosi");
                red = content.Load<Texture2D>("Sprites/red");
                
                _player.ChangePosition(XmlReader.StartPoint("Levels/Testilevel2/Items/Testilevel2_StartPoint"));
              
                gameFont = content.Load<SpriteFont>(Constants.GameFont);
                
                enemies = XmlReader.EnemyList("Levels/Testilevel2/Items/Testilevel2_Enemies");

                SoundEffectManager.Instance.MaxSoundEffectDistance = 750;

                platforms = new List<Platform>();
                Platform plat = new Platform(content, Constants.PlatformType.MovingGrassPlatform, new Vector2(7670, 4900));
                Platform fallingPlat = new Platform(content, Constants.PlatformType.FallingPlatform, new Vector2(7600, 4100));
                Platform fallingPlat2 = new Platform(content, Constants.PlatformType.FallingPlatform, new Vector2(7850, 4100));
                Platform fallingPlat3 = new Platform(content, Constants.PlatformType.FallingPlatform, new Vector2(8100, 4100));
                Platform fallingPlat4 = new Platform(content, Constants.PlatformType.FallingPlatform, new Vector2(8350, 4100));
                fallingPlat.InitCollapse(500);
                fallingPlat2.InitCollapse(500);
                fallingPlat3.InitCollapse(500);
                fallingPlat4.InitCollapse(500);
                plat.InitPatrol(5, 0.2f, 7670, 8400, 200f);
                platforms.Add(plat);
                platforms.Add(fallingPlat);
                platforms.Add(fallingPlat2);
                platforms.Add(fallingPlat3);
                platforms.Add(fallingPlat4);

                /*Enemy worm = new Enemy(Constants.EnemyType.OscillatorWorm, Constants.BehaviourType.Default, 3825, 3333, _player, 0, 0, 0, content);
                Enemy worm2 = new Enemy(Constants.EnemyType.OscillatorWorm, Constants.BehaviourType.Default, 3925, 3233, _player, 0, 0, 0, content);
                Enemy worm3 = new Enemy(Constants.EnemyType.OscillatorWorm, Constants.BehaviourType.Default, 4025, 3133, _player, 0, 0, 0, content);
                Enemy worm4 = new Enemy(Constants.EnemyType.OscillatorWorm, Constants.BehaviourType.Default, 4125, 3033, _player, 0, 0, 0, content);
                enemies.Add(worm);
                enemies.Add(worm2);
                enemies.Add(worm3);
                enemies.Add(worm4);
                */

                #region SPAWNER TESTING
                spawners = new List<GameObjectSpawner>();


                GameObjectSpawner badgerSpawner = new GameObjectSpawner(null, new Vector2(3730, 3340), enemies, 3000f, _player, content);
                badgerSpawner.AddEffect(new VisualEffect("VisualEffects/spawnTest"));
                badgerSpawner.AddSoundEffect(SoundEffectManager.Instance.SpawnerSound);
                badgerSpawner.SetActivation(Constants.SpawnerActivationType.Proximity);
                badgerSpawner.ActivationDistance = 200;
                badgerSpawner.AddObject(Constants.EnemyType.BlackMetalBadger, 1, new Vector2(0, -10));
                badgerSpawner.SetScaleModifier(0.25f, 1.2f, 0.1f);
                
                spawners.Add(badgerSpawner);

                GameObjectSpawner badgerSpawner2 = new GameObjectSpawner(null, new Vector2(3540, 4040), enemies, 3000f, _player, content);
                badgerSpawner2.AddEffect(new VisualEffect("VisualEffects/spawnTest"));
                badgerSpawner2.AddSoundEffect(SoundEffectManager.Instance.SpawnerSound);
                badgerSpawner2.SetActivation(Constants.SpawnerActivationType.Proximity);
                badgerSpawner2.ActivationDistance = 200;
                badgerSpawner2.AddObject(Constants.EnemyType.BlackMetalBadger, 1, new Vector2(0, -10));
                badgerSpawner2.SetScaleModifier(0.25f, 1.2f, 0.1f);
                
                spawners.Add(badgerSpawner2);

                GameObjectSpawner badgerSpawner3 = new GameObjectSpawner(null, new Vector2(3775, 4040), enemies, 3000f, _player, content);
                badgerSpawner3.AddEffect(new VisualEffect("VisualEffects/spawnTest"));
                badgerSpawner3.AddSoundEffect(SoundEffectManager.Instance.SpawnerSound);
                badgerSpawner3.SetActivation(Constants.SpawnerActivationType.Proximity);
                badgerSpawner3.ActivationDistance = 200;
                badgerSpawner3.AddObject(Constants.EnemyType.BlackMetalBadger, 1, new Vector2(0, -10));
                badgerSpawner3.SetScaleModifier(0.25f, 1.2f, 0.1f);
                
                spawners.Add(badgerSpawner3);

                GameObjectSpawner badgerSpawner4 = new GameObjectSpawner(null, new Vector2(4420, 4170), enemies, 3000f, _player, content);
                badgerSpawner4.AddEffect(new VisualEffect("VisualEffects/spawnTest"));
                badgerSpawner4.AddSoundEffect(SoundEffectManager.Instance.SpawnerSound);
                badgerSpawner4.SetActivation(Constants.SpawnerActivationType.Proximity);
                badgerSpawner4.ActivationDistance = 200;
                badgerSpawner4.AddObject(Constants.EnemyType.BlackMetalBadger, 1, new Vector2(0, -10));
                badgerSpawner4.SetScaleModifier(0.25f, 1.2f, 0.1f);
                
                spawners.Add(badgerSpawner4);

                //Animation worm = new Animation(content.Load<Texture2D>("Sprites/matoallas"), 0, 612, 264, 0, 5, 30);

                //GameObjectSpawner wormPool = new GameObjectSpawner(worm, new Vector2(8100, 5000), enemies, 500, _player, content);
                //spawners.Add(wormPool);
                #endregion

                ladders = new List<Ladder>();
                //ladders.Add(new Ladder(content, Constants.LadderType.Wooden, new Vector2(3050, 3600)));
                //ladders.Add(new Ladder(content, Constants.LadderType.Wooden, new Vector2(3050, 3838)));

                CombatManager.Instance.Initialize(_player, enemies);
                //hud = new HeadsUpDisplay();
                //hud.Initialize(content, ResolutionManager.graphicsDevice, enemies, _player);
                cameraController = new CameraController();
                cameraController.AssingTo(_player);

                

                Texture2D[] collisionMap = new Texture2D[layeredBackground.Fractions];

                CollisionHandler.Initialize(new CollisionMap("Levels/Testilevel2/CollisionMap/Testilevel2CollisionMap_", 8, 2), enemies, _player, platforms, ladders, content);

                CharacterPhysics.Gravity = 1.5f;

                MusicManager.Instance.LoadContent(content);
                camera = new Camera(ScreenManager.GraphicsDevice.Viewport, new Vector2(layeredBackground.LevelWidth, layeredBackground.LevelHeight));
                
                MusicManager.Instance.PlayLevel1Music();

                VisualEffectManager.Instance.Initialize(content, ResolutionManager.graphicsDevice);
                VisualEffectManager.Instance.InitializeTerrainEffects(_player, enemies);
                // once the load has finished, we use ResetElapsedTime to tell the game's
                // timing mechanism that we have just finished a very long frame, and that
                // it should not try to catch up.s
                ScreenManager.Game.ResetElapsedTime();
                hudScreen = new HUDScreen(content, _player, enemies, camera);
                ScreenManager.AddScreen(new HUDScreen(content, _player, enemies, camera), null);
                hudScreen.Activate();
                
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
            enemies.Clear();
            ladders.Clear();
            platforms.Clear();
            _player = null;
            CollisionHandler.ClearData();
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
                if (_player.IsActive)
                    _player.Update(gameTime);

                foreach (GameObjectSpawner spawner in spawners)
                {
                    spawner.Update(gameTime);
                }

                foreach (Enemy enemy in enemies)
                {
                    if (enemy.IsActive)
                    {
                        enemy.Update(gameTime);

                        enemy.PlayStepSounds();
                    }
                }

                foreach (Platform p in platforms)
                {
                    if(p.IsActive)
                    p.Update(gameTime);
                }

                foreach (Ladder l in ladders)
                {
                    if(l.IsActive)
                    l.Update(gameTime);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
                {
                    camera.Zoom -= 0.1f;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
                {
                    camera.Zoom += 0.1f;
                }
             
                cameraController.Update(gameTime);
                
                camera.LookAt(cameraController.Position, new Vector2(layeredBackground.LevelWidth, layeredBackground.LevelHeight));
                VisualEffectManager.Instance.UpdateEffects(gameTime);
                layeredBackground.Update(camera.Pos);
                SoundEffectManager.Instance.UpdatePlayerPosition(_player.Position);
                hudScreen.Update(gameTime);
                
                
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
            inputHandler.HandleInput(input, _player);
            

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
                ScreenManager.GetScreens();
                //ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
                ScreenManager.AddScreen(new DeathScreen(), ControllingPlayer);
                //DestroyScreen();
#endif
            }                   
            if (skillScreenAction.Evaluate(input, ControllingPlayer, out player))
            {
                ScreenManager.GetScreens();
                ScreenManager.AddScreen(new SkillScreen(), ControllingPlayer);
                
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
            

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None,
                    RasterizerState.CullNone, null, camera.GetTransformation(ResolutionManager.graphicsDevice.GraphicsDevice));

            layeredBackground.Draw(spriteBatch);
            //layeredBackground.DrawGrid(spriteBatch);
            
            //CollisionHandler.Map.DrawMap(spriteBatch);
            //CollisionHandler.Map.DrawGrid(spriteBatch);
            //CollisionHandler.DrawCollisionRectangles(spriteBatch, yellow, _player, _player.velocity);
            //_player.DrawBoundingBox(spriteBatch, red);

            foreach (GameObjectSpawner spawner in spawners)
            {
                spawner.Draw(spriteBatch);
            }


            foreach (Ladder l in ladders)
            {
                l.Draw(spriteBatch);
            }

            

            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(spriteBatch);
                //enemy.DrawBoundingBox(spriteBatch, red);
            }

            _player.Draw(spriteBatch);
            if (_player.ActiveSkill != null)
            {
                //_player.ActiveSkill.DrawHitBox(spriteBatch, red);
            }

            foreach (Platform p in platforms)
            {
                p.Draw(spriteBatch);
                //testing
                
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                //CollisionHandler.DrawCollisionRectangles(spriteBatch, yellow, enemies[i], enemies[i].velocity);
                //enemies[i].DrawBoundingBox(spriteBatch, red);
                //if (enemies[i].ActiveSkill != null)
                    //enemies[i].ActiveSkill.DrawHitBox(spriteBatch, red);
            }

            VisualEffectManager.Instance.DrawEffects(spriteBatch);
            
            //CollisionHandler.DrawCollisionRectangles(spriteBatch, _player, _player.velocity);

            //if(player.ActiveSkill != null)
                //player.ActiveSkill.DrawHitBox(spriteBatch, content.Load<Texture2D>("Sprites/player"));

            //hud.Draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin();
            hudScreen.Draw(spriteBatch);
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
