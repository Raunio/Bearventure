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
        private Animation animation;
        private Player player;
        private ContentManager content;
        private SoundEffect spawnSound;
        private Constants.SpawnerActivationType activationType;
        private bool isActive = false;
        private float initialScale;
        private float ultimateScale;
        private float scalingSpeed;

        private Animation preActivationAnimation;
        private Animation defaultAnimation;

        public int ActivationDistance
        {
            get;
            set;
        }

        public Animation PreActivationAnimation
        {
            get
            {
                return preActivationAnimation;
            }
            set
            {
                preActivationAnimation = value;
            }
        }

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
        public GameObjectSpawner(Animation animation, Vector2 position, List<Enemy> gameObjects, float spawnInterval, Player player, ContentManager content)
        {
            objectsToSpawn = new List<ObjectSpawn>();
            this.gameObjects = gameObjects;
            this.spawnInterval = spawnInterval;
            this.position = position;
            this.player = player;
            this.content = content;
            this.spawnTimer = spawnInterval;
            this.animation = animation;
            this.defaultAnimation = animation;
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

        public void SetScaleModifier(float initial, float ultimate, float speed)
        {
            this.initialScale = initial;
            this.ultimateScale = ultimate;
            this.scalingSpeed = speed;
        }

        public void AddSoundEffect(SoundEffect sound)
        {
            this.spawnSound = sound;
        }

        public void SetActivation(Constants.SpawnerActivationType type)
        {
            this.activationType = type;
        }

        public void Update(GameTime gameTime)
        {
            if(animation != null)
                animation.Animate(gameTime);

            UpdateActivation();

            spawnTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (activationType == Constants.SpawnerActivationType.Automatic || isActive)
            {
                if(preActivationAnimation == null || preActivationAnimation.HasFinished)
                    if (spawnTimer >= spawnInterval && objectIndexCounter < objectsToSpawn.Count)
                    {
                        SpawnObject(objectsToSpawn[objectIndexCounter]);

                        spawnCounter++;

                        spawnTimer = 0;

                        if (spawnCounter == objectsToSpawn[objectIndexCounter].amount)
                            objectIndexCounter++;
                    }
            }
        }

        private void UpdateActivation()
        {
            if (Vector2.Distance(player.Position, position) <= ActivationDistance)
            {
                isActive = true;

                if (preActivationAnimation != null)
                    animation = preActivationAnimation;
            }
            else
                animation = defaultAnimation;
        }

        private void SpawnObject(ObjectSpawn spawn)
        {
            Enemy e = new Enemy(spawn.objectType, Constants.BehaviourType.Default, (int)position.X, (int)position.Y, player, 0, 0, 0, content);
            e.ChangeVelocity(spawn.spawnVelocity.X, spawn.spawnVelocity.Y);

            e.SetState(Constants.CharacterState.Spawning);

            if (initialScale != 0 && ultimateScale != 0 && scalingSpeed != 0)
            {
                e.SetScaling(initialScale, ultimateScale, scalingSpeed);
            }

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
            if(animation != null)
                spriteBatch.Draw(animation.spriteSheet, position, animation.FrameRectangle, Color.White, animation.Rotation, animation.Origin, 2f, animation.Effects, animation.LayerDepth);
        }
    }
}
