using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace SlowCanterToTheRight
{
    // The EnemyManager class acts as a data structure to hold all of the enemies currently spawned in the game
    // so that their methods can be called simultaneously in one place. The manager also spawns new enemies based
    // upon input from the external tool and the Level class
    class EnemyManager
    {
        // Fields
        // Holds a list of all enemies in the game
        private List<Enemy> enemies;
        // Holds data for the current wave and for spawning enemies
        private Level spawnManager;

        // A collection of the enemy spritesheets
        private List<Texture2D> spriteList;
        private Texture2D groundEnemySprite;
        private Texture2D flyingEnemySprite;
        private Texture2D coinsprite;

        // A list of the coins in the game at all times
        private List<Coin> coinlist;

        // Enemy SFX
        private SoundEffectInstance WillS;
        private SoundEffectInstance Fly;
        private SoundEffectInstance flydie;
        private Texture2D flysmoke;

        // A random object for passing to enemies and choosing spawn locations
        private Random rng;

        // Reads the input from the etrenal tool
        StreamReader input;

        // Properties
        public List<Enemy> EnemyList
        {
            get { return enemies; }
        }

        public List<Coin> Coinlist { get { return coinlist; } set { coinlist = value; } }
        public Texture2D CoinSprite
        {
            get { return coinsprite; }
        }

        public Level SpawnManager
        {
            set { spawnManager = value; }
        }

        // Constructor
        public EnemyManager(List<Texture2D> spriteList, Level spawnManager, StreamReader input, Random rng, SoundEffectInstance flysound, SoundEffectInstance Flyingdie, Texture2D Smoke, SoundEffectInstance willS)
        {
            this.rng = rng;

            Fly = flysound;
            WillS = willS;

            flysmoke = Smoke;

            this.spriteList = spriteList;
            groundEnemySprite = spriteList[0];
            flyingEnemySprite = spriteList[1];
            coinsprite = spriteList[2];
            coinlist = new List<Coin>();

            flydie = Flyingdie;

            this.spawnManager = spawnManager;

            enemies = new List<Enemy>();

            this.input = input;
        }

        // Methods
        // Calls the update method for all of the enemies depending on their type
        public void UpdateEnemies(GameTime gameTime, Player player)
        {
            foreach (Enemy e in enemies)
            {
                e.EnemyUpdate(gameTime, player, rng);
            }

            // Removes enemies that have died from the list 
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].EnemyState == EnemyState.dead)
                {
                    // Updates the values for the Level object based upon what has died
                    if (enemies[i] is FlyingEnemy)
                    {
                        spawnManager.FlyingEnemiesDefeated++;
                        spawnManager.FlyingEnemiesCurrent--;
                    }
                    else
                    {
                        spawnManager.GroundEnemiesDefeated++;
                        spawnManager.GroundEnemiesCurrent--;
                    }

                    enemies.RemoveAt(i);
                    i--;
                }
            }

            // Updates the coin physics and animates their spin
            foreach (Coin c in coinlist)
            {
                c.CoinAnimation(gameTime);
                c.Coinfall();
            }
        }

        // Draws all coins and enemies to the screen
        public void DrawEnemies(SpriteBatch sb)
        {
            foreach (Enemy e in enemies)
            {
                e.Draw(sb);
            }
            foreach (Coin c in coinlist)
            {
                c.DrawCoinSpinning(SpriteEffects.None, sb);
            }
        }

        // Spawns ground enemies at either end of the game map randomly
        public void SpawnEnemy()
        {
            if (rng.Next(2) == 0)
            {
                enemies.Add(InitializeEnemy(new Rectangle(0, 800 - groundEnemySprite.Height,
                200, 275)));
            }
            else
            {
                enemies.Add(InitializeEnemy(new Rectangle(3000, 800 - groundEnemySprite.Height,
                200, 275)));
            }
        }

        // Spawns flying enemies at either end of the game map randomly
        public void SpawnFlyingEnemy()
        {
            if (rng.Next(2) == 0)
            {
                enemies.Add(InitializeFlyingEnemy(new Rectangle(0, rng.Next(0, 200),
                350, 240)));
            }
            else
            {
                enemies.Add(InitializeFlyingEnemy(new Rectangle(3200 - flyingEnemySprite.Width, rng.Next(0, 200),
                350, 240)));
            }

        }

        // Utilizes data from the external tool to initialize new ground enemies during spawn
        public Enemy InitializeEnemy(Rectangle loc)
        {
            Enemy enemy = null;

            //Attempts to read in all values from the text file
            try
            {
                //Setting the Streamreader to the appropriate file
                input = new StreamReader("Content/CharacterStats.txt");

                //The array to hold values
                string[] stats = null;

                //The line used for checking the contents of the file
                string line = null;

                //Used to see which line goes with which control list
                int counter = 0;

                //Reading all file lines
                while ((line = input.ReadLine()) != null)
                {
                    //Increments the counter to match the number of each list
                    counter++;

                    //Using a comma to split each line
                    stats = line.Split(',');

                    //The player controls
                    if (counter == 2)
                    {
                        int health = int.Parse(stats[0]);
                        int xSpeed = int.Parse(stats[1]);
                        int ySpeed = int.Parse(stats[2]);
                        int damage = int.Parse(stats[3]);

                        enemy = new Enemy(health, xSpeed, ySpeed, damage,
                        loc, groundEnemySprite, WillS);

                        return enemy;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

            //Saves by closing the file stream
            if (input != null)
            {
                input.Close();
            }

            return enemy;
        }

        // Utilizes data from the external tool to initialize new flying enemies during spawn
        public FlyingEnemy InitializeFlyingEnemy(Rectangle loc)
        {
            FlyingEnemy flyingEnemy = null;

            //Attempts to read in all values from the text file
            try
            {
                //Setting the Streamreader to the appropriate file
                input = new StreamReader("Content/CharacterStats.txt");

                //The array to hold values
                string[] stats = null;

                //The line used for checking the contents of the file
                string line = null;

                //Used to see which line goes with which control list
                int counter = 0;

                //Reading all file lines
                while ((line = input.ReadLine()) != null)
                {
                    //Increments the counter to match the number of each list
                    counter++;

                    //Using a comma to split each line
                    stats = line.Split(',');

                    //The player controls
                    if (counter == 3)
                    {
                        int health = int.Parse(stats[0]);
                        int xSpeed = int.Parse(stats[1]);
                        int ySpeed = int.Parse(stats[2]);
                        int damage = int.Parse(stats[3]);

                        flyingEnemy = new FlyingEnemy(health, xSpeed, ySpeed, damage,
                        loc, flyingEnemySprite, Fly, flydie, flysmoke);

                        return flyingEnemy;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

            //Saves by closing the file stream
            if (input != null)
            {
                input.Close();
            }

            return flyingEnemy;
        }
    }
}
