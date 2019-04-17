using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;


namespace MyTetris
{
    public partial class GameForm : Form
    {
        
        Color[,] backCol;
        Color[,] smallCol;
        GameField nextField = new GameField(5, 5);
        GameField gameField;
        SoundPlayer gameOver = new SoundPlayer("GameOver.wav");
        SoundPlayer mainSound = new SoundPlayer("music.wav");
        public GameForm()
        {
            InitializeComponent();
                 backCol = new Color[10, 20];
            gameField = new GameField(10, 20, nextField);
            for (int i = 0; i < 10; i++)
            {
                for (int y = 0; y < 20; y++)
                {
                    backCol[i, y] = Color.Black;
                }

            }
            smallCol = new Color[5, 5];
            for (int i = 0; i < 5; i++)
            {
                for (int y = 0; y < 5; y++)
                {
                    smallCol[i, y] = Color.Gray;
                }

            }
            timer1.Start();
            mainSound.PlayLooping();
        }
        private void PixelDisp_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            using (SolidBrush sb = new SolidBrush(backCol[e.Column, e.Row]))
            {
                e.Graphics.FillRectangle(sb, e.CellBounds);
            }
        }

        void display() {
            for (int i = 0; i < 10; i++)
            {
                for (int y = 0; y < 20; y++)
                {
                    backCol[i, y] = Color.Black;
                }

            }
            for (int i = 0; i < 4; i++) {
                backCol[gameField.сurrentFigure.points.ElementAt(i).x, gameField.сurrentFigure.points.ElementAt(i).y] = Color.Wheat; 
            }
            for(int y = 0; y < 20;y++)
                for(int x = 0; x < 10; x++)
                {
                    if(gameField.field[x,y]==1)
                        backCol[x, y] = Color.Red;
                }
            gameTable.Refresh();
            nextFigureField.Refresh();
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                gameField.DropFigure();
            }
            else if (e.KeyCode == Keys.Up)
            {
                gameField.FigureRotate();
                display();
            }
            else if (e.KeyCode == Keys.Right)
            {
                gameField.FigureMove(1);
                display();
            }
            else if (e.KeyCode == Keys.Left)
            {
                gameField.FigureMove(0);
                display();
            }
            else if (e.KeyCode == Keys.F)
            {
                Close();
            }
            else if (e.KeyCode == Keys.R)
            {
                
            }
        }
        private void Timer1_Tick(object sender, EventArgs e)
        {
            gameField.FigureStepDown();
            display();
            dispSmall();
            score.Text = gameField.score.ToString();
            for (int y = 0; y < 20; y++)
            {
                if (gameField.LineFull(y) == true)
                {
                    gameField.MoveLinesDown(y);
                }
            }
            if (gameField.CheckLose())
            {
                gameOver.Play();
                timer1.Stop();
                ggLabel.Visible = true;
            }
        }

        private void NextFigureField_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            using (SolidBrush sb = new SolidBrush(smallCol[e.Column, e.Row]))
            {
                e.Graphics.FillRectangle(sb, e.CellBounds);
            }

        }

        void dispSmall()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int y = 0; y < 5; y++)
                {
                    smallCol[i, y] = Color.Gray;
                }

            }
            for (int i = 0; i < 4; i++)
            {
                smallCol[nextField.сurrentFigure.points.ElementAt(i).x, nextField.сurrentFigure.points.ElementAt(i).y] = Color.Red;
            }

        }

        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            mainSound.Stop();
        }
    }
}
