using System;
using System.Drawing;
using System.Reflection;

namespace Pocket1945
{
	/// <summary>
	/// To ensure that there is only one instance of the SpriteList class
	/// it implements the Singleton-pattern. This class is used to access 
	/// the sprites used in the game.
	/// </summary>
	public class SpriteList
	{
		/// <summary>
		/// Private variable indicating if the sprites are loaded or not.
		/// </summary>
		private bool doneLoading;

		#region Public bitmap arrays.
		/// <summary>
		/// Public field giving access to an array of sprites used for small planes.
		/// </summary>
		public Bitmap[] SmallPlanes;

		/// <summary>
		/// Public field giving access to an array of sprites used for a big planes.
		/// </summary>
		public Bitmap[] BigPlanes;

		/// <summary>
		/// Public field giving access to an array of sprites used for bullets.
		/// </summary>
		public Bitmap[] Bullets;

		/// <summary>
		/// Public field giving access to an array of sprites used for bonus items.
		/// </summary>
		public Bitmap[] Bonuses;

		/// <summary>
		/// Public field giving access to an array of background tiles.
		/// </summary>
		public Bitmap[] Tiles;

		/// <summary>
		/// Public field giving access to an array of big background elements.
		/// </summary>
		public Bitmap[] BigBackgroundElements;

		/// <summary>
		/// Public field giving access to an array of small background elements.
		/// </summary>
		public Bitmap[] SmallBackgroundElements;

		/// <summary>
		/// Public field giving access to an array of sprites used for a small explotion.
		/// </summary>
		public Bitmap[] SmallExplotion; 

		/// <summary>
		/// Public field giving access to an array of sprites used for a big explotion.
		/// </summary>
		public Bitmap[] BigExplotion;
		#endregion

		/// <summary>
		/// Public field giving access the the instance of the SpriteList class.
		/// </summary>
		public static readonly SpriteList Instance = new SpriteList();

		/// <summary>
		/// Metod loading the sprites from the assembly resource files
		/// into the public bitmap array. To be sure the sprites are only loaded
		/// once a private bool is set to true/false indicating if the sprites
		/// have been loaded or not.
		/// </summary>
		public void LoadSprites()
		{
			if(!doneLoading)
			{				
				//Accessing the executing assembly to read embeded resources.
				Assembly asm = Assembly.GetExecutingAssembly();
				
				//Reads the sprite strip containing the sprites you want to "parse".
				Bitmap tiles = new Bitmap(asm.GetManifestResourceStream("Pocket1945.Data.Sprites.Tiles.bmp"));
				Bitmap bonuses = new Bitmap(asm.GetManifestResourceStream("Pocket1945.Data.Sprites.Bonuses.bmp"));
				Bitmap bullets = new Bitmap(asm.GetManifestResourceStream("Pocket1945.Data.Sprites.Bullets.bmp"));
				Bitmap smallPlanes = new Bitmap(asm.GetManifestResourceStream("Pocket1945.Data.Sprites.SmallPlanes.bmp"));
				Bitmap smallExplotion = new Bitmap(asm.GetManifestResourceStream("Pocket1945.Data.Sprites.SmallExplotion.bmp"));
				Bitmap bigBackgroundElements = new Bitmap(asm.GetManifestResourceStream("Pocket1945.Data.Sprites.BigBackgroundElements.bmp"));
				Bitmap bigExplotion = new Bitmap(asm.GetManifestResourceStream("Pocket1945.Data.Sprites.BigExplotion.bmp"));
				Bitmap bigPlanes = new Bitmap(asm.GetManifestResourceStream("Pocket1945.Data.Sprites.BigPlanes.bmp"));

				//Parse the sprite strips into bitmap arrays.
				Tiles = ParseSpriteStrip(tiles);
				Bullets = ParseSpriteStrip(bullets);
				Bonuses = ParseSpriteStrip(bonuses);
				SmallPlanes = ParseSpriteStrip(smallPlanes);
				SmallExplotion = ParseSpriteStrip(smallExplotion);
				BigBackgroundElements = ParseSpriteStrip(bigBackgroundElements);
				BigExplotion = ParseSpriteStrip(bigExplotion);
				BigPlanes = ParseSpriteStrip(bigPlanes);

				//Clean up.
				tiles.Dispose();
				bullets.Dispose();
				bonuses.Dispose();
				smallPlanes.Dispose();
				smallExplotion.Dispose();
				bigBackgroundElements.Dispose();
				bigExplotion.Dispose();
				bigPlanes.Dispose();

				doneLoading = true;
			}
		}

		/// <summary>
		/// Method parsing a sprite strip into a bitmap array.
		/// </summary>
		/// <param name="destinationArray">The destination array for the sprites.</param>		
		/// <param name="spriteStrip">The sprite strip to read the sprites from.</param>
		private Bitmap[] ParseSpriteStrip(Bitmap spriteStrip)
		{							
			Rectangle spriteRectangle = new Rectangle(1, 1, spriteStrip.Height - 2, spriteStrip.Height - 2);
			Bitmap[] destinationArray = new Bitmap[(spriteStrip.Width - 1) / (spriteStrip.Height - 1)];

			//Loop drawing the sprites into the bitmap array. 			
			for(int i = 0; i < destinationArray.Length; ++i)
			{
				destinationArray[i] = new Bitmap(spriteRectangle.Width, spriteRectangle.Height);
				Graphics g = Graphics.FromImage(destinationArray[i]);
				spriteRectangle.X = i * (spriteRectangle.Width + 2) - (i - 1);				
				g.DrawImage(spriteStrip, 0, 0, spriteRectangle, GraphicsUnit.Pixel);				
				g.Dispose();
			}

			return destinationArray;
		}

		/// <summary>
		/// Method returning the tile index for a given ascii char.
		/// </summary>
		/// <param name="tileChar">The ASCII char you want a tile index for.</param>
		/// <returns>The tile index for the ASCII char.</returns>
		public int GetTileIndex(char tileChar)
		{
			switch(tileChar)
			{
				case 'A' :
					return 0;					
				case 'B' : 
					return 1;					
				case 'C' : 
					return 0;									
				case 'D' :
					return 1;					
				default : 
					return 0;					
			}
		}

		/// <summary>
		/// The constructor is made private to ensure that 
		/// the class cannot be explicitly created
		/// </summary>
		private SpriteList() 
		{			
		}
	}
}