using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleGame
{
    public partial class Form1 : Form
    {

        #region Properties
        /// <summary>
        /// Purpose : Property List
        /// </summary>
        /// lights , lightsMatrix : Array of lights
        /// MB_OK : Message for Game Status
        public Button[,] lights = new Button[5, 5];

        public bool[,] lightsMatrix = new bool[5, 5];

        public MessageBoxButtons MB_OK { get; private set; }
        #endregion

        #region Constructor
        public Form1()
        {
            // Code generated from Visual Studio for the form
            InitializeComponent();
            // Turn on a random number of lights to start game
            randomStart();
        }
        #endregion

        #region randomStart
        /// <summary>
        /// Purpose : Turn on a random number of lights to start game
        /// </summary>
        public void randomStart()
        {
            // Loop through all lights to generate the buttons on form
            for (int i = 0; i < lights.GetLength(1); i++)
            {
                for (int j = 0; j < lights.GetLength(0); j++)
                {
                    lights[i, j] = new Button();
                    lights[i, j].Size = new Size(40, 40);
                    // Name of button object is index in 3D array
                    lights[i, j].Name = i.ToString() + j.ToString();
                    lights[i, j].Click += light_Click;
                    lights[i, j].Location = new Point(30 + (j * 60), 20 + (i * 60));
                    lights[i, j].BackColor = Color.Black;
                    lightsMatrix[i, j] = false;
                    this.Controls.Add(lights[i, j]);
                }
            }

            Random rnd = new Random();
            // Random number of lights to turn on between 1 and 10
            for (int i = 0; i < rnd.Next(1, 10); i++)
            {
                // Get random indexes
                int x = rnd.Next(0, lights.GetLength(1));
                int y = rnd.Next(0, lights.GetLength(0));
                // Call invertButton function with indexes
                invertButton(lights[x, y], x, y);
            }

            // Special case check
            // If lights have been toggled and no lights remain on
            // Set one random light to be on
            if (checkStatus() == true)
            {
                // Get random indexes
                int x = rnd.Next(0, lights.GetLength(1));
                int y = rnd.Next(0, lights.GetLength(0));
                // Call invertButton function with indexes
                invertButton(lights[x, y], x, y);
            }
        }
        #endregion

        #region LightButtonClick
        /// <summary>
        /// Purpose : Event for Button Select/Deselect 
        /// </summary>
        public void light_Click(object sender, EventArgs e)
        {
            Button b = sender as Button; //Clicked object is a Button
            // Get the index of the button clicked
            int i = (int)Char.GetNumericValue(b.Name[0]);
            int j = (int)Char.GetNumericValue(b.Name[1]);

            // Invert button clicked
            invertHandler(lights[i, j], i, j);

            // Check if game has ended
            checkEnd();
        }
        #endregion

        #region InvertHandler
        /// <summary>
        /// Purpose : Event for Handler - Set buttons on adjecent sides
        /// which calls invertButton that sets Light On/Off
        /// </summary>
        public void invertHandler(object sender, int i, int j)
        {
            // Invert button clicked
            invertButton(lights[i, j], i, j);

            // Invert the correct buttons around the clicked one
            if (i > 0)
            {
                invertButton(lights[i - 1, j], i - 1, j); //Above
            }
            if (i < (lights.GetLength(1) - 1))
            {
                invertButton(lights[i + 1, j], i + 1, j); //Below
            }
            if (j > 0)
            {
                invertButton(lights[i, j - 1], i, j - 1); //Left
            }
            if (j < (lights.GetLength(1) - 1))
            {
                invertButton(lights[i, j + 1], i, j + 1); //Right
            }

        }
        #endregion
        #region InvertButton
        /// <summary>
        /// Purpose : Event to check and set Light On/Off (set background color based on status) 
        /// </summary>
        public void invertButton(object sender, int i, int j)
        {
            Button b = sender as Button; 

            // Invert the boolean value for the button
            lightsMatrix[i, j] = !lightsMatrix[i, j];

            // If light is turned on
            if (lightsMatrix[i, j] == true)
            {
                // Set colour of matched button to LightGreen
                b.BackColor = Color.LightGreen;
            }
            else
            {
                // Set colour of matched button to DarkGreen
                b.BackColor = Color.DarkGreen;
            }

        }
        #endregion

        #region checkStatus
        /// <summary>
        /// Purpose : Event to Check Status of Game through state of Light  
        /// </summary>
        public bool checkStatus()
        {
            // Loop through all bools in bool array
            for (int i = 0; i < lightsMatrix.GetLength(1); i++)
            {
                for (int j = 0; j < lightsMatrix.GetLength(0); j++)
                {
                    // Check if light is on
                    if (lightsMatrix[i, j] == true)
                    {
                        // If any light is on game is still running
                        return false;
                    }
                }
            }

            // If all lights have been checked then they are all off
            return true;
        }
        #endregion
        #region checkEnd
        /// <summary>
        /// Purpose : Event to Check Game Completed or not 
        /// </summary>
        public void checkEnd()
        {
            // Check if all lights are off
            if (checkStatus() == true)
            {
                // Display message to user
                MessageBox.Show("Game Completed!",
                    "Congratulations!",
                     MB_OK);
                // Close Application
                Application.Exit();
            }
        }
        #endregion




    }
}
