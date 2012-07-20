using System;

namespace Pocket1945
{
	/// <summary>
	/// This enum defines the different movement patterns
	/// for the enemy planes.
	/// </summary>
	public enum MovePattern
	{
		/// <summary>
		/// The plane should move straight forward.
		/// </summary>
		Straight = 0
	}

	/// <summary>
	/// This enum defines all the different types of enemies 
	/// available in the game.
	/// </summary>
	public enum EnemyType
	{
		/// <summary>
		/// Small dark green fighter plane.
		/// </summary>
		DarkGreenFighter = 0,
		/// <summary>
		/// Small white fighter plane.
		/// </summary>
		WhiteFighter = 1,
		/// <summary>
		/// Small light green fighter plane.
		/// </summary>
		LightGreenFighter = 2,
		/// <summary>
		/// Small blue fighter plane.
		/// </summary>
		BlueFighter = 3,
		/// <summary>
		/// Small yellow fighter plane.
		/// </summary>
		YellowFighter = 4, 
		/// <summary>
		/// Small green bomber plane.
		/// </summary>
		GreenBomber = 5,
		/// <summary>
		/// Medium sized light green boss.
		/// </summary>
		LightGreenBoss = 6,
		/// <summary>
		/// Lagre dark green boss.
		/// </summary>
		DarkGreenBoss = 7,
		/// <summary>
		/// Large green bomber boss.
		/// </summary>
		GreenBomberBoss = 8,
		/// <summary>
		/// Medium sized green boss.
		/// </summary>
		WhiteBoss = 9	
	}

	/// <summary>
	/// Enum definging the different types of bullets the enemey and player can
	/// fire.
	/// </summary>
	public enum BulletType
	{
		/// <summary>
		/// A single shot. The default bullet of a fighter plane.
		/// </summary>
		SingleShot = 0,
		/// <summary>
		/// A big single shot. The default bullet of medium sized bosses.
		/// </summary>
		BigSingleShot = 1,
		/// <summary>
		/// Double shot. The bullet of a upgraded enemey plane.
		/// </summary>
		DoubleShot = 2,		
		/// <summary>
		/// Small fireball, The default bullet of a small bomber.
		/// </summary>
		SmallFireball = 3,
		/// <summary>
		/// Big firewall, the default bullet of a big bomber.
		/// </summary>
		BigFireball = 4
	}

	/// <summary>
	/// Enum defining different types of bonuses the player can collect.
	/// </summary>
	public enum BonusType
	{
		/// <summary>
		/// Upgrading the power of the weapon		
		/// </summary>
		WheaponUpgrade = 0,
		/// <summary>
		/// Upgrading speed.
		/// </summary>
		SpeedUpgrade = 1,
		/// <summary>
		/// Small power upgrade.
		/// </summary>
		SmallPowerUpgrade = 2,
		/// <summary>
		/// Lagre power upgrade.
		/// </summary>
		PowerUpgrade = 3,
		/// <summary>
		/// Upgrade the rank.
		/// </summary>
		RankUpgrade = 4,
		/// <summary>
		/// Extra life.
		/// </summary>
		ExtraLife = 5,
		/// <summary>
		/// Shield upgrade.
		/// </summary>
		ShieldUpgrade = 6
	}

	public enum EnemyStatus
	{
		Moving,
		Dying,
		Dead,
		Idle
	}

	public enum PlayerStatus
	{
		Playing,
		Dying,
		Dead		
	}

	/// <summary>
	/// Enum defining different move directions. Used to determine which way
	/// a bullet is moving, for instance.
	/// </summary>
	public enum MoveDirection
	{
		/// <summary>
		/// Moving left.
		/// </summary>
		Left,
		/// <summary>
		/// Moving right.
		/// </summary>
		Right,
		/// <summary>
		/// Moving up.
		/// </summary>
		Up,
		/// <summary>
		/// Moving down.
		/// </summary>
		Down
	}
}