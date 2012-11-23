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

namespace Bearventure
{
    public static class XmlReader
    {
        private static ContentManager content;
        private static Player player;

        public static void Initialize(ContentManager _content, Player _player)
        {
            content = _content;
            player = _player;
        }
        public static List<Enemy> EnemyList(string XmlPath)
        {
            List<XmlEnemy> xEnemies = content.Load<List<XmlEnemy>>(XmlPath);
            List<Enemy> enemies = new List<Enemy>();         

            foreach (XmlEnemy xEnemy in xEnemies)
            {
                Bearventure.Constants.EnemyType type = Bearventure.Constants.EnemyType.BlackMetalBadger;
                Texture2D spriteSheet = null;

                switch (xEnemy.Type)
                {
                    case "BlackmetalBadger":
                        type = Constants.EnemyType.BlackMetalBadger;
                        spriteSheet = content.Load<Texture2D>(Constants.BlackMetalBadger);
                        break;
                    case "DelayOwl":
                        type = Constants.EnemyType.DelayOwl;
                        break;
                    case "WahCat":
                        type = Constants.EnemyType.WahCat;
                        break;
                }

                Enemy e = new Enemy(type, xEnemy.X, xEnemy.Y, spriteSheet, player, xEnemy.PatrolPoint_A, xEnemy.PatrolPoint_B);
                e.Name = xEnemy.Name;
                enemies.Add(e);
            }

            return enemies;
        }

        public static LevelInfo LevelInformation(string XmlPath)
        {
            return content.Load<LevelInfo>(XmlPath);
        }

        public static Vector2 StartPoint(string XmlPath)
        {
            XmlStartPoint xSp = content.Load<XmlStartPoint>(XmlPath);

            return new Vector2(xSp.X, xSp.Y);
        }
    }
}
