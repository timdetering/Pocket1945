using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace Pocket1945
{
    /// <summary>
    /// This is the actual game form used to display the game to the user.
    /// The class inherits the <see cref="System.Windows.Forms.Form"/> class.
    /// </summary>
    public partial class GameForm : Form
    {
        //Private game variables.
        private bool playing;
        private Bitmap offScreenBitmap;
        private Graphics offScreenGraphics;
        private Graphics onScreenGraphics;
        private LevelGui levelGui;
        private Input input;
        private string[] levelFiles;
        private int menuTick;
        private int titleTick;
        private GameFont levelFont;
        private GameFont exitFont;

        /// <summary>
        /// Which level the player is on.
        /// </summary>
        public static int LevelCount;

        /// <summary>
        /// A public static instance of the player.
        /// </summary>
        public static Player Player;

        /// <summary>
        /// A public static int counting the number of ticks used
        /// in the game.
        /// </summary>
        public static int TickCount;

        /// <summary>
        /// A public static rectangle determing the size of the 
        /// game area.
        /// </summary>
        public static Rectangle GameArea;

        /// <summary>
        /// A public static randomizer used by all game objects to
        /// generate random numbers.
        /// </summary>
        public static Random Randomizer;

        /// <summary>
        /// Field holding a public static reference to the current level. 
        /// Trough this field other game objects like bullets, players and enemies 
        /// gain access to the level properties.
        /// </summary>
        public static Level CurrentLevel;

        /// <summary>
        /// The framerate of the game. This constant is used to determine speed
        /// for all game object. The speed of the game is measured in pixel per second.
        /// So, if a enemy moves 30 pixel per second the speed variable is PixelPerSecond / FramesPerSecond.
        /// 
        /// Variables like ReloadTime is measured in seconds, so the tick variable is Seconds * FramesPerSecond.
        /// </summary>
        internal const float FramesPerSecond = 30F;

        /// <summary>
        /// The length of one frame (1 second / FramesPerSecond).
        /// </summary>
        internal const float SecondsPerFrame = 1.0F / FramesPerSecond;

        /// <summary>
        /// Instantiates the main game form.
        /// </summary>
        public GameForm()
        {
            this.InitializeComponent();
            TickCount = 0;
            this.Visible = true;
            this.WindowState = FormWindowState.Maximized;
            offScreenBitmap = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);
            offScreenGraphics = Graphics.FromImage(offScreenBitmap);
            offScreenGraphics.Clear(Color.Black);
            GameArea = this.ClientRectangle;

            SpriteList.Instance.LoadSprites();

            Randomizer = new Random(DateTime.Now.Millisecond);
            Player = new Player();
            input = new Input();
            levelGui = new LevelGui();
            levelFiles = GetLevels();
            levelFont = new GameFont(Color.Yellow, string.Empty, new Font(FontFamily.GenericSansSerif, 24, FontStyle.Bold), 60, 130);
            exitFont = new GameFont(Color.Red, string.Empty, new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold), 70, 160);

            titleTick = 120;
            DoGameLoop();
        }

        /// <summary>
        /// The game loop.
        /// </summary>
        private void DoGameLoop()
        {
            playing = true;
            onScreenGraphics = this.CreateGraphics();
            input.RegisterAllHardwareKeys();

            while (playing)
            {
                DoLevel(levelFiles[LevelCount]);
            }

            input.UnregisterAllHardwareKeys();
            onScreenGraphics.Dispose();
            Application.DoEvents();
            Application.Exit();
        }

        /// <summary>
        /// The level loop.
        /// </summary>
        private void DoLevel(string filename)
        {
            CurrentLevel = new Level(GetFullPath(filename));
            StopWatch sw = new StopWatch();
            bool levelCompleted = false;
            bool displayMenu = false;

            while ((playing) && (!levelCompleted))
            {
                // Store the tick at which this frame started
                Int64 startTick = sw.CurrentTick();
                input.Update();

                //Update the rownumber.
                TickCount++;

                //Draw the background map.
                CurrentLevel.BackgroundMap.Draw(offScreenGraphics);

                //Update bullets, enemies and bonuses.
                HandleBonuses();
                HandleBullets();
                HandleEnemies();

                //Update and draw the player.
                Player.Update(input);
                Player.Draw(offScreenGraphics);
                playing = (Player.Status != PlayerStatus.Dead);

                //Draw in-game user interface
                levelGui.Draw(offScreenGraphics);

                //Draw level title.
                if (titleTick > 0)
                {
                    titleTick--;
                    levelFont.Text = CurrentLevel.Name;
                    levelFont.Draw(offScreenGraphics);
                }

                //Display a simple menu.
                if (displayMenu)
                {
                    menuTick--;
                    if (menuTick == 0)
                        displayMenu = false;

                    if (input.KeyJustPressed(193))
                        playing = false;
                    exitFont.Text = "Click again to Exit";
                    exitFont.Draw(offScreenGraphics);
                }

                if (input.KeyJustPressed(193))
                {
                    menuTick = 120;
                    displayMenu = true;
                }

                //Check if the level is compleated.
                if (CurrentLevel.BackgroundMap.Y == 0)
                {
                    LevelCount++;
                    titleTick = 120;
                    if (LevelCount == levelFiles.Length)
                        LevelCount = 0;
                    Player.Power = Player.ShieldThickness;
                    Player.UpdateHealth();
                    levelCompleted = true;
                }

                //Move the offScreenBitmap buffer to the screen.
                onScreenGraphics.DrawImage(offScreenBitmap, 0, 0, this.ClientRectangle, GraphicsUnit.Pixel);

                //Process all events in the event que.
                Application.DoEvents();

                // Lock the framerate...
                Int64 deltaMs = sw.DeltaTime_ms(sw.CurrentTick(), startTick);
                Int64 targetMs = (Int64)(1000.0F * SecondsPerFrame);

                // Check if the frame time was fast enough
                if (deltaMs <= targetMs)
                {
                    // Loop until the frame time is met
                    while (sw.DeltaTime_ms(sw.CurrentTick(), startTick) < targetMs)
                    {
                        Thread.Sleep(0);
                        Application.DoEvents();
                    }
                }
            }
        }

        #region Method handeling bonus elements.
        /// <summary>
        /// Method handeling all the bonus elements in the game.
        /// </summary>
        private void HandleBonuses()
        {
            for (int i = 0; i < CurrentLevel.Bonuses.Length; ++i)
            {
                Bonus b = (Bonus)CurrentLevel.Bonuses[i];
                if (b.HasFocus())
                {
                    b.Move();
                    b.Draw(offScreenGraphics);

                    //Check if the player have collected the bonus.
                    if (b.GetCollitionRectangle().IntersectsWith(Player.GetCollitionRectangle()))
                    {
                        Player.GetBonus(b);
                        b.Collected = true;
                    }
                }
            }
        }
        #endregion

        #region Method handeling bullets.
        /// <summary>
        /// Method handeling all the bullets in the game.
        /// </summary>
        private void HandleBullets()
        {
            //Loop trough all bullets in the game.
            for (int i = 0; i < CurrentLevel.Bullets.Count; ++i)
            {
                Bullet b = (Bullet)CurrentLevel.Bullets[i];

                //Check if the bullet has focus.
                if (b.HasFoucs())
                {
                    b.Move();
                    b.Draw(offScreenGraphics);

                    //If the bullet is mowing upwards it's aimed at enemies.
                    if (b.Direction == MoveDirection.Up)
                    {
                        //Loop trough the enemies and check for a hit.
                        for (int j = 0; j < CurrentLevel.Enemies.Length; ++j)
                        {
                            Enemy e = (Enemy)CurrentLevel.Enemies[j];
                            //Check if we got a hit.
                            if (e.HasFocus() && e.Status != EnemyStatus.Dying && b.GetCollitionRectangle().IntersectsWith(e.GetCollitionRectangle()))
                            {
                                //The bullet hit a enemey.
                                e.Hit(b);

                                //Remove the bullet from the game.
                                CurrentLevel.Bullets.RemoveAt(i);

                                //Collect score.
                                if (e.Status == EnemyStatus.Dying)
                                    Player.Score += e.GiveScore();
                                break;
                            }
                        }
                    }
                    //If the bullet is moving downwards it's aimed at the player.
                    else
                    {
                        //Check if the enemey hits the player.
                        if (b.GetCollitionRectangle().IntersectsWith(Player.GetCollitionRectangle()))
                        {
                            //Hit the player and remove the bullet.
                            Player.Hit(b);
                            CurrentLevel.Bullets.RemoveAt(i);
                        }
                    }
                }
                //The bullet is out of scope remove the bullet to free memory.
                else
                    CurrentLevel.Bullets.RemoveAt(i);
            }
        }
        #endregion

        #region Method handeling enemies.
        /// <summary>
        /// Method handling all the enemies of the game.
        /// </summary>
        public void HandleEnemies()
        {
            //Loop trough all enemies and move, draw and fire.
            for (int i = 0; i < CurrentLevel.Enemies.Length; ++i)
            {
                Enemy e = (Enemy)CurrentLevel.Enemies[i];
                if (e.HasFocus())
                {
                    //Move the enemey.
                    e.Move();

                    //If the enemy can fire, add a new bullet to the game.
                    if (e.CanFire())
                        CurrentLevel.Bullets.Add(e.Fire());

                    //Draw the enemey.
                    e.Draw(offScreenGraphics);

                    //Check if the player collides with a enemy plane.
                    if (e.Status != EnemyStatus.Dying && e.GetCollitionRectangle().IntersectsWith(Player.GetCollitionRectangle()))
                    {
                        //The effect of a collision is the same as if they where hit by a bullet.
                        Player.Hit(e.Fire());
                        e.Hit(Player.Fire());

                        //Give score if the enemy is dying.
                        if (e.Status == EnemyStatus.Dying)
                            Player.Score += e.GiveScore();
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// Method returning a string array with all level files in the game folder.
        /// </summary>
        /// <returns>All level files.</returns>
        public string[] GetLevels()
        {
#if PocketPC || Smartphone
            Assembly asm = Assembly.GetExecutingAssembly();
            string levelPath = Path.GetDirectoryName(asm.GetName().CodeBase);
#else
            string levelPath = Path.Combine(Application.StartupPath, @"Data\Levels");
#endif
            DirectoryInfo dir = new DirectoryInfo(levelPath);
            FileInfo[] files = dir.GetFiles("*.xml");
            string[] fileNames = new string[files.Length];
            for (int i = files.Length; i > 0; --i)
            {
                fileNames[i - 1] = files[files.Length - i].FullName;
            }
            return fileNames;
        }

        /// <summary>
        /// Method used to determine the full path of a file.
        /// </summary>
        /// <param name="fileName">The filename you want the full path for.</param>
        /// <returns>The full paht of the file.</returns>
        private string GetFullPath(string fileName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string fullName = Path.GetDirectoryName(asm.GetName().CodeBase);
            return Path.Combine(fullName, fileName);
        }

        /// <summary>
        /// Left empty, all drawing is done in the game loop.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnPaint(PaintEventArgs e) { }

        /// <summary>
        /// Since all drawing is done in the gameloop the Form does not need 
        /// to draw a background for us. By leaving this method empty we 
        /// avoids undesirable flickering.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnPaintBackground(PaintEventArgs e) { }
    }
}
