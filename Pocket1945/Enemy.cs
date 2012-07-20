using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Pocket1945
{
	/// <summary>
	/// This class represents a enemey in the game.
	/// </summary>
	public class Enemy : Sprite, IArmed, ICollidable
	{								
		private int score;
		private int bulletPower;		
		private int startPosition;
		private int power;				
		private int bulletTick;		
		private int[] spriteIndexes;
		private int[] explotionSpriteIndexes;
		private double tempY;
		private double speed;		
		private double spriteTick;
		private double bulletSpeed;
		private double bulletReloadTime;		
		private EnemyType enemyType;
		private BulletType bulletType;
		private MovePattern movePattern;		

		/// <summary>
		/// Field defining the status of the enemey.
		/// </summary>
		public EnemyStatus Status;		

		/// <summary>
		/// This method instanciates a new enemey.
		/// </summary>
		/// <param name="x">The x start positon.</param>
		/// <param name="startPosition">The start position on the level map.</param>
		/// <param name="speed">The speed of the enemey</param>
		/// <param name="movePattern">Which move pattern the enemy uses.</param>
		/// <param name="enemyType">Which type of enemey this is.</param>
		/// <param name="bulletType">Which type of bullets the enemy fires.</param>
		/// <param name="bulletPower">How powerfull the enemy bullets are.</param>
		/// <param name="bulletSpeed">How fast the enemey bullets are.</param>
		/// <param name="bulletReloadTime">How long it takes for the plane to reload.</param>
		/// <param name="shieldThicknes">How thick the shiled of the enemy is.</param>
		/// <param name="score">The score the player recives by killing this enemey.</param>
		public Enemy(int x, int startPosition, double speed, MovePattern movePattern, EnemyType enemyType, BulletType bulletType, int bulletPower, double bulletSpeed, double bulletReloadTime, int power, int score)
		{							
			this.x = x;
			this.y = -32;		
			this.tempY = this.y;
			this.speed = speed;
			this.score = score;
			this.startPosition = startPosition;
			this.movePattern = movePattern;
			this.enemyType = enemyType;
			this.bulletType = bulletType;
			this.bulletPower = bulletPower;
			this.bulletSpeed = bulletSpeed;
			this.bulletReloadTime = bulletReloadTime;		
			this.bulletTick = (int)(bulletReloadTime - (bulletReloadTime / 4)); //Making the plane able to fire the first bullet after one fourth of the bullet reload time.
			this.power = power;						
			this.spriteTick = 0;
			this.collitionRectangle = new Rectangle(x, y, spriteSize.Width, spriteSize.Height);
			this.Status = EnemyStatus.Idle;
			SetEnemySpesific();
		}

		/// <summary>
		/// This method sets enemey spesific properties like size, 
		/// sprite indexes, explotionindexes and collition rectangle.
		/// </summary>
		private void SetEnemySpesific()
		{
			spriteSize.Width = 32;
			spriteSize.Height = 32;
			collitionRectangle = new Rectangle(x, y, spriteSize.Width, spriteSize.Height);
			explotionSpriteIndexes = new int[6] {0, 1, 2, 3, 4, 5};
			switch(enemyType)
			{
				case EnemyType.GreenBomber : 
					spriteIndexes = new int[1] {3};
					break;
				case EnemyType.DarkGreenFighter : 
					spriteIndexes = new int[3] {4, 5, 6};
					break;
				case EnemyType.WhiteFighter : 
					spriteIndexes = new int[3] {7, 8, 9};
					break;
				case EnemyType.LightGreenFighter : 
					spriteIndexes = new int[3] {10, 11, 12};
					break;
				case EnemyType.BlueFighter :
					spriteIndexes = new int[3] {13, 14, 15};
					break;
				case EnemyType.YellowFighter : 
					spriteIndexes = new int[3] {16, 17, 18};
					break;
				default : 
					break;
			}			
		}
		
		/// <summary>
		/// Method returning the index of the enemy sprite.
		/// </summary>
		/// <returns>The index of the enemey sprite.</returns>
		public override int GetSpriteIndex()
		{
			if(spriteIndexes.Length > 1)
			{
				spriteTick++;
				if(spriteTick == spriteIndexes.Length)
					spriteTick = 0;
				return spriteIndexes[(int)spriteTick];
			}
			else
				return spriteIndexes[0];
		}

		/// <summary>
		/// Method checking if the enemey is on screen and has focus.
		/// </summary>
		/// <returns>True if the enemey is on screen, alive, armed and dangerous.</returns>
		public bool HasFocus()
		{
			return(Status != EnemyStatus.Dead && startPosition >= GameForm.CurrentLevel.BackgroundMap.Y && GameForm.GameArea.Height > y);
		}

		/// <summary>
		/// Method moving the enemey.
		/// </summary>
		public void Move()
		{
			if(Status == EnemyStatus.Idle)
				Status = EnemyStatus.Moving;
			switch(movePattern)
			{
				case MovePattern.Straight : 
				default : 
					tempY += speed;
					y = (int)tempY;
					break;
			}
		}

		/// <summary>
		/// Method checking if the enemy can fire. Each enemey have a given reload time.
		/// </summary>
		/// <returns>True if the enemy can fire a bullet.</returns>
		public bool CanFire()
		{		
			if(Status == EnemyStatus.Moving)
			{
				bulletTick++;
				if(bulletTick >= (int)bulletReloadTime)
				{
					bulletTick = 0;
					return true;
				}
				else
					return false;
			}
			else
				return false;
		}	

		/// <summary>
		/// Method firing a bullet.
		/// </summary>
		/// <returns>The bullet fired by the enemey.</returns>
		public Bullet Fire()
		{
			return new Bullet(x, y + (spriteSize.Height / 2), bulletPower, bulletSpeed, bulletType, MoveDirection.Down);
		}

		/// <summary>
		/// Method executed when the enemey is hit by a player bullet.
		/// </summary>
		/// <param name="bullet">The bullet hitting the plane.</param>
		public void Hit(Bullet b)
		{
			if(Status == EnemyStatus.Moving)
			{
				power -= b.BulletPower;
				if(power <= 0)			
				{
					Status = EnemyStatus.Dying;			
					spriteTick = 0;
				}
			}
		}

		/// <summary>
		/// Method giving the player a score for killing this enemey.
		/// </summary>
		/// <returns>The score awarded to the player for killing this enemey.</returns>
		public int GiveScore()
		{			
			return score;
		}

		/// <summary>
		/// Overriding the default draw method of the sprite. 
		/// </summary>
		/// <param name="g">The graphics object used to draw the sprite.</param>
		public override void Draw(Graphics g)
		{
			//If the player is moving, use the default draw method.
			if(Status == EnemyStatus.Moving)
			{
				int index = GetSpriteIndex();
				if(spriteSize.Width > 32)
					g.DrawImage(SpriteList.Instance.BigPlanes[index], new Rectangle(x, y, spriteSize.Width, spriteSize.Height), 0, 0, spriteSize.Width, spriteSize.Height, GraphicsUnit.Pixel, imgAttribs);
				else				
					g.DrawImage(SpriteList.Instance.SmallPlanes[index], new Rectangle(x, y, spriteSize.Width, spriteSize.Height), 0, 0, spriteSize.Width, spriteSize.Height, GraphicsUnit.Pixel, imgAttribs);
			}
			//If the player is dying, draw an explotion.
			else if(Status == EnemyStatus.Dying)
				DrawExplotion(g);
		}

		/// <summary>
		/// Method drawing an exploding plane.
		/// </summary>
		/// <param name="g">The graphics object used to draw the explotion.</param>
		private void DrawExplotion(Graphics g)
		{			
			if(spriteSize.Width > 32)
				g.DrawImage(SpriteList.Instance.BigExplotion[explotionSpriteIndexes[(int)spriteTick]], new Rectangle(x, y, spriteSize.Width, spriteSize.Height), 0, 0, spriteSize.Width, spriteSize.Height, GraphicsUnit.Pixel, imgAttribs);
			else
				g.DrawImage(SpriteList.Instance.SmallExplotion[explotionSpriteIndexes[(int)spriteTick]], new Rectangle(x, y, spriteSize.Width, spriteSize.Height), 0, 0, spriteSize.Width, spriteSize.Height, GraphicsUnit.Pixel, imgAttribs);
			spriteTick += 0.5;
			if(spriteTick == explotionSpriteIndexes.Length)
				Status = EnemyStatus.Dead;
		}		
	}
}