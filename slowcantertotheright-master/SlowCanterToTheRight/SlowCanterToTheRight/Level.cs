using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace SlowCanterToTheRight
{
    class Level
    {
        // Fields
        private int wave;
        private bool endOfRound;

        private int groundEnemyIncrement;
        private int flyingEnemyIncrement;

        private int groundEnemiesCurrent;
        private int flyingEnemiesCurrent;
        private int shooterEnemiesCurrent;
        private int groundEnemiesDefeated;
        private int flyingEnemiesDefeated;

        private double groundSpawnTimer;
        private double groundSpawnFrames;

        private double flyingSpawnTimer;
        private double flyingSpawnFrames;

        private EnemyManager spawnManager;

        //Boss Fields
        private StreamReader input;
        private Boss finalBoss;
        private int windowWidth;
        private int windowHeight;
        private Texture2D boss;
        private Random rng;
        private Texture2D moneyBomb;
        private ParticleManager particleManager;
        private Player nacho;
        private GameTime gameTime;
        private SpriteBatch sb;

        private bool activeBoss;
        private bool bossDefeated;

        // Properties
        public bool EndOfRound
        {
            get { return endOfRound; }
            set { endOfRound = value; }
        }

        public int WaveTotal
        {
            get { return FlyingEnemyTotal + GroundEnemyTotal; }
        }

        public int TotalDefeated
        {
            get { return groundEnemiesDefeated + flyingEnemiesDefeated; }
        }

        public int Wave
        {
            get { return wave; }
            set { wave = value; }
        }

        public int GroundEnemyTotal
        {
            get { return wave * groundEnemyIncrement; }
        }

        public int FlyingEnemyTotal
        {
            get { return wave * flyingEnemyIncrement; }
        }

        public int GroundEnemiesCurrent
        {
            get { return groundEnemiesCurrent; }
            set { groundEnemiesCurrent = value; }
        }

        public int FlyingEnemiesCurrent
        {
            get { return flyingEnemiesCurrent; }
            set { flyingEnemiesCurrent = value; }
        }

        public int ShooterEnemiesCurrent
        {
            get { return shooterEnemiesCurrent; }
            set { shooterEnemiesCurrent = value; }
        }

        public int GroundEnemiesDefeated
        {
            get { return groundEnemiesDefeated; }
            set { groundEnemiesDefeated = value; }
        }

        public int FlyingEnemiesDefeated
        {
            get { return flyingEnemiesDefeated; }
            set { flyingEnemiesDefeated = value; }
        }

        public bool BossDefeated
        {
            get { return bossDefeated; }
        }

        // Constructor
        public Level(EnemyManager spawnManager, StreamReader input, int windowWidth, int windowHeight, Texture2D boss, 
            Random rng, Texture2D moneyBomb, ParticleManager particleManager, Player nacho, GameTime gameTime, SpriteBatch sb)
        {
            this.spawnManager = spawnManager;

            endOfRound = false;
            wave = 1;

            groundEnemiesCurrent = 0;
            FlyingEnemiesCurrent = 0;

            groundEnemyIncrement = 4;
            flyingEnemyIncrement = 2;

            groundSpawnFrames = 1;
            flyingSpawnFrames = 2;

            //Boss Fields
            this.input = input;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            this.boss = boss;
            this.rng = rng;
            this.moneyBomb = moneyBomb;
            this.particleManager = particleManager;
            this.nacho = nacho;
            this.gameTime = gameTime;
            this.sb = sb;

            bossDefeated = false;
        }

        // Methods

        /// <summary>
        /// Increments the count of the wave.
        /// </summary>
        public void IncrementWave()
        {
            wave++;

            groundEnemiesDefeated = 0;
            flyingEnemiesDefeated = 0;
        }

        /// <summary>
        /// Keeps track of enemy spawn
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdateSpawns(GameTime gameTime)
        {
            //If it is the boss wave and there is no
            //active boss yet, this makes sure that
            //there are no new spawning enemies and 
            //that a boss is initialized.
            if (wave >= 15 && activeBoss == false)
            {
                groundEnemyIncrement = 0;
                flyingEnemyIncrement = 0;
                InitializeBoss();
                activeBoss = true;
            }

            //Handles the enemy spawns
            groundSpawnTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (groundSpawnTimer >= groundSpawnFrames &&
                groundEnemiesCurrent + GroundEnemiesDefeated < GroundEnemyTotal)
            {
                spawnManager.SpawnEnemy();
                groundEnemiesCurrent++;

                groundSpawnTimer = 0;
            }

            //Handles the flying enemy spawns
            flyingSpawnTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (flyingSpawnTimer >= flyingSpawnFrames &&
                flyingEnemiesCurrent + flyingEnemiesDefeated < FlyingEnemyTotal)
            {
                spawnManager.SpawnFlyingEnemy();
                flyingEnemiesCurrent++;

                flyingSpawnTimer = 0;
            }

            //If there is a boss, update it.
            if (activeBoss == true)
            {
                finalBoss.BossUpdate(nacho, gameTime);
            }
            
            //If all enemies are defeated and it is not the boss level, progress
            //to the next wave
            if(TotalDefeated == GroundEnemyTotal + FlyingEnemyTotal && wave < 15)
            {
                IncrementWave();
                endOfRound = true;
            }

            //Else if it is the boss level and the boss has
            //been defeated, increment the wave
            else if (wave >= 15 && finalBoss.Position.Y > 1500)
            {
                bossDefeated = true;
            }
        }

        /// <summary>
        /// Draws the final boss if it exists.
        /// </summary>
        public void DrawFinalBoss()
        {
            if (activeBoss == true)
            {
                finalBoss.Draw(sb);
            }
            
        }

        /// <summary>
        /// Reads in data from the file and creates a new boss object.
        /// </summary>
        public void InitializeBoss()
        {
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
                    if (counter == 4)
                    {
                        int health = int.Parse(stats[0]);
                        int xSpeed = int.Parse(stats[1]);
                        int ySpeed = int.Parse(stats[2]);
                        int damage = int.Parse(stats[3]);

                        finalBoss = new Boss(health, xSpeed, ySpeed, damage,
                            new Rectangle(windowWidth - 200, 1500, 400, 700),
                               boss, new Rectangle(windowWidth - 200, windowHeight - 700, 400, 300), rng, moneyBomb, particleManager);
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
        }

    }
}
