using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Pocket1945
{
    /// <summary>
    /// This class defines a game bonus element. A bonus can be 
    /// a extra life, a power up, more speed etc. The type of bonus is defined
    /// by the BonusType enum. The player object have a method, GetBonus(Bonus b), 
    /// that process a bonus element.
    /// </summary>
    public class Bonus : Sprite
    {
        private int spriteIndex;
        private int startPosition;
        private double tempY;
        private double speed;

        public bool Collected;
        public BonusType Type;

        /// <summary>
        /// Instanciates a new bonus object.
        /// </summary>
        /// <param name="x">The x position of the bonus element.</param>
        /// <param name="startPosition">The y start position of the bonus element.</param>
        /// <param name="speed">The speed of the bonus element.</param>
        /// <param name="bonusType">The type of bonus.</param>
        public Bonus(int x, int startPosition, double speed, BonusType bonusType)
        {
            this.x = x;
            this.y = -32;
            this.tempY = this.y;
            this.speed = speed;
            this.startPosition = startPosition;
            this.Type = bonusType;
            this.spriteSize.Height = 32;
            this.spriteSize.Width = 32;
            SetBonusSpesific();
        }

        /// <summary>
        /// Method setting bonus spesific properties like sprite index and sprite size.
        /// </summary>
        private void SetBonusSpesific()
        {
            switch (Type)
            {
                case BonusType.ExtraLife:
                    spriteIndex = 1;
                    collitionRectangle.Width = 23;
                    collitionRectangle.Height = 18;
                    collitionPoint.X = 4;
                    collitionPoint.Y = 6;
                    break;
                case BonusType.SmallPowerUpgrade:
                    spriteIndex = 4;
                    collitionRectangle.Width = 24;
                    collitionRectangle.Height = 25;
                    collitionPoint.X = 4;
                    collitionPoint.Y = 5;
                    break;
                case BonusType.PowerUpgrade:
                    spriteIndex = 5;
                    collitionRectangle.Width = 24;
                    collitionRectangle.Height = 25;
                    collitionPoint.X = 4;
                    collitionPoint.Y = 5;
                    break;
                case BonusType.RankUpgrade:
                    spriteIndex = 2;
                    collitionRectangle.Width = 29;
                    collitionRectangle.Height = 15;
                    collitionPoint.X = 1;
                    collitionPoint.Y = 7;
                    break;
                case BonusType.SpeedUpgrade:
                    spriteIndex = 3;
                    collitionRectangle.Width = 20;
                    collitionRectangle.Height = 28;
                    collitionPoint.X = 7;
                    collitionPoint.Y = 2;
                    break;
                case BonusType.ShieldUpgrade:
                    spriteIndex = 6;
                    collitionRectangle.Width = 20;
                    collitionRectangle.Height = 29;
                    collitionPoint.X = 7;
                    collitionPoint.Y = 1;
                    break;
                case BonusType.WheaponUpgrade:
                default:
                    spriteIndex = 0;
                    collitionRectangle.Width = 20;
                    collitionRectangle.Height = 27;
                    collitionPoint.X = 7;
                    collitionPoint.Y = 3;
                    break;
            }
        }

        /// <summary>
        /// Method returning the sprite index of the bonus.
        /// </summary>
        /// <returns>Returns the sprite index of the bonus sprite.</returns>
        public override int GetSpriteIndex()
        {
            return spriteIndex;
        }

        /// <summary>
        /// Method determing weather the bonus element has focus or not.
        /// </summary>
        /// <returns>True if the bonus is on screen and has focus.</returns>
        public bool HasFocus()
        {
            return (!Collected && startPosition >= GameForm.CurrentLevel.BackgroundMap.Y && GameForm.GameArea.Height > y);
        }

        /// <summary>
        /// Method moving the bonus element.
        /// </summary>
        public void Move()
        {
            tempY += speed;
            y = (int)tempY;
        }

        public override void Draw(Graphics g)
        {
            int index = GetSpriteIndex();
            g.DrawImage(SpriteList.Instance.Bonuses[index], new Rectangle(x, y, spriteSize.Width, spriteSize.Height), 0,
                0, spriteSize.Width, spriteSize.Height, GraphicsUnit.Pixel, imgAttribs);
        }
    }
}