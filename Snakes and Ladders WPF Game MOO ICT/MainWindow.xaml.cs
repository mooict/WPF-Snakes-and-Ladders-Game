using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Threading; // import the threading name space for the timer

namespace Snakes_and_Ladders_WPF_Game_MOO_ICT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    // Tutorial Created by Moo ICT 2020
    // find the images @ mooict.com and search for snakes and ladders game

    public partial class MainWindow : Window
    {
        Rectangle landingRec; // landing rec will help identify the rectangles on the board

        Rectangle player; // player rectangle
        Rectangle opponent; // opponent rectangle

        List<Rectangle> Moves = new List<Rectangle>(); // list of rectangles to store the board pieces into

        DispatcherTimer gameTimer = new DispatcherTimer(); // new instance of the dispatcher timer called game timer

        ImageBrush playerImage = new ImageBrush(); // image brush to import the player GIF image and attach to the player rectangle
        ImageBrush opponentImage = new ImageBrush(); // image brush to import the opponent GIF image and attach to the opponent rectangle

        // int I and J will be used for the player and the opponent
        // they will help on where the player and opponent are on the board
        // default value when the game loads is set up -1 for both
        int i = -1;
        int j = -1;

        // position and current position integer for the player
        int position;
        int currentPosition;

        // position and current position for the opponent
        int opponentPosition;
        int opponentCurrentPosition;

        // this images integer will be used to show the board images when we create them
        int images = -1;

        // new random class instance called rand will be used to calculate the dice rolls in the game
        Random rand = new Random();

        // two Boolean which will detemrine whos turn it is in the game
        bool playerOneRound, playerTwoRound;

        // this integer will show the current position of the player and the opponent to the GUI
        int tempPos;


        public MainWindow()
        {
            InitializeComponent();

            SetupGame(); // run the set up game function from inside this constructor
        }

        private void OnClickEvent(object sender, MouseButtonEventArgs e)
        {
            // this on click event is linked to the Canvas, so player is able to click anywhere on the canvas to play

            // below is the if statement thats checking if the player 1 and 2 boolean are set to false first
            // if they are then we can do the following inside of the if statement
            if (playerOneRound == false && playerTwoRound == false)
            {
                position = rand.Next(1, 7); // generate a random number for the player
                txtPlayer.Content = "You Rolled a " + position; // show that number to the txt player label
                currentPosition = 0; // set current position to 0

                //in the if statement below we check if the i integer thats the player current position in the game
                // if less than or equals to 99 if so then we can do the follow

                if ((i + position) <= 99)
                {
                    
                    playerOneRound = true; // in side of this if statement, change the player one round to true
                    gameTimer.Start(); // start the game timer
                }
                else
                {
                    // if the above statement is false the do the following
                    if (playerTwoRound == false)
                    {
                        // check if the player two round Boolean is false
                        playerTwoRound = true; // if it false then change it to true
                        opponentPosition = rand.Next(1, 7); // generate a random number for the opponent
                        txtOpponent.Content = "Opponent Rolled a " + opponentPosition; // show that number rolled to the txt opponent label
                        opponentCurrentPosition = 0; // set opponent current position to 0
                        gameTimer.Start(); // start the game timer
                    }
                    else
                    {
                        // if the player two round is already true then
                        gameTimer.Stop(); // stop the timer
                        // change both boolean back to false
                        playerOneRound = false;
                        playerTwoRound = false;
                    }
                }


            }
        }

        private void SetupGame()
        {

            // this is the set up game function. In this function we will set up the game board, the player and the opponent

            // in order to create the board we will need to make 3 local variables below
            int leftPos = 10; // left pos will help us position the boxes from right to left 
            int topPos = 600; // top pos will help us position the boxes from bottom to top
            int a = 0; // a integer will help us to lay 10 boxes in a row

            // the two lines below are importing the images for the player and the opponent and attaching them to the image brush we created earlier
            playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/player.gif"));
            opponentImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/opponent.gif"));


            // this is the main for loop where we will make the game board
            // this loop will run a 100 times inside of this function
            // it will run like this because we need 100 tiles for this game to work
            for (int i = 0; i < 100; i++)
            {
                // first we increment the images integer we created in the program before
                images++;
                // create a new image brush called tile images, this will attach an image to the rectangles for the board
                ImageBrush tileImages = new ImageBrush();

                // import the board rectangle images inside of the tile images
                // inside the new uri you can see that we are adding the images integer there too, this is because we have images names from 0.jpg to 99.jpg
                // so as the loop will increament this images integer will increment too and we will be able to capture all of the images for the board
                tileImages.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/" + images + ".jpg"));

                // below we are creating a new rectangle called box
                // this rectangle will have 60x60 height and width, fill is the tile images and a black border around it
                Rectangle box = new Rectangle
                {
                    Height = 60,
                    Width = 60, 
                    Fill = tileImages,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };

                // we need to indentify the rectangle created in this loop, so we will give each box a unique name
                box.Name = "box" + i.ToString(); // name the boxes 
                this.RegisterName(box.Name, box); // register the name inside of the WPF app

                Moves.Add(box); // add the newly created box to the moves rectangles list

                // below we are making the algorithm we need to lay the boxes 10 in a row
                // we will make the boxes from left to right then move up and reverse that process
                // remember "a" integer is controlling how we position the boxes down so we need to keep in mind on it can be controlled inside of this loop

                // if a is equals to 10
                if (a == 10)
                {
                    // this will happen when we have positioned 10 boxes from left to right
                    topPos -= 60; // in that case reduce 60 from the top pos integer 
                    a = 30; // change the value of a to 30, we are doing this to move the boxes from right to left now
                }

                // if a is equals to 20
                if (a == 20)
                {
                    
                    topPos -= 60; // again reduce 60 from the top pos integer
                    a = 0; // set a integer back to 0
                }

                // if a is greater than 20
                if (a > 20)
                {
                    // if the value of a is greater than 20 then we can
                    // this if statement will help us position the boxes from right to left
                    a--; // reduce 1 from a each loop
                    Canvas.SetLeft(box, leftPos); // set the box inside the canvas by the value of the left pos integer
                    leftPos -= 60; // reduce 60 from the left pos each loop
                }

                // if a is less than 10
                if (a < 10)
                {
                    // this will happen when we want to position the boxes from left to right
                    //if the value of a is less than 10 
                    a++; // add 1 to a integer each loop
                    Canvas.SetLeft(box, leftPos); // set the box left position to the value of left pos
                    leftPos += 60; // add 60 to the left pos integer 
                    Canvas.SetLeft(box, leftPos); // set the box left position to the value of the left pos integer
                }

                Canvas.SetTop(box, topPos); //set the box top position to the value of top pos integer each loop

                MyCanvas.Children.Add(box); // finally add the box to the canvas display

                // end the loop
            }

            // out the main board loop we can now set up the timer

            gameTimer.Tick += GameTimerEvent; // link the Game Timer Event to the timer tick
            gameTimer.Interval = TimeSpan.FromSeconds(.2); // this timer will tick every .2 seconds

            // set up the player rectangle
            // the player rectangle will have 30x30 height and width, it have the player image as its fill and have a border 2 pixels
            player = new Rectangle
            {
                Height = 30, 
                Width = 30,
                Fill = playerImage,
                StrokeThickness = 2
            };
            // set up the opponent rectangle the same way as the player
            opponent = new Rectangle
            {
                Height = 30,
                Width = 30, 
                Fill = opponentImage,
                StrokeThickness = 2
            };

            // add both player and the opponent to the canvas
            MyCanvas.Children.Add(player);
            MyCanvas.Children.Add(opponent);

            // run the move piece function and reference the player and opponent inside of it
            // also reference where we want the player and the opponent to be positioned beginning of the game
            MovePiece(player, "box" + 0);
            MovePiece(opponent, "box" + 0);



        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            // this is the game timer event this event will move the player and the opponent on the board

            // in the if statement below we are first checking if the player one round is true and the player two round is false
            if (playerOneRound == true && playerTwoRound == false)
            {
                // if this condition is true then we will do the following

                // check if i is less than the total number of board pieces inside of the moves list
                if (i < Moves.Count)
                {
                    // if yes now check if the current position is less than the position that we generated with the random class
                    if (currentPosition < position)
                    {
                        // if so, now we add 1 to the current position with each tick
                        currentPosition++;
                        i++;// add 1 to the i integer with each tick
                        MovePiece(player, "box" + i); // update the player position using the move piece function
                    }
                    else
                    {
                        // if the player one round is set to false then do the following
                        playerTwoRound = true; // set player two round to true
                        // now run the i which is the player position through the check snakes and ladders function
                        i = CheckSnakesOrLadders(i);
                        // update the player position on the move piece function
                        MovePiece(player, "box" + i);

                        // now we have ended the player round we need to set up the CPU to make its own
                        opponentPosition = rand.Next(1, 7); // generate a random number for the cpu
                        txtOpponent.Content = "Opponent Rolled a " + opponentPosition; // show the random number on the txt opponent label 
                        opponentCurrentPosition = 0; // set opponent current position to 0
                        tempPos = i; // now we will pass the value of i inside of the temp integer
                        txtPlayerPosition.Content = "Player is @ " + (tempPos + 1); // show the player current position on the player position label
                        // the board we are generating in the game will generate a board piece from 0 - 99 so we will add 1 to the temp pos intger to show the currect info on where the player is on the board
                    }
                }
                // this if statement below will check if the player has made to top of the board
                if (i == 99)
                {
                    // if so, stop the game time, show a message on the screen and when player clicks ok restart the game
                    gameTimer.Stop();
                    MessageBox.Show("Game Over!, You Win" + Environment.NewLine + "Click Ok to Play Again" , "Moo Says");
                    RestartGame();
                }
            } // player if statement ends here

            // this section below is for the CPU, this will only run when the player two round is set to true

            if (playerTwoRound == true)
            {
                // same as before we check if the CPU position is less than the board numbers
                if (j < Moves.Count)
                {
                    // if yes we are checking if the current position of the opponent is less than the generated position
                    // and we are checking is the CPU has more moves ahead of it, this way we can stop the cpu from making last minutes moves and allow the player to move after its turn
                    if (opponentCurrentPosition < opponentPosition && (j + opponentPosition < 101))
                    {

                        opponentCurrentPosition++; // increase the current position of the opponent
                        j++; // increase the actual position of the opponent
                        MovePiece(opponent, "box" + j); // show the movements through the move piece function
                    }
                    else
                    {
                        // if the cpu has taken its turn then we do the following
                        // set both player one and two rounds to false
                        playerOneRound = false;
                        playerTwoRound = false;
                        // check CPU position with the snakes and ladders function
                        j = CheckSnakesOrLadders(j);
                        MovePiece(opponent, "box" + j);
                        // set tempos to the opponent position and show it on display
                        tempPos = j;
                        txtOpponentPosition.Content = "Opponent is @ " + (tempPos + 1);
                        // stop the game timer
                        gameTimer.Stop();
                    }
                }

                // if the opponent has reached 99 end of its turn then we will end the game
                if (j == 99)
                {
                    // stop the game timer, show a message box and when the player clicks oK, restart the game
                    gameTimer.Stop();
                    MessageBox.Show("Game Over!, Opponent Wins" + Environment.NewLine + "Click Ok to Play Again", "Moo Says");
                    RestartGame();
                }


            } // opponent if statement ends here
        }

        private void RestartGame()
        {
            // this is the restart game function, it will set everything back to default when it runs

            // if I and J back to -1 and set the player and opponent the 0 position on the board
            i = -1;
            j = -1;
            MovePiece(player, "box" + 0);
            MovePiece(opponent, "box" + 0);

            // set player position and current position to 0
            position = 0;
            currentPosition = 0;

            // set opponent position and current position to 0
            opponentPosition = 0;
            opponentCurrentPosition = 0;

            // set player one and player two rounds to false
            playerOneRound = false;
            playerTwoRound = false;

            // set the player and opponent labels back to their default content
            txtPlayer.Content = "You Rolled a " + position;
            txtPlayerPosition.Content = "Player is @ 1";

            txtOpponent.Content = "Opponent Rolled a " + opponentPosition;
            txtOpponentPosition.Content = "Opponent is @ 1";

            // stop the timer
            gameTimer.Stop();



        }

        private int CheckSnakesOrLadders(int num)
        {
            // this is the check snakes or ladders function. The purpose of this function is to check if thep player has
            // landed on the bottom of a ladder or top of the snake

            // this function will return an integer when it runs this is why we have linked it to the player and opponentment movements

            // there are several if statements inside of this function and you can see if the number that is being passed inside of this function
            // matches any of the if conditions then it will change the number to that final number and return it back to the program

            // this way we can simply check if the player has landed on a ladder then move it up to where the ladder ends
            // and if it landed on snake then we can move it down to where the snake ends. 

            // This function is designed for the board we are using in this tutorial, if you have another board you are you may need to change these numbers to better suit your board


            if (num == 1)
            {
                num = 37;
            }

            if (num == 6)
            {
                num = 13;
            }

            if (num == 7)
            {
                num = 30;
            }

            if (num == 14)
            {
                num = 25;
            }

            if (num == 15)
            {
                num = 5;
            }
            if (num == 20)
            {
                num = 41;
            }
            if (num == 27)
            {
                num = 83;
            }
            if (num == 35)
            {
                num = 43;
            }
            if (num == 45)
            {
                num = 24;
            }
            if (num == 48)
            {
                num = 10;
            }
            if (num == 50)
            {
                num = 66;
            }
            if (num == 61)
            {
                num = 18;
            }
            if (num == 63)
            {
                num = 59;
            }
            if (num == 70)
            {
                num = 90;
            }
            if (num == 73)
            {
                num = 52;
            }
            if (num == 77)
            {
                num = 97;
            }
            if (num == 86)
            {
                num = 93;
            }
            if (num == 88)
            {
                num = 67;
            }
            if (num == 91)
            {
                num = 87;
            }
            if (num == 94)
            {
                num = 74;
            }
            if (num == 98)
            {
                num = 79;
            }


           
            return num;
        }

        private void MovePiece(Rectangle player, string posName)
        {

            // this function will move the player and the opponent across the board
            // the way it does it is very simply, we have added of the board rectangles to the moves list 
            // from the for each loop below we can loop through all of the rectangles from that list
            // we are also checking if any of the rectangle has the posName, if they do then we will link the landing rect to that rectangle found inside of the for each loop
            // this way we can move the rectangle that is being passed inside of this function and run in the timer event to animate it when it starts

            foreach (Rectangle rectangle in Moves)
            {
                if (rectangle.Name == posName)
                {
                    landingRec = rectangle;
                }
            }

            // the two lines here will place the "player" object that is being passed in this function to the landingRec location
            Canvas.SetLeft(player, Canvas.GetLeft(landingRec) + player.Width / 2);
            Canvas.SetTop(player, Canvas.GetTop(landingRec) + player.Height / 2);
        }
    }
}
