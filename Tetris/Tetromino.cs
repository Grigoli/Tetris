using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace Tetris
{
    /// <summary>
    /// Abstract class of tetromino
    /// </summary>
    abstract class Tetromino
    {

        /// <summary>
        /// Board
        /// </summary>
        public int boxSize = 20;

        /// <summary>
        /// location
        /// </summary>
        public Point location = new Point();

        /// <summary>
        /// Ground
        /// </summary>
        public Point Ground
        {
            get
            {
                Point P1 = location;

                foreach (Point P2 in figureOnBoard)
                {
                    if (P1.Y < P2.Y)
                        P1 = P2;

                }
                return (P1);
            }
        }


        /// <summary>
        /// right side
        /// </summary>
        public Point Right
        {
            get
            {
                Point p3 = location;
                foreach (Point p in figureOnBoard)
                {
                    if (p3.X < p.X)
                    {
                        p3 = p;
                    }
                }
                return p3;
            }
        }


        /// <summary>
        /// right side
        /// </summary>
        public virtual Point Left
        {
            get
            {
                Point p3 = location;
                foreach (Point p in figureOnBoard)
                {
                    if (p3.X > p.X)
                    {
                        p3 = p;
                    }
                }
                return p3;
            }
        }
        /// <summary>
        /// Figure appearance on board with different rotated positions
        /// </summary>
        public abstract List<Point> figureOnBoard { get; }
        //Constructor

        public int turnForwarddPositions { get; set; }


        //Methods
        /// <summary>
        ///  Figure vizuals
        /// </summary>
        /// <param name="G"></param>
        public virtual void figureTrace(Graphics G) {}


        public void move()
        {
            location = new Point(location.X, location.Y + boxSize);
        }

        /// <summary>
        /// Rotate clockwise
        /// </summary>
        public virtual void turnForward() { }

        /// <summary>
        /// Rotate anticlockwise
        /// </summary>
        public virtual void turnBackwards() { }
    }

    /// <summary>
    /// Class of T shape tetromino
    /// </summary>
    class T : Tetromino
    {

        /// <summary>
        ///  Figure vizuals
        /// </summary>
        /// <param name="G"></param>
        public override void figureTrace(Graphics G)
        {
            Pen p = new Pen(Color.Black, 2);
            SolidBrush b = new SolidBrush(Color.Purple);  // Tetromino Color
            foreach (Point point in figureOnBoard)
            {
                G.FillRectangle(b, point.X, point.Y, boxSize, boxSize);
                G.DrawRectangle(p, point.X, point.Y, boxSize, boxSize);
            }
        }

        /// <summary>
        /// Fills location spots on board as 
        /// </summary>
        public override List<Point> figureOnBoard
        {
            get
            {
                List<Point> ret = new List<Point>();
                switch (turnForwarddPositions)
                {
                    case 0:
                        ret.Add(location);
                        ret.Add(new Point(location.X + boxSize, location.Y));
                        ret.Add(new Point(location.X + 2 * boxSize, location.Y));
                        ret.Add(new Point(location.X + boxSize, location.Y + boxSize));
                        break;
                    case 1:
                        ret.Add(location);
                        ret.Add(new Point(location.X, location.Y + boxSize));
                        ret.Add(new Point(location.X - boxSize, location.Y + boxSize));
                        ret.Add(new Point(location.X, location.Y + boxSize * 2));
                        break;
                    case 2:
                        ret.Add(location);
                        ret.Add(new Point(location.X - boxSize, location.Y));
                        ret.Add(new Point(location.X - boxSize, location.Y - boxSize));
                        ret.Add(new Point(location.X - 2 * boxSize, location.Y));
                        break;
                    case 3:
                        ret.Add(location);
                        ret.Add(new Point(location.X, location.Y - boxSize));
                        ret.Add(new Point(location.X + boxSize, location.Y - boxSize));
                        ret.Add(new Point(location.X, location.Y - boxSize * 2));
                        break;
                }

                return ret;
            }

        }
        /// <summary>
        /// Rotate clockwise
        /// </summary>
        public override void turnForward()
        {
            turnForwarddPositions = (turnForwarddPositions + 1) % 4;

        }
        /// <summary>
        /// Rotate anticlockwise
        /// </summary>
        public override void turnBackwards()
        {
            turnForwarddPositions = (turnForwarddPositions - 1);
            if (turnForwarddPositions == -1)
                turnForwarddPositions = 3;
        }



    }

    /// <summary>
    /// Class of Leftgun tetromino
    /// </summary>
    class leftGun : Tetromino
    {
        /// <summary>
        ///  Figure vizuals
        /// </summary>
        /// <param name="G"></param>

        public override void figureTrace(Graphics G)
        {
            Pen p = new Pen(Color.Black, 2);
            SolidBrush b = new SolidBrush(Color.Orange);  // Tetromino Color
            foreach (Point point in figureOnBoard)
            {
                G.FillRectangle(b, point.X, point.Y, boxSize, boxSize);
                G.DrawRectangle(p, point.X, point.Y, boxSize, boxSize);
            }
        }

        /// <summary>
        /// Figure appearance on board with different rotated positions
        /// </summary>
        public override List<Point> figureOnBoard
        {
            get
            {
                List<Point> ret = new List<Point>();
                switch (turnForwarddPositions)
                {
                    case 0:
                        {
                            ret.Add(location);
                            ret.Add(new Point(location.X + boxSize, location.Y));
                            ret.Add(new Point(location.X + boxSize, location.Y + boxSize));
                            ret.Add(new Point(location.X + boxSize, location.Y + 2 * boxSize));
                        }
                        break;

                    case 1:
                        {
                            ret.Add(location);
                            ret.Add(new Point(location.X + boxSize, location.Y - boxSize));
                            ret.Add(new Point(location.X - boxSize, location.Y));
                            ret.Add(new Point(location.X + boxSize, location.Y));
                        }
                        break;

                    case 2:
                        {
                            ret.Add(location);
                            ret.Add(new Point(location.X + boxSize, location.Y));
                            ret.Add(new Point(location.X, location.Y - boxSize));
                            ret.Add(new Point(location.X, location.Y - 2 * boxSize));
                        }
                        break;

                    case 3:
                        {
                            ret.Add(location);
                            ret.Add(new Point(location.X - boxSize, location.Y + boxSize));
                            ret.Add(new Point(location.X + boxSize, location.Y));
                            ret.Add(new Point(location.X - boxSize, location.Y));
                        }
                        break;

                }
                return ret;
            }
        }
        /// <summary>
        /// Rotate clockwise
        /// </summary>
        public override void turnForward()
        {
            turnForwarddPositions = (turnForwarddPositions + 1) % 4;

        }
        /// <summary>
        /// Rotate anticlockwise
        /// </summary>
        public override void turnBackwards()
        {
            turnForwarddPositions = (turnForwarddPositions - 1);
            if (turnForwarddPositions == -1)
                turnForwarddPositions = 3;
        }






    }

    /// <summary>
    /// Class of stick tetromino
    /// </summary>
    class stick : Tetromino
    {
        /// <summary>
        ///  Figure vizuals
        /// </summary>
        /// <param name="G"></param>
        public override void figureTrace(Graphics G)
        {
            Pen p = new Pen(Color.Black, 2);
            SolidBrush b = new SolidBrush(Color.Cyan);  // Tetromino Color
            foreach (Point point in figureOnBoard)
            {
                G.FillRectangle(b, point.X, point.Y, boxSize, boxSize);
                G.DrawRectangle(p, point.X, point.Y, boxSize, boxSize);
            }
        }

        /// <summary>
        /// Figure appearance on board with different rotated positions
        /// </summary>
        public override List<Point> figureOnBoard
        {
            get
            {
                List<Point> ret = new List<Point>();
                switch (turnForwarddPositions)
                {
                    case 0:


                        ret.Add(location);
                        ret.Add(new Point(location.X + boxSize, location.Y));
                        ret.Add(new Point(location.X + 2 * boxSize, location.Y));
                        ret.Add(new Point(location.X + 3 * boxSize, location.Y));
                        break;
                    case 1:
                        ret.Add(location);
                        ret.Add(new Point(location.X, location.Y + boxSize));
                        ret.Add(new Point(location.X, location.Y + boxSize * 2));
                        ret.Add(new Point(location.X, location.Y + boxSize * 3));
                        break;

                }
                return ret;
            }
        }
        /// <summary>
        /// Rotate clockwise
        /// </summary>
        public override void turnForward()
        {
            turnForwarddPositions = (turnForwarddPositions + 1) % 2;

        }
        /// <summary>
        /// Rotate anticlockwise
        /// </summary>
        public override void turnBackwards()
        {
            turnForwarddPositions = (turnForwarddPositions - 1);
            if (turnForwarddPositions == -1)
                turnForwarddPositions = 1;

        }


        public override Point Left
        {
            get
            {

                Point ret = new Point(location.X, location.Y);
                return ret;
            }
        }



    }

    /// <summary>
    /// Class of skew tetramino
    /// </summary>
    class skew : Tetromino
    {
        /// <summary>
        ///  Figure vizuals
        /// </summary>
        /// <param name="G"></param>
        public override void figureTrace(Graphics G)
        {
            Pen p = new Pen(Color.Black, 2);
            SolidBrush b = new SolidBrush(Color.Red);  // Tetromino Color
            foreach (Point point in figureOnBoard)
            {
                G.FillRectangle(b, point.X, point.Y, boxSize, boxSize);
                G.DrawRectangle(p, point.X, point.Y, boxSize, boxSize);
            }
        }

        /// <summary>
        /// Figure appearance on board with different rotated positions
        /// </summary>
        public override List<Point> figureOnBoard
        {
            get
            {
                List<Point> ret = new List<Point>();
                switch (turnForwarddPositions)
                {
                    case 0:
                        {
                            ret.Add(location);
                            ret.Add(new Point(location.X + boxSize, location.Y));
                            ret.Add(new Point(location.X + boxSize, location.Y + boxSize));
                            ret.Add(new Point(location.X + 2 * boxSize, location.Y + boxSize));
                        }
                        break;

                    case 1:
                        {
                            ret.Add(location);
                            ret.Add(new Point(location.X, location.Y + boxSize));
                            ret.Add(new Point(location.X - boxSize, location.Y + boxSize));
                            ret.Add(new Point(location.X - boxSize, location.Y + 2 * boxSize));
                        }
                        break;

                    case 2:
                        {
                            ret.Add(location);
                            ret.Add(new Point(location.X - boxSize, location.Y));
                            ret.Add(new Point(location.X - boxSize, location.Y - boxSize));
                            ret.Add(new Point(location.X - 2 * boxSize, location.Y - boxSize));
                        }
                        break;

                    case 3:
                        {
                            ret.Add(location);
                            ret.Add(new Point(location.X, location.Y - boxSize));
                            ret.Add(new Point(location.X + boxSize, location.Y - boxSize));
                            ret.Add(new Point(location.X + boxSize, location.Y - 2 * boxSize));
                        }
                        break;


                }

                return ret;
            }


        }
        /// <summary>
        /// Rotate clockwise
        /// </summary>
        public override void turnForward()
        {
            turnForwarddPositions = (turnForwarddPositions + 1) % 4;

        }
        /// <summary>
        /// Rotate anticlockwise
        /// </summary>
        public override void turnBackwards()
        {
            turnForwarddPositions = (turnForwarddPositions - 1);
            if (turnForwarddPositions == -1)
                turnForwarddPositions = 3;
        }


    }

    /// <summary>
    /// Class of inverse skew tetramino
    /// </summary>
    class inverseSkew : Tetromino
    {
        /// <summary>
        ///  Figure vizuals
        /// </summary>
        /// <param name="G"></param>
        public override void figureTrace(Graphics G)
        {
            Pen p = new Pen(Color.Black, 2);
            SolidBrush b = new SolidBrush(Color.Green);  // Tetromino Color
            foreach (Point point in figureOnBoard)
            {
                G.FillRectangle(b, point.X, point.Y, boxSize, boxSize);
                G.DrawRectangle(p, point.X, point.Y, boxSize, boxSize);
            }
        }

        /// <summary>
        /// Figure appearance on board with different rotated positions
        /// </summary>
        public override List<Point> figureOnBoard
        {
            get
            {
                List<Point> ret = new List<Point>();
                switch (turnForwarddPositions)
                {
                    case 0:
                        {
                            ret.Add(new Point(location.X+2*boxSize, location.Y ));
                            ret.Add(new Point(location.X + boxSize, location.Y));
                            ret.Add(new Point(location.X + boxSize, location.Y + boxSize));
                            ret.Add(new Point(location.X , location.Y + boxSize));
                        }
                        break;

                    case 1:
                        {
                            ret.Add(location);
                            ret.Add(new Point(location.X, location.Y + boxSize));
                            ret.Add(new Point(location.X + boxSize, location.Y + boxSize));
                            ret.Add(new Point(location.X + boxSize, location.Y + 2 * boxSize));
                        }
                        break;

                    case 2:
                        {
                            ret.Add(location);
                            ret.Add(new Point(location.X + boxSize, location.Y));
                            ret.Add(new Point(location.X + boxSize, location.Y - boxSize));
                            ret.Add(new Point(location.X + 2 * boxSize, location.Y - boxSize));
                        }
                        break;

                    case 3:
                        {
                            ret.Add(location);
                            ret.Add(new Point(location.X, location.Y - boxSize));
                            ret.Add(new Point(location.X - boxSize, location.Y - boxSize));
                            ret.Add(new Point(location.X - boxSize, location.Y - 2 * boxSize));
                        }
                        break;

                }
                return ret;
            }
        }

        /// <summary>
        /// Rotate clockwise
        /// </summary>
        public override void turnForward()
        {
            turnForwarddPositions = (turnForwarddPositions + 1) % 4;

        }
        /// <summary>
        /// Rotate anticlockwise
        /// </summary>
        public override void turnBackwards()
        {
            turnForwarddPositions = (turnForwarddPositions - 1);
            if (turnForwarddPositions == -1)
                turnForwarddPositions = 3;
        }


    }

    /// <summary>
    /// Class of right Gun tetramino
    /// </summary>
    class rightGun : Tetromino
    {
        /// <summary>
        ///  Figure vizuals
        /// </summary>
        /// <param name="G"></param>
        public override void figureTrace(Graphics G)
        {
            Pen p = new Pen(Color.Black, 2);
            SolidBrush b = new SolidBrush(Color.Blue);  // Tetromino Color
            foreach (Point point in figureOnBoard)
            {
                G.FillRectangle(b, point.X, point.Y, boxSize, boxSize);
                G.DrawRectangle(p, point.X, point.Y, boxSize, boxSize);
            }
        }

        /// <summary>
        /// Figure appearance on board with different rotated positions
        /// </summary>
        public override List<Point> figureOnBoard
        {
            get
            {
                List<Point> ret = new List<Point>();
                switch (turnForwarddPositions)
                {
                    case 0:
                        {
                            ret.Add(location);
                            ret.Add(new Point(location.X + boxSize, location.Y));
                            ret.Add(new Point(location.X, location.Y + boxSize));
                            ret.Add(new Point(location.X, location.Y + 2 * boxSize));
                        }
                        break;

                    case 1:
                        {
                            ret.Add(location);
                            ret.Add(new Point(location.X + 2 * boxSize, location.Y + boxSize));
                            ret.Add(new Point(location.X + boxSize, location.Y));
                            ret.Add(new Point(location.X + 2 * boxSize, location.Y));
                        }
                        break;

                    case 2:
                        {
                            ret.Add(location);
                            ret.Add(new Point(location.X - boxSize, location.Y));
                            ret.Add(new Point(location.X, location.Y - boxSize));
                            ret.Add(new Point(location.X, location.Y - 2 * boxSize));
                        }
                        break;

                    case 3:
                        {
                            ret.Add(location);
                            ret.Add(new Point(location.X - 2 * boxSize, location.Y - boxSize));
                            ret.Add(new Point(location.X - boxSize, location.Y));
                            ret.Add(new Point(location.X - 2 * boxSize, location.Y));
                        }
                        break;

                }
                return ret;
            }
        }

        /// <summary>
        /// Rotate clockwise
        /// </summary>
        public override void turnForward()
        {
            turnForwarddPositions = (turnForwarddPositions + 1) % 4;

        }
        /// <summary>
        /// Rotate anticlockwise
        /// </summary>
        public override void turnBackwards()
        {
            turnForwarddPositions = (turnForwarddPositions - 1);
            if (turnForwarddPositions == -1)
                turnForwarddPositions = 3;

        }



    }

    /// <summary>
    /// Class of block tetramino
    /// </summary>
    class block : Tetromino
    {
        /// <summary>
        /// Gives Square shape Yellow collor
        /// </summary>
        /// <param name="gr"></param>
        public override void figureTrace(Graphics G)
        {
            
            Pen p = new Pen(Color.Black, 2);
            SolidBrush b = new SolidBrush(Color.Yellow);  // Tetromino Color
            foreach (Point point in figureOnBoard)
            {
                G.FillRectangle(b, point.X, point.Y, boxSize, boxSize);
                G.DrawRectangle(p, point.X, point.Y, boxSize, boxSize);
            }
        }

        /// <summary>
        /// Figure appearance on board with different rotated positions
        /// </summary>
        public override List<Point> figureOnBoard
        {
            get
            {

                List<Point> ret = new List<Point>();
                

                ret.Add(location);
                ret.Add(new Point(location.X, location.Y + boxSize));
                ret.Add(new Point(location.X + boxSize, location.Y + boxSize));
                ret.Add(new Point(location.X + boxSize, location.Y));
                return ret;
            }
        }

    }

}
