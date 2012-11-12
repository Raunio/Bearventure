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
        #endregion
        #region Music
        public const String Level1Music = "Music/Level1";
        public const String MenuMusic = "Music/Splash";
        #endregion
        #region Fonts
        public const String GameFont = "Fonts/GameFont";
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
        };
        public enum Direction
        {
            Left,
            Right
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
            VelocityLowerThan,
            VelocityEqualTo,
            VelocityHigherThan,
            VelocityOtherThan,
            DistanceToPlayerLowerThan,
            DistanceToPlayerEqualTo,
            DistanceToPlayerGreaterThan,
            DistanceToPlayerOtherThan,
        };

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
