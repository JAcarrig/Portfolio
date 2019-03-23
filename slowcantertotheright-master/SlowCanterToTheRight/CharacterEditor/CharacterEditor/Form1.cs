using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//-------------------------------------------------------------------
//Class Author: Dylan Hollender

//Class Purpose: This class creates a windows forms application
//that reads and writes data to a text file which contains values
//for the characters of Slow Canter to the Right

//Class Restrictions: Currently the main restriction is that there
//is not any way to check if the entered values are numbers and not
//random strings. I am trying to find a way to resolve this issue.
//-------------------------------------------------------------------

namespace CharacterEditor
{
    public partial class CharacterEditorForm : Form
    {
        //File IO Fields
        private StreamWriter output;
        private StreamReader input;

        //Control Lists
        private List<TextBox> playerControls;
        private List<TextBox> enemyControls;
        private List<TextBox> flyingEnemyControls;
        private List<TextBox> bossControls;

        //-------------------------------------------------

        //Constructor
        public CharacterEditorForm()
        {
            //Form Component
            InitializeComponent();

            //File IO Fields
            input = null;
            output = null;

            //Control Lists
            playerControls = new List<TextBox>();
            enemyControls = new List<TextBox>();
            flyingEnemyControls = new List<TextBox>();
            bossControls = new List<TextBox>();

            //Initializes all lists
            InitializeLists();

            //Reads in all current values and prints them to the proper boxes
            ReadInfo();
        }

        //-------------------------------------------------

        //Methods

        /// <summary>
        /// Takes in all lists of stat textboxes and adds the
        /// proper textboxes to their respective list.
        /// </summary>
        public void InitializeLists()
        {
            //Player Stats
            playerControls.Add(textBoxPlayerHealth);
            playerControls.Add(textBoxPlayerXSpeed);
            playerControls.Add(textBoxPlayerYSpeed);
            playerControls.Add(textBoxPlayerDamage);

            //Enemy Stats
            enemyControls.Add(textBoxEnemyHealth);
            enemyControls.Add(textBoxEnemyXSpeed);
            enemyControls.Add(textBoxEnemyYSpeed);
            enemyControls.Add(textBoxEnemyDamage);

            //Flying Enemy Stats
            flyingEnemyControls.Add(textBoxFlyingEnemyHealth);
            flyingEnemyControls.Add(textBoxFlyingEnemyXSpeed);
            flyingEnemyControls.Add(textBoxFlyingEnemyYSpeed);
            flyingEnemyControls.Add(textBoxFlyingEnemyDamage);

            //Boss Stats
            bossControls.Add(textBoxBossHealth);
            bossControls.Add(textBoxBossXSpeed);
            bossControls.Add(textBoxBossYSpeed);
            bossControls.Add(textBoxBossDamage);
        }

        /// <summary>
        /// Checks to see if all textboxes contain a value. This way,
        /// data cannot be written to the text file if there are empty boxes.
        /// </summary>
        /// <returns>A boolean to see if every textbox has a value</returns>
        public bool ValueChecker()
        {
            //The boolean value that is used to decide if each box is full
            bool fullBoxes = false;

            //Checking player list
            for (int i = 0; i < playerControls.Count; i++)
            {
                if (playerControls[i].Text == "")
                {
                    return fullBoxes;
                }
            }

            //Checking enemy list
            for (int i = 0; i < enemyControls.Count; i++)
            {
                if (enemyControls[i].Text == "")
                {
                    return fullBoxes;
                }
            }

            //Checking flying enemy list
            for (int i = 0; i < flyingEnemyControls.Count; i++)
            {
                if (flyingEnemyControls[i].Text == "")
                {
                    return fullBoxes;
                }
            }

            //Checking boss list
            for (int i = 0; i < bossControls.Count; i++)
            {
                if (bossControls[i].Text == "")
                {
                    return fullBoxes;
                }
            }

            //If the lists have all passed the checks, that means
            //every box has a value in it. That means it is good 
            //to write to the file.
            fullBoxes = true;

            return fullBoxes;
        }

        /// <summary>
        /// Reads in the current stats from the text file.
        /// </summary>
        public void ReadInfo()
        {
            //Attempts to read in all values from the text file
            try
            {
                //Setting the Streamreader to the appropriate file
                input = new StreamReader("..\\..\\..\\..\\SlowCanterToTheRight/Content/CharacterStats.txt");

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
                        playerControls[0].Text = stats[0];
                        playerControls[1].Text = stats[1];
                        playerControls[2].Text = stats[2];
                        playerControls[3].Text = stats[3];
                    }
                    //The enemy controls
                    else if (counter == 2)
                    {
                        enemyControls[0].Text = stats[0];
                        enemyControls[1].Text = stats[1];
                        enemyControls[2].Text = stats[2];
                        enemyControls[3].Text = stats[3];
                    }
                    //The flying enemy controls
                    else if (counter == 3)
                    {
                        flyingEnemyControls[0].Text = stats[0];
                        flyingEnemyControls[1].Text = stats[1];
                        flyingEnemyControls[2].Text = stats[2];
                        flyingEnemyControls[3].Text = stats[3];
                    }
                    //The boss controls
                    else if (counter == 4)
                    {
                        bossControls[0].Text = stats[0];
                        bossControls[1].Text = stats[1];
                        bossControls[2].Text = stats[2];
                        bossControls[3].Text = stats[3];
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

        /// <summary>
        /// Saves the current stat values and writes them to the text file.
        /// </summary>
        public void WriteInfo()
        {
            //Attempts to write out all current textbox values
            try
            {
                //Setting the Streamreader to the appropriate file
                output = new StreamWriter("..\\..\\..\\..\\SlowCanterToTheRight/Content/CharacterStats.txt");

                //The player control values
                string line1 = playerControls[0].Text + "," + playerControls[1].Text + "," + playerControls[2].Text + "," + playerControls[3].Text;
                //The enemy control values
                string line2 = enemyControls[0].Text + "," + enemyControls[1].Text + "," + enemyControls[2].Text + "," + enemyControls[3].Text;
                //The flying enemy control values
                string line3 = flyingEnemyControls[0].Text + "," + flyingEnemyControls[1].Text + "," + flyingEnemyControls[2].Text + "," + flyingEnemyControls[3].Text;
                //The boss control values
                string line4 = bossControls[0].Text + "," + bossControls[1].Text + "," + bossControls[2].Text + "," + bossControls[3].Text;

                //Writes all lines
                output.WriteLine(line1);
                output.WriteLine(line2);
                output.WriteLine(line3);
                output.WriteLine(line4);

                //Saves by closing the file stream
                output.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        /// <summary>
        /// When the "Confirm Changes" button is clicked, a message box will appear
        /// to confirm the decision. If "yes" is clicked, then all the current stat 
        /// values are saved and written out to the text file by calling the
        /// WriteInfo method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonConfirm_Click(object sender, EventArgs e)
        {
            //Can only confirm changes if all boxes are filled
            if (ValueChecker() == true)
            {
                DialogResult confirmResult = MessageBox.Show("Do you wish to confirm changes?", "Save Status", MessageBoxButtons.YesNoCancel);

                if (confirmResult.ToString() == "Yes")
                {
                    WriteInfo();

                    MessageBox.Show("Changes saved.", "Save Status");
                }
            }
            else
            {
                MessageBox.Show("Please enter all values before saving.", "Invalid Input");
            }

            
        }
    }
}
