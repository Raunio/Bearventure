using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bearventure.Gameplay.Characters.Skills;
using Bearventure.Gameplay.Characters;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Bearventure.Gameplay.GameObjects.Characters.Skills
{
    public class PlayerSkills
    {
        public static CharacterSkill HeadStab
        {
            get;
            private set;
        }

        public static CharacterSkillCombo SevenDragons
        {
            get;
            private set;
        }

        public static CharacterSkill TestSkill
        {
            get;
            private set;
        }
        /// <summary>
        /// TODO: Pass selected skills from skill screen as an array. For now we will initialize all the skills.
        /// </summary>
        public static void InitSelectedSkills(ContentManager content, Player player)
        {
            #region Seven Dragons

            SevenDragons = new CharacterSkillCombo();

            #region Comboskill 1 init
            CharacterAnimation StraightPunch_R = new CharacterAnimation(content.Load<Texture2D>(Constants.PlayerSpriteSheet), 
                2, 200, 190, 0, 3, 37, SpriteEffects.None, 0f, 0f, false, false);

            StraightPunch_R.FreezeFrames = new Animation.FrameFreezer
            {
                Frames = new List<int>
                {
                    0,
                    2,
                },

                Amount = 95,
            };

            StraightPunch_R.CalculateBoundingBoxOffsets(player.BoundingBoxSize, Constants.DirectionX.Right);

            CharacterAnimation StraightPunch_L = new CharacterAnimation(content.Load<Texture2D>(Constants.PlayerSpriteSheet), 
                2, 200, 190, 0, 3, 37, SpriteEffects.FlipHorizontally, 0f, 0f, false, false);

            StraightPunch_L.FreezeFrames = new Animation.FrameFreezer
            {
                Frames = new List<int>
                {
                    0,
                    2,
                },

                Amount = 95,
            };

            StraightPunch_L.CalculateBoundingBoxOffsets(player.BoundingBoxSize, Constants.DirectionX.Left);

            CharacterSkill skill1 = new CharacterSkill(player, StraightPunch_R, StraightPunch_L, 300, 3, Constants.DamageType.Crushing);
            skill1.HitSoundEffect = SoundEffectManager.Instance.Punch;

            skill1.ActivationSoundEffect = SoundEffectManager.Instance.Woosh;
            skill1.Acceleration = 0.25f;
            //skill1.StartVelocity = new Vector2(4, 0);
            skill1.UltimateVelocityX = 0;
            //skill1.InflictForce = new Vector2(10, -5);
            skill1.DamagingFrames = new List<int>
            {
                2,
            };
            skill1.HitBoxPositions[0] = new Vector2(63, -27);
            skill1.HitBoxHeight = 30;
            skill1.HitBoxWidth = 30;

            #endregion
            #region ComboSkill 2 Initialization
            CharacterAnimation SweepingPunch_R = new CharacterAnimation(content.Load<Texture2D>(Constants.PlayerSpriteSheet), 
                3, 200, 190, 0, 3, 22, SpriteEffects.None, 0f, 0f, false, false);

            SweepingPunch_R.FreezeFrames = new Animation.FrameFreezer
            {
                Frames = new List<int>
                {
                    0,
                    2,
                },

                Amount = 95,
            };

            SweepingPunch_R.CalculateBoundingBoxOffsets(player.BoundingBoxSize, Constants.DirectionX.Right);

            CharacterAnimation SweepingPunch_L = new CharacterAnimation(content.Load<Texture2D>(Constants.PlayerSpriteSheet), 
                3, 200, 190, 0, 3, 22, SpriteEffects.FlipHorizontally, 0f, 0f, false, false);

            SweepingPunch_L.FreezeFrames = new Animation.FrameFreezer
            {
                Frames = new List<int>
                {
                    0,
                    2,
                },

                Amount = 95,
            };

            SweepingPunch_L.CalculateBoundingBoxOffsets(player.BoundingBoxSize, Constants.DirectionX.Left);

            CharacterSkill skill2 = new CharacterSkill(player, SweepingPunch_R, SweepingPunch_L, 300, 4, Constants.DamageType.Crushing);

            skill2.ActivationSoundEffect = SoundEffectManager.Instance.Woosh;
            skill2.HitSoundEffect = SoundEffectManager.Instance.Punch;
            skill2.Acceleration = 0.25f;
            //skill2.StartVelocity = new Vector2(3, 0);
            //skill2.InflictForce = new Vector2(10, -5);
            skill2.UltimateVelocityX = 0;
            skill2.DamagingFrames = new List<int>
            {
                2,
            };
            skill2.HitBoxPositions[0] = new Vector2(65, -38);
            skill2.HitBoxHeight = 25;
            skill2.HitBoxWidth = 25;

            #endregion
            #region ComboSkill 3 Initialization

            CharacterAnimation UpperCut_R = new CharacterAnimation(content.Load<Texture2D>(Constants.PlayerSpriteSheet), 
                4, 200, 190, 0, 6, 30, SpriteEffects.None, 0f, 0f, false, false);

            UpperCut_R.FreezeFrames = new Animation.FrameFreezer
            {
                Frames = new List<int>
                {
                    1,
                    5,
                },

                Amount = 125,
            };

            UpperCut_R.CalculateBoundingBoxOffsets(player.BoundingBoxSize, Constants.DirectionX.Right);

            CharacterAnimation UpperCut_L = new CharacterAnimation(content.Load<Texture2D>(Constants.PlayerSpriteSheet), 
                4, 200, 190, 0, 6, 30, SpriteEffects.FlipHorizontally, 0f, 0f, false, false);

            UpperCut_L.CalculateBoundingBoxOffsets(player.BoundingBoxSize, Constants.DirectionX.Left);

            UpperCut_L.FreezeFrames = new Animation.FrameFreezer
            {
                Frames = new List<int>
                {
                    1,
                    5,
                },

                Amount = 125,
            };

            CharacterSkill skill3 = new CharacterSkill(player, UpperCut_R, UpperCut_L, 300, 10, Constants.DamageType.Crushing);

            skill3.ActivationSoundEffect = SoundEffectManager.Instance.Woosh2;
            skill3.HitSoundEffect = SoundEffectManager.Instance.Jab;
            skill3.Acceleration = 0.25f;
            //skill3.StartVelocity = new Vector2(8, 0);
            skill3.UltimateVelocityX = 0;

            skill3.InflictForce = new Vector2(40, -15);

            skill3.DamagingFrames = new List<int>
            {
                4,
                5,
            };
            skill3.HitBoxPositions[0] = new Vector2(63, -27);
            skill3.HitBoxPositions[1] = new Vector2(63, -38);
            skill3.HitBoxHeight = 25;
            skill3.HitBoxWidth = 25;

            #endregion

            SevenDragons.SkillArray = new List<CharacterSkill>
            {
                skill1,
                skill2,
                skill3,
            };

            SevenDragons.ResetTime = 1200;

            SevenDragons.Name = "Seven Dragons";
            SevenDragons.Description = "A basic 3-strike combo with the final strike causing a medium force knockback";
            SevenDragons.Icon = content.Load<Texture2D>("Sprites/7");

            #endregion

            #region Test Skill
            CharacterAnimation testRight = new CharacterAnimation(content.Load<Texture2D>(Constants.PlayerSpriteSheet), 0, 134, 190, 0, 0, 50, SpriteEffects.None, 0f, 0f, false, false);
            CharacterAnimation testLeft = new CharacterAnimation(content.Load<Texture2D>(Constants.PlayerSpriteSheet), 0, 134, 190, 0, 0, 50, SpriteEffects.None, 0f, 0f, false, false);
            TestSkill = new CharacterSkill(player, testRight, testLeft, 5, 1);
            TestSkill.Icon = content.Load<Texture2D>("Sprites/testSkill");
            #endregion
        }
    }
}
