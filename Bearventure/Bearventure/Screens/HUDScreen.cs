using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bearventure.Gameplay.HUD;
using Microsoft.Xna.Framework.Content;
using Bearventure.Gameplay.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bearventure.Gameplay;
using Bearventure.Gameplay.Levels;

namespace Bearventure
{
    class HUDScreen : GameScreen
    {
        HeadsUpDisplay hud;
        ContentManager content;
        Player _player;
        List<Enemy> enemies;
        Camera camera;
    
        public HUDScreen(ContentManager content, Player _player, List<Enemy> enemies, Camera camera)
        {
            hud = new HeadsUpDisplay();
            this.content = content;
            this._player = _player;
            this.enemies = enemies;
            this.camera = camera;
            

        }

        public void Activate()
        {
            hud.Initialize(content, ResolutionManager.graphicsDevice, enemies, _player);
        }
        public void Update(GameTime gameTime)
        {
            hud.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            hud.Draw(spriteBatch);
        }

    }
}
