using System;
using Microsoft.Xna.Framework;

namespace Bearventure
{
    public class Constants
    {
        #region MenuTexts
        public const String MenuExitPromptText = "Are you sure you want to exit?";
        public const String PauseExitPromptText = "Are you sure you want to end game?";
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
        public static Color Solid = Color.Black;
        public static Color Water = Color.Blue;
        public static Color Lava = Color.Red;
        public static Color Quicksand = Color.Yellow;
        #endregion

        #region Background Textures

        public const String MenuBackgroundImage = "Backgrounds/Menu";
        
        #endregion
        #region Character Textures

        public const String BlackMetalBadger = "Sprites/bmbadger";
        public const String DelayOwl = "Sprites/paskapollo";

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
        };

        #endregion
        #region CombatEnumerations
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
