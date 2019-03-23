using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

namespace SlowCanterToTheRight
{
    #region Enum
    public enum GameState
    {
        Menu = 0,
        Game = 1,
        Pause = 2,
        GameOver = 3,
        Shop = 4,
        Win = 5
    }
#endregion
    /// <summary>
    /// This is the main type for your game. Handles the various game states and the sprites and objects
    /// the game uses throughout. Also loads and stores the assets in one place, allow them to interact
    /// and pass information to other classes and objects
    /// </summary>
    public class Game1 : Game
    {
        #region Fields
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Window sizes
        private int windowWidth;
        private int windowHeight;

        // States
        private GameState gameState;
        private Level waveManager;
        private Texture2D transitionRectangle;
        private int transitionAlpha;

        // Music Loops
        private SoundEffect menuSong;
        private SoundEffectInstance menuSongInstance;
        private SoundEffect gameSong;
        private SoundEffectInstance gameSongInstance;
        private SoundEffect Wah;
        private SoundEffectInstance WahInstance;
        private SoundEffect WahD;
        private SoundEffectInstance WahDInstance;
        private SoundEffect Will;
        private SoundEffectInstance WillInstance;


        // Screen Effects
        private Camera camera;
        private bool fadeIn;
        private bool fadeOut;
        private Texture2D whiteFlash;

        // Random Object
        private Random rng;

        // Keyboard
        private KeyboardState KBnow;
        private KeyboardState KBprev;

        // Menu Fields
        private Texture2D menuNacho;
        private Texture2D menuBG;
        private Texture2D menuDust;
        private Texture2D menuTitle;
        private Texture2D startButtonT;
        private Texture2D startButtonF;
        private Texture2D exitButtonT;
        private Texture2D exitButtonF;
        private int menuSelection;
        private float menuNachoYPos;
        private float menuNachoXpos;
        private bool nachoUp;
        private float menuDustY1;
        private float menuDustY2;
        private float menuButtonScale;
        private bool menuButtonGrow;


        // Game Fields
        private Texture2D bg;
        private Texture2D waveDirections;
        private Texture2D particleSpriteSheet;
        private Texture2D gameUI;
        private Texture2D bomb;
        private Texture2D gameDirections;
        private bool directions;

        // Game Over Fields
        private Texture2D gameOverScreen;
        private Texture2D gameOverFlash;
        private Texture2D gameOverHeads;
        private Texture2D gameOverNacho;
        private Texture2D gameOverMessage;
        private Texture2D gameOverEnter;

        // Win Fields
        private Texture2D winNewspaper;
        private float paperRot;
        private float paperScale;

        // Pause Fields
        private Texture2D pause;
        private Rectangle pausePosition;
        private bool pauseIn;
        private bool pauseOut;

        // Enemies Fields
        private Texture2D tempEnemy;
        private Texture2D flyingEnemy;
        private List<Texture2D> enemySprites;
        private EnemyManager enemyManager;
        private Texture2D boss;
        private Texture2D moneyBomb;
        private Texture2D smoke;

        // Shop Fields
        private Shop shop;
        private Texture2D shopBG;
        private Texture2D shopUI;
        private Texture2D shopMoneyCount;
        private Texture2D shopCursor;
        private Texture2D shopKeeperSprite;
        private Rectangle shopKeeperPos;

        // Fonts
        private SpriteFont UIFont;
        private SpriteFont shopFont20;
        private SpriteFont shopFont50;
        private SpriteFont shopFont72;

        // Player fields
        private Player nacho;
        private Texture2D trump;
        private Texture2D playerFullHeart;
        private Texture2D playerHalfHeart;
        private List<Rectangle> playerHealth;
        private Texture2D attack;
        private ParticleManager particleManager;
        private Texture2D nachoSpriteSheet;

        // Coins Fields
        List<Coin> coinlist;
        private Texture2D coinSpriteSheet;

        // Platform Fields
        private List<Platform> platformList;
        private Texture2D platform;

        // StreamReader
        private StreamReader input;

        Texture2D box;

        #endregion

        #region Constructor
        public Game1()
        {
            // Sets up the window size
            graphics = new GraphicsDeviceManager(this);
            windowWidth = 1600;
            windowHeight = 800;

            // Sets variables that use the set window size
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.ApplyChanges();

            // The Random object for the game
            rng = new Random();

            // Allows for fading transitions between states
            transitionAlpha = 255;
            fadeIn = true;
            fadeOut = false;

            // Sets up the main menu animations for when the game is started
            menuSelection = 0;
            nachoUp = false;
            menuNachoXpos = -windowWidth / 2;
            menuDustY1 = 0;
            menuDustY2 = 0 - windowHeight;
            menuButtonScale = 1.2f;
            menuButtonGrow = true;

            // Sets the pause state settings
            pausePosition = new Rectangle(-windowWidth, 0, windowWidth, windowHeight);
            pauseIn = true;
            pauseOut = false;

            Content.RootDirectory = "Content";
        }
        #endregion

        #region Initialize
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            gameState = GameState.Menu;

            // Adds the enemy sprites to a list to be passed into the manager
            enemySprites = new List<Texture2D>();

            // Makes a list for the player's heart display
            playerHealth = new List<Rectangle>();

            // Initializes the game camera
            camera = new Camera(windowWidth, windowHeight);

            base.Initialize();
        }
        #endregion

        #region LoadContent
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Loads the menu assets
            transitionRectangle = Content.Load<Texture2D>("SlowCanterTransition");
            menuNacho = Content.Load<Texture2D>("SlowCanterNachoPose");
            menuBG = Content.Load<Texture2D>("SlowCanterMenuBGWall");
            menuDust = Content.Load<Texture2D>("SlowCanterMenuDust");
            menuTitle = Content.Load<Texture2D>("SlowCanterMenuTitle");
            startButtonT = Content.Load<Texture2D>("SlowCanterStartButtonT");
            startButtonF = Content.Load<Texture2D>("SlowCanterStartButtonF");
            exitButtonT = Content.Load<Texture2D>("SlowCanterExitButtonT");
            exitButtonF = Content.Load<Texture2D>("SlowCanterExitButtonF");

            // Loads the game assets
            bg = Content.Load<Texture2D>("SlowCanterBG");
            particleSpriteSheet = Content.Load<Texture2D>("HitParticleText");
            gameOverScreen = Content.Load<Texture2D>("SlowCanterGameOverBG");
            gameOverHeads = Content.Load<Texture2D>("SlowCanterRichHeads");
            gameOverNacho = Content.Load<Texture2D>("SlowCanterNachoDefeated");
            gameOverEnter = Content.Load<Texture2D>("SlowCanterPressEnter");
            gameOverMessage = Content.Load<Texture2D>("SlowCanterGameOverMessage");
            gameOverFlash = Content.Load<Texture2D>("brickwall");
            trump = Content.Load<Texture2D>("NachoSpriteSheet");
            tempEnemy = Content.Load<Texture2D>("SlowCanterGroundEnemySpriteSheet");
            flyingEnemy = Content.Load<Texture2D>("SlowCanterFlyingEnemySpriteSheet");
            boss = Content.Load<Texture2D>("SlowCanterBossSpriteSheet");
            moneyBomb = Content.Load<Texture2D>("moneybag");
            pause = Content.Load<Texture2D>("SlowCanterPauseFull");
            attack = Content.Load<Texture2D>("brickwall");
            platform = Content.Load<Texture2D>("Shelf");
            coinSpriteSheet = Content.Load<Texture2D>("coinspritesheetfull");
            playerFullHeart = Content.Load<Texture2D>("heart");
            playerHalfHeart = Content.Load<Texture2D>("halfheart");
            shopBG = Content.Load<Texture2D>("ShopKeepSketch");
            shopUI = Content.Load<Texture2D>("SlowCanterShopUIBase");
            shopMoneyCount = Content.Load<Texture2D>("SlowCanterMoneyCount");
            shopCursor = Content.Load<Texture2D>("SlowCanterShopCursor");
            shopKeeperSprite = Content.Load<Texture2D>("ShopSprite");
            gameUI = Content.Load<Texture2D>("SlowCanterUI");
            bomb = Content.Load<Texture2D>("SlowCanterBomb");
            whiteFlash = Content.Load<Texture2D>("SlowCanterFlash");
            waveDirections = Content.Load<Texture2D>("SlowCanterPressC");
            smoke = Content.Load<Texture2D>("SCsmoke");
            winNewspaper = Content.Load<Texture2D>("SlowCanterWinNewspaper");
            gameDirections = Content.Load<Texture2D>("SlowCanterDirections");

            // Loads the fonts for the game
            UIFont = Content.Load<SpriteFont>("Temp");
            shopFont20 = Content.Load<SpriteFont>("ShopFont20");
            shopFont50 = Content.Load<SpriteFont>("ShopFont50");
            shopFont72 = Content.Load<SpriteFont>("ShopFont72");

            box = Content.Load<Texture2D>("blackbox");

            // Load the music loops
            menuSong = Content.Load<SoundEffect>("11 - Polo Coriano");
            gameSong = Content.Load<SoundEffect>("Libertango");

            // Makes the songs into controllable instances
            menuSongInstance = menuSong.CreateInstance();
            menuSongInstance.IsLooped = true;
            gameSongInstance = gameSong.CreateInstance();
            gameSongInstance.IsLooped = true;

            // Loads enemy sound effects
            Wah = Content.Load<SoundEffect>("wah");
            WahInstance = Wah.CreateInstance();
            WahInstance.Volume = .3F; // Flying Enemy attack volume

            WahD = Content.Load<SoundEffect>("wahD");
            WahDInstance = WahD.CreateInstance();
            WahDInstance.Volume = .2F; // flying enemy death volume

            Will = Content.Load<SoundEffect>("wariofall");
            WillInstance = Will.CreateInstance();
            WillInstance.Volume = .6F; // ground enemy death volume
            
            // Adds the enemy sprites to the list after the sprites have been loaded
            enemySprites.Add(tempEnemy);
            enemySprites.Add(flyingEnemy);
            enemySprites.Add(coinSpriteSheet);
        }
#endregion

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Nothing necessary here
        }

        #region Update
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            // Gets the keyboard input for the current frame
            KBnow = Keyboard.GetState();

            switch (gameState)
            {
                case GameState.Menu:
                    // Controls the fade in for the menu state
                    // Animates the character busting through the side of the screen
                    if (fadeIn == true)
                    {
                        if (transitionAlpha >= 100)
                        {
                            camera.ShakeCamera(new Vector2(0, windowHeight), rng);
                        }
                        else
                        {
                            camera.UpdateCamera(new Vector2(0, windowHeight));
                        }

                        if(menuNachoXpos <= 0 - 50)
                        {
                            menuNachoXpos += 50;
                        }
   
                        transitionAlpha -= 5;

                        // Once the fade is completed the menu song begins
                        if (transitionAlpha <= 0)
                        {
                            transitionAlpha = 0;
                            fadeIn = false;
                            menuSongInstance.Play();
                        }
                    }
                    else
                    {
                        // Makes sure that the camera does not move from the origin position
                        camera.UpdateCamera(new Vector2(0, windowHeight));
                    }

                    // When a fade out is initiated, the screen begins to go black
                    if (fadeOut == true)
                    {
                        transitionAlpha += 2;

                        // When the screen is completely black, the game moves into the game states and begins to play the game song
                        if (transitionAlpha >= 255)
                        {
                            fadeOut = false;
                            fadeIn = true;
                            menuSongInstance.Stop();
                            gameSongInstance.Play();
                            gameState = GameState.Pause;
                        }
                    }

                    // Controls the falling dust on the menu
                    menuDustY1 += .5f;
                    menuDustY2 += .5f;

                    // Makes sure that the falling dust loop once it reaches the edge of the screen
                    if(menuDustY1 >= windowHeight)
                    {
                        menuDustY1 = 0 - menuDust.Height;
                    }
                    if (menuDustY2 >= windowHeight)
                    {
                        menuDustY2 = 0 - menuDust.Height;
                    }

                    // Controls the animation for Nacho on the menu
                    if (nachoUp == false)
                    {
                        menuNachoYPos += .5f;
                        if(menuNachoYPos >= 10)
                        {
                            nachoUp = true;
                        }
                    }
                    else
                    {
                        menuNachoYPos -= .1f;
                        if(menuNachoYPos <= 0)
                        {
                            nachoUp = false;
                        }
                    }

                    // Controls animation for the menu buttons
                    if(menuButtonGrow == true)
                    {
                        menuButtonScale += 0.005f;
                        if(menuButtonScale >= 1.2)
                        {
                            menuButtonGrow = false;
                        }
                    }
                    else
                    {
                        menuButtonScale -= 0.005f;
                        if (menuButtonScale <= 1.0)
                        {
                            menuButtonGrow = true;
                        }
                    }

                    // Keyboard inputs for the menu
                    // Allows for the user to move up the selection
                    if(KBnow.IsKeyDown(Keys.Up) && KBprev != KBnow && fadeOut == false)
                    {
                        if(menuSelection == 0)
                        {
                            menuSelection = 1;
                        }
                        else
                        {
                            menuSelection = 0;
                        }
                    }
                    // Allows for the user to move down the selection
                    else if (KBnow.IsKeyDown(Keys.Down) && KBprev != KBnow && fadeOut == false)
                    {
                        if (menuSelection == 0)
                        {
                            menuSelection = 1;
                        }
                        else
                        {
                            menuSelection = 0;
                        }
                    }
                    // Handles setting up the neccessary components to start the game when
                    // the START button is pressed
                    else if (KBnow.IsKeyDown(Keys.Z) && KBprev != KBnow && menuSelection == 0)
                    {
                        directions = false;
                        
                        particleManager = new ParticleManager(particleSpriteSheet, gameTime);

                        // Gathers the information form the tool to create the Player object
                        InitializeNacho();

                        // Sets up the heart display
                        for (int x = 0; x < nacho.Health/2; x++)
                        {
                            playerHealth.Add(new Rectangle((x * playerFullHeart.Width) + (x*35), 0, 80, 80));
                        }

                        // This code involves creating a temporary Level object then replacing it with
                        // the real one when it is instantiated.
                        enemyManager = new EnemyManager(enemySprites, new Level(enemyManager, input, windowWidth, windowHeight, boss, rng, moneyBomb, particleManager, nacho, gameTime, spriteBatch), input, rng, WahInstance, WahDInstance, smoke, WillInstance);
                        waveManager = new Level(enemyManager, input, windowWidth, windowHeight, boss, rng, moneyBomb, particleManager, nacho, gameTime, spriteBatch);
                        enemyManager.SpawnManager = waveManager;

                        // Sets up the platforms on the map
                        platformList = new List<Platform>();
                        platformList.Add(new Platform(new Rectangle(windowWidth / 3, windowHeight / 2, windowWidth / 3, platform.Height), platform));
                        platformList.Add(new Platform(new Rectangle(windowWidth / 3 * 4, windowHeight / 2, windowWidth / 3, platform.Height), platform));

                        // Sets up the show and the position for the shop sprite
                        shop = new Shop(nacho, rng, shopFont72, shopFont50, shopFont20, shopCursor, shopMoneyCount, waveManager);
                        shopKeeperPos = new Rectangle(windowWidth - shopKeeperSprite.Width / 2, windowHeight,
                            shopKeeperSprite.Width, shopKeeperSprite.Height);

                        // Begins the fade out process
                        fadeIn = false;
                        fadeOut = true;
                    }
                    // When the EXIT button is pressed, the game is exited immediately
                    else if (KBnow.IsKeyDown(Keys.Z) && KBprev != KBnow && menuSelection == 1 && fadeOut == false)
                    {
                        Exit();
                    }

                break;

                case GameState.Game:
                    // Controls the fade in for the game state
                    if (fadeIn == true)
                    {
                        transitionAlpha -= 2;

                        if (transitionAlpha <= 0)
                        {
                            transitionAlpha = 0;
                            fadeIn = false;
                        }
                    }

                    // When the player enters the dead state or wins, the fadeout is initiated
                    if (fadeOut == true)
                    {
                        transitionAlpha++;

                        // Once screen is completely black, the game enters the GameOver or Win state,
                        // Depending upon whether or not the boss has been defeated
                        if (transitionAlpha >= 255)
                        {
                            fadeOut = false;
                            fadeIn = true;
                            if(waveManager.BossDefeated == true)
                            {
                                gameState = GameState.Win;
                                paperRot = 0;
                                paperScale = 0;

                                gameSongInstance.Stop();
                                menuSongInstance.Play();
                            }
                            else
                            {
                                gameState = GameState.GameOver;
                            }
                        }
                    }

                    // Updates the player
                    nacho.PlayerUpdate(gameTime, enemyManager, platformList);

                    // When the round is ongoing, enemy spawns are managed
                    if(waveManager.EndOfRound == false)
                    {
                        waveManager.UpdateSpawns(gameTime);
                    }
                    
                    // Updates all of the enemies currently in the EnemyManager
                    enemyManager.UpdateEnemies(gameTime, nacho);

                    // If the boss has been defeated, initiate a fade out
                    if(waveManager.BossDefeated == true)
                    {
                        fadeOut = true;
                    }

                    // When the round ends, the camera shakes and the shop comes out of the ground
                    if(waveManager.EndOfRound == true && shopKeeperPos.Y >= windowHeight - shopKeeperSprite.Height + 5)
                    {
                        camera.ShakeRadius = 8;
                        camera.ShakeCamera(new Vector2(nacho.posX + nacho.Width / 2, nacho.posY), rng);
                        shopKeeperPos.Y -= 3;
                    }
                    // Moves the shop back into the ground when the next wave begins
                    else if(waveManager.EndOfRound == false && shopKeeperPos.Y <= windowHeight)
                    {
                        camera.ShakeRadius = 8;
                        camera.ShakeCamera(new Vector2(nacho.posX + nacho.Width / 2, nacho.posY), rng);
                        shopKeeperPos.Y += 3;
                    }

                    // Pauses the game and plays the menu song
                    if (KBnow.IsKeyDown(Keys.Space) && KBprev != KBnow)
                    {
                        gameSongInstance.Stop();
                        menuSongInstance.Play();
                        gameState = GameState.Pause;
                    }

                    // Begins the next round during the end of wave phase
                    if (KBnow.IsKeyDown(Keys.C) && KBprev != KBnow && waveManager.EndOfRound == true)
                    {
                        waveManager.EndOfRound = false;
                    }

                    if (KBnow.IsKeyDown(Keys.P) && KBprev != KBnow && waveManager.EndOfRound == true)
                    {
                        waveManager.Wave = 15;
                    }

                    // When the player is in the dead phase, the screen begins the fade out process
                    // in order to enter the GameOver state
                    if (nacho.PlayerState == PlayerState.Dead && transitionAlpha <= 0)
                    {
                        fadeOut = true;
                    }

                    // Allows the player to enter the shop when they press the attack key while standing in front of it
                    if(shopKeeperPos.Intersects(nacho.Position))
                    {
                        if(KBnow.IsKeyDown(Keys.Z) && KBprev != KBnow && waveManager.EndOfRound == true && shopKeeperPos.Y <= windowHeight - shopKeeperSprite.Height + 5)
                        {
                            // Sets up the initial cursor position for the shop state
                            shop.ShopState = ShopMenuState.Options;
                            shop.Selection = 0;
                            shop.CursorY = 415;
                            gameState = GameState.Shop;
                        }
                    }
                    break;

                case GameState.Shop:
                    // Handles the updating if the shop switch statement
                    shop.UpdateShop();

                    // When the shop is left, it returns to the game state
                    if (shop.ShopState == ShopMenuState.Leave)
                    {
                        gameState = GameState.Game;
                    }
                    break;

                case GameState.Pause:
                    // Makes it so that the pause graphic moves in from the side of the screen
                    if (pauseIn)
                    {
                        pausePosition.X += 100;

                        if(pausePosition.X >= 0)
                        {
                            pauseIn = false;
                            pausePosition.X = 0;
                        }
                    }

                    // Gets rid of the directions during the first pause
                    if (KBnow.IsKeyDown(Keys.Enter) && KBprev != KBnow
                        && pauseIn == false && pauseOut == false)
                    {
                        directions = true;
                    }

                    // When the pause graphic is in place and SPACE is pressed, the graphic begins to move out
                    if (KBnow.IsKeyDown(Keys.Space) && KBprev != KBnow 
                        && pauseIn == false && pauseOut == false)
                    {
                        pauseOut = true;
                    }

                    // Once the pause graphic moves off the screen the game reumes and the game song begins playing
                    if (pauseOut)
                    {
                        pausePosition.X += 100;

                        if (pausePosition.X >= windowWidth)
                        {
                            pauseOut = false;
                            pauseIn = true;
                            pausePosition.X = -windowWidth;

                            menuSongInstance.Stop();
                            gameSongInstance.Play();

                            gameState = GameState.Game;
                        }
                    }

                    break;

                case GameState.GameOver:
                    transitionAlpha -= 3;

                    // Shakes the heads during the game over screen
                    camera.ShakeRadius = 1;
                    camera.ShakeCamera(new Vector2(0, windowHeight / 5), rng);

                    // Makes the game over directions change size for increased visibility
                    if (menuButtonGrow == true)
                    {
                        menuButtonScale += 0.005f;
                        if (menuButtonScale >= 1.2)
                        {
                            menuButtonGrow = false;
                        }
                    }
                    else
                    {
                        menuButtonScale -= 0.005f;
                        if (menuButtonScale <= 1.0)
                        {
                            menuButtonGrow = true;
                        }
                    }

                    // When ENTER is pressed, the game fades out and enters the menu state,
                    // so that the game can be exited or started again from the beginning
                    if (KBnow.IsKeyDown(Keys.Enter))
                    {
                        menuNachoXpos = -windowWidth / 2;
                        gameSongInstance.Stop();
                        gameState = GameState.Menu;
                        transitionAlpha = 255;

                        fadeIn = true;
                        fadeOut = false;
                    }
                    break;

                case GameState.Win:
                    transitionAlpha -= 3;
                    if(paperScale < 1)
                    {
                        paperScale += .01f;
                        paperRot += .5f;
                    }
                    else
                    {
                        paperRot = 0;
                    }

                    // Makes the game over directions change size for increased visibility
                    if (menuButtonGrow == true)
                    {
                        menuButtonScale += 0.005f;
                        if (menuButtonScale >= 1.2)
                        {
                            menuButtonGrow = false;
                        }
                    }
                    else
                    {
                        menuButtonScale -= 0.005f;
                        if (menuButtonScale <= 1.0)
                        {
                            menuButtonGrow = true;
                        }
                    }

                    // When ENTER is pressed, the game fades out and enters the menu state,
                    // so that the game can be exited or started again from the beginning
                    if (KBnow.IsKeyDown(Keys.Enter))
                    {
                        menuNachoXpos = -windowWidth / 2;
                        gameSongInstance.Stop();
                        gameState = GameState.Menu;
                        transitionAlpha = 255;

                        fadeIn = true;
                        fadeOut = false;
                    }

                    break;
            }

            // Sets a previous keyboard state to avoid repeated button presses
            KBprev = KBnow;

            base.Update(gameTime);
        }
        #endregion

        #region Draw
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Sets the base game to black rather than the gaudy cornflower blue
            GraphicsDevice.Clear(Color.Black);

            switch (gameState)
            {
                case GameState.Menu:
                    // Sets the spritebatch to use the game camera matrix to allow dynamic movement
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.ViewMatrix);

                    spriteBatch.Draw(transitionRectangle, new Vector2(0 - windowWidth / 2, 0 - windowHeight / 2),
                        new Rectangle(0, 0, transitionRectangle.Width, transitionRectangle.Height),
                        Color.White, 0, new Vector2(0, 0), 2.0f,
                        SpriteEffects.None, 1);

                    spriteBatch.Draw(menuBG, new Vector2(0, 0),
                        new Rectangle(0, 0, menuBG.Width, menuBG.Height),
                        Color.White, 0, new Vector2(0, 0), 1.0f,
                        SpriteEffects.None, 1);

                    spriteBatch.Draw(menuDust, new Vector2(0, menuDustY1),
                        new Rectangle(0, 0, menuDust.Width, menuDust.Height),
                        Color.White, 0, new Vector2(0, 0), 1.0f,
                        SpriteEffects.None, 1);

                    spriteBatch.Draw(menuDust, new Vector2(0, menuDustY2),
                        new Rectangle(0, 0, menuDust.Width, menuDust.Height),
                        Color.White, 0, new Vector2(0, 0), 1.0f,
                        SpriteEffects.None, 1);

                    spriteBatch.Draw(menuNacho, new Vector2(menuNachoXpos, menuNachoYPos),
                        new Rectangle(0, 0, menuNacho.Width, menuNacho.Height),
                        Color.White, 0, new Vector2(0, 0), 1.0f,
                        SpriteEffects.None, 1);

                    spriteBatch.Draw(menuTitle, new Vector2(windowWidth / 2, 0),
                        new Rectangle(0, 0, menuTitle.Width, menuTitle.Height),
                        Color.White, 0, new Vector2(0, 0), 1.0f,
                        SpriteEffects.None, 1);

                    // Allows for the menu buttons to change color and size when they are the current selected option
                    if(menuSelection == 0)
                    {
                        spriteBatch.Draw(startButtonT, new Vector2(970, 430),
                        new Rectangle(0, 0, startButtonT.Width, startButtonT.Height),
                        Color.White, 0, new Vector2(0, 0), menuButtonScale,
                        SpriteEffects.None, 1);
                    }
                    else
                    {
                        spriteBatch.Draw(startButtonF, new Vector2(970, 430),
                        new Rectangle(0, 0, startButtonF.Width, startButtonF.Height),
                        Color.White, 0, new Vector2(0, 0), 1.0f,
                        SpriteEffects.None, 1);
                    }

                    if(menuSelection == 1)
                    {
                        spriteBatch.Draw(exitButtonT, new Vector2(990, 600),
                        new Rectangle(0, 0, exitButtonT.Width, exitButtonT.Height),
                        Color.White, 0, new Vector2(0, 0), menuButtonScale,
                        SpriteEffects.None, 1);
                    }
                    else
                    {
                        spriteBatch.Draw(exitButtonF, new Vector2(990, 600),
                        new Rectangle(0, 0, exitButtonF.Width, exitButtonF.Height),
                        Color.White, 0, new Vector2(0, 0), 1.0f,
                        SpriteEffects.None, 1);
                    }

                    // The transition rectangle that allows for the fade in and out of the states
                    spriteBatch.Draw(transitionRectangle, new Vector2(0, 0),
                        new Rectangle(0, 0, transitionRectangle.Width, transitionRectangle.Height),
                        new Color(255, 255, 255, transitionAlpha), 0, new Vector2(0, 0), 2.0f,
                        SpriteEffects.None, 1);

                    break;

                case GameState.Game:
                    // Draw within the advanced Draw() for sprites affected by camera motion
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.ViewMatrix);

                    // The transition rectangle that allows for the fade in and out of the states
                    spriteBatch.Draw(transitionRectangle, new Vector2(0 - windowWidth / 2, 0 - windowHeight / 2),
                        new Rectangle(0, 0, transitionRectangle.Width, transitionRectangle.Height),
                        Color.White, 0, new Vector2(0, 0), 5.0f,
                        SpriteEffects.None, 1);

                    // Draws the platforms
                    spriteBatch.Draw(bg, new Rectangle(0, -windowHeight, windowWidth * 2, windowHeight * 2), Color.White);
                    foreach(Platform p in platformList)
                    {
                        p.Draw(spriteBatch);
                    }

                    // Draws the shop sprite in the game state
                    if(shopKeeperPos.Y < windowHeight)
                    {
                        spriteBatch.Draw(shopKeeperSprite, shopKeeperPos, Color.White);
                    }

                    // Draws all enemies
                    enemyManager.DrawEnemies(spriteBatch);

                    // Draws the player
                    nacho.Draw(gameTime, spriteBatch, attack);

                    // Draws the boss
                    waveManager.DrawFinalBoss();


                    //spriteBatch.Draw(box, new Rectangle(1350, 200, 350, 350), Color.White);

                    // Ends the spritebatch for sprites effected by the camera matrix
                    spriteBatch.End();

                    // Objects not affected by the camera use the simple Draw() (Not effected by the camera);
                    spriteBatch.Begin();

                    // Draws the health display for the player based upon current health
                    for (int i = 0; i < nacho.Health; i++)
                    {
                        // Draws the first row of hearts
                        if(i <= 13)
                        {
                            spriteBatch.Draw(playerFullHeart, new Rectangle(playerFullHeart.Width * i, 0, 
                                playerFullHeart.Width, playerFullHeart.Height), Color.White);
                        }
                        // Makes a second row of hearts if the player reaches over 14 hearts
                        else
                        {
                            spriteBatch.Draw(playerFullHeart, new Rectangle(playerFullHeart.Width * (i - 15), playerFullHeart.Height,
                                playerFullHeart.Width, playerFullHeart.Height), Color.White);
                        }
                    }

                    // When the wave is over, directions appear to tell the player how to advance the wave
                    if(waveManager.EndOfRound == true)
                    {
                        spriteBatch.Draw(waveDirections, new Vector2(windowWidth / 2 - waveDirections.Width / 2, 150),
                            new Rectangle(0, 0, waveDirections.Width, waveDirections.Height),
                            Color.White, 0, new Vector2(0, 0), 1.0f,
                            SpriteEffects.None, 1);
                    }
                    
                    // Draws the remaining GUI elements to the screen
                    spriteBatch.Draw(gameUI, new Rectangle(windowWidth - gameUI.Width, 0, gameUI.Width, gameUI.Height), Color.White);

                    // Draws the text to the GUI
                    spriteBatch.DrawString(UIFont, " " + waveManager.Wave, new Vector2(windowWidth - 85, 3), Color.White);
                    spriteBatch.DrawString(UIFont, " " + nacho.Coincount, new Vector2(windowWidth - 415, 3), Color.White);
                    spriteBatch.DrawString(UIFont, " " + nacho.NumBombs, new Vector2(windowWidth - 102, 65), Color.White);

                    // The transition rectangle that allows for the fade in and out of the states
                    spriteBatch.Draw(transitionRectangle, new Vector2(0, 0),
                        new Rectangle(0, 0, transitionRectangle.Width, transitionRectangle.Height),
                        new Color(255, 255, 255, transitionAlpha), 0, new Vector2(0, 0), 1.0f,
                        SpriteEffects.None, 1);

                    break;

                case GameState.Shop:
                    spriteBatch.Begin();

                    // Draws the sphopkeeper srite
                    spriteBatch.Draw(shopBG, new Rectangle(0, 0, windowWidth / 2, windowHeight), Color.White);

                    // Draws the shop UI boxes
                    spriteBatch.Draw(shopUI, new Rectangle(windowWidth / 2, 0, windowWidth / 2, windowHeight), Color.White);

                    // Uses the Shop object to draw the text, prices and option to the GUI
                    shop.DrawShop(spriteBatch, transitionRectangle);

                    break;

                case GameState.Pause:

                    // When the game is paused, the draw method acts excatly as it does during the game state, 
                    // except with the pause graphic in front

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.ViewMatrix);

                    spriteBatch.Draw(transitionRectangle, new Vector2(0 - windowWidth / 2, 0 - windowHeight / 2),
                        new Rectangle(0, 0, transitionRectangle.Width, transitionRectangle.Height),
                        Color.White, 0, new Vector2(0, 0), 5.0f,
                        SpriteEffects.None, 1);

                    spriteBatch.Draw(bg, new Rectangle(0, -windowHeight, windowWidth * 2, windowHeight * 2), Color.White);
                    foreach (Platform p in platformList)
                    {
                        p.Draw(spriteBatch);
                    }

                    enemyManager.DrawEnemies(spriteBatch);

                    spriteBatch.Draw(shopKeeperSprite, shopKeeperPos, Color.White);

                    nacho.Draw(gameTime, spriteBatch, attack);

                    waveManager.DrawFinalBoss();

                    spriteBatch.End();

                    spriteBatch.Begin();

                    for (int i = 0; i < nacho.Health; i++)
                    {
                        if (i <= 13)
                        {
                            spriteBatch.Draw(playerFullHeart, new Rectangle(playerFullHeart.Width * i, 0,
                                playerFullHeart.Width, playerFullHeart.Height), Color.White);
                        }
                        else
                        {
                            spriteBatch.Draw(playerFullHeart, new Rectangle(playerFullHeart.Width * (i - 15), playerFullHeart.Height,
                                playerFullHeart.Width, playerFullHeart.Height), Color.White);
                        }
                    }

                    spriteBatch.Draw(gameUI, new Rectangle(windowWidth - gameUI.Width, 0, gameUI.Width, gameUI.Height), Color.White);

                    spriteBatch.DrawString(UIFont, " " + waveManager.Wave, new Vector2(windowWidth - 85, 3), Color.White);
                    spriteBatch.DrawString(UIFont, " " + nacho.Coincount, new Vector2(windowWidth - 415, 3), Color.White);
                    spriteBatch.DrawString(UIFont, " " + nacho.NumBombs, new Vector2(windowWidth - 102, 65), Color.White);

                    spriteBatch.Draw(transitionRectangle, new Vector2(0, 0),
                        new Rectangle(0, 0, transitionRectangle.Width, transitionRectangle.Height),
                        new Color(255, 255, 255, transitionAlpha), 0, new Vector2(0, 0), 1.0f,
                        SpriteEffects.None, 1);

                    // Draws the pause graphic to the screen
                    spriteBatch.Draw(pause, pausePosition, Color.White);

                    // Draws the directions when the game begins
                    if (directions == false)
                    {
                        spriteBatch.Draw(gameDirections, new Rectangle(0, 0, gameDirections.Width, gameDirections.Height), Color.White);
                        spriteBatch.Draw(gameOverEnter, new Rectangle(windowWidth / 2 - gameOverEnter.Width / 2, windowHeight - gameOverEnter.Height,
                            gameOverEnter.Width, gameOverEnter.Height), Color.White);
                    }

                    break;

                case GameState.GameOver:
                    spriteBatch.Begin();

                    // Draws the game over graphic without the camera so that it is in the correct position
                    spriteBatch.Draw(gameOverScreen, new Rectangle(0, 0, windowWidth, windowHeight), Color.White);

                    spriteBatch.End();

                    // The camera allows for the game over screen to shake
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.ViewMatrix);

                    // The three heads for dramatic effect
                    spriteBatch.Draw(gameOverHeads, new Rectangle(windowWidth / 2 - gameOverHeads.Width / 2, 0,
                        gameOverHeads.Width, gameOverHeads.Height), Color.White);

                    spriteBatch.End();

                    // Every other graphic is drawn without the camera's translation matrix
                    spriteBatch.Begin();

                    spriteBatch.DrawString(shopFont72, "Wave " + waveManager.Wave, new Vector2(50, 620),
                        Color.White, 0.25f, new Vector2(0, 0), 1, SpriteEffects.None, 1);

                    spriteBatch.Draw(gameOverNacho, new Rectangle(windowWidth / 2 - gameOverNacho.Width / 2, windowHeight - gameOverNacho.Height,
                        gameOverNacho.Width, gameOverNacho.Height), Color.White);

                    spriteBatch.Draw(gameOverMessage, new Rectangle(0, 0,
                        gameOverMessage.Width, gameOverMessage.Height), Color.White);

                    // The game over text uses the button scale to change size for visibility
                    spriteBatch.Draw(gameOverEnter, new Vector2(windowWidth - gameOverEnter.Width, 
                        windowHeight - gameOverEnter.Height - 10),
                        new Rectangle(0, 0, gameOverEnter.Width, gameOverEnter.Height),
                        Color.White, 0, new Vector2(0, 0), menuButtonScale - .2f,
                        SpriteEffects.None, 1);

                    // The transition rectangle for fading to black
                    spriteBatch.Draw(transitionRectangle, new Vector2(0, 0),
                        new Rectangle(0, 0, transitionRectangle.Width, transitionRectangle.Height),
                        new Color(255, 255, 255, transitionAlpha), 0, new Vector2(0, 0), 1.0f,
                        SpriteEffects.None, 1);

                    break;

                case GameState.Win:

                    spriteBatch.Begin();

                    spriteBatch.Draw(gameOverScreen, new Rectangle(0, 0, windowWidth, windowHeight), Color.White);

                    // Draws the win over graphic without the camera so that it is in the correct position
                    spriteBatch.Draw(winNewspaper, new Vector2(0, 0),
                        new Rectangle(0, 0, winNewspaper.Width, winNewspaper.Height),
                        Color.White, paperRot, new Vector2(0, 0), paperScale,
                        SpriteEffects.None, 1);

                    // The win text uses the button scale to change size for visibility
                    spriteBatch.Draw(gameOverEnter, new Vector2(0,
                        windowHeight - gameOverEnter.Height - 10),
                        new Rectangle(0, 0, gameOverEnter.Width, gameOverEnter.Height),
                        Color.White, 0, new Vector2(0, 0), menuButtonScale - .2f,
                        SpriteEffects.None, 1);

                    // The transition rectangle for fading to black
                    spriteBatch.Draw(transitionRectangle, new Vector2(0, 0),
                        new Rectangle(0, 0, windowWidth, windowHeight),
                        new Color(255, 255, 255, transitionAlpha), 0, new Vector2(0, 0), 1.0f,
                        SpriteEffects.None, 1);

                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);

        }
        #endregion

        #region Text file reader
        public void InitializeNacho()
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
                    if (counter == 1)
                    {
                        int health = int.Parse(stats[0]);
                        int xSpeed = int.Parse(stats[1]);
                        int ySpeed = int.Parse(stats[2]);
                        int damage = int.Parse(stats[3]);

                        nacho = new Player(health, xSpeed, ySpeed, damage,
                            new Rectangle(windowHeight * 2 - trump.Width / 8, windowHeight - trump.Height / 2, trump.Width/2 - 150 , trump.Height/2),
                               trump, camera, rng, particleManager, bomb, spriteBatch);
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

        
        #endregion
    }
}