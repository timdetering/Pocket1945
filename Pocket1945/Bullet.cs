using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Pocket1945
{
	/// <summary>
	/// This class represents a bullet in the game. Both player
	/// and enemy planes use this class when a new bullet is fired.
	/// </summary>
	public class Bullet : Sprite
	{				
		private int spriteIndex;		
		private BulletType bulletType;				
		private double speed; 

		/// <summary>
		/// The power of the buller. When the bullet hit a enemey or the 
		/// player the shield is decreased by this property.
		/// </summary>
		public int BulletPower;

		/// <summary>
		/// The direction the bullet is moving. If it's moving up it's aimed at 
		/// one of the enemies. If it's moving down it's aimed at the player.
		/// </summary>
		public MoveDirection Direction;

		/// <summary>
		/// Instanciates a new bullet.
		/// </summary>
		/// <param name="outX">The x position the bullet was fired from.</param>
		/// <param name="outY">The y position the bullet was fired from.</param>
		/// <param name="bulletPower">The power of the bullet.</param>
		/// <param name="bulletSpeed">The speed of the bullet.</param>
		/// <param name="bulletType">The type of the bullet.</param>
		/// <param name="direction">The direction of the bullet.</param>
		public Bullet(int outX, int outY, int bulletPower, double bulletSpeed, BulletType bulletType, MoveDirection direction)
		{
			this.speed = bulletSpeed;
			this.x = outX;			
			this.y = outY;
			this.BulletPower = bulletPower;
			this.bulletType = bulletType;
			this.spriteSize.Width = 32;
			this.spriteSize.Height = 32;
			this.Direction = direction;
			this.collitionRectangle = new Rectangle(x, y, spriteSize.Width, spriteSize.Height);
			SetBulletSpesific();
		}

		/// <summary>
		/// Method seting bullet spesific properties like sprite index and collition rectangle.
		/// </summary>
		private void SetBulletSpesific()
		{
			switch(bulletType)
			{
				case BulletType.SingleShot :
					collitionRectangle.Width = 7;
					collitionRectangle.Height = 16;
					if(Direction == MoveDirection.Down)
					{
						spriteIndex = 0;
						collitionPoint.Y = 7;
						collitionPoint.X = 13;
					}
					else
					{
						spriteIndex = 1;
						collitionPoint.Y = 9;
						collitionPoint.X = 12;												
					}
					break;
				case BulletType.BigSingleShot : 
					collitionRectangle.Width = 9;
					collitionRectangle.Height = 20;
					if(Direction == MoveDirection.Down)
					{
						spriteIndex = 2;
						collitionPoint.Y = 5;
						collitionPoint.X = 12;
					}
					else
					{
						spriteIndex = 3;
						collitionPoint.Y = 7;
						collitionPoint.X = 11;						
					}
					break;
				case BulletType.DoubleShot : 
					collitionRectangle.Width = 17;
					collitionRectangle.Height = 16;
					collitionPoint.Y = 8;
					collitionPoint.X = 7;
					if(Direction == MoveDirection.Down)
						spriteIndex = 4;
					else
						spriteIndex = 5;					
					break;
				case BulletType.SmallFireball : 
					collitionRectangle.Width = 9;
					collitionRectangle.Height = 9;
					collitionPoint.Y = 12;
					collitionPoint.X = 13;
					spriteIndex = 9;
					break;
				case BulletType.BigFireball : 
					collitionRectangle.Width = 13;
					collitionRectangle.Height = 13;
					collitionPoint.Y = 10;
					collitionPoint.X = 10;
					spriteIndex = 8;
					break;
				default :
					collitionRectangle.Width = 7;
					collitionRectangle.Height = 16;
					collitionPoint.Y = 7;
					collitionPoint.X = 13;
					spriteIndex = 8;
					break;
			}
		}

		/// <summary>
		/// Method checking if the bullet has focus or not.
		/// </summary>
		/// <returns>True if the bullet is on the screen and has focus.</returns>
		public bool HasFoucs()
		{
			if(Direction == MoveDirection.Down)
				return(y < GameForm.GameArea.Height);
			else
				return(y > -spriteSize.Height);
		}

		/// <summary>
		/// Method moving the bullet.
		/// </summary>
		public void Move()
		{
			if(Direction == MoveDirection.Down)
				y = y + (int)speed;
			else
				y = y - (int)speed;
		}

		/// <summary>
		/// Method returning the sprite index for this bullet.
		/// </summary>
		/// <returns>The index of the sprite in the SpriteList class.</returns>
		public override int GetSpriteIndex()
		{
			return spriteIndex;
		}

		public override void Draw(Graphics g)
		{
			int index = GetSpriteIndex();
			g.DrawImage(SpriteList.Instance.Bullets[index], new Rectangle(x, y, spriteSize.Width, spriteSize.Height), 0, 0, spriteSize.Width, spriteSize.Height, GraphicsUnit.Pixel, imgAttribs);
		}
	}
}
