using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SlowCanterToTheRight
{
    //Boss state enum
    public enum BossState
    {
        spawn = 0,
        facingLeft = 1,
        facingRight = 2,
        hitLeft = 3,
        hitRight = 4,
        dying = 5,
    }

    class Boss : Character
    {
        //Fields
        protected BossState bossState;
        private Rectangle weakSpot;
        Random RNG;
        private List<MoneyBomb> projectilesLeft;
        private List<MoneyBomb> projectilesRight;
        private Texture2D projectileTexture;
        ParticleManager particleManager;

        // Animation Fields
        // Values for drawing the enemy from the sprite sheet
        private int bossFrameX;
        private double bossTimeCounter;
        private double bossFPS;                      // Instantiate these in the constructor
        private double bossTimePerFrame;             // ---

        // Constants for Sprite Sheet
        private const int BossFrameCountX = 4; // Determines how many frames the animation is ( the number of sprites it runs through before going back to the first one)
        private const int BossRectWidth = 400; // Width of a single frame (you get this from counting the width between each sprite)
        private const int BossRectHeight = 700; // Height of a single frame (You get this from counting the height of the sprite + a little bit of extra room)

        //Constructor
        public Boss(int health, int xSpeed, int ySpeed, int damage, Rectangle position, Texture2D sprite, Rectangle weakSpot, Random RNG, Texture2D projectileTexture, ParticleManager particleManager)
            : base(health, xSpeed, ySpeed, damage, position, sprite)
        {
            bossState = BossState.spawn;
            this.weakSpot = weakSpot;
            this.RNG = RNG;
            projectilesLeft = new List<MoneyBomb>();
            projectilesRight = new List<MoneyBomb>();
            this.projectileTexture = projectileTexture;
            this.particleManager = particleManager;

            bossFPS = 10;
            bossTimePerFrame = 1.0 / bossFPS;
        }

        //Methods

        //Update
        /// <summary>
        /// Updates the boss's current state and actions.
        /// </summary>
        /// <param name="nacho"></param>
        /// <param name="gameTime"></param>
        public void BossUpdate(Player nacho, GameTime gameTime)
        {
            //Determines which state the boss is in
            switch (bossState)
            {
                //The Spawn states summons the boss from the ground
                case BossState.spawn:
                    position.Y -= 5;

                    if (position.Y == 100)
                    {
                        bossState = BossState.facingLeft;
                    }
                    break;

                //The facing left state, when Nacho is on the left
                //half of the screen
                case BossState.facingLeft:
                    CheckDamage(nacho);
                    Attack(nacho);
                    if (weakSpot.Intersects(nacho.Position) && nacho.PlayerState == PlayerState.AerialAttackR)
                    {
                        bossState = BossState.hitLeft;
                    }

                    if (nacho.posX > posX + (Position.Width / 2))
                    {
                        bossState = BossState.facingRight;
                    }

                    break;

                //The facing right state, when Nacho is on the right
                //half of the screen
                case BossState.facingRight:

                    CheckDamage(nacho);
                    Attack(nacho);

                    if (weakSpot.Intersects(nacho.Position) && nacho.PlayerState == PlayerState.AerialAttackL)
                    {
                        bossState = BossState.hitRight;
                    }

                    if (nacho.posX < posX + (Position.Width / 2))
                    {
                        bossState = BossState.facingLeft;
                    }

                    break;

                //The hit left state, when Nacho hits the boss
                //in its facing left state
                case BossState.hitLeft:

                    if (health <= 0)
                    {
                        projectilesLeft.Clear();
                        projectilesRight.Clear();

                        bossState = BossState.dying;
                    }
                    else
                    {
                        bossState = BossState.facingLeft;
                    }

                    break;
                
                //The hit right state, when Nacho hits the boss
                //in its facing right state
                case BossState.hitRight:
                    if (health <= 0)
                    {
                        projectilesLeft.Clear();
                        projectilesRight.Clear();

                        bossState = BossState.dying;
                    }
                    else
                    {
                        bossState = BossState.facingLeft;
                    }

                    break;
                
                //The dying state, sends the boss into the ground 
                //when it is defeated
                case BossState.dying:

                    for (int i = 0; i < 2; i++)
                    {
                        int randX = RNG.Next(posX, posX + (Position.Width / 2));
                        int randY = RNG.Next(posY, posY + Position.Height);
                        particleManager.ParticleList.Add(new Particle(randX, randY, true, 2));
                    }

                    position.Y += 5;

                    break;
            }

            //Updates the boss's animation and particle effects
            BossEnemyAnimation(gameTime);
            particleManager.UpdateParticles();

        }

        /// <summary>
        /// Handles the boss's animations.
        /// </summary>
        /// <param name="gameTime"></param>
        public void BossEnemyAnimation(GameTime gameTime)
        {
            bossTimeCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if (bossTimeCounter >= bossTimePerFrame)
            {
                bossFrameX++;

                bossTimeCounter -= bossTimePerFrame;

                if (bossFrameX > BossFrameCountX - 1)
                {
                    bossFrameX = 0;
                }
            }
        }


        //Draw
        /// <summary>
        /// Draws the boss's projectiles, as well as the proper animation
        /// based on what state it is in.
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            //Left Projectile
            if (projectilesLeft.Count == 1 && health > 0)
            {
                sb.Draw(projectileTexture, projectilesLeft[0].Position, Color.White);
            }

            //Right Projectile
            if (projectilesRight.Count == 1 && health > 0)
            {
                sb.Draw(projectileTexture, projectilesRight[0].Position, Color.White);
            }

            
            //The drawing switch state for the boss
            switch (bossState)
            {
                //Spawn
                case BossState.spawn:
                    sb.Draw(
                        sprite,
                        new Vector2(this.posX, this.posY),
                        new Rectangle(
                            bossFrameX * BossRectWidth,
                            0,
                            BossRectWidth,
                            BossRectHeight),
                        Color.White,
                        0,
                        Vector2.Zero,
                        1,
                        SpriteEffects.None,
                        0);

                    break;
                
                //Facing left
                case BossState.facingLeft:
                    sb.Draw(
                        sprite,
                        new Vector2(this.posX, this.posY),
                        new Rectangle(
                            bossFrameX * BossRectWidth,
                            0,
                            BossRectWidth,
                            BossRectHeight),
                        Color.White,
                        0,
                        Vector2.Zero,
                        1,
                        SpriteEffects.None,
                        0);

                    break;

                //Facing right
                case BossState.facingRight:
                    sb.Draw(
                        sprite,
                        new Vector2(this.posX, this.posY),
                        new Rectangle(
                            bossFrameX * BossRectWidth,
                            0,
                            BossRectWidth,
                            BossRectHeight),
                        Color.White,
                        0,
                        Vector2.Zero,
                        1,
                        SpriteEffects.None,
                        0);

                    break;

                //Hit left
                case BossState.hitLeft:
                    sb.Draw(
                        sprite,
                        new Vector2(this.posX, this.posY),
                        new Rectangle(
                            bossFrameX * BossRectWidth,
                            0,
                            BossRectWidth,
                            BossRectHeight),
                        Color.Red,
                        0,
                        Vector2.Zero,
                        1,
                        SpriteEffects.None,
                        0);

                    break;

                //Hit Right
                case BossState.hitRight:
                    sb.Draw(
                        sprite,
                        new Vector2(this.posX, this.posY),
                        new Rectangle(
                            bossFrameX * BossRectWidth,
                            0,
                            BossRectWidth,
                            BossRectHeight),
                        Color.Red,
                        0,
                        Vector2.Zero,
                        1,
                        SpriteEffects.None,
                        0);

                    break;

                //Dying
                case BossState.dying:
                    sb.Draw(
                        sprite,
                        new Vector2(this.posX, this.posY),
                        new Rectangle(
                            bossFrameX * BossRectWidth,
                            0,
                            BossRectWidth,
                            BossRectHeight),
                        Color.White,
                        0,
                        Vector2.Zero,
                        1,
                        SpriteEffects.None,
                        0);

                    break;
            }

            //Draws particles
            particleManager.DrawParticles(sb);
        }


        //Attack
        /// <summary>
        /// Launches projectiles of varying heights at the player.
        /// </summary>
        /// <param name="nacho"></param>
        public void Attack(Player nacho)
        {
            //Determines which height to shoot at
            int bombHeight = RNG.Next(1, 4);

            //The top height
            if (bombHeight == 1)
            {
                //Left
                if (projectilesLeft.Count == 0)
                {
                    projectilesLeft.Add(new MoneyBomb(new Rectangle(position.X, 365, 150, 150), projectileTexture));
                }

                //Left
                if (projectilesLeft.Count == 1)
                {
                    projectilesLeft[0].posX -= xSpeed;


                    if (projectilesLeft[0].DoDamage(nacho) == true)
                    {
                        particleManager.ParticleList.Add(new Particle(projectilesLeft[0].posX, projectilesLeft[0].posY, true, 2));
                        projectilesLeft.RemoveAt(0);
                    }

                    else if (projectilesLeft[0].posX < -15)
                    {
                        projectilesLeft.RemoveAt(0);
                    }
                }

                //Right
                if (projectilesRight.Count == 0)
                {
                    projectilesRight.Add(new MoneyBomb(new Rectangle(position.X + (position.Width / 2), 365, 150, 150), projectileTexture));
                }

                //Right
                if (projectilesRight.Count == 1)
                {
                    projectilesRight[0].posX += xSpeed;


                    if (projectilesRight[0].DoDamage(nacho) == true)
                    {
                        particleManager.ParticleList.Add(new Particle(projectilesRight[0].posX, projectilesRight[0].posY, true, 2));
                        projectilesRight.RemoveAt(0);
                    }

                    else if (projectilesRight[0].posX > 3200)
                    {
                        projectilesRight.RemoveAt(0);
                    }
                }

            }

            //The middle height
            else if (bombHeight == 2)
            {
                //Left
                if (projectilesLeft.Count == 0)
                {
                    projectilesLeft.Add(new MoneyBomb(new Rectangle(position.X, 500, 150, 150), projectileTexture));
                }

                //Left
                if (projectilesLeft.Count == 1)
                {
                    projectilesLeft[0].posX -= xSpeed;

                    if (projectilesLeft[0].DoDamage(nacho) == true)
                    {
                        particleManager.ParticleList.Add(new Particle(projectilesLeft[0].posX, projectilesLeft[0].posY, true, 2));
                        projectilesLeft.RemoveAt(0);
                    }

                    else if (projectilesLeft[0].posX < -15 || projectilesLeft[0].posX > 3200)
                    {
                        projectilesLeft.RemoveAt(0);
                    }
                }

                //Right
                if (projectilesRight.Count == 0)
                {
                    projectilesRight.Add(new MoneyBomb(new Rectangle(position.X + (position.Width / 2), 500, 150, 150), projectileTexture));
                }

                //Right
                if (projectilesRight.Count == 1)
                {
                    projectilesRight[0].posX += xSpeed;


                    if (projectilesRight[0].DoDamage(nacho) == true)
                    {
                        particleManager.ParticleList.Add(new Particle(projectilesRight[0].posX, projectilesRight[0].posY, true, 2));
                        projectilesRight.RemoveAt(0);
                    }

                    else if (projectilesRight[0].posX > 3200)
                    {
                        projectilesRight.RemoveAt(0);
                    }
                }
            }

            //The lowest height
            else if (bombHeight == 3)
            {
                //Left
                if (projectilesLeft.Count == 0)
                {
                    projectilesLeft.Add(new MoneyBomb(new Rectangle(position.X, 625, 150, 150), projectileTexture));
                }

                //Left
                if (projectilesLeft.Count == 1)
                {
                    projectilesLeft[0].posX -= xSpeed;

                    if (projectilesLeft[0].DoDamage(nacho) == true)
                    {
                        particleManager.ParticleList.Add(new Particle(projectilesLeft[0].posX, projectilesLeft[0].posY, true, 2));
                        projectilesLeft.RemoveAt(0);
                    }

                    else if (projectilesLeft[0].posX < -15 || projectilesLeft[0].posX > 3200)
                    {
                        projectilesLeft.RemoveAt(0);
                    }
                }

                //Right
                if (projectilesRight.Count == 0)
                {
                    projectilesRight.Add(new MoneyBomb(new Rectangle(position.X + (position.Width / 2), 625, 150, 150), projectileTexture));
                }

                //Right
                if (projectilesRight.Count == 1)
                {
                    projectilesRight[0].posX += xSpeed;


                    if (projectilesRight[0].DoDamage(nacho) == true)
                    {
                        particleManager.ParticleList.Add(new Particle(projectilesRight[0].posX, projectilesRight[0].posY, true, 2));
                        projectilesRight.RemoveAt(0);
                    }

                    else if (projectilesRight[0].posX > 3200)
                    {
                        projectilesRight.RemoveAt(0);
                    }
                }
            }
        }

        //Check Damage
        /// <summary>
        /// Checks to see if Nacho damages the boss.
        /// </summary>
        /// <param name="nacho"></param>
        public void CheckDamage(Player nacho)
        {
            if (nacho.Position.Intersects(weakSpot) && (nacho.PlayerState == PlayerState.AerialAttackL || nacho.PlayerState == PlayerState.AerialAttackR))
            {
                health -= nacho.Damage;

                //Pushes the player back after a blow is dealt
                if(nacho.PlayerState == PlayerState.AerialAttackL)
                {
                    nacho.XSpeed = 30;
                }
                else
                {
                    nacho.XSpeed = -30;
                }
            }
        }
    }
}
