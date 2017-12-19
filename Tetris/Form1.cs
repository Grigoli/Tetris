using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Tetris
{
    
    /// <summary>
    /// Game class
    /// </summary>
    public partial class Form1 : Form
    {

        /// <summary>
        /// Store highest score
        /// </summary>
        int highestScoreRecord = 0;

        /// <summary>
        /// Score count
        /// </summary>
        int Scores = 0;

        /// <summary>
        /// Line count
        /// </summary>
        int Lines = 0;

        /// <summary>
        /// Speed control
        /// </summary>
        int c = 0;
        
        /// <summary>
        /// Level count
        /// </summary>
        int Level = 1;

        string player;


        /// <summary>
        /// Counts bonus
        /// </summary>
        int bonus = 0;

        /// <summary>
        /// Speed
        /// </summary>
        int speed = 1000;

        /// <summary>
        /// Player Name
        /// </summary>
        string Name;

        /// <summary>
        /// list of high scores
        /// </summary>
        List<HighScore> highScores = new List<HighScore>()
        {
            new HighScore { PlayerName = "Williams", Score = 1000 },
            new HighScore { PlayerName = "George", Score = 1500 },
            new HighScore { PlayerName = "Amanda", Score = 4000 },
            new HighScore { PlayerName = "Alexsandra", Score = 5000 }
        };



        /// <summary>
        /// Stopwatch
        /// </summary
        Stopwatch stopWatch = new Stopwatch();

        /// <summary>
        /// Board size initialization
        /// </summary>
        int[,] Board = new int[10, 22]; // Board size 10 X 22
        
        /// <summary>
        /// box size
        /// </summary>
        public int boxSize = 20;

        /// <summary>
        /// Initialize location
        /// </summary>
        Point location = new Point();

        /// <summary>
        /// Tetromino
        /// </summary>
        Tetromino tetromino;
        List<Point> pL = new List<Point>();
       
        
        /// <summary>
        /// Initializes components and starts stopwatch add new player to the score list
        /// </summary>
        public Form1( string playerName)
        {
            InitializeComponent();
            
            highScores.Insert(4, new HighScore { PlayerName=playerName, Score=Scores});
            player = playerName;
            
            this.Text = playerName + " - " + Scores;
            stopWatch.Start();
        }




        /// <summary>
        /// Check if a tetromino can move
        /// Returns true if tetrimino's next step doesn't exceed board height
        /// </summary>
        /// <returns></returns>
        public bool moving()
        {
            foreach (Point point in pL)
            {
                foreach (Point P1 in tetromino.figureOnBoard)
                {
                    if (point.Equals(new Point(P1.X, P1.Y + tetromino.boxSize)))
                        return false;
                }
            }


            return tetromino.Ground.Y + tetromino.boxSize < pictureBox1.Height;
        }

        

        /// <summary>
        /// Timer 1 that controles game 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void timer1_Tick(object sender, EventArgs e)
        {

            if (moving())
            {
                tetromino.move();
                // move tetromino down
                pictureBox1.Invalidate();
            }
            else
            {
                
                reachedCeiling();  // check if tetrominos filled the board

                foreach (Point p in tetromino.figureOnBoard)
                {
                    pL.Add(p);
                    Board[p.X / boxSize, p.Y / boxSize] = 1;
                }

                check(); // check line if filled

                if (c > 0)
                {
                    if (c % 10 == 0)
                    {
                        
                        speed = (int)((double)speed * 0.75); // After each 10 cleared row speed up by 25%

                        timer1.Interval = speed; // speed control
                        c = 0;
                        Level++; // increase levels
                        level.Text = "" + Level; // display level on board
                    }

                }

                randomTetromino(); // generate random tetromino
                pictureBox1.Invalidate();
            }
        }

        /// <summary>
        /// Check if line is filled
        /// </summary>
        private void check()
        {
            int i;

            int j;
            for (i = 0; i < Board.GetLength(1); i++)
            {
                bool isDel = true;
                for (j = 0; j < Board.GetLength(0); j++)
                {
                    if (Board[j, i] == 0)
                    {
                        isDel = false;
                        break;
                    }
                }
                if (isDel)
                {
                    c++;
                    Lines++;
                    if (Lines > 1)
                    {
                        bonus = 50;// add 50 point bonus if lines cleared in a row
                    }
                    Scores += 100 * Level + bonus;  // add score

                    
                    int k;
                    for (j = 0; j < Board.GetLength(0); j++)
                    {
                        Board[j, i] = 0;
                        for (k = i; k > 0; k--)
                        {
                            Board[j, k] = Board[j, k - 1];
                        }
                    }

                    pL.Clear();

                    for (k = 0; k < Board.GetLength(0); k++)
                    {
                        for (j = 0; j < Board.GetLength(1); j++)
                        {
                            if (Board[k, j] != 0)
                            {
                                pL.Add(new Point(k * 20, j * 20));
                            }
                        }
                    }

                    line.Text = "" + Lines; // update lines cound

                    this.Text = "Your Points: " + Scores; // update score

                    labeldisp.Text = "" + Scores; // dysplay score on board

                }
                else
                {
                    bonus = 0;
                }

            }

        }


        /// <summary>
        /// Hichscore leaderboard list Icomperable interface Implementer
        /// </summary>
        [Serializable]
        public class HighScore : IComparable 
        {
            public string PlayerName { get; set; }

            public int Score { get; set; }

            public int CompareTo(object obj)
            {
                var otherScore = (HighScore)obj;
                if (Score == otherScore.Score)
                    return 0;

                if (Score < otherScore.Score)
                    return 1;

                return -1;
            }
        }

        /// <summary>
        /// Check if tetromonos on top each other reached the board ceiling
        /// If YES than Game Over
        /// </summary>
        public void reachedCeiling()
        {
            if (tetromino.location.Y < 1)
            {
                pictureBox1.Invalidate();

                timer1.Enabled = false;

                if (Scores >= highestScoreRecord) highestScoreRecord = Scores;

                // game finished write scores in file
                using (var fileStream = new FileStream(@"scores.txt", FileMode.Append, FileAccess.Write))
                {

                    var formatter = new BinaryFormatter();
                    formatter.Serialize(fileStream, highScores);
                }


                var newGame = new Menu(); // open menu
                newGame.Show();
                stopWatch.Stop();
                this.Close();
                MessageBox.Show("Game Over!"); // display message game over
                
            }
        }







        /// <summary>
        /// Colors locked tetromono on the board
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            tetromino.figureTrace(e.Graphics);
            colorWhenOnBottom(e.Graphics);
        }





        /// <summary>
        /// Color set to red when tetromino is on bottom
        /// </summary>
        /// <param name="G"></param>
        public void colorWhenOnBottom(Graphics G)
        {
            Pen p = new Pen(Color.Black, 2);
            SolidBrush b = new SolidBrush(Color.Red);
            foreach (Point point in pL)
            {
                G.FillRectangle(b, point.X, point.Y, boxSize, boxSize);
                G.DrawRectangle(p, point.X, point.Y, boxSize, boxSize);
            }
            
        }

        /// <summary>
        /// if tetromino can move right
        /// </summary>
        /// <returns></returns>
        private bool freeToMoveRight()
        {
            foreach (Point point in pL)
            {
                foreach (Point point2 in tetromino.figureOnBoard)
                {
                    if (point.Equals(new Point(point2.X + tetromino.boxSize, point2.Y)))
                        return false;
                }
            }

            return tetromino.Right.X + tetromino.boxSize < pictureBox1.Width - 1;
        }

        /// <summary>
        /// if tetromino can move on left
        /// </summary>
        /// <returns></returns>
        private bool freeToMoveLeft()
        {
            foreach (Point point in pL)
            {
                foreach (Point point2 in tetromino.figureOnBoard)
                {
                    if (point.Equals(new Point(point2.X - tetromino.boxSize, point2.Y)))
                        return false;
                }
            }
            return tetromino.Left.X - tetromino.boxSize >= 0;
        }

        /// <summary>
        /// make tetromino to fall
        /// </summary>
        private void Fall()
        {
            while (moving())
            {
                tetromino.move();
                pictureBox1.Invalidate();
            }
        }

        /// <summary>
        /// if tetromino can fall
        /// </summary>
        private void canFall()
        {
            if (moving())
            {
                tetromino.location = new Point(tetromino.location.X, tetromino.location.Y + 20); ;
                pictureBox1.Invalidate();
            }
        }

        /// <summary>
        /// if tetromino can turnForward
        /// </summary>
        /// <returns></returns>
        public bool checkRotation()
        {
            tetromino.turnForward();
            foreach (Point P1 in tetromino.figureOnBoard)
            {
                if (P1.X >= 0 && P1.Y < pictureBox1.Height && P1.X < pictureBox1.Width)
                {
                    foreach (Point point in pL)
                    {

                        if (point.Equals(new Point(P1.X, P1.Y)))
                        {
                            tetromino.turnBackwards();
                            return false;
                        }
                    }
                }
                else
                {
                    tetromino.turnBackwards();
                    return false;
                }
            }
            tetromino.turnBackwards();
            return true;

        }


        /// <summary>
        /// If keyboard is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.P:
                    {
                        pause(); //pause
                    }
                    break;
                case Keys.Right:
                case Keys.D:
                    {
                        if (freeToMoveRight()) tetromino.location = new Point(tetromino.location.X + tetromino.boxSize, tetromino.location.Y);
                    }
                    break;
                case Keys.Left:
                case Keys.A:
                    {
                        if (freeToMoveLeft())  tetromino.location = new Point(tetromino.location.X - tetromino.boxSize, tetromino.location.Y);
                    }
                    break;
                case Keys.Down:
                case Keys.S:
                    {
                        canFall();

                    }
                    break;
                case Keys.Space:
                    {
                        Fall();
                        break;
                    }
                case Keys.Up:
                case Keys.W:
                    if (checkRotation())
                        tetromino.turnForward();
                    break;
            }
            pictureBox1.Invalidate();
        }



        /// <summary>
        /// move
        /// </summary>
        public void move()
        {
            location = new Point(location.X, location.Y + boxSize);
        }



        /// <summary>
        /// When the form is loaded generates tetromino and prevents arrow keys focuse on buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            randomTetromino();
            foreach (Control control in this.Controls)
            {
                control.PreviewKeyDown += new PreviewKeyDownEventHandler(control_PreviewKeyDown);
            }

        }

        int g =0;
        int random;
        /// <summary>
        /// Generates random tetromino 
        /// </summary>
        private void randomTetromino()
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);
            if (g == 0)
            {
                random = rnd.Next(0, 7);
                g = 1;
            }
            
            
            switch (0)//CHEAT MECHANISM shoulb be 'random' instead of '0'
            {
                case 0:
                    tetromino = new block();
                    random = rnd.Next(0, 7);
                    break;
                case 1:
                    tetromino = new stick();
                    random = rnd.Next(0, 7);
                    break;
                case 2:
                    tetromino = new leftGun();
                    random = rnd.Next(0, 7);
                    break;
                case 3:
                    tetromino = new rightGun();
                    random = rnd.Next(0, 7);
                    break;
                case 4:
                    tetromino = new T();
                    random = rnd.Next(0, 7);
                    break;
                case 5:
                    tetromino = new skew();
                    random = rnd.Next(0, 7);
                    break;
                case 6:
                    tetromino = new inverseSkew();
                    random = rnd.Next(0, 7);
                    break;
            }
        }



        /// <summary>
        /// Prevents arrow keys focuse on buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void control_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                e.IsInputKey = true;
            }
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// pause menu click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItem2_Click(object sender, EventArgs e)
        {
            pause();
            menuItem3.Visible = true;
            menuItem2.Visible = false;
            
            
        }
        /// <summary>
        /// pause
        /// </summary>
        private void pause()
        {
            timer1.Enabled = false;
            stopWatch.Stop();
        }

        /// <summary>
        /// resume
        /// </summary>
        private void resume()
        {
            timer1.Enabled = true;
            stopWatch.Start();
        }

        /// <summary>
        /// pause click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItem3_Click(object sender, EventArgs e)
        {
            menuItem3.Visible = false;
            resume();
            menuItem2.Visible = true;
        }

        /// <summary>
        /// rules click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItem4_Click(object sender, EventArgs e)
        {
            var rules = new Rules();
            rules.Show();
        }

        /// <summary>
        /// exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItem6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// How click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItem5_Click(object sender, EventArgs e)
        {
            var how = new How();
            how.Show();
        }

        /// <summary>
        /// Timer to display time elapsed, main timer to control whole time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            TimerLable.Text = "" + stopWatch.Elapsed.ToString("mm\\:ss"); // Updates and displayes Time elapsed after the game started

            highScores[4] = new HighScore { Score = Scores, PlayerName = player };

            label3.Text = "" + highestScoreRecord;

            var sortedScores = highScores.OrderByDescending(s => s.Score).ToList();
            
            label1.Text = sortedScores[0].PlayerName + "\n" + sortedScores[1].PlayerName + "\n" + sortedScores[2].PlayerName + "\n" + sortedScores[3].PlayerName + "\n" + sortedScores[4].PlayerName + "\n"; // score.PlayerName; 
            label2.Text = "" + sortedScores[0].Score + "\n" + sortedScores[1].Score + "\n" + sortedScores[2].Score + "\n" + sortedScores[3].Score + "\n" + sortedScores[4].Score + "\n";
            
            switch (random)
            {
                case 0:
                    pictureBox3.Image = Properties.Resources._6;
                    break;
                case 1:
                    pictureBox3.Image = Properties.Resources._5;
                    break;
                case 2:
                    pictureBox3.Image = Properties.Resources._1;
                    break;
                case 3:
                    pictureBox3.Image = Properties.Resources._2;
                    break;
                case 4:
                    pictureBox3.Image = Properties.Resources._4;
                    break;
                case 5:
                    pictureBox3.Image = Properties.Resources._8;
                    break;
                case 6:
                    pictureBox3.Image = Properties.Resources._3;
                    break;
            }

        }

       


      
        

    }
}
