using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bearventure.Engine.Effects;
using Microsoft.Xna.Framework;
using Bearventure.Gameplay.Characters;
using Microsoft.Xna.Framework.Graphics;
using Bearventure.Engine.CollisionDetection;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace Bearventure.Gameplay.GameObjects
{
    public class GameObjectSpawner
    {
        private List<ObjectSpawn> objectsToSpawn;
        private VisualEffect effect;
        private List<Enemy> gameObjects;
        private float spawnInterval;
        private float spawnTimer;
        private int spawnCounter;
        private int objectIndexCounter;
        private Vector2 position;
        private Texture2D texture;
        private Player player;
        private ContentManager content;
        private SoundEffect spawnSound;

        private struct ObjectSpawn
        {
            public Constants.EnemyType objectType;
            public int amount;
            public Vector2 spawnVelocity;
        };
        /// <summary>
        /// Initializes a game object spawner that creates new objects to the game with given parameters.
        /// </summary>
        /// <param name="gameObjects"></param>
        public GameObjectSpawner(Texture2D texture, Vector2 position, List<Enemy> gameObjects, float spawnInterval, Player player, ContentManager content)
        {
            objectsToSpawn = new List<ObjectSpawn>();
            this.gameObjects = gameObjects;
            this.spawnInterval = spawnInterval;
            this.position = position;
            this.texture = texture;
            this.player = player;
            this.content = content;
        }
        /// <summary>
        /// Add an object to spawn.
        /// </summary>
        /// <param name="obj">The object</param>
        /// <param name="amount">How many objects</param>
        /// <param name="spawnVelocity">Starting velocity for object</param>
        public void AddObject(Constants.EnemyType type, int amount, Vector2 spawnVelocity)
        {
            ObjectSpawn spawn = new ObjectSpawn();
            spawn.objectType = type;
            spawn.amount = amount;
            spawn.spawnVelocity = spawnVelocity;
            objectsToSpawn.Add(spawn);
        }
        /// <summary>
        /// Add a visual effect for spawning event.
        /// </summary>
        /// <param name="effect"></param>
        public void AddEffect(VisualEffect effect)
        {
            this.effect = effect;
        }

        public void AddSoundEffect(SoundEffect sound)
        {
            this.spawnSound = sound;
        }

        public void Update(GameTime gameTime)
        {
            spawnTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (spawnTimer >= spawnInterval && objectIndexCounter < objectsToSpawn.Count)
            {
                SpawnObject(objectsToSpawn[objectIndexCounter]);

                spawnCounter++;

                spawnTimer = 0;

                if (spawnCounter == objectsToSpawn[objectIndexCounter].amount)
                    objectIndexCounter++;
            }
        }

        private void SpawnObject(ObjectSpawn spawn)
        {
            Enemy e = new Enemy(spawn.objectType, Constants.BehaviourType.Default, (int)position.X, (int)position.Y, player, 0, 0, 0, content);
            e.ChangeVelocity(spawn.spawnVelocity.X, spawn.spawnVelocity.Y);
            e.ChangePosition(position);
            e.SetState(Constants.CharacterState.Knocked);
            CollisionHandler.AddObject(e);
            gameObjects.Add(e);

            SoundEffectManager.Instance.PlaySpawnSound(position, e.Type);

            if (effect != null)
                VisualEffectManager.Instance.CreateEffect(effect.TextureAsset, this.position, 700);
            if (spawnSound != null)
                SoundEffectManager.Instance.PlaySoundFromPosition(this.position, spawnSound);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(texture != null)
                spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
