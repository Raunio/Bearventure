using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Content;
using XmlItems;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Bearventure.Gameplay.Characters;
using Bearventure.Gameplay.GameObjects;

namespace Bearventure
{
    public static class XmlReader
    {
        private static ContentManager mContent;
        private static Player mPlayer;

        /// <summary>
        /// Initializes the XmlReader by passing it pointers to content and the game player.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="player">Pointer to player is required for enemy AI initializations</param>
        public static void Initialize(ContentManager content, Player player)
        {
            mContent = content;
            mPlayer = player;
        }
        public static List<Enemy> EnemyList(string XmlPath)
        {
            List<XmlEnemy> xEnemies = mContent.Load<List<XmlEnemy>>(XmlPath);
            List<Enemy> enemies = new List<Enemy>();         

            foreach (XmlEnemy xEnemy in xEnemies)
            {
                Bearventure.Constants.EnemyType type = Bearventure.Constants.EnemyType.BlackMetalBadger;
                Bearventure.Constants.BehaviourType behaviourType = Constants.BehaviourType.Default;
                Texture2D spriteSheet = null;

                switch (xEnemy.Type)
                {
                    case "BlackmetalBadger":
                        type = Constants.EnemyType.BlackMetalBadger;
                        spriteSheet = mContent.Load<Texture2D>(Constants.BlackMetalBadger);
                        break;
                    case "DelayOwl":
                        type = Constants.EnemyType.DelayOwl;
                        spriteSheet = mContent.Load<Texture2D>(Constants.DelayOwl);
                        break;
                    case "WahCat":
                        type = Constants.EnemyType.WahCat;
                        break;
                    case "OscillatorWorm":
                        type = Constants.EnemyType.OscillatorWorm;
                        spriteSheet = mContent.Load<Texture2D>(Constants.OscillatorWorm);
                        break;
                }

                switch (xEnemy.behaviour)
                {
                    case XmlEnemy.Behaviour.Default:
                        behaviourType = Constants.BehaviourType.Default;
                        break;
                    case XmlEnemy.Behaviour.FixedPatrol:
                        behaviourType = Constants.BehaviourType.FixedPatrol;
                        break;
                    case XmlEnemy.Behaviour.FreePatrol:
                        behaviourType = Constants.BehaviourType.FreePatrol;
                        break;
                    case XmlEnemy.Behaviour.Passive:
                        behaviourType = Constants.BehaviourType.Passive;
                        break;
                }

                Enemy e = new Enemy(type, behaviourType, xEnemy.X, xEnemy.Y, mPlayer, xEnemy.PatrolPoint_A, xEnemy.PatrolPoint_B, 2000, mContent);
                e.Name = xEnemy.Name;

                enemies.Add(e);
                
            }

            return enemies;
        }
        /// <summary>
        /// Returns a LevelInfo object from xml.
        /// </summary>
        /// <param name="XmlPath"></param>
        /// <returns></returns>
        public static LevelInfo LevelInformation(string XmlPath)
        {
            return mContent.Load<LevelInfo>(XmlPath);
        }
        /// <summary>
        /// Returns the start point of a level.
        /// </summary>
        /// <param name="XmlPath"></param>
        /// <returns></returns>
        public static Vector2 StartPoint(string XmlPath)
        {
            XmlStartPoint xSp = mContent.Load<XmlStartPoint>(XmlPath);

            return new Vector2(xSp.X, xSp.Y);
        }

        public static List<CharacterAnimation.AnatomicInfo> LoadAnatomicInfo(string XmlPath)
        {
            return mContent.Load<List<CharacterAnimation.AnatomicInfo>>(XmlPath);
        }
    }
}
