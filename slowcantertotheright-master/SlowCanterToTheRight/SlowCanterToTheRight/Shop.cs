using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SlowCanterToTheRight
{
    // Shop State Enums
    enum ShopMenuState 
    {
        Options = 0,
        Buy = 1,
        Leave = 2
    }

    // The shop class control the visual GUI for the shop and allows for the changing of stats thorugh purchase
    class Shop 
    {
        #region Fields
        private ShopMenuState shopState;
        private string shopKeepText; // text displayed in shop window
        private int menuSelection; // determines which menu item is selected
        private int shopSelection; // selectied item in "buy" state
        // graphical fade effect values
        private int transitionAlpha;
        private bool fadeIn;
        private bool fadeOut;
        
        private Player nacho;
        private Random rng;
        // shop fonts
        private SpriteFont largeFont;
        private SpriteFont mediumFont;
        private SpriteFont smallFont;

        private Texture2D cursor; // option selection arrow sprite
        private Texture2D moneyCounter;
        private float cursorX; //position of player's arrow
        private float cursorY; //--

        private KeyboardState KBnow; // current keyboard data
        private KeyboardState KBprev; // last recorded keyboard data

        private Level waveManager;
        // price values for all items in shop
        private int healthPrice;
        private int weaponPrice;
        private int bombPrice;
        #endregion

        #region Properties
        public Rectangle CursorPosition
        {
            get { return new Rectangle((int)cursorX, (int)cursorY, cursor.Width, cursor.Height); }
        }

        public ShopMenuState ShopState
        {
            get { return shopState; }
            set { shopState = value; }
        }

        public int Selection
        {
            set { menuSelection = value; }
        }

        public float CursorY
        {
            set { cursorY = value; }
        }

        private int HealthPrice
        {
            get { return 50 * waveManager.Wave; }
        }

        private int BombPrice
        {
            get { return 25 * waveManager.Wave; }
        }
#endregion

        #region Constructor
        public Shop(Player player, Random rng, SpriteFont largeFont, SpriteFont mediumFont, SpriteFont smallFont, Texture2D cursor, Texture2D moneyCounter, Level waveManager)
        {
            nacho = player;
            this.rng = rng;

            transitionAlpha = 255;
            fadeIn = true;
            fadeOut = false;

            this.largeFont = largeFont;
            this.mediumFont = mediumFont;
            this.smallFont = smallFont;
            this.cursor = cursor;
            this.moneyCounter = moneyCounter;

            cursorX = 1050;
            cursorY = 480;

            shopState = ShopMenuState.Options;
            WelcomeTextGenerator();

            this.waveManager = waveManager;

            healthPrice = HealthPrice;
            weaponPrice = 500;
            bombPrice = BombPrice;

            menuSelection = 0;
            shopSelection = 0;
        }
        #endregion

        #region Methods
        public void UpdateShop()
        {
            KeyboardState KBnow = Keyboard.GetState();

            switch (shopState)
            {
                case ShopMenuState.Options:
                    if(fadeIn == true) // plays fade in effects
                    {
                        transitionAlpha -= 5;
                        if (transitionAlpha <= 0)
                        {
                            transitionAlpha = 0;
                            fadeIn = false;
                        }
                    }

                    if(fadeOut == true) // plays fade out effects
                    {
                        transitionAlpha += 3;
                        if(transitionAlpha >= 255)
                        {
                            transitionAlpha = 255;
                            fadeIn = true;
                            fadeOut = false;

                            shopState = ShopMenuState.Leave;
                        }
                    }

                    // moves cursor down & changes selection
                    if(KBnow.IsKeyDown(Keys.Down) && KBprev != KBnow && 
                        fadeIn == false && fadeOut == false)
                    {
                        cursorY += 95;
                        menuSelection++;
                    }

                    // moves the cursor up & changes selection
                    if (KBnow.IsKeyDown(Keys.Up) && KBprev != KBnow &&
                        fadeIn == false && fadeOut == false)
                    {
                        cursorY -= 95;
                        menuSelection--;
                    }

                    // facilitates screen wrapping of cursor
                    if (menuSelection < 0)
                    {
                        menuSelection = 2;
                        cursorY += 285;
                    }
                    // facilitates screen wrapping of cursor
                    if (menuSelection > 2)
                    {
                        menuSelection = 0;
                        cursorY -= 285;
                    }

                    //moves to shop screen (where player can actually buy items)
                    if (KBnow.IsKeyDown(Keys.Z) && KBprev != KBnow && menuSelection == 0
                        && transitionAlpha == 0)
                    {
                        BuyTextGenerator();
                        shopState = ShopMenuState.Buy;
                        shopSelection = 0;
                        cursorX = 850;
                        cursorY = 405;
                    }
                    // prints out random text from Talulah's Diolauge (below)
                    else if (KBnow.IsKeyDown(Keys.Z) && KBprev != KBnow && menuSelection == 1 
                        && transitionAlpha == 0)
                    {
                        TalkTextGenerator();
                    }
                    // exits the shop
                    else if(KBnow.IsKeyDown(Keys.Z) && KBprev != KBnow && menuSelection == 2
                        && transitionAlpha == 0)
                    {
                        fadeIn = false;
                        fadeOut = true;
                    }

                    break;

                    // moves cursor and displays text for selected item
                case ShopMenuState.Buy:
                    if (KBnow.IsKeyDown(Keys.Down) && KBprev != KBnow)
                    {
                        cursorY += 65;
                        shopSelection++;
                        BuyTextGenerator();
                    }

                    if (KBnow.IsKeyDown(Keys.Up) && KBprev != KBnow)
                    {
                        cursorY -= 65;
                        shopSelection--;
                        BuyTextGenerator();
                    }

                    if (shopSelection < 0)
                    {
                        shopSelection = 3;
                        cursorY += 255;
                        BuyTextGenerator();
                    }

                    if (shopSelection > 3)
                    {
                        shopSelection = 0;
                        cursorY -= 255;
                        BuyTextGenerator();
                    }

                    // buying health
                    if (KBnow.IsKeyDown(Keys.Z) && KBprev != KBnow && shopSelection == 0)
                    {
                        if(nacho.Coincount >= HealthPrice && nacho.Health <= 28) // subtracts coins equal to health price and adds one health
                        {
                            nacho.Coincount -= HealthPrice;

                            nacho.Health += 1;
                        }
                        // When the maximum health has been reached, health cannot be bought
                        else if(nacho.Coincount >= HealthPrice)
                        {
                            shopKeepText = "You're a healthy man Nacho." +
                                "\nEat a doughnut once in a while.";
                        }
                        else // runs if player cannot afford health
                        {
                            BrokeTextGenerator();
                        }
                    }
                    // buying damage upgrade
                    else if (KBnow.IsKeyDown(Keys.Z) && KBprev != KBnow && shopSelection == 1)
                    {

                        if (nacho.Coincount >= weaponPrice) // subtracts coins equal to damage price and increases damage
                        {
                            nacho.Coincount -= weaponPrice;

                            nacho.Damage += 10;
                            weaponPrice += 500;
                        }
                        else // runs if player cannot afford upgrade
                        {
                            BrokeTextGenerator();
                        }
                    }
                    // buying bombs
                    else if (KBnow.IsKeyDown(Keys.Z) && KBprev != KBnow && shopSelection == 2)
                    {
                        if (nacho.Coincount >= BombPrice) // subtracts coins equal to bomb price and adds bomb
                        {
                            nacho.Coincount -= BombPrice;

                            nacho.NumBombs++;
                        }
                        else // runs if player cannot afford bomb
                        {
                            BrokeTextGenerator();
                        }
                    }
                    // moves player back to main shop screen
                    else if (KBnow.IsKeyDown(Keys.Z) && KBprev != KBnow && shopSelection == 3)
                    {
                        WelcomeTextGenerator();
                        shopState = ShopMenuState.Options;
                        menuSelection = 0;
                        cursorX = 1050;
                        cursorY = 415;
                    }

                    break;

                case ShopMenuState.Leave:
                    // When the shop menu state enters the leave state, 
                    // the gameState changes back to the Game state

                    break;
            }

            KBprev = KBnow;
        }
        #endregion

        #region Draw Method
        public void DrawShop(SpriteBatch sb, Texture2D transitionRectangle)
        {
            switch (shopState)
            {
                // main shop screen
                case ShopMenuState.Options:
                    sb.DrawString(smallFont, shopKeepText, new Vector2(830, 25), Color.White); // draws specified string of text (changes based on cursor position)

                    sb.Draw(cursor, CursorPosition, Color.White); // draws player cursor
                    sb.DrawString(largeFont, "Buy \nTalk \nLeave", new Vector2(1175, 430), Color.White); // draws text options

                    sb.Draw(moneyCounter, new Rectangle(0, 800 - moneyCounter.Height / 2, moneyCounter.Width - 50, moneyCounter.Height / 2), Color.White);
                    sb.DrawString(mediumFont, "$" + nacho.Coincount, new Vector2(20, 820 - moneyCounter.Height / 2), Color.White); // draws player's money value

                    sb.Draw(transitionRectangle, new Vector2(0, 0),
                        new Rectangle(0, 0, transitionRectangle.Width, transitionRectangle.Height),
                        new Color(255, 255, 255, transitionAlpha), 0, new Vector2(0, 0), 1.0f,
                        SpriteEffects.None, 1); // fade transition rectangle

                    break;

                    // "buy" state
                case ShopMenuState.Buy:
                    sb.DrawString(smallFont, shopKeepText, new Vector2(830, 25), Color.White); // draws specified string of text (changes based on cursor position

                    sb.Draw(cursor, CursorPosition, Color.White); // draws player cursor
                    sb.DrawString(mediumFont, "Health - $" + HealthPrice +  // displays shop items and prices
                        "\nWeapon - $" + weaponPrice +
                        "\nBomb - $" + BombPrice + 
                        "\nBack", new Vector2(975, 430), Color.White);

                    sb.Draw(moneyCounter, new Rectangle(0, 800 - moneyCounter.Height / 2, moneyCounter.Width - 50, moneyCounter.Height / 2), Color.White);
                    sb.DrawString(mediumFont, "$" + nacho.Coincount, new Vector2(20, 820 - moneyCounter.Height / 2), Color.White); // draws player's money value

                    break;

                case ShopMenuState.Leave:
                    // The leave state technically doesn't get the chance to be drawn,
                    // but I'll leave this here for now cause I feel like it
                    // maybe draw in jack black for a frame

                    break;
            }
        }
#endregion

        #region Talulah's Wacky Diolouge
        // umbrella for all of Talulah's shop diolauge

        public void WelcomeTextGenerator() // runs when the player enters shop main screen
        {
            int randomWelcomeText = rng.Next(2);

            switch(randomWelcomeText)
            {
                case 0:
                    shopKeepText = "Welcome to my shack, Nacho. I got stuff " +
                        "\nyou might like, weapon upgrades, more " +
                        "\nhealth, and bombs.";
                        
                    break;

                case 1:
                    shopKeepText = "Yo, Nacho, what's crack-a-lackin'! " +
                        "\nYou got the lettuce, I got the " +
                        "\nproduct. By which I mean weapon " +
                        "\nupgrades and health, not drugs.";
                    break;
            }
        }

        public void BuyTextGenerator() // runs when player is in "buy" shop state
        {
            switch (shopSelection)
            {
                case 0: // health
                    shopKeepText = "Increasing your mental health might" +
                        "\nbe more worth my time, but we" +
                        "\ndon't really have time for that.";
                    break;

                case 1: // damage
                    shopKeepText = "I tape barbed wire to your mallet for" +
                        "\n+1 perception.";
                    break;

                case 2: // bombs
                    shopKeepText = "The use of the atomic bomb, with " +
                        "\nits indiscriminate killing of women " +
                        "\nand children, revolts my soul. These " +
                        "\nrich asshats on the other hand...";
                    break;

                case 3: // Just Monika
                    shopKeepText = "I had another really bad dream. " +
                        "\nIt seems to happen whenever you " +
                        "\nquit the game... So if you could " +
                        "\ntry to avoid doing that, I would " +
                        "\nbe really grateful.";
                    break;
            }
        }

        public void BrokeTextGenerator() // runs when player doesent have enough money for a purchase
        {
            int randomTalkText = rng.Next(5);

            switch (randomTalkText)
            {
                case 0:
                    shopKeepText = "What!? You're flat broke!";
                    break;

                case 1:
                    shopKeepText = "You realize I need money to" +
                        "\nupgrade your whatchamadingies, right?";
                    break;

                case 2:
                    shopKeepText = "Uh maybe my price wasn't" +
                        "\nclear enough, cause you can't afford" +
                        "\nthis...";
                    break;

                case 3:
                    shopKeepText = "Put it on your tab? Sure yeah " +
                        "\nI can put it on your tab, oh " +
                        "\nwait no I frickin' can't I'm " +
                        "\nnot made of money this ain't a " +
                        "\ncharity Nacho my boy.";
                    break;

                case 4:
                    shopKeepText = "WHEN WILL YOU LEARN! WHEN " +
                        "\nWILL YOU LEARN THAT YOUR " +
                        "\nUPGRADES COST MONEY!!!";
                    break;
            }
        }

        public void TalkTextGenerator() // random bits of text displayed when the talk button is pressed
        {
            int randomTalkText = rng.Next(14);

            switch (randomTalkText)
            {
                case 0:
                    shopKeepText = "Remember to save your game! " +
                        "\nYou never know when something " +
                        "\nimportant might happen.";
                    break;

                case 1:
                    shopKeepText = "*door is violently broken down* " +
                        "\n\"FBI OPEN UP!\"";
                    break;

                case 2:
                    shopKeepText = "Nematodes are people too.";
                    break;

                case 3:
                    shopKeepText = "It's not like I wanted you " +
                        "\nto visit the shop " +
                        "\nor anything... Baka!";
                    break;

                case 4:
                    shopKeepText = "Nacho, when you are a man, " +
                        "\nsometimes you wear stretchy " +
                        "\npants in your room...";
                    break;

                case 5:
                    shopKeepText = "I am the gatekeeper of my " +
                        "\nown destiny and I will have my " +
                        "\nglory day in the hot sun.";
                    break;

                case 6:
                    shopKeepText = "Talulah's Polo Tip of the Day! " +
                        "\nWhen you try something for " +
                        "\nthe first time, you're probably " +
                        "\ngoing to suck at it. But maybe " +
                        "\nafter a few weeks you come " +
                        "\nback to it, and you realize " +
                        "\nyou were never really good at all.";
                    break;

                case 7:
                    shopKeepText = "...Are you trying to fast-forward? " +
                        "\nI'm not boring you, am I?";
                    break;

                case 8:
                    shopKeepText = "Wanna hear about my Sonic OC? " +
                        "\nHer name is Mystical the Koala " +
                        "\nand she is a translucent camoflauge " +
                        "\nhalf-demon from the Chemical Plant " +
                        "\nZone Act 2. She graduated top of " +
                        "\nher class at the Sonic Fighting " +
                        "\nAcademy and...";
                    break;

                case 9:
                    shopKeepText = "The mitochondria is the " +
                        "\npowerhouse of the cell.";
                    break;

                case 10:
                    shopKeepText = "*Stares in Japanese*";
                    break;

                case 11:
                    shopKeepText = "Whoever coded this line of " +
                        "\ntext must be the greatest " +
                        "\nhacker of all time...";
                    break;

                case 12:
                    shopKeepText = "Be kind whenever possible. " +
                        "\nIt is always possible. " +
                        "\nExcept here. " +
                        "\nKnock their teeth out for me.";
                    break;

                case 13:
                    shopKeepText = "My dad always said that " +
                        "\nwhen you have nothing nice " +
                        "\nto say, don't say anything at all. " +
                        "\nBut I don't have a dad " +
                        "\nso you're a weenie!";
                    break;
            }
        }
#endregion
    }
}
