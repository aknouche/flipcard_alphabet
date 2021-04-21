using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatchingGame
{
    public partial class Form1 : Form
    {
        // firstClicked points to the first Label control 
        // that the player clicks, but it will be null 
        // if the player hasn't clicked a label yet
        Label firstClicked = null;

        // secondClicked points to the second Label control 
        // that the player clicks
        Label secondClicked = null;

        // Choose random letters for the squares
        Random random = new Random();

        //Alphabet letters
        List<string> letters = new List<string>()
    {
        "A", "A", "B", "B", "C", "C", "D", "D",
        "E", "E", "F", "F", "G", "G", "H", "H"
    };

        /// <summary>
        /// Assign each icon from the list of letters to a random square
        /// </summary>
        private void AssignLettersToSquares()
        {
            // The TableLayoutPanel has 16 labels,
            // and the alphabet list has 16 letters,
            // so a letter is pulled at random from the list
            // and added to each label
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label alphabetLabel = control as Label;
                if (alphabetLabel != null)
                {
                    int randomNumber = random.Next(letters.Count);
                    alphabetLabel.Text = letters[randomNumber];
                    alphabetLabel.ForeColor = alphabetLabel.BackColor;
                    letters.RemoveAt(randomNumber);
                }
            }
        }
        public Form1()
        {
            InitializeComponent();
            AssignLettersToSquares();
        }


        //Every label's Click event is handled by this event handler
        private void label_Click(object sender, EventArgs e)
        {
            //Ignore any clicks if the timer is running
            if (timer1.Enabled == true)
                return;

            Label clickedLabel = sender as Label;

            if (clickedLabel != null)
            {
                // If the clicked label is black, the player clicked
                // an icon that's already been revealed --
                // ignore the click
                if (clickedLabel.ForeColor == Color.Black)
                    return;

                // If firstClicked is null, this is the first letter
                // in the pair that the player clicked, 
                // so set firstClicked to the label that the player 
                // clicked, change its color to black, and return
                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.Black;
                    return;
                }

                // If the player gets this far, the timer isn't
                // running and firstClicked isn't null,
                // so this must be the second letter the player clicked
                // Set its color to black
                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;

                // Look to see if the player won
                LookForWinner();

                // If the player clicked two matching letters, keep them 
                // black and reset firstClicked and secondClicked 
                // so the player can click another letter
                if (firstClicked.Text == secondClicked.Text)
                {
                    firstClicked = null;
                    secondClicked = null;
                    return;
                }

                // If the player gets this far, the player 
                // clicked two different icons, so start the 
                // timer which will wait one second, and then hide the icons
                timer1.Start();
            }
        }

        /// This timer is started when the player clicks 
        /// two icons that don't match,
        /// so it counts one second 
        /// and then turns itself off and hides both icons
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            // Reset firstClicked and secondClicked so the next time a label is
            // clicked, the program knows it's the first click
            firstClicked = null;
            secondClicked = null;
        }

        /// <summary>
        /// Check every letter to see if it is matched, by 
        /// comparing its foreground color to its background color. 
        /// If all of the letters match the player wins
        /// </summary>
        private void LookForWinner()
        {
            // Go through all of the labels in the TableLayoutPanel, 
            // checking each one to see if its letters is matched
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                        return;
                }
            }


            // If the loop didn’t return, it didn't find
            // any unmatched letters which means the user won
            pictureBoxWin.Show();
            string message = "Grattis du vann! Avsluta spelet?";
            string title = "Spela igen";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
            else
            {
               
            }
        }
    }
}
