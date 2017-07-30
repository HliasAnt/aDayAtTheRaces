using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aDayAtTheRaces
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            SetStartingRadio(); //players infos
            UpdateTheRadios();  //info radios texts
            SetDogsValues();    //dogs infos

        }

        // Panel code

        Players[] RacePlayers = new Players[3]; //create the betplayers
        Bets[] MyBets = new Bets[3];            // create the bets of the players
        
        
        public void SetStartingRadio() // create players bets objects
        {
            RacePlayers[0] = new Players() { Name = "HLIAS", Cash = 100, MyRadioButton = radioHlias, MyTextBox = textBoxHlias };
            RacePlayers[1] = new Players() { Name = "XARA", Cash = 85, MyRadioButton = radioXara,MyTextBox=textBoxXara };
            RacePlayers[2] = new Players() { Name = "HRA", Cash = 75, MyRadioButton = radioHra,MyTextBox=textBoxHra };
            MyBets[0] = new Bets();
            MyBets[1] = new Bets();
            MyBets[2] = new Bets();
        }
        public void UpdateTheRadios()  // update the radios text
        {
            RacePlayers[0].UpdateRadio();
            RacePlayers[1].UpdateRadio();
            RacePlayers[2].UpdateRadio();
            
        }

        private void radioHlias_CheckedChanged(object sender, EventArgs e)
        {
            if (radioHlias.Enabled==true)
            {
                BetPlayerName.Text = RacePlayers[0].Name; //update the player bet label depends the selected radio
            }
        }

        private void radioXara_CheckedChanged(object sender, EventArgs e)
        {
            if (radioXara.Enabled == true)
            {
                BetPlayerName.Text = RacePlayers[1].Name;
            }
        }

        private void radioHra_CheckedChanged(object sender, EventArgs e)
        {
            if (radioHra.Enabled == true)
            {
                BetPlayerName.Text = RacePlayers[2].Name;
            }
        }

        private void BetButton_Click(object sender, EventArgs e) // place the bet and inform textbox
        {
            for (int i = 0; i < 3; i++)
            {

                if (BetPlayerName.Text == RacePlayers[i].Name)
                {
                    RacePlayers[i].MyTextBox.Text = BetPlayerName.Text + " has bet " + numericUpDownBets.Value + " on " +
                        numericUpDownDog.Value + " Dog";
                    MyBets[i].StoreBetAmount = (int)numericUpDownBets.Value; 
                    MyBets[i].StoreDogChoice = (int)numericUpDownDog.Value;
                }

               
                
            }

        }


        //Dogs code

        Dogs[] Mydog = new Dogs[4]; // create the 4dogs object
        Random MyRandomizer = new Random(); // random value of dogs movement
        

        public void SetDogsValues()
        {

            Mydog[0] = new Dogs()
            {
                MyPictureBox = pictureBox2,
                StartingPotition = pictureBox2.Left,
                RacetrackLength = pictureBox1.Width - pictureBox2.Width, Randomizer = MyRandomizer
            };
            Mydog[1] = new Dogs()
            {
                MyPictureBox = pictureBox3,
                StartingPotition = pictureBox3.Left,
                RacetrackLength = pictureBox1.Width - pictureBox3.Width,Randomizer=MyRandomizer
                

            };
            Mydog[2] = new Dogs()
            {
                MyPictureBox = pictureBox4,
                StartingPotition = pictureBox4.Left,
                RacetrackLength = pictureBox1.Width - pictureBox4.Width,Randomizer=MyRandomizer
                

            };
            Mydog[3] = new Dogs()
            {
                MyPictureBox = pictureBox5,
                StartingPotition = pictureBox5.Left,
                RacetrackLength = pictureBox1.Width - pictureBox5.Width,Randomizer=MyRandomizer
                
            };
        }  // set the 4 dogs infos

        private void timer1_Tick(object sender, EventArgs e) //timer for calling the run method when the start button is pressed
        {

            for (int i = 0; i < 4; i++)
            {
                if (Mydog[i].Run()==true)
                {
                    Mydog[i].Run();
                    
                }
                else
                {
                    timer1.Stop();
                    MessageBox.Show("Dog number #" + i.ToString() + " Won");
                    int Winner = i;
                    for (int j = 0; j < 3; j++)
                    {
                        RacePlayers[j].Cash = MyBets[j].WinnerPayBack(RacePlayers[j].Cash, MyBets[j].StoreBetAmount, Winner);
                    }
                    UpdateTheRadios();
                    for (int k = 0; k <4; k++)
                    {
                        Mydog[k].TakeStartingPotition();
                    }
                    break;
                }
                
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
           // SetDogsValues();
            timer1.Start();
        }
    }


    public class Players
    {
        public string Name;
        public int Cash;
        public RadioButton MyRadioButton;
        public Label MyLabel;
        public TextBox MyTextBox;


        public void  UpdateRadio()
        {
            MyRadioButton.Text = Name + " has " + Cash + "$";
          
        }

    }

    public class Dogs
    {
        public int StartingPotition;
        public int RacetrackLength;
        public PictureBox MyPictureBox = null;
        public int location = 0;
        public Random Randomizer;

        public bool Run()
        {
            
            location = Randomizer.Next(10, 30);
            MyPictureBox.Left += location;
            if (MyPictureBox.Left<=RacetrackLength)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public void TakeStartingPotition()
        {
            MyPictureBox.Left = StartingPotition;
        }
    }

    public class Bets
    {
        
        public int StoreBetAmount;
        public int StoreDogChoice;
        public int WinnerPayBack(int Bcash,int BetAmount,int DogWinner)
        {
            if (DogWinner==StoreDogChoice)
            {
                Bcash += (BetAmount * 2);
                return Bcash;
            }
            else
            {
                Bcash -= BetAmount;
                return Bcash;
            }
            
        }
       


    }
}
