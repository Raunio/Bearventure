using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bearventure.Gameplay.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Bearventure
{
    public class CharacterAnimation : Animation
    {
        private bool calculateBoundingBoxOffsets = false;
        private Point boundingBoxSize;
        private Constants.DirectionX animationDirection;

        public int BoundingBoxOffset
        {
            get;
            private set;
        }

        public struct AnatomicInfo
        {
            public Vector2 Mouth;
            public Vector2 LeftEye;
            public Vector2 RightEye;
            public Vector2 LeftHand;
            public Vector2 RightHand;
            public Vector2 LeftFoot;
            public Vector2 RightFoot;
            public Vector2 Groin;
            public Vector2 Belly;
        }

        private AnatomicInfo[] AnatInfo
        {
            get;
            set;
        }

        public CharacterAnimation(Texture2D spriteSheet, int spriteSheetRow, int frameWidth, int frameHeight, 
            int startFrame, int endFrame, float speed, SpriteEffects spriteEffects, 
            float layerDepth, float rotation, bool backwards, bool looping) : base(spriteSheet, spriteSheetRow, frameWidth, frameHeight, startFrame, endFrame, 
            speed, spriteEffects, layerDepth, rotation, backwards, looping)
        {
            AnatInfo = new AnatomicInfo[FrameCount];
        }

        public void SetAnatomicInfo(AnatomicInfo info, int frame)
        {
            AnatInfo[frame] = info;
        }

        public void SetAnatomicInfo(List<AnatomicInfo> infoList)
        {
            for (int i = 0; i < infoList.Count; i++)
                AnatInfo[i] = infoList[i];
        }
        /// <summary>
        /// Sets anatomic info from a text file.
        /// </summary>
        /// <param name="path">Path to the text file</param>
        /// <param name="effects"></param>
        public void SetAnatomicInfo(string path, SpriteEffects effects)
        {
            string line = string.Empty;
            AnatInfo = new AnatomicInfo[FrameCount];

            using (StreamReader sr = new StreamReader(path))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    string info = line;
                    string part = info.Split(':')[0];
                    int position = part.Length + 1;
                    string positions = info.Substring(position);
                    string[] positionArray = positions.Split(';');
                    List<Vector2> positionsAsVectors = new List<Vector2>();

                    for (int i = 0; i < positionArray.Length; i++)
                    {
                        string[] pos = positionArray[i].Split('.');
                        if(effects == SpriteEffects.None)
                            positionsAsVectors.Add(new Vector2(Convert.ToInt32(pos[0]) - i * FrameWidth, Convert.ToInt32(pos[1])));
                        else if (effects == SpriteEffects.FlipHorizontally)
                        {
                            Vector2 first = new Vector2(Convert.ToInt32(pos[0]) - i * FrameWidth, Convert.ToInt32(pos[1]));
                            int x = FrameWidth - (int)first.X;
                            positionsAsVectors.Add(new Vector2(x, first.Y));
                        }
                    }

                    for (int i = 0; i < FrameCount; i++)
                    {
                        switch(part)
                        {
                            case "LeftHand":
                                if (effects == SpriteEffects.None)
                                    AnatInfo[i].LeftHand = positionsAsVectors[i];
                                else if (effects == SpriteEffects.FlipHorizontally)
                                    AnatInfo[i].RightHand = positionsAsVectors[i];
                                break;
                            case "LeftEye":
                                    AnatInfo[i].LeftEye = positionsAsVectors[i];
                                break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Returns the anatomic info of a specific frame.
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public AnatomicInfo GetAnatomicInfo(int frame)
        {
            return AnatInfo[frame];
        }
        /// <summary>
        /// Returns the anatomic info of the current frame.
        /// </summary>
        /// <returns></returns>
        public AnatomicInfo GetAnatomicInfo()
        {
            return AnatInfo[CurrentFrame - StartFrame];
        }

        public void CalculateBoundingBoxOffsets(Point boundingBoxSize, Constants.DirectionX animationDirection)
        {
            calculateBoundingBoxOffsets = true;
            this.boundingBoxSize = boundingBoxSize;
            this.animationDirection = animationDirection;
        }

        /// <summary>
        /// Main animation method. Should be called in the update of a character.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Animate(GameTime gameTime)
        {
            switch (backwards)
            {
                case true:
                    AnimateBackward(gameTime);
                    break;
                case false:
                    AnimateForward(gameTime);
                    break;
            }

            if (calculateBoundingBoxOffsets)
                CalcBBOffsets();
        }

        private void CalcBBOffsets()
        {
            if (FrameRectangle.Width != boundingBoxSize.X)
            {
                BoundingBoxOffset = animationDirection == Constants.DirectionX.Left ? (FrameRectangle.Width - boundingBoxSize.X) : -(boundingBoxSize.X - FrameRectangle.Width) / 2;
            }
        }
    }
}
