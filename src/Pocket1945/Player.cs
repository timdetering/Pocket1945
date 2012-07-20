using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Pocket1945
{
	/// <summary>
	/// This class defines the player of the game.
	/// </summary>
	public class Player : Sprite, IArmed, ICollidable
	{						
		private int bulletTick;				
		private int desiredSpeedX;
		private int desiredSpeedY;
		private int pixlesPrSecond;
		private float speedX;
		private float speedY;		
		private double maxSpeed;
		private double spriteIndex;

		private bool movingLeft;		
		private bool movingUp;

		private BulletType bulletType;
		private int bulletPower;
		private double bulletSpeed;


		/// <summary>
		/// The score the player have achived trough the game.
		/// </summary>
		public int Score;

		/// <summary>
		/// The number of lives the player has left.
		/// </summary>
		public int Lives;

		/// <summary>
		/// The power (life of the player). When the player is hit 
		/// the power decreases. When it's 0 the player is dead.
		/// </summary>
		public int Power;

		/// <summary>
		/// The health of the player messured in 0 - 100%.
		/// </summary>
		public int Health;

		/// <summary>
		/// The maximum power the player can have. 
		/// </summary>
		public int ShieldThickness;

		/// <summary>
		/// The status of the player.
		/// </summary>
		public PlayerStatus Status;

		/// <summary>
		/// Instanciates a new player.
		/// </summary>
		public Player()
		{			
			y = 200;		
			x = (GameForm.GameArea.Width / 2) - (spriteSize.Width / 2);
			speedX = 0;
			speedY = 0;
			pixlesPrSecond = 160;
			maxSpeed = pixlesPrSecond / GameForm.FramesPerSecond;
			desiredSpeedX = 0;
			desiredSpeedY = 0;			
			spriteSize.Width = 65;
			spriteSize.Height = 65;
			spriteIndex = 0;
			bulletTick = (int)((200 / 1000) * GameForm.FramesPerSecond);			
			ShieldThickness = 100;
			Power = ShieldThickness;
			UpdateHealth();
			collitionRectangle = new Rectangle(x, y, 59, 42);
			collitionPoint.X = 3;
			collitionPoint.Y = 13;
			bulletType = BulletType.SingleShot;
			bulletPower = 10;
			bulletSpeed = 300 / GameForm.FramesPerSecond;
			Status = PlayerStatus.Playing;
			Score = 0;
			Lives = 3;
		}

		/// <summary>
		/// Method returning the sprite index of the player sprites.
		/// </summary>
		/// <returns>The sprite index for the player sprite</returns>
		public override int GetSpriteIndex()
		{			
			spriteIndex++;
			if(spriteIndex == 3)
				spriteIndex = 0;			
			return (int)spriteIndex;
		}

		/// <summary>
		/// Method determing if the player can fire a bullet or not.
		/// </summary>
		/// <returns>Returning true if the player can fire a new bullet.</returns>
		public bool CanFire()
		{
			return(Status == PlayerStatus.Playing && bulletTick == 0);
		}

		/// <summary>
		/// Method fiering a new bullet.
		/// </summary>
		/// <returns>The bullet fired by the player.</returns>
		public Bullet Fire()
		{						
			return new Bullet(x + 17, y, bulletPower, bulletSpeed, bulletType, MoveDirection.Up);
		}

		/// <summary>
		/// Method updating the player based on user input.
		/// </summary>
		/// <param name="input">Object holding the state of all hardware keys.</param>
		public void Update(Input input)
		{		
			if(bulletTick > 0)
				bulletTick--;

			if(speedX < desiredSpeedX)
				speedX += (desiredSpeedX - speedX) / 4;
			else if(speedX > desiredSpeedX && speedX > 1)
				speedX += (desiredSpeedX - speedX) / 4;

			if(speedY < desiredSpeedY)
				speedY += (desiredSpeedY - speedY) / 4;
			else if(speedY > desiredSpeedY && speedY > 1)
				speedY += (desiredSpeedY - speedY) / 4;			

			if(input.KeyPressed((int)Keys.Up))
			{
				desiredSpeedY = (int)maxSpeed;			
				movingUp = true;
			}
			else if(input.KeyJustReleased((int)Keys.Up))
			{
				desiredSpeedY = 0;			
				movingUp = true;
			}

			if(input.KeyPressed((int)Keys.Down))
			{
				desiredSpeedY = (int)maxSpeed;
				movingUp = false;
			}
			else if(input.KeyJustReleased((int)Keys.Down))
			{
				desiredSpeedY = 0;
				movingUp = false;
			}

			if(input.KeyPressed((int)Keys.Left))
			{
				desiredSpeedX = (int)maxSpeed;
				movingLeft = true;
			}
			else if(input.KeyJustReleased((int)Keys.Left))
			{
				desiredSpeedX = 0;
				movingLeft = true;
			}
			
			if(input.KeyPressed((int)Keys.Right))
			{
				desiredSpeedX = (int)maxSpeed;
				movingLeft = false;
			}
			else if(input.KeyJustReleased((int)Keys.Right))
			{
				desiredSpeedX = 0;
				movingLeft = false;
			}

			if( 
				input.KeyJustPressed(194) ||
				input.KeyPressed(194)
				)
			{				
				if(CanFire())
				{
					GameForm.CurrentLevel.Bullets.Add(Fire());
					bulletTick = (int)(0.25 * GameForm.FramesPerSecond);
				}
			}
					
			if(movingUp)
			{
				if(y > 0)
					y = y - (int)speedY;
			}
			else
			{
				if(GameForm.GameArea.Height > y + (spriteSize.Height - 6))
					y = y + (int)speedY;
			}

			if(movingLeft)
			{
				if(x > -(spriteSize.Width / 2))
					x = x - (int)speedX;
			}
			else
			{
				if(GameForm.GameArea.Width > x + (spriteSize.Width / 2))
					x = x + (int)speedX;	
			}
		}				

		/// <summary>
		/// Method executed when the player is hit by a enemy bullet.
		/// </summary>
		/// <param name="b">The bullet hitting the player.</param>
		public void Hit(Bullet b)
		{		
			if(Status == PlayerStatus.Playing)
			{
				Power -= b.BulletPower;
				UpdateHealth();			
				if(Power <= 0)
				{			
					spriteIndex = 0;
					Status = PlayerStatus.Dying;
				}
			}
		}

		/// <summary>
		/// Method executed when the player hits a bonus.
		/// </summary>
		/// <param name="bonus">The bonus the player is awared</param>
		public void GetBonus(Bonus bonus)
		{
			switch(bonus.Type)
			{
				case BonusType.ExtraLife : 
					Lives++;
					Score += 800;
					break;
				case BonusType.PowerUpgrade : 					
					Power += 50;
					if(Power > ShieldThickness)
						Power = ShieldThickness;					
					Score += 250;
					UpdateHealth();
					break;
				case BonusType.SmallPowerUpgrade : 
					Power += 25;
					if(Power > ShieldThickness)
						Power = ShieldThickness;					
					Score += 150;				
					UpdateHealth();
					break;
				case BonusType.RankUpgrade : 
					Score += 800;
					break;
				case BonusType.SpeedUpgrade : 		
					pixlesPrSecond += 20;
					maxSpeed = pixlesPrSecond / GameForm.FramesPerSecond;
					Score += 150;
					break;
				case BonusType.WheaponUpgrade :
					bulletPower += 5;
					bulletSpeed += 1;
					Score += 150;
					break;
				case BonusType.ShieldUpgrade : 
					ShieldThickness += 50;
					Score += 150;
					Power += 50;
					if(Power > ShieldThickness)
						Power = ShieldThickness;
					UpdateHealth();
					break;
				default : 
					break;
			}
		}

		/// <summary>
		/// Method updating the players helth after beeing hit by a 
		/// bulllet, getting shield or power upgrades. The health is 
		/// messured in %.
		/// </summary>
		public void UpdateHealth()
		{
			double percent = (double)Power / (double)ShieldThickness * (double)100;
			Health = (int)percent;
		}

		/// <summary>
		/// Method drawing the player to the screen.
		/// </summary>
		/// <param name="g">Graphics object used to draw the player.</param>
		public override void Draw(Graphics g)
		{
			//If everything is okey, use base implementation.
			if(Status == PlayerStatus.Playing)
			{
				int index = GetSpriteIndex();
				g.DrawImage(SpriteList.Instance.BigPlanes[index], new Rectangle(x, y, spriteSize.Width, spriteSize.Height), 0, 0, spriteSize.Width, spriteSize.Height, GraphicsUnit.Pixel, imgAttribs);
			}
			//If player is dying, draw an explotion.
			else if(Status == PlayerStatus.Dying)
				DrawExplotion(g);
		}		

		/// <summary>
		/// Method drawing an explotion when the player is dying.
		/// </summary>
		/// <param name="g">The graphics object used to draw the player.</param>
		private void DrawExplotion(Graphics g)
		{
			g.DrawImage(SpriteList.Instance.BigExplotion[(int)spriteIndex], new Rectangle(x, y, spriteSize.Width, spriteSize.Height), 0, 0, spriteSize.Width, spriteSize.Height, GraphicsUnit.Pixel, imgAttribs);
			spriteIndex += 0.5;
			if(spriteIndex == 7)
			{
				Lives--;
				if(Lives == 0)
					Status = PlayerStatus.Dead;
				else
				{										
					Status = PlayerStatus.Playing;
					Power = 100;
					UpdateHealth();
					spriteIndex = 0;
				}
			}
		}
	}
}