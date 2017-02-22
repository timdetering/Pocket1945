using System;
using System.IO;
using System.Xml;
using System.Collections;

namespace Pocket1945
{
    /// <summary>
    /// This class represents a level of the game. All level 
    /// data is loaded from a XML file passed into the constructor. 
    /// 
    /// The GameForm keeps a instance to a static Level class at all time.
    /// That way other game objects like enemies etc. can access the level.
    /// 
    /// Data that's used trough out the game is kept in the GameForm class,
    /// while level data is kept in this class.
    /// </summary>
    public class Level
    {
        public int Height;
        public int Speed;
        public string Name;
        public Bonus[] Bonuses;
        public Enemy[] Enemies;
        public string[] Map;
        public BackgroundElement[] BackgroundElements;
        public Background BackgroundMap;
        public ArrayList Bullets;

        /// <summary>
        /// Method instantiating a new level based on a XML level file.
        /// </summary>
        /// <param name="filename">The path to the XML level file.</param>
        public Level(string filename)
        {
            //Loading a XmlDocument based on the file path.
            XmlDocument xml = new XmlDocument();
            xml.Load(filename);

            //Setting the scroll speed of the level. The speed is set by pixels per second.
            Speed = Convert.ToInt32(xml.GetElementsByTagName("Level")[0].Attributes["Speed"].InnerText);
            Name = xml.GetElementsByTagName("Level")[0].Attributes["Name"].InnerText;

            //Loading the map ASCII table.
            char[] seperator = {'\n'};
            string mapString = xml.GetElementsByTagName("Map")[0].InnerText;
            Map = mapString.Split(seperator);

            //Selecting all background elements.
            XmlNodeList bgElements = xml.GetElementsByTagName("Element");
            BackgroundElements = new BackgroundElement[bgElements.Count];

            //Creating all background elements.
            for (int i = 0; i < bgElements.Count; ++i)
            {
                BackgroundElements[i] = new BackgroundElement(
                    Convert.ToInt32(bgElements[i].Attributes["X"].InnerText),
                    Convert.ToInt32(bgElements[i].Attributes["Y"].InnerText),
                    Convert.ToInt32(bgElements[i].Attributes["Height"].InnerText),
                    Convert.ToInt32(bgElements[i].Attributes["Width"].InnerText),
                    Convert.ToInt32(bgElements[i].Attributes["SpriteIndex"].InnerText));
            }

            //Selecting all bonus elements.
            XmlNodeList bonusElements = xml.GetElementsByTagName("Bonus");
            Bonuses = new Bonus[bonusElements.Count];

            //Creating all bonus elements.
            for (int i = 0; i < bonusElements.Count; ++i)
            {
                Bonuses[i] = new Bonus(
                    Convert.ToInt32(bonusElements[i].Attributes["X"].InnerText),
                    Convert.ToInt32(bonusElements[i].Attributes["Y"].InnerText),
                    Convert.ToInt32(bonusElements[i].Attributes["Speed"].InnerText) / GameForm.FramesPerSecond,
                    (BonusType)Convert.ToInt32(bonusElements[i].Attributes["BonusType"].InnerText));
            }

            //Selecting all enemies.
            XmlNodeList enemies = xml.GetElementsByTagName("Enemy");
            Enemies = new Enemy[enemies.Count];

            //Creating all enemies.
            for (int i = 0; i < enemies.Count; ++i)
            {
                Enemies[i] = new Enemy(
                    Convert.ToInt32(enemies[i].Attributes["X"].InnerText),
                    Convert.ToInt32(enemies[i].Attributes["Y"].InnerText),
                    Convert.ToInt32(enemies[i].Attributes["Speed"].InnerText) / GameForm.FramesPerSecond,
                    (MovePattern)Convert.ToInt32(enemies[i].Attributes["MovePattern"].InnerText),
                    (EnemyType)Convert.ToInt32(enemies[i].Attributes["EnemyType"].InnerText),
                    (BulletType)Convert.ToInt32(enemies[i].Attributes["BulletType"].InnerText),
                    Convert.ToInt32(enemies[i].Attributes["BulletPower"].InnerText),
                    (Convert.ToDouble(enemies[i].Attributes["BulletSpeed"].InnerText) / GameForm.FramesPerSecond),
                    (Convert.ToDouble(enemies[i].Attributes["BulletReloadTime"].InnerText) / (double)1000) *
                    GameForm.FramesPerSecond,
                    Convert.ToInt32(enemies[i].Attributes["Power"].InnerText),
                    Convert.ToInt32(enemies[i].Attributes["Score"].InnerText));
            }

            //Creating the background map for this level.
            BackgroundMap = new Background(Map, BackgroundElements);
            BackgroundMap.Speed = Speed / GameForm.FramesPerSecond;

            //Creating a arraylist to hold all the bullets on the level.
            Bullets = new ArrayList();
        }

        /// <summary>
        /// Get's the current progress in %.
        /// </summary>
        /// <returns>% of the game that is finished.</returns>
        public int GetProgress()
        {
            double percent = (double)BackgroundMap.Y / (double)BackgroundMap.Height * (double)100;
            return 100 - (int)percent;
        }
    }
}