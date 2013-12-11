using System;
using Microsoft.Xna.Framework;
using Bearventure.Gameplay.Characters;

namespace Bearventure
{
    public class Constants
    {
        #region MenuTexts
        public const String MenuExitPromptText = "Are you sure you want to exit?";
        public const String PauseExitPromptText = "Are you sure you want to end game?";
        public const String RestartPromptText = "Are you sure you want to restart?";
        #endregion
        #region SoundEffects
        public const String MenuSelectedIndexChange = "SoundEffects/IndexChange";
        public const String MenuIndexSelected = "SoundEffects/IndexSelect";
        public const String BadgerAttack = "SoundEffects/testiOh";
        public const String BadgerSkill = "SoundEffects/bmbadger";
        public const String KarhuHit1 = "SoundEffects/hit1";
        public const String KarhuHit2 = "SoundEffects/hit2";
        public const String KarhuHit3 = "SoundEffects/hit3";
        public const String Splat = "SoundEffects/splat";
        public const String Scratch = "SoundEffects/scratch1";
        public const String Crush = "SoundEffects/crush1";
        public const String Step = "SoundEffects/step";
        public const String karhuJump = "SoundEffects/hyppy";
        public const String quietStep = "SoundEffects/quietStep";
        public const String matoTest = "SoundEffects/matotest";
        public const String puukkoSound = "SoundEffects/puukotus";
        #endregion
        #region Music
        public const String Level1Music = "Music/Level1";
        public const String MenuMusic = "Music/Splash";
        #endregion
        #region Fonts
        public const String GameFont = "Fonts/GameFont";
        public const String HudFont = "Fonts/HudFont";
        #endregion
        #region VisualEffects
        public const String DustEffect = "VisualEffects/Dust";
        public const String BloodSplat1 = "VisualEffects/blood";
        public const String TestEffect = "VisualEffects/testi123";
        #endregion

        #region Terrain Colors
        public static Color Solid = Color.Black; // 0,0,0
        public static Color Water = Color.Blue; // 0,0,255
        public static Color Lava = Color.Red; // 255,0,0
        public static Color Quicksand = Color.Yellow; // 255,255,0
        #endregion

        #region Background Textures

        public const String MenuBackgroundImage = "Backgrounds/Menu";
        
        #endregion
        #region Character Textures

        public const String BlackMetalBadger = "Sprites/badger";
        public const String DelayOwl = "Sprites/paskapollofix";
        public const String OscillatorWorm = "Sprites/mato";

        #endregion

        #region GameObjects

        public const String BasicPlatform = "Sprites/hover";
        public const String WoodenLadder = "Sprites/WoodenLadder";

        #endregion

        #region Character Enumerations

        public enum CharacterState
        {
            Walking,
            Running,
            Attacking,
            Stopped,
            Jumping,
            Falling,
            Disabled,
            UsingSkill,
            Dead,
            Knocked,
            Stunned,
            Climbing,
            LatchedToObject,
            DoubleJump,
        };
        public enum DirectionX
        {
            Left,
            Right
        };
        public enum DirectionY
        {
            Up,
            Hold,
            Down,
        };
        public enum CharacterOrientation
        {
            Ground,
            Air,
        };

        #endregion
        #region Enemy Enumerations

        public enum EnemyType
        {
            BlackMetalBadger,
            DelayOwl,
            WahCat,
            OscillatorWorm,
        };

        #endregion
        #region A.I. Enumerations

        public enum CombatBehaviour
        {
            Default,
            AttackAndFlee,
        };
        public enum BehaviourType
        {
            Passive,
            FreePatrol,
            FixedPatrol,
        };
        public enum ActionType
        {
            Default,
            Chase,
            Attack,
            Flee,
            Stop,
            Jump,
            Latch,
            CancelSkill,
        };
        public enum ConditionType
        {
            HealthLowerThan,
            HealthEqualTo,
            HealthHigherThan,
            HealthOtherThan,
            PlayerState,
            SubjectState,
            VelocityLowerThan,
            VelocityEqualTo,
            VelocityHigherThan,
            VelocityOtherThan,
            DistanceToPlayerLowerThan,
            DistanceToPlayerEqualTo,
            DistanceToPlayerGreaterThan,
            DistanceToPlayerOtherThan,
            AttackReady,
            Blocked,
            FacingPlayer,
            CollidesWithPlayer,
        };

        #endregion
        #region Combat Enumerations
        public enum DamageType
        {
            Slashing,
            Piercing,
            Crushing,
            Fire,
            Cold,
            Energy,
            NegativeEnergy,
            Force,
        };

        public enum SkillTarget
        {
            Enemy,
            Self,
            Both,
        };

        public enum ArmorType
        {
            Skin,
            Fur,
            Feathers,
            Leather,
            Metal,
            Rock,
            Energy,
            NegativeEnergy,
            Force,
        }

        public enum DebuffType
        {
            Stun,
            Blind,
            DoT,
            Slow,
        }
        #endregion
        #region GameObject Enumerations

        public enum PlatformType
        {
            Basic,
        }

        public enum LadderType
        {
            Wooden,
        }

        public enum PlatformState
        {
            Stopped,
            Moving,
        }

        #endregion

        #region Resolution
        public enum ScreenMode
        {
            v1080p,  // 1920x1080
            v720p,   // 1280x720
            v480p60, // 720x480
            v360p,   // 640x360
            v240p    // 320x240
        }
        #endregion
    }
}
