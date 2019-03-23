using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SlowCanterToTheRight
{
    #region Enums

    // Sets up the enum for all of the player states (yes there are a lot of them)
    enum PlayerState
    {
        FacingLeft = 0,
        FacingRight = 1,
        WalkLeft = 2,
        WalkRight = 3,
        DuckLeft = 4,
        DuckRight = 5,
        JumpLeft = 6,
        JumpRight = 7,
        AttackR = 8,
        AttackL = 9,
        DuckAttackL = 10,
        DuckAttackR = 11,
        AerialAttackL = 12,
        AerialAttackR = 13,
        DownwardStrike = 14,
        Dying = 15,
        Dead = 16
    }
#endregion

    // Player Class
    // The player class is a massive class that handles the interations with the player and the user,
    // like keyboard input, and also handles the player's interation with the other elements of the 
    // game, like the enemies, bombs, and the bounds of the map
    class Player : Character
    {
        #region Fields
        private PlayerState playerState;
        private bool prevPlayerDirection;

        // Variables that store the dimensions of the playable game area
        private int xBound;
        private int yBound;

        private KeyboardState KBprev;

        // Controls the length of frames that the player attacks for
        private double attackTimeCounter;
        private double attackFrames;

        // Controls the frames that the player is invincible for after taking damage
        private double invincibilityTimeCounter;
        private double invincibilityFrames;
        private float scl;
        private float rot;

        private SpriteBatch sb;

        // The Random object for the player class
        private Random rng;
        private Camera playerCamera;

        // Controls the frames that the screen shakes for after a successful hit with an attack
        private double shakeTimeCounter;
        private double shakeFrames;
        private bool isShaking;

        // The particle manager for the bomb explosion particles and the hit particles
        ParticleManager particleManager;
        private Texture2D bombSprite;
        private List<Bomb> bombManager;
        private bool thrownBomb;

        // Controls the frames the bomb is active for
        private double bombTimeCounter;
        private double bombFrames;

        // Amount of bombs the player has at their disposal
        private int numBombs;

        // Checks if the player is invincble after a hit
        private bool isHurt;
        private int originalHeight;
        private int coincount;
        private int tempCoinPerEnemy;

        //For Nacho animation
        private int nachoFrame;
        private double nachoTimeCounter;
        private double nachoFps;
        private double nachoTimePerFrame;

        //constants for nacho rectangle
        private const int NachoWalkFrameCount = 3;
        private const int NachoDuckFrameCount = 2;
        private const int NachoJumpFrameCount = 2;
        private const int NachoJumpAttackFrameCount = 4;
        private const int NachoDownwardstrikeFrameCount = 3;
        private const int NachoStandingAttackFrameCount = 3;

        private const int NachoRectHeight = 64;
        private const int NachoRectWidth = 41;

        //nacho rect offsets on the spritesheet
        private const int NachoWalkOffsetY = 0;
        private const int NachoDuckOffsetY = 65;
        private const int NachoJumpAttackOffsetY = 129;
        private const int NachoDownwardStrikeOffsetY = 190;
        private const int NachoStandingAttackOffsetY = 251;
        private const int NachoJumpOffsetY = 318;
        #endregion

        #region Properties
        public int Coincount { get { return coincount; } set { coincount = value; } }

        public PlayerState PlayerState
        {
            get { return playerState; }
        }

        // The platform collider is a rectangle that allows the player to land on platforms within a given range
        public Rectangle PlatformCollider
        {
            get { return new Rectangle(posX, posY + Height, Width, 5); }
        }

        // The base attack hitbox for the player in the X direction
        public int AttackX
        {
            get
            {
                if (playerState == PlayerState.AttackL)
                {
                    return this.posX - 300;
                }
                else
                {
                    return this.posX + Width;
                }
            }
        }

        // The base attack hitbox for the player in the Y direction
        public int AttackY
        {
            get { return this.posY + 50; }
        }

        public int NumBombs
        {
            get { return numBombs; }
            set { numBombs = value; }
        }

        public bool IsHurt
        {
            get { return isHurt; }
            set { isHurt = value; }
        }
        #endregion

        # region Constructor
        public Player(int Phealth, int PxSpeed, int PySpeed, int Pdamage, Rectangle Pposition, Texture2D Psprite, 
            Camera playerCamera, Random rng, ParticleManager particleManager, Texture2D bombSprite, SpriteBatch sb) 
            : base(Phealth, PxSpeed, PySpeed, Pdamage, Pposition, Psprite)
        {
            // The player begin the game facing to the right
            playerState = PlayerState.FacingRight;
            originalHeight = Height;

            // Sets the attack timings and invincibility frames
            attackFrames = .2;
            invincibilityFrames = 2000;

            // Sets the amound of coins dropped per enemy
            tempCoinPerEnemy = 100;

            // Sets the scale and rotation for drawing
            scl = 6;
            rot = 0;
            this.sb = sb;

            // Initiates the paricle list and bomb list in order to hancle both opertaions
            this.particleManager = particleManager;
            bombManager = new List<Bomb>();
            this.bombSprite = bombSprite;
            thrownBomb = false;
            bombFrames = .5;

            // Sets the starting amount of bombs the player has
            numBombs = 5;

            // Sets the map bounds
            xBound = 3200;
            yBound = 800;

            // Initiates the random object and the camera to follow the player
            this.playerCamera = playerCamera;
            this.rng = rng;

            // The screen begins not shaking
            isShaking = false;

            // The time that player attacks make the screen shake for
            shakeFrames = .1;

            isHurt = false;

            // Sets the FPS for the player animation
            nachoFrame = 15;
            nachoTimePerFrame = 1.0 / nachoFps;
        }
        #endregion

        // Updates the player object based upon keyboard input and the positions of various objects
        #region Player Update Method
        public void PlayerUpdate(GameTime gameTime, EnemyManager enemies, List<Platform> platforms)
        {
            // Sets the current keyboard state based upon input
            KeyboardState KBnow = Keyboard.GetState();
            NachoUpdateAnimation(gameTime);
            // If isShaking is set to true, the screen will shake until the timer runs out
            if (isShaking == true)
            {
                playerCamera.ShakeCamera(new Vector2(posX + Width / 2, posY), rng);

                shakeTimeCounter += gameTime.ElapsedGameTime.TotalSeconds;
                if (shakeTimeCounter >= shakeFrames)
                {
                    isShaking = false;
                    shakeTimeCounter = 0;
                    shakeFrames = .1;
                }
            }
            else
            {
                // When the camera is not shaking, it is updated as normal and is set to the default shake radius
                playerCamera.ShakeRadius = 10;
                playerCamera.UpdateCamera(new Vector2(posX + Width / 2, posY));
            }

            // Yes the player switch statement is very large. Had there been more time, the statement would likely be broken up further 
            // by helper methods, but as it stands currently, a large method was necessary for fluid and varied movement at this coding skill level
            #region Player Switch State
            switch (playerState)
            {
                // The normal standing attacks of the player
                #region AtttackL
                case PlayerState.AttackL:
                    // Iterates through the enemy hitboxes to check if the attack hitbox is contacting any of them
                    foreach (Enemy e in enemies.EnemyList)
                    {
                        if (new Rectangle(AttackX + 160, AttackY, 150, Height).Intersects(e.EnemyHitbox) &&
                            (e.EnemyState == EnemyState.facingLeft || e.EnemyState == EnemyState.facingRight ||
                            e.EnemyState == EnemyState.scout || e.EnemyState == EnemyState.pursue))
                        {
                            // When the attack lands, the screen shakes, the enemy takes damage, and the particle is spawned upon contact
                            isShaking = true;
                            particleManager.ParticleList.Add(new Particle(e.posX, e.posY, true, 2));

                            xSpeed = 10;
                            e.Health -= damage;

                            // If the enemy attacked does not die, they are merely knocked back
                            if (e.Health > 0)
                            {
                                e.EnemyState = EnemyState.hitLeft;
                                e.XSpeed = -30;
                                e.YSpeed = -20;
                            }
                            // If the enemy is in fact dead, coins burst from the body and the enemy enter its dying state
                            else
                            {
                                // Flying enemy deaths
                                if (e.EnemyState == EnemyState.scout || e.EnemyState == EnemyState.pursue)
                                {
                                    for (int j = 0; j < tempCoinPerEnemy; j++)
                                    {
                                        enemies.Coinlist.Add(new Coin(new Rectangle(e.posX + e.Width / 2, e.posY, 20, 20),
                                            rng.Next(-80, 40), -(rng.Next(40)), enemies.CoinSprite));
                                    }
                                    e.EnemyState = EnemyState.dying;
                                    e.XSpeed = 0;
                                    e.YSpeed = 0;
                                }
                                // Ground enemy deaths
                                else
                                {
                                    for (int j = 0; j < tempCoinPerEnemy; j++)
                                    {
                                        enemies.Coinlist.Add(new Coin(new Rectangle(e.posX + e.Width / 2, e.posY, 20, 20),
                                            rng.Next(-80, 40), -(rng.Next(40)), enemies.CoinSprite));
                                    }
                                    e.EnemyState = EnemyState.dying;
                                    e.EnemyState = EnemyState.dying;
                                    e.XSpeed = -60;
                                    e.YSpeed = -30;
                                }
                            }
                        }
                    }

                    // Translates the player by their current speed and checks the walls of the game area
                    posX += xSpeed;
                    CheckBounds();

                    // Decelerates the player while they are moving
                    if (xSpeed > 0)
                    {
                        xSpeed--;
                    }

                    // When the attack has run for a set time, the attack hitbox goes away and the player reverts
                    // to its previous state
                    attackTimeCounter += gameTime.ElapsedGameTime.TotalSeconds;
                    if (attackTimeCounter >= attackFrames)
                    {
                        playerState = PlayerState.FacingLeft;
                        attackTimeCounter = 0;
                    }
                    break;
                    #endregion

                // The code for the Attack R is very similar to Attack L, with numbers changed to 
                // match hitbox positions and enemy knockback direction
                // The comments from Attack L apply to Attack R
                #region Attack R
                case PlayerState.AttackR:
                    foreach (Enemy e in enemies.EnemyList)
                    {
                        if(new Rectangle(AttackX + 170, AttackY, 150, Height).Intersects(e.EnemyHitbox) &&
                            (e.EnemyState == EnemyState.facingLeft || e.EnemyState == EnemyState.facingRight ||
                            e.EnemyState == EnemyState.scout || e.EnemyState == EnemyState.pursue))
                        {
                            isShaking = true;
                            particleManager.ParticleList.Add(new Particle(e.posX, e.posY, false, 2));

                            xSpeed = -10;
                            e.Health -= damage;
                            if(e.Health > 0)
                            {
                                e.EnemyState = EnemyState.hitRight;
                                e.XSpeed = 30;
                                e.YSpeed = -20;
                            }
                            else
                            {
                                if (e.EnemyState == EnemyState.scout || e.EnemyState == EnemyState.pursue)
                                {
                                    for (int j = 0; j < tempCoinPerEnemy; j++)
                                    {
                                        enemies.Coinlist.Add(new Coin(new Rectangle(e.posX + e.Width / 2, e.posY, 20, 20),
                                            rng.Next(-40, 80), -(rng.Next(40)), enemies.CoinSprite));
                                    }
                                    e.EnemyState = EnemyState.dying;
                                    e.XSpeed = 0;
                                    e.YSpeed = 0;
                                }
                                else
                                {
                                    for (int j = 0; j < tempCoinPerEnemy; j++)
                                    {
                                        enemies.Coinlist.Add(new Coin(new Rectangle(e.posX + e.Width / 2, e.posY, 20, 20),
                                            rng.Next(-40, 80), -(rng.Next(40)), enemies.CoinSprite));
                                    }
                                    e.EnemyState = EnemyState.dying;
                                    e.XSpeed = 60;
                                    e.YSpeed = -30;
                                }
                            }
                        }
                    }

                    posX += xSpeed;
                    CheckBounds();
                    if (xSpeed < 0)
                    {
                        xSpeed++;
                    }

                    attackTimeCounter += gameTime.ElapsedGameTime.TotalSeconds;
                    if(attackTimeCounter >= attackFrames)
                    {
                        playerState = PlayerState.FacingRight;
                        attackTimeCounter = 0;
                    }
                    
                    break;
                #endregion
                
                // The player's default standing states when no keys are being pressed and speed is 0
                #region Face Left
                case PlayerState.FacingLeft:
                    // Translates the player position by the current X speed
                    position.X += xSpeed;

                    // If the X speed is greater than or less than 0, it reduces down until it is equal to 0
                    if(xSpeed < 0)
                    {
                        xSpeed++;
                    }
                    else if(xSpeed > 0)
                    {
                        xSpeed--;
                    }

                    // Checks the bounds of the player area to make sure walls are not phased through
                    CheckBounds();

                    // Checks if there is a platform underneath
                    if (CheckPlatform(platforms) == false && position.Y <= yBound - position.Height
                        && posY != yBound - Height)
                    {
                        playerState = PlayerState.JumpRight;
                    }

                    // Checks the current keyboard input to decide which state to go to for the next iteration,
                    // changing speed values accordingly
                    if (KBnow.IsKeyDown(Keys.Down))
                    {
                        playerState = PlayerState.DuckLeft;
                    }
                    else if (KBnow.IsKeyDown(Keys.Up))
                    {
                        playerState = PlayerState.JumpLeft;
                        ySpeed = -40;
                    }
                    else if (KBnow.IsKeyDown(Keys.Right))
                    {
                        playerState = PlayerState.WalkRight;
                    }
                    else if (KBnow.IsKeyDown(Keys.Left))
                    {
                        playerState = PlayerState.WalkLeft;
                    }
                    else if (KBnow.IsKeyDown(Keys.Z))
                    {
                        playerState = PlayerState.AttackL;
                        xSpeed = 0;
                    }
                    else if (KBnow.IsKeyDown(Keys.X) && thrownBomb == false && numBombs > 0)
                    {
                        thrownBomb = true;
                        bombManager.Add(new Bomb(bombSprite, new Rectangle(posX, posY, bombSprite.Width, bombSprite.Height), -5));
                        numBombs--;
                    }
                    break;
                #endregion

                // The code for the Face Left is very similar to Face Right, with numbers changed to 
                // match hitbox positions and orientations
                // The comments from Face Left apply to Face Right
                #region Face Right
                case PlayerState.FacingRight:
                    position.X += xSpeed;

                    if (xSpeed < 0)
                    {
                        xSpeed++;
                    }
                    else if (xSpeed > 0)
                    {
                        xSpeed--;
                    }

                    CheckBounds();

                    if (CheckPlatform(platforms) == false && position.Y <= yBound - position.Height
                        && posY != yBound - Height)
                    {
                        playerState = PlayerState.JumpRight;
                    }

                    if (KBnow.IsKeyDown(Keys.Down))
                    {
                        playerState = PlayerState.DuckRight;
                    }
                    else if (KBnow.IsKeyDown(Keys.Up))
                    {
                        playerState = PlayerState.JumpRight;
                        ySpeed = -40;
                    }
                    else if (KBnow.IsKeyDown(Keys.Right))
                    {
                        playerState = PlayerState.WalkRight;
                    }
                    else if (KBnow.IsKeyDown(Keys.Left))
                    {
                        playerState = PlayerState.WalkLeft;
                    }
                    else if (KBnow.IsKeyDown(Keys.Z))
                    {
                        playerState = PlayerState.AttackR;
                        xSpeed = 0;
                    }
                    else if (KBnow.IsKeyDown(Keys.X) && thrownBomb == false && numBombs > 0)
                    {
                        thrownBomb = true;
                        bombManager.Add(new Bomb(bombSprite, new Rectangle(posX, posY, bombSprite.Width, bombSprite.Height), 5));
                        numBombs--;
                    }

                    break;
                #endregion
                
                // The basic movements for the player walking while pressing the RIGHT and LEFT arrow keys
                #region Walk L
                case PlayerState.WalkLeft:
                    // Accelerates the player while they are under the maximum speed
                    if (xSpeed > -20)
                    {
                        xSpeed -= 2;
                    }

                    // Moves the player by their current speed
                    position.X += xSpeed;

                    // Checks wall colisons
                    CheckBounds();

                    // Takes keyboard input to decide the next state that the player will assume
                    if (KBnow.IsKeyDown(Keys.Down))
                    {
                        playerState = PlayerState.DuckLeft;
                    }
                    else if (KBnow.IsKeyDown(Keys.Up))
                    {
                        playerState = PlayerState.JumpLeft;
                        ySpeed = -40;
                    }
                    else if (KBnow.IsKeyDown(Keys.Z) && KBnow != KBprev)
                    {
                        xSpeed = 0;
                        playerState = PlayerState.AttackL;
                    }
                    else if (KBnow.IsKeyDown(Keys.Right))
                    {
                        playerState = PlayerState.WalkRight;
                    }
                    else if (KBnow.IsKeyDown(Keys.Left))
                    {
                        playerState = PlayerState.WalkLeft;
                    }
                    else
                    {
                        playerState = PlayerState.FacingLeft;
                    }

                    if (KBnow.IsKeyDown(Keys.X) && thrownBomb == false && numBombs > 0)
                    {
                        thrownBomb = true;
                        bombManager.Add(new Bomb(bombSprite, new Rectangle(posX, posY, bombSprite.Width, bombSprite.Height), -15));
                        numBombs--;
                    }

                    // Checks if there is a platform underneath the player
                    if (CheckPlatform(platforms) == false && position.Y != yBound - position.Height &&
                        playerState != PlayerState.AttackL)
                    {
                        playerState = PlayerState.JumpLeft;
                    }

                    break;
                #endregion

                // The code for the Walk R is very similar to Walk L, with numbers changed to 
                // match hitbox positions and accelerations, as well as next state calculations that reflect direction
                // The comments from Walk L apply to Walk R
                #region Walk R
                case PlayerState.WalkRight:
                    if(xSpeed < 20)
                    {
                        xSpeed += 2;
                    }

                    position.X += xSpeed;

                    CheckBounds();

                    if (KBnow.IsKeyDown(Keys.Down))
                    {
                        playerState = PlayerState.DuckRight;
                    }
                    else if (KBnow.IsKeyDown(Keys.Up))
                    {
                        playerState = PlayerState.JumpRight;
                        ySpeed = -40;
                    }
                    else if (KBnow.IsKeyDown(Keys.Z) && KBnow != KBprev)
                    {
                        xSpeed = 0;
                        playerState = PlayerState.AttackR;
                    }
                    else if (KBnow.IsKeyDown(Keys.Right))
                    {
                        playerState = PlayerState.WalkRight;
                    }
                    else if (KBnow.IsKeyDown(Keys.Left))
                    {
                        playerState = PlayerState.WalkLeft;
                    }
                    else
                    {
                        playerState = PlayerState.FacingRight;
                    }

                    if (KBnow.IsKeyDown(Keys.X) && thrownBomb == false && numBombs > 0)
                    {
                        thrownBomb = true;
                        bombManager.Add(new Bomb(bombSprite, new Rectangle(posX, posY, bombSprite.Width, bombSprite.Height), 15));
                        numBombs--;
                    }

                    if (CheckPlatform(platforms) == false && position.Y != yBound - position.Height &&
                        playerState != PlayerState.AttackR)
                    {
                        playerState = PlayerState.JumpRight;
                    }

                    break;
                #endregion
                
                // When the player holds the DOWN arrow their height is reduced and they cannot move, though they can still attack 
                #region Duck L
                case PlayerState.DuckLeft:
                    // Cuts the hitbox for the player in half while ducking
                    if(Height == originalHeight)
                    {
                        posY += Height / 2;
                        Height /= 2;
                    }

                    // Translates along X axis
                    position.X += xSpeed;

                    // Decelerate the player until they have a velocity magnitude of 0
                    if (xSpeed < 0)
                    {
                        xSpeed++;
                    }
                    else if (xSpeed > 0)
                    {
                        xSpeed--;
                    }

                    // Checks wall collisions
                    CheckBounds();

                    // Checks the keyboard input in order to determine the nect state of the player
                    if (KBnow.IsKeyDown(Keys.Z) && KBnow != KBprev)
                    {
                        xSpeed = 0;
                        playerState = PlayerState.DuckAttackL;
                    }
                    // When the player jumps or stops holding the DOWN arrow key,
                    // The player hitbox is returned to its original value
                    else if (KBnow.IsKeyDown(Keys.Down) != true)
                    {
                        position.Height *= 2;
                        position.Y -= position.Height / 2;
                        playerState = PlayerState.FacingLeft;
                    }
                    else if (posY <= yBound - Height && posY != yBound - Height)
                    {
                        position.Height *= 2;
                        playerState = PlayerState.JumpLeft;
                        position.Y += 1;
                    }
                    break;
                #endregion

                // The code for the Duck R is very similar to Duck L, with numbers changed to 
                // match hitbox positions and accelerations, as well as next state calculations that reflect direction
                // The comments from Duck L apply to Duck R
                #region Duck R
                case PlayerState.DuckRight:
                    if (Height == originalHeight)
                    {
                        posY += Height / 2;
                        Height /= 2;
                    }

                    position.X += xSpeed;

                    if (xSpeed < 0)
                    {
                        xSpeed++;
                    }
                    else if (xSpeed > 0)
                    {
                        xSpeed--;
                    }

                    CheckBounds();

                    if (KBnow.IsKeyDown(Keys.Z) && KBnow != KBprev)
                    {
                        xSpeed = 0;
                        playerState = PlayerState.DuckAttackR;
                    }
                    else if (KBnow.IsKeyDown(Keys.Down) != true)
                    {
                        position.Height *= 2;
                        position.Y -= position.Height / 2;
                        playerState = PlayerState.FacingRight;
                    }
                    else if (posY <= yBound - Height && posY != yBound - Height)
                    {
                        position.Height *= 2;
                        playerState = PlayerState.JumpRight;
                        position.Y += 1;
                    }
                    break;
                #endregion

                // Pressing the UP arrow allows the player to gain upward velocity and perform a number of airborne attacks
                #region Jump L
                case PlayerState.JumpLeft:
                    // Moves the player by its X and Y speeds
                    position.X += xSpeed;
                    position.Y += ySpeed;

                    // Checks wall collisions
                    CheckBounds();

                    // Increases velocity downward up to a maximum
                    if(ySpeed <= 45)
                    {
                        ySpeed += 2;
                    }

                    // Updates the playerstate based upon the current keyboard input
                    if (KBnow.IsKeyDown(Keys.Z) && KBnow != KBprev)
                    {
                        playerState = PlayerState.AerialAttackL;
                    }
                    else if (KBnow.IsKeyDown(Keys.Down))
                    {
                        playerState = PlayerState.DownwardStrike;
                        prevPlayerDirection = true;
                    }
                    else if(KBnow.IsKeyDown(Keys.Left) && xSpeed >= -20)
                    {
                        xSpeed--;
                    }
                    else if (KBnow.IsKeyDown(Keys.Right))
                    {
                        xSpeed++;
                        playerState = PlayerState.JumpRight;
                    }
                    else if (xSpeed < 0)
                    {
                        xSpeed++;
                    }

                    // Pressing X throws a bomb, independent of other inputs
                    if (KBnow.IsKeyDown(Keys.X) && thrownBomb == false && numBombs > 0)
                    {
                        thrownBomb = true;
                        bombManager.Add(new Bomb(bombSprite, new Rectangle(posX, posY, bombSprite.Width, bombSprite.Height), -10));
                        numBombs--;
                    }

                    // Checks platforms for landing on top of
                    if (ySpeed >= 0 && CheckPlatform(platforms))
                    {
                        position.Y = platforms[0].posY - Height;
                        playerState = PlayerState.FacingLeft;
                        ySpeed = 0;
                    }

                    // When the ground of the map is reached, player reverts to a standing state
                    if (position.Y >= yBound - position.Height)
                    {
                        position.Y = yBound - position.Height;
                        playerState = PlayerState.FacingLeft;
                    }

                    break;
                #endregion

                // The code for the Jump R is very similar to Jump L, with numbers changed to 
                // match hitbox positions and accelerations, as well as next state calculations that reflect direction
                // The comments from Jump L apply to Jump R
                #region Jump R
                case PlayerState.JumpRight:
                    position.X += xSpeed;
                    position.Y += ySpeed;

                    CheckBounds();

                    if (ySpeed <= 45)
                    {
                        ySpeed += 2;
                    }

                    if (KBnow.IsKeyDown(Keys.Z) && KBnow != KBprev)
                    {
                        playerState = PlayerState.AerialAttackR;
                    }
                    else if (KBnow.IsKeyDown(Keys.Down))
                    {
                        playerState = PlayerState.DownwardStrike;
                        prevPlayerDirection = false;
                    }
                    else if(KBnow.IsKeyDown(Keys.Right) && xSpeed <= 20)
                    {
                        xSpeed++;
                    }
                    else if (KBnow.IsKeyDown(Keys.Left))
                    {
                        xSpeed--;
                        playerState = PlayerState.JumpLeft;
                    }
                    else if(xSpeed > 0)
                    {
                        xSpeed--;
                    }

                    if (KBnow.IsKeyDown(Keys.X) && thrownBomb == false && numBombs > 0)
                    {
                        thrownBomb = true;
                        bombManager.Add(new Bomb(bombSprite, new Rectangle(posX, posY, bombSprite.Width, bombSprite.Height), 10));
                        numBombs--;
                    }

                    if (ySpeed >= 0 && CheckPlatform(platforms))
                    {
                        position.Y = platforms[0].posY - Height;
                        playerState = PlayerState.FacingRight;
                        ySpeed = 0;
                    }

                    if (position.Y >= yBound - position.Height)
                    {
                        position.Y = yBound - position.Height;
                        playerState = PlayerState.FacingRight;
                    }

                    break;
#endregion

                // The attack state for while the player is ducking and presses the Z key
                #region DuckAtk L
                case PlayerState.DuckAttackL:
                    xSpeed = 0;

                    foreach (Enemy e in enemies.EnemyList)
                    {
                        // Checks if the attack hitbox intersects with any enemies in the list
                        if (new Rectangle(posX - 120, posY + 25, 150, Height).Intersects(e.EnemyHitbox) &&
                            (e.EnemyState == EnemyState.facingLeft || e.EnemyState == EnemyState.facingRight ||
                            e.EnemyState == EnemyState.scout || e.EnemyState == EnemyState.pursue))
                        {
                            // The screen shakes and a particle is spawned when a hit is registered
                            isShaking = true;
                            particleManager.ParticleList.Add(new Particle(e.posX, e.posY, true, 2));

                            // If the enemy survives the hit, it is knocked back
                            e.Health -= damage;
                            if (e.Health > 0)
                            {
                                e.EnemyState = EnemyState.hitLeft;
                                e.XSpeed = -20;
                                e.YSpeed = -10;
                            }
                            // If the enemy is killed, it is set into its dying states and coins burst out
                            else
                            {
                                // Handles flying enemy death
                                if (e.EnemyState == EnemyState.scout || e.EnemyState == EnemyState.pursue)
                                {
                                    for (int j = 0; j < tempCoinPerEnemy; j++)
                                    {
                                        enemies.Coinlist.Add(new Coin(new Rectangle(e.posX + e.Width / 2, e.posY, 20, 20),
                                            rng.Next(-60, 30), -(rng.Next(30)), enemies.CoinSprite));
                                    }
                                    e.EnemyState = EnemyState.dying;
                                    e.XSpeed = 0;
                                    e.YSpeed = 0;
                                }
                                else
                                {
                                    // Handles ground enemy death
                                    for (int j = 0; j < tempCoinPerEnemy; j++)
                                    {
                                        enemies.Coinlist.Add(new Coin(new Rectangle(e.posX + e.Width / 2, e.posY, 20, 20),
                                            rng.Next(-60, 30), -(rng.Next(30)), enemies.CoinSprite));
                                    }
                                    e.EnemyState = EnemyState.dying;
                                    e.XSpeed = -60;
                                    e.YSpeed = -30;
                                }
                            }
                        }
                    }

                    // When the attack animation is complete, the player reverts back to their previous state
                    attackTimeCounter += gameTime.ElapsedGameTime.TotalSeconds;
                    if (attackTimeCounter >= attackFrames)
                    {
                        playerState = PlayerState.DuckLeft;
                        attackTimeCounter = 0;
                    }

                    break;
                #endregion

                // The code for the AirAtk R is very similar to DuckAtk L, with numbers changed to 
                // match hitbox positions, as well as next state calculations that reflect direction
                // The comments from DuckAtk L apply to DuckAtk R
                #region DuckAtk R
                case PlayerState.DuckAttackR:
                    xSpeed = 0;

                    foreach (Enemy e in enemies.EnemyList)
                    {
                        if (new Rectangle(posX + Width + 120, posY + 25, 150, Height).Intersects(e.EnemyHitbox) &&
                            (e.EnemyState == EnemyState.facingLeft || e.EnemyState == EnemyState.facingRight ||
                            e.EnemyState == EnemyState.scout || e.EnemyState == EnemyState.pursue))
                        {
                            isShaking = true;
                            particleManager.ParticleList.Add(new Particle(e.posX, e.posY, false, 2));

                            e.Health -= damage;
                            if (e.Health > 0)
                            {
                                e.EnemyState = EnemyState.hitRight;
                                e.XSpeed = 20;
                                e.YSpeed = -10;
                            }
                            else
                            {
                                if (e.EnemyState == EnemyState.scout || e.EnemyState == EnemyState.pursue)
                                {
                                    for (int j = 0; j < tempCoinPerEnemy; j++)
                                    {
                                        enemies.Coinlist.Add(new Coin(new Rectangle(e.posX + e.Width / 2, e.posY, 20, 20),
                                            rng.Next(-30, 60), -(rng.Next(30)), enemies.CoinSprite));
                                    }
                                    e.EnemyState = EnemyState.dying;
                                    e.XSpeed = 0;
                                    e.YSpeed = 0;
                                }
                                else
                                {
                                    for (int j = 0; j < tempCoinPerEnemy; j++)
                                    {
                                        enemies.Coinlist.Add(new Coin(new Rectangle(e.posX + e.Width / 2, e.posY, 20, 20),
                                            rng.Next(-30, 60), -(rng.Next(30)), enemies.CoinSprite));
                                    }
                                    e.EnemyState = EnemyState.dying;
                                    e.XSpeed = 60;
                                    e.YSpeed = -30;
                                }
                            }
                        }
                    }

                    attackTimeCounter += gameTime.ElapsedGameTime.TotalSeconds;
                    if (attackTimeCounter >= attackFrames)
                    {
                        playerState = PlayerState.DuckRight;
                        attackTimeCounter = 0;
                    }

                    break;
                #endregion

                // The basic attack performed when pressing the Z key while jumping
                #region AirAtk L
                case PlayerState.AerialAttackL:
                    // Evaluates the current X and Y speed of the player for translation durin and aerial attack
                    position.X += xSpeed;
                    position.Y += ySpeed;

                    // Checks wall bounds
                    CheckBounds();

                    foreach (Enemy e in enemies.EnemyList)
                    {
                        // When the attack hitbox interects with an enemy, the enemy sustains damage 
                        // inflicted by the player
                        if (new Rectangle(posX - 250, posY, 250, Height).Intersects(e.EnemyHitbox) &&
                            (e.EnemyState == EnemyState.facingLeft || e.EnemyState == EnemyState.facingRight ||
                            e.EnemyState == EnemyState.scout || e.EnemyState == EnemyState.pursue))
                        {
                            // The screen shakes and a particle is spawned
                            isShaking = true;
                            particleManager.ParticleList.Add(new Particle(e.posX, e.posY, true, 2));

                            // If the enemy survives the strike, it is knocked backward
                            xSpeed = 10;
                            e.Health -= damage;
                            if (e.Health > 0)
                            {
                                e.EnemyState = EnemyState.hitLeft;
                                e.XSpeed = -40;
                                e.YSpeed = -10;
                            }
                            // If the enemy is killed by the strike, it enters its dying state and coins burst out
                            else
                            {
                                // Handles flying enemy death
                                if (e.EnemyState == EnemyState.scout || e.EnemyState == EnemyState.pursue)
                                {
                                    for (int j = 0; j < tempCoinPerEnemy; j++)
                                    {
                                        enemies.Coinlist.Add(new Coin(new Rectangle(e.posX + e.Width / 2, e.posY, 20, 20),
                                            rng.Next(-80, 40), -(rng.Next(40)), enemies.CoinSprite));
                                    }
                                    e.EnemyState = EnemyState.dying;
                                    e.XSpeed = 0;
                                    e.YSpeed = 0;
                                }
                                // Handles ground enemy death
                                else
                                {
                                    for (int j = 0; j < tempCoinPerEnemy; j++)
                                    {
                                        enemies.Coinlist.Add(new Coin(new Rectangle(e.posX + e.Width / 2, e.posY, 20, 20),
                                            rng.Next(-80, 40), -(rng.Next(40)), enemies.CoinSprite));
                                    }
                                    e.EnemyState = EnemyState.dying;
                                    e.XSpeed = -60;
                                    e.YSpeed = -30;
                                }
                            }
                        }
                    }

                    // After the attack timer runs its course, the player returns to their previous state
                    attackTimeCounter += gameTime.ElapsedGameTime.TotalSeconds;
                    if (attackTimeCounter >= attackFrames)
                    {
                        playerState = PlayerState.JumpLeft;
                        attackTimeCounter = 0;
                    }

                    // The player slows down while attacking in mid-air
                    if(xSpeed > 0)
                    {
                        xSpeed--;
                    }
                    else
                    {
                        xSpeed++;
                    }

                    // The ySpeed increases while falling up to a maximum amount
                    if(ySpeed <= 45)
                    {
                        ySpeed += 2;
                    }

                    // Checks if there is a platform underneath
                    if (ySpeed >= 0 && CheckPlatform(platforms))
                    {
                        position.Y = platforms[0].posY - Height;
                        playerState = PlayerState.FacingLeft;
                        ySpeed = 0;
                    }

                    // When the player makes contact with the ground, they are reverted back to their standing state
                    if (position.Y >= yBound - position.Height)
                    {
                        position.Y = yBound - position.Height;
                        playerState = PlayerState.FacingLeft;
                    }

                    break;
                #endregion

                // The code for the AirAtk R is very similar to AirAtk L, with numbers changed to 
                // match hitbox positions, as well as next state calculations that reflect direction
                // The comments from AirAtk L apply to AirAtk R
                #region AirArk R
                case PlayerState.AerialAttackR:
                    position.X += xSpeed;
                    position.Y += ySpeed;

                    CheckBounds();

                    foreach (Enemy e in enemies.EnemyList)
                    {
                        if (new Rectangle(posX + 400, posY, 250, Height).Intersects(e.EnemyHitbox) &&
                            (e.EnemyState == EnemyState.facingLeft || e.EnemyState == EnemyState.facingRight ||
                            e.EnemyState == EnemyState.scout || e.EnemyState == EnemyState.pursue))
                        {
                            isShaking = true;
                            particleManager.ParticleList.Add(new Particle(e.posX, e.posY, false, 2));

                            xSpeed = -10;
                            e.Health -= damage;
                            if (e.Health > 0)
                            {
                                e.EnemyState = EnemyState.hitRight;
                                e.XSpeed = 40;
                                e.YSpeed = -10;
                            }
                            else
                            {
                                if (e.EnemyState == EnemyState.scout || e.EnemyState == EnemyState.pursue)
                                {
                                    for (int j = 0; j < tempCoinPerEnemy; j++)
                                    {
                                        enemies.Coinlist.Add(new Coin(new Rectangle(e.posX + e.Width / 2, e.posY, 20, 20),
                                            rng.Next(-40, 80), -(rng.Next(40)), enemies.CoinSprite));
                                    }
                                    e.EnemyState = EnemyState.dying;
                                    e.XSpeed = 0;
                                    e.YSpeed = 0;
                                }
                                else
                                {
                                    for (int j = 0; j < tempCoinPerEnemy; j++)
                                    {
                                        enemies.Coinlist.Add(new Coin(new Rectangle(e.posX + e.Width / 2, e.posY, 20, 20),
                                            rng.Next(-40, 80), -(rng.Next(40)), enemies.CoinSprite));
                                    }
                                    e.EnemyState = EnemyState.dying;
                                    e.XSpeed = 60;
                                    e.YSpeed = -30;
                                }
                            }
                        }
                    }

                    attackTimeCounter += gameTime.ElapsedGameTime.TotalSeconds;
                    if (attackTimeCounter >= attackFrames)
                    {
                        playerState = PlayerState.JumpRight;
                        attackTimeCounter = 0;
                    }

                    if (xSpeed > 0)
                    {
                        xSpeed--;
                    }
                    else
                    {
                        xSpeed++;
                    }

                    if (ySpeed <= 45)
                    {
                        ySpeed += 2;
                    }

                    if (ySpeed >= 0 && CheckPlatform(platforms))
                    {
                        position.Y = platforms[0].posY - Height;
                        playerState = PlayerState.FacingRight;
                        ySpeed = 0;
                    }

                    if (position.Y >= yBound - position.Height)
                    {
                        position.Y = yBound - position.Height;
                        playerState = PlayerState.FacingRight;
                    }

                    break;
                #endregion

                // When holding the down arrow while airborne, the player can bounce on top of enemies, hurting them in the process 
                #region DownAtk
                case PlayerState.DownwardStrike:
                    // Translates the player by the current speed
                    position.X += xSpeed;
                    position.Y += ySpeed;

                    // Checks wall collisions
                    CheckBounds();

                    foreach (Enemy e in enemies.EnemyList)
                    {
                        // Checks for enemy hitboxes colliding with the attack hitbox
                        if (new Rectangle(posX, posY + Height, Width, 150).Intersects(e.EnemyHitbox) &&
                            (e.EnemyState == EnemyState.facingLeft || e.EnemyState == EnemyState.facingRight ||
                            e.EnemyState == EnemyState.scout || e.EnemyState == EnemyState.pursue))
                        {
                            // When an attack hits, the player receives an upward velocity
                            ySpeed = -30;

                            // The screen shakes and a particle spawns
                            isShaking = true;
                            particleManager.ParticleList.Add(new Particle(e.posX, e.posY, true, 2));

                            e.Health -= damage;
                            // If an enemy dies to the attack, coins curst out
                            if(e.Health <= 0)
                            {
                                for (int j = 0; j < tempCoinPerEnemy; j++)
                                {
                                    enemies.Coinlist.Add(new Coin(new Rectangle(e.posX + e.Width / 2, e.posY, 20, 20),
                                        rng.Next(-40, 40), -(rng.Next(50)), enemies.CoinSprite));
                                }

                                e.EnemyState = EnemyState.dying;
                            }
                        }
                    }

                    // Accelerates vertical speed up to a maximum value
                    if(ySpeed <= 45)
                    {
                        ySpeed += 2;
                    }

                    // Changes accelaration based upon which arrow keys are being held down
                    if (KBnow.IsKeyDown(Keys.Left) && xSpeed >= -20)
                    {
                        xSpeed--;
                    }
                    else if (KBnow.IsKeyDown(Keys.Right) && xSpeed <= 20)
                    {
                        xSpeed++;
                    }
                    else if (xSpeed < 0)
                    {
                        xSpeed++;
                    }
                    else if(xSpeed > 0)
                    {
                        xSpeed--;
                    }

                    // Decids the next playerstate based upon the current keyboard input
                    if (KBnow.IsKeyDown(Keys.Down))
                    {
                        playerState = PlayerState.DownwardStrike;
                    }
                    else if(prevPlayerDirection == true)
                    {
                        playerState = PlayerState.JumpLeft;
                    }
                    else if(prevPlayerDirection == false)
                    {
                        playerState = PlayerState.JumpRight;
                    }

                    // When the player reachs the ground, they are reverted to the standing state
                    if (position.Y >= yBound - position.Height)
                    {
                        position.Y = yBound - position.Height;
                        if(prevPlayerDirection == true)
                        {
                            playerState = PlayerState.FacingLeft;
                        }
                        else
                        {
                            playerState = PlayerState.FacingRight;
                        }
                    }

                    break;
                #endregion

                // When the player's health is at 0 or less, they begin a death animation that ends when they are far below the game window,
                // setting thier state to dead and initiating the GameOver state of the Game1 class
                #region Dying
                case PlayerState.Dying:
                    // When the player dies they are flung into the air and begin to spin out
                    posY += ySpeed;

                    ySpeed += 2;
                    rot += 0.1f;

                    // Once they have fallen 10 times the height of the screen, the player is considered dead, and can no longer change state,
                    // ultimately triggering a Game Over
                    if (position.Y >= yBound * 10)
                    {
                        playerState = PlayerState.Dead;
                    }

                    break;
            #endregion
            }
#endregion

            // Checks enemy hitboxes.
            // If an enemy is colliding with the player, the player sustains one heart of damage
            foreach(Enemy e in enemies.EnemyList)
            {
                if (e.Position.Intersects(Position) && isHurt == false && e.EnemyState != EnemyState.spawn 
                    && e.EnemyState != EnemyState.dying)
                {
                    health -= e.Damage;
                    isHurt = true;
                }
            }
            // When the player health reaches 0, the player enters the dying state
            if(health <= 0 && playerState != PlayerState.Dying && playerState != PlayerState.Dead)
            {
                playerState = PlayerState.Dying;
                ySpeed = -40;
            }

            // Update bombs
            UpdateBombs(enemies);

            // When the timer is running, no bombs can be thrown. This prevents the player from throwing
            // many bombs at one time
            if (thrownBomb)
            {
                bombTimeCounter += gameTime.ElapsedGameTime.TotalSeconds;
                if (bombTimeCounter >= bombFrames)
                {
                    thrownBomb = false;
                    bombTimeCounter = 0;
                }
            }

            // Update on-hit particle effects
            particleManager.UpdateParticles();

            // Checks coin collisions
            foreach (Coin c in enemies.Coinlist.ToList<Coin>())
            {
                if (c.Position.Intersects(Position))
                {
                    coincount += 1;
                    enemies.Coinlist.Remove(c);
                }
            }

            // When the player is hurt, their size changes rapidly to simulate them blinking in and out of existence
            // While the timer is running, they are immune to any and all incoming damage and can still perform any other actions
            if (isHurt == true)
            {
                invincibilityTimeCounter += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (Math.Floor(invincibilityTimeCounter) == Math.Round(invincibilityTimeCounter))
                {
                    scl = 0;
                }
                else
                {
                    scl = 6;
                }

                if (invincibilityTimeCounter >= invincibilityFrames)
                {
                    invincibilityTimeCounter = 0;
                    isHurt = false;
                    scl = 6;
                }
            }

            // Sets the previous keyboard state and animates the player
            KBprev = KBnow;
        }
        #endregion

        #region Draw Player Method
        // Code to draw player based on current state
        // The code is fairly self-explanatory
        public void Draw(GameTime gameTime, SpriteBatch sb, Texture2D attack)
        {
            //float layerDepth = 0;

            switch (playerState)
            {
                case PlayerState.AttackR:
                    DrawNachoStandingAttack(SpriteEffects.None);
                    sb.Draw(attack, new Rectangle(AttackX + 170,AttackY,150,Height), Color.White);

                    break;
                case PlayerState.AttackL:
                    DrawNachoStandingAttack(SpriteEffects.FlipHorizontally);
                    sb.Draw(attack, new Rectangle(AttackX + 160, AttackY, 150, Height), Color.White);
                    break;

                case PlayerState.FacingLeft:

                    DrawNachoStanding(SpriteEffects.FlipHorizontally);
                    break;

                case PlayerState.FacingRight:

                    DrawNachoStanding(SpriteEffects.None);
                    break;

                case PlayerState.WalkLeft:

                    DrawNachoWalking(SpriteEffects.FlipHorizontally);

                    break;

                case PlayerState.WalkRight:

                    DrawNachoWalking(SpriteEffects.None);
                    break;

                case PlayerState.DuckLeft:
                    DrawNachoDuck(SpriteEffects.FlipHorizontally);
                    
                    break;

                case PlayerState.DuckRight:
                    DrawNachoDuck(SpriteEffects.None);
                    
                    break;

                case PlayerState.JumpLeft:
                    DrawNachoJumping(SpriteEffects.FlipHorizontally);
                    break;

                case PlayerState.JumpRight:
                    DrawNachoJumping(SpriteEffects.None);
                    break;

                case PlayerState.DuckAttackL:
                    DrawNachoDuckAttack(SpriteEffects.FlipHorizontally);

                    sb.Draw(attack,new Rectangle(posX- 120, posY + 25, 150, Height), Color.White);
                    break;

                case PlayerState.DuckAttackR:
                    DrawNachoDuckAttack(SpriteEffects.None);
                    sb.Draw(attack, new Rectangle(posX + Width + 120, posY + 25, 150, Height), Color.White);
                    break;

                case PlayerState.AerialAttackL:
                    DrawNachoJumpAttack(SpriteEffects.FlipHorizontally);
                    sb.Draw(attack, new Rectangle(posX - 250, posY, 250, Height), Color.White);

                    break;

                case PlayerState.AerialAttackR:
                    DrawNachoJumpAttack(SpriteEffects.None);
                    sb.Draw(attack, new Rectangle(posX + 400, posY, 250, Height), Color.White);

                    break;

                case PlayerState.DownwardStrike:
                    DrawNachoDownwardStrike(SpriteEffects.None);

                    break;

                case PlayerState.Dying:
                    DrawNachoJumping(SpriteEffects.FlipHorizontally);

                break;
            }

            // Draw the bombs
            DrawBombs();

            // Draw the on-hit particles to the screen
            particleManager.DrawParticles(sb);
        }
        #endregion

        #region Other Methods

        // Checks to see if the player is colliding with the left or right bounds of the map
        // If so, the X speed is set to 0 to simulate a collision with a solid object
        public void CheckBounds()
        {
            if (position.X <= 0)
            {
                position.X = 0;
                xSpeed = 0;
            }
            else if (position.X >= xBound - this.Width)
            {
                position.X = xBound - this.Width;
                xSpeed = 0;
            }
        }

        // Checks if the player is colliding with a platofrm and returns a bool
        public bool CheckPlatform(List<Platform> platformList)
        {
            foreach(Platform p in platformList)
            {
                if (PlatformCollider.Intersects(p.PlatformCollision))
                {
                    return true;
                }
            }

            return false;
        }

        // Updates all bombs on the screen
        public void UpdateBombs(EnemyManager enemies)
        {

            for(int i = 0; i < bombManager.Count; i++)
            {
                if(bombManager[i].posY >= posY && bombManager[i].YSpeed <= 0)
                {
                    bombManager[i].posY = posY;
                }

                // Updates the bombs from the method in the bomb class
                bombManager[i].UpdateBomb();

                // Checks if the bomb comes into contact with an enemy
                foreach (Enemy e in enemies.EnemyList)
                {
                    // When A bomb hits an enemy, it dies and creates an explosion hitbox that
                    // kills any enemy that are intersecting its radius
                    if (bombManager[i].Position.Intersects(e.Position) && e.EnemyState != EnemyState.dying)
                    {
                        // A massive explosion is spawned when a bombs makes contact
                        particleManager.ParticleList.Add(new Particle(e.posX - 450, e.posY - 600, true, 12));

                        // The camera violently shakes when a bomb hits
                        playerCamera.ShakeRadius = 35;
                        shakeFrames = .5;
                        isShaking = true;

                        // Each enemy is checked against the explosion hitbox. If they intersect,
                        // the enemy dies and coins burst out
                        foreach (Enemy n in enemies.EnemyList)
                        {
                            if(new Rectangle(bombManager[i].posX + bombManager[i].Width / 2 - 1000, 
                                bombManager[i].posY + bombManager[i].Height / 2 - 1000, 2000, 2000).Intersects(n.Position))
                            {
                                for (int j = 0; j < tempCoinPerEnemy; j++)
                                {
                                    enemies.Coinlist.Add(new Coin(new Rectangle(n.posX + n.Width / 2, n.posY, 20, 20),
                                        rng.Next(-40, 40), -(rng.Next(50)), enemies.CoinSprite));
                                }

                                n.EnemyState = EnemyState.dying;

                                bombManager[i].IsExploding = true;

                                n.XSpeed = 0;
                                n.YSpeed = -20;
                            }
                        }
                    }
                }

                // Causes the bomb to explode when it hits the ground
                if(bombManager[i].posY >= 800)
                {
                    // Creates an explosion hitbox around the area where the bomb hit the ground
                    Rectangle explosionHitBox = new Rectangle(bombManager[i].posX + bombManager[i].Width / 2 - 1000, bombManager[i].posY + bombManager[i].Height / 2 - 1000, 2000, 2000);

                    // Adds a massive explosion particle and shakes the camera violently
                    particleManager.ParticleList.Add(new Particle(bombManager[i].posX - 450, bombManager[i].posY - 650, true, 12));

                    playerCamera.ShakeRadius = 35;
                    shakeFrames = .5;
                    isShaking = true;

                    bombManager[i].IsExploding = true;

                    // All enemies in contact with the bomb explosion die and burst out coins upon death
                    foreach (Enemy e in enemies.EnemyList)
                    {
                        if (explosionHitBox.Intersects(e.Position))
                        {
                            for (int j = 0; j < tempCoinPerEnemy; j++)
                            {
                                enemies.Coinlist.Add(new Coin(new Rectangle(e.posX + e.Width / 2, e.posY, 20, 20),
                                    rng.Next(-40, 40), -(rng.Next(50)), enemies.CoinSprite));
                            }

                            e.EnemyState = EnemyState.dying;

                            e.XSpeed = 0;
                            e.YSpeed = -20;
                        }
                    }
                }

                // When the bomb propety isExploding is equal to true, they are removed from the list
                if(bombManager[i].IsExploding)
                {
                    bombManager.RemoveAt(i);
                    i--;
                }
            }
        }

        // Draws the bombs to the screen
        public void DrawBombs()
        {
            for (int i = 0; i < bombManager.Count; i++)
            {
                bombManager[i].DrawBomb(sb);
            }
        }

        // Animates the player animation
        public void NachoUpdateAnimation(GameTime gameTime)
        {
            nachoTimeCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if(nachoTimeCounter >= nachoTimePerFrame)
            {
                nachoFrame ++; //adjuts the frame

                //checks to see if it ever goes over the frame count of nacho
                if(nachoFrame > NachoWalkFrameCount || nachoFrame > NachoDuckFrameCount || nachoFrame > NachoJumpFrameCount || nachoFrame > NachoJumpAttackFrameCount ||
                    nachoFrame > NachoDownwardstrikeFrameCount || nachoFrame > NachoStandingAttackFrameCount)
                    nachoFrame = 1;
                

                nachoTimeCounter -= nachoTimePerFrame;
            }
        }

        // Draws the standing animation
        private void DrawNachoStanding(SpriteEffects flipSprite)
        {
            sb.Draw(
                sprite,
                new Vector2(this.posX, this.posY),
                new Rectangle(
                    0,
                    NachoWalkOffsetY,
                    NachoRectWidth,
                    NachoRectHeight),
                Color.White,
                0,
                Vector2.Zero,
                scl,
                flipSprite,
                0);
        }

        // Draws the walking animation
        private void DrawNachoWalking(SpriteEffects flipSprite)
        {
            sb.Draw(
                sprite,
                new Vector2(this.posX, this.posY),
                new Rectangle(
                    nachoFrame * 3,
                    NachoWalkOffsetY,
                    NachoRectWidth,
                    NachoRectHeight),
                Color.White,
                0,
                Vector2.Zero,
                scl,
                flipSprite,
                0);
        }

        // Draws the jumping animation
        private void DrawNachoJumping(SpriteEffects flipSprite)
        {
            sb.Draw(
                sprite,
                new Vector2(this.posX, this.posY),
                new Rectangle(
                    nachoFrame * 3,
                    NachoJumpOffsetY,
                    50,
                    NachoRectHeight),
                Color.White,
                0,
                Vector2.Zero,
                scl,
                flipSprite,
                0);
        }

        // Draws the attack animation
        private void DrawNachoStandingAttack(SpriteEffects flipSprite)
        {
            sb.Draw(
                sprite,
                new Vector2(this.posX, this.posY),
                new Rectangle(
                    nachoFrame * 8,
                NachoStandingAttackOffsetY,
                64,
                NachoRectHeight),
            Color.White,
            0,
            Vector2.Zero,
            scl,
            flipSprite,
            0);
        }

        // Draws the aerial attack animation
        private void DrawNachoJumpAttack(SpriteEffects flipSprite)
        {
            sb.Draw(
                sprite,
                new Vector2(this.posX, this.posY),
                new Rectangle(
                    nachoFrame * 12,
                    NachoJumpAttackOffsetY,
                    70,
                    70),
                Color.White,
                0,
                Vector2.Zero,
                scl,
                flipSprite,
                0);
        }

        // Draws the downward strike animation
        private void DrawNachoDownwardStrike(SpriteEffects flipSprite)
        {
            sb.Draw(
                sprite,
                new Vector2(this.posX, this.posY),
                new Rectangle(
                    nachoFrame * 6,
                    NachoDownwardStrikeOffsetY,
                    NachoRectWidth,
                    NachoRectHeight),
                Color.White,
                0,
                Vector2.Zero,
                scl,
                flipSprite,
                0);
        }

        // Draws the duck animation
        private void DrawNachoDuck(SpriteEffects flipSprite)
        {
            sb.Draw(
                sprite,
                new Vector2(this.posX, this.posY),
                new Rectangle(
                    nachoFrame * 3,
                    NachoDuckOffsetY,
                    NachoRectWidth,
                    64),
                Color.White,
                0,
                Vector2.Zero,
                scl,
                flipSprite,
                0);
        }

        // Draws the duck attack animation
        private void DrawNachoDuckAttack(SpriteEffects flipSprite)
        {
            sb.Draw(
                sprite,
                new Vector2(this.posX, this.posY),
                new Rectangle(
                    nachoFrame *6,
                    NachoDuckOffsetY,
                    60,
                    NachoRectHeight),
                Color.White,
                0,
                Vector2.Zero,
                scl,
                flipSprite,
                0);
        }
        #endregion
    }
}
