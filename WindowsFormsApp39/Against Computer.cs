using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp39
{
    public partial class Aginst_Computer : Form
    {
        public Aginst_Computer()
        {
            InitializeComponent();
        }
        #region Global_Parameters
        Image Black = Properties.Resources.Black;
        Image White = Properties.Resources.White;
        Image Black_King = Properties.Resources.Black_King;
        Image White_King = Properties.Resources.White_King;
        Image Tiles_Background = Properties.Resources.רקע_למשבצות_עליהן_אין_שחקנים;
        Button[,] board;
        int[,] board1;
        int[,] CopySemi;
        int[,] Copy;
        int[,] CopySemi1;
        int[,] Copy1;
        int[,] CopySemi2;
        int[,] Copy2;
        int l, g, size, turn = 0, LeftorRight, UporDown = 0;
        int max0, min0, max1, points, FinalRow, FinalCol, FR, FC;
        #endregion
        #region Button_Click
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string t = textBox1.Text;
                l = int.Parse(t);
            }
            catch
            {

            }
            if (l > 7 && l < 14)
            {
                g++;
                if (g == 1)
                {
                    try
                    {
                        string s = textBox1.Text;
                        size = int.Parse(s);
                        board = new Button[size, size];
                        board1 = new int[size, size];
                        Copy = new int[size, size];
                        CopySemi = new int[size, size];
                        Copy1 = new int[size, size];
                        CopySemi1 = new int[size, size];
                        Copy2 = new int[size, size];
                        CopySemi2 = new int[size, size];
                        int btnSize = 75;
                        int num = 0;
                        for (int r = 0; r < board.GetLength(1); r++)
                        {
                            for (int c = 0; c < board.GetLength(1); c++)
                            {
                                board[r, c] = new Button();
                                board[r, c].Size = new Size(btnSize, btnSize);
                                board[r, c].Location = new Point(c * btnSize, r * btnSize);
                                board[r, c].Tag = num;
                                num++;
                                board[r, c].Click += Aginst_Computer_Click;
                                panel1.Controls.Add(board[r, c]);
                                if ((r + c) % 2 == 0)
                                {
                                    board[r, c].BackColor = Color.White;
                                    if (r > board.GetLength(1) - 4)
                                    {
                                        board1[r, c] = -1;
                                        board[r, c].BackgroundImage = Black;
                                    }
                                    if (r < 3)
                                    {
                                        board1[r, c] = 1;
                                        board[r, c].BackgroundImage = White;
                                    }
                                }
                                else
                                {
                                    board[r, c].BackgroundImage = Tiles_Background;
                                    board[r, c].BackgroundImageLayout = ImageLayout.Stretch;
                                }
                            }
                        }
                        panel1.Width = btnSize * board.GetLength(1);
                        panel1.Height = btnSize * board.GetLength(0);
                        this.Width = panel1.Width + 90;
                        this.Height = panel1.Height + 190;
                    }
                    catch
                    {
                        MessageBox.Show("ERROR");
                    }
                }
                ShowBlackOptions(board1, 1, 1);
                ShowBlackOptions(board1, 0, 0);
                UpdateBoard(board1);
                this.Controls.Remove(button1);
                this.Controls.Remove(textBox1);
                this.Controls.Add(button2);
                this.Controls.Add(button3);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (turn == 1)
            {
                max0 = -5555;
                first(board1, 0, 0);
                first(board1, 1, 1);
                if (board1[FR, FC] == 3)
                {
                    OptionsForWhite(board1, FR, FC);
                    WhiteMovement(board1, LeftorRight, FinalRow, FinalCol);
                    UpdateBoard(board1);
                    CheckWin(board1);
                    ShowBlackOptions(board1, 0, 0);
                    ShowBlackOptions(board1, 1, 1);
                    ShowBlack_KingOptions(board1, 0, 0);
                    ShowBlack_KingOptions(board1, 1, 1);
                    UpdateBoard(board1);
                }
                if (board1[FR, FC] == 5)
                {
                    OptionsForWhite_King(board1, FR, FC);
                    White_KingMovement(board1, UporDown, LeftorRight, FinalRow, FinalCol);
                    UpdateBoard(board1);
                    CheckWin(board1);
                    ShowBlackOptions(board1, 0, 0);
                    ShowBlackOptions(board1, 1, 1);
                    ShowBlack_KingOptions(board1, 0, 0);
                    ShowBlack_KingOptions(board1, 1, 1);
                    UpdateBoard(board1);
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            SkipTurn();
        }
        private void Aginst_Computer_Click(object sender, EventArgs e)
        {
            string s = (((Button)(sender)).Tag).ToString();
            int n = int.Parse(s);
            int r = n / board.GetLength(0);
            int c = n % board.GetLength(1);
            if (turn == 0)
            {
                OptionsForBlack(board1, r, c);
                OptionsForBlack_King(board1, r, c);
                BlackMovement(board1, LeftorRight, r, c);
                Black_KingMovement(board1, UporDown, LeftorRight, r, c);
                if (turn == 1)
                {
                    UpdateBoard(board1);
                    CheckWin(board1);
                    ShowWhiteOptions(board1, 0, 0);
                    ShowWhiteOptions(board1, 1, 1);
                    ShowWhite_KingOptions(board1, 0, 0);
                    ShowWhite_KingOptions(board1, 1, 1);
                }
                UpdateBoard(board1);
            }
        }
        #endregion
        #region Board
        private void CopyBoard(int[,] TheCopied, int[,] TheCopy)
        {
            for (int r = 0; r < TheCopied.GetLength(0); r = r + 2)
            {
                for (int c = 0; c < TheCopied.GetLength(1); c = c + 2)
                {
                    TheCopy[r, c] = TheCopied[r, c];
                }
            }
            for (int r = 1; r < TheCopied.GetLength(0); r = r + 2)
            {
                for (int c = 1; c < TheCopied.GetLength(1); c = c + 2)
                {
                    TheCopy[r, c] = TheCopied[r, c];
                }
            }
        }
        private void CleanBoardBlack(int[,] a, int r, int c)
        {
            int col = c;
            #region ניקוי מסך
            for (; r < a.GetLength(0); r = r + 2)
            {
                for (c = col; c < a.GetLength(1); c = c + 2)
                {
                    if (a[r, c] == -3)
                    {
                        a[r, c] = 0;
                    }
                    if (a[r, c] == 3)
                    {
                        a[r, c] = -1;
                    }
                    if (a[r, c] == 5)
                    {
                        a[r, c] = -2;
                    }
                    if (a[r, c] == -4)
                    {
                        a[r, c] = 1;
                    }
                    if (a[r, c] == -5)
                    {
                        a[r, c] = 2;
                    }
                    if (a[r, c] == 4 || a[r, c] == 6)
                    {
                        a[r, c] = 0;
                    }
                }
            }
            #endregion
        }
        private void CleanBoardWhite(int[,] a, int r, int c)
        {
            int col = c;
            #region ניקוי מסך
            for (; r < a.GetLength(0); r = r + 2)
            {
                for (c = col; c < a.GetLength(1); c = c + 2)
                {
                    if (a[r, c] == -3)
                    {
                        a[r, c] = 0;
                    }
                    if (a[r, c] == 3)
                    {
                        a[r, c] = 1;
                    }
                    if (a[r, c] == 5)
                    {
                        a[r, c] = 2;
                    }
                    if (a[r, c] == -4)
                    {
                        a[r, c] = -1;
                    }
                    if (a[r, c] == -5)
                    {
                        a[r, c] = -2;
                    }
                    if (a[r, c] == 4 || a[r, c] == 6)
                    {
                        a[r, c] = 0;
                    }
                }
            }
            #endregion
        }
        private void UpdateBoard(int[,] a)
        {
            #region  עדכן את לוח המשחק
            for (int r = 0; r < a.GetLength(0); r = r + 2)
            {
                for (int c = 0; c < a.GetLength(1); c = c + 2)
                {
                    if (a[r, c] == 0)
                    {
                        board[r, c].BackColor = Color.White;
                        board[r, c].BackgroundImage = null;
                    }
                    if (a[r, c] == 1)
                    {
                        board[r, c].BackgroundImage = White;
                        board[r, c].BackColor = Color.White;
                    }
                    if (a[r, c] == -1)
                    {
                        board[r, c].BackgroundImage = Black;
                        board[r, c].BackColor = Color.White;
                    }
                    if (a[r, c] == 2)
                    {
                        board[r, c].BackgroundImage = White_King;
                        board[r, c].BackColor = Color.White;
                    }
                    if (a[r, c] == -2)
                    {
                        board[r, c].BackgroundImage = Black_King;
                        board[r, c].BackColor = Color.White;
                    }
                    if (a[r, c] == 3 || a[r, c] == 5)
                    {
                        board[r, c].BackColor = Color.PaleTurquoise;
                    }
                    if (a[r, c] == -3)
                    {
                        board[r, c].BackColor = Color.Turquoise;
                    }
                    if (a[r, c] == 4 || a[r, c] == 6)
                    {
                        board[r, c].BackColor = Color.PaleVioletRed;
                    }
                    if (a[r, c] == -4 || a[r, c] == -5)
                    {
                        board[r, c].BackColor = Color.DarkViolet;
                    }
                }
            }
            for (int r = 1; r < a.GetLength(0); r = r + 2)
            {
                for (int c = 1; c < a.GetLength(1); c = c + 2)
                {
                    if (a[r, c] == 0)
                    {
                        board[r, c].BackColor = Color.White;
                        board[r, c].BackgroundImage = null;
                    }
                    if (a[r, c] == 1)
                    {
                        board[r, c].BackgroundImage = White;
                        board[r, c].BackColor = Color.White;
                    }
                    if (a[r, c] == -1)
                    {
                        board[r, c].BackgroundImage = Black;
                        board[r, c].BackColor = Color.White;
                    }
                    if (a[r, c] == 2)
                    {
                        board[r, c].BackgroundImage = White_King;
                        board[r, c].BackColor = Color.White;
                    }
                    if (a[r, c] == -2)
                    {
                        board[r, c].BackgroundImage = Black_King;
                        board[r, c].BackColor = Color.White;
                    }
                    if (a[r, c] == 3 || a[r, c] == 5)
                    {
                        board[r, c].BackColor = Color.PaleTurquoise;
                    }
                    if (a[r, c] == -3)
                    {
                        board[r, c].BackColor = Color.Turquoise;
                    }
                    if (a[r, c] == 4 || a[r, c] == 6)
                    {
                        board[r, c].BackColor = Color.PaleVioletRed;
                    }
                    if (a[r, c] == -4 || a[r, c] == -5)
                    {
                        board[r, c].BackColor = Color.DarkViolet;
                    }
                }
            }
            #endregion
        }
        private void ShowIntBoard(int[,] a)
        {
            string s = "";
            int r, c;
            for (r = 0; r < board1.GetLength(0); r++)
            {
                for (c = 0; c < board1.GetLength(1); c++)
                {
                    s = s + a[r, c].ToString() + "\t";
                }
                s = s + " \n";
            }
            MessageBox.Show(s);
        }
        public int TheScoreThatTheBoardReturns(int[,] a)
        {
            int ThePointsThatTheBoardReturns = 0;
            for (int r = 0, AddToWhite = 0, AddToBlack = a.GetLength(0) - 1; r < a.GetLength(0); r = r + 2, AddToWhite += 2, AddToBlack -= 2)
            {
                for (int c = 0; c < a.GetLength(1); c = c + 2)
                {
                    if (a[r, c] == 1)
                    {
                        ThePointsThatTheBoardReturns += (10 + AddToWhite);
                        if (r == 0)
                        {
                            ThePointsThatTheBoardReturns += 10;
                        }
                    }
                    if (a[r, c] == -1)
                    {
                        ThePointsThatTheBoardReturns = ThePointsThatTheBoardReturns - (10 + AddToBlack);
                        if (r == a.GetLength(0) - 1)
                        {
                            ThePointsThatTheBoardReturns -= 10;
                        }
                    }
                    if (a[r, c] == 2)
                    {
                        ThePointsThatTheBoardReturns += 55;
                    }
                    if (a[r, c] == -2)
                    {
                        ThePointsThatTheBoardReturns -= 55;
                    }

                }
            }
            for (int r = 1, AddToWhite = 1, AddToBlack = a.GetLength(0) - 2; r < a.GetLength(0); r = r + 2, AddToWhite += 2, AddToBlack -= 2)
            {
                for (int c = 1; c < a.GetLength(1); c = c + 2)
                {
                    if (a[r, c] == 1)
                    {
                        ThePointsThatTheBoardReturns += (10 + AddToWhite);
                    }
                    if (a[r, c] == -1)
                    {
                        ThePointsThatTheBoardReturns = ThePointsThatTheBoardReturns - (10 + AddToBlack);
                        if (r == a.GetLength(0) - 1)
                        {
                            ThePointsThatTheBoardReturns = ThePointsThatTheBoardReturns - (10 + AddToBlack);
                        }
                    }
                    if (a[r, c] == 2)
                    {
                        ThePointsThatTheBoardReturns += 55;
                    }
                    if (a[r, c] == -2)
                    {
                        ThePointsThatTheBoardReturns -= 55;
                    }
                }
            }

            return ThePointsThatTheBoardReturns;
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            panel1.Left = (this.ClientSize.Width - panel1.Width) / 2;
            panel1.Top = (this.ClientSize.Height - panel1.Height) / 2;
        }
        #endregion
        #region Players
        #region Turns
        private void SkipTurn()
        {
            int CheckTurn = 0;
            if (turn == 1)
            {
                CleanBoardWhite(board1, 0, 0);
                CleanBoardWhite(board1, 1, 1);
                ShowBlackOptions(board1, 0, 0);
                ShowBlackOptions(board1, 1, 1);
                ShowBlack_KingOptions(board1, 0, 0);
                ShowBlack_KingOptions(board1, 1, 1);
                UpdateBoard(board1);
                turn = 0;
                CheckTurn = 1;
            }
            if (turn == 0 && CheckTurn == 0)
            {
                CleanBoardBlack(board1, 0, 0);
                CleanBoardBlack(board1, 1, 1);
                ShowWhiteOptions(board1, 0, 0);
                ShowWhiteOptions(board1, 1, 1);
                ShowWhite_KingOptions(board1, 0, 0);
                ShowWhite_KingOptions(board1, 1, 1);
                UpdateBoard(board1);
                turn = 1;
            }
        }
        #endregion
        #region Black
        private void ShowBlackOptions(int[,] a, int r, int c)
        {
            #region אילו חיילים שחורים יכולים ללכת
            int col = c;
            for (; r < a.GetLength(0); r = r + 2)
            {
                for (c = col; c < a.GetLength(0); c = c + 2)
                {
                    if (a[r, c] == -1)
                    {
                        if (r > 0 && c > 0 && a[r - 1, c - 1] == 0)
                        {
                            a[r, c] = 3;
                        }
                        if (r > 1 && c > 1 && (a[r - 1, c - 1] == 1 || a[r - 1, c - 1] == 2) && a[r - 2, c - 2] == 0)
                        {
                            a[r, c] = 3;
                        }
                        if (r > 0 && c < a.GetLength(1) - 1 && a[r - 1, c + 1] == 0)
                        {
                            a[r, c] = 3;
                        }
                        if (r > 1 && c < a.GetLength(1) - 2 && c < a.GetLength(1) - 2 && (a[r - 1, c + 1] == 1 || a[r - 1, c + 1] == 2) && a[r - 2, c + 2] == 0)
                        {
                            a[r, c] = 3;
                        }
                    }
                }
            }
            #endregion
        }
        private void OptionsForBlack(int[,] a, int r, int c)
        {
            #region סימון משבצות לחייל שחור רגיל
            if (a[r, c] == 3)
            {
                CleanBoardBlack(a, 0, 0);
                CleanBoardBlack(a, 1, 1);
                a[r, c] = -3;
                if (r > 0 && c > 0 && a[r - 1, c - 1] == 0)
                {
                    a[r - 1, c - 1] = 4;
                }
                if (r > 1 && c > 1 && (a[r - 1, c - 1] == 1 || a[r - 1, c - 1] == 2) && a[r - 2, c - 2] == 0)
                {
                    if (a[r - 1, c - 1] == 1)
                    {
                        a[r - 1, c - 1] = -4;
                    }
                    if (a[r - 1, c - 1] == 2)
                    {
                        a[r - 1, c - 1] = -5;
                    }
                    a[r - 2, c - 2] = 4;
                }
                if (r > 0 && c < a.GetLength(1) - 1 && a[r - 1, c + 1] == 0)
                {
                    a[r - 1, c + 1] = 4;
                }
                if (r > 1 && c < a.GetLength(1) - 2 && c < a.GetLength(1) - 2 && (a[r - 1, c + 1] == 1 || a[r - 1, c + 1] == 2) && a[r - 2, c + 2] == 0)
                {
                    if (a[r - 1, c + 1] == 1)
                    {
                        a[r - 1, c + 1] = -4;
                    }
                    if (a[r - 1, c + 1] == 2)
                    {
                        a[r - 1, c + 1] = -5;
                    }
                    a[r - 2, c + 2] = 4;
                }
                LeftorRight = c;
            }
            #endregion
        }
        private void BlackMovement(int[,] a, int side, int r, int c)
        {
            #region תזוזה לחייל שחור
            if (a[r, c] == 4)
            {
                if (r == 0)
                {
                    a[r, c] = -2;
                }
                else
                {
                    a[r, c] = -1;
                }
                #region תזוזה שמאלה לחייל שחור
                if (r < a.GetLength(1) - 1 && side > c && a[r + 1, c + 1] == -3)
                {
                    a[r + 1, c + 1] = 0;
                }
                #endregion
                #region אכילה שמאלה לחייל שחור
                if (c < a.GetLength(1) - 2 && r < a.GetLength(0) - 2 && side > c && a[r + 2, c + 2] == -3)
                {
                    a[r + 1, c + 1] = 0;
                    a[r + 2, c + 2] = 0;
                }
                #endregion
                #region תזוזה ימינה לחייל שחור
                if (r < a.GetLength(1) - 1 && side < c && a[r + 1, c - 1] == -3)
                {
                    a[r + 1, c - 1] = 0;
                }
                #endregion
                #region אכילה ימינה לחייל שחור
                if (r < a.GetLength(1) - 2 && c > 1 && side < c && a[r + 2, c - 2] == -3)
                {
                    a[r + 1, c - 1] = 0;
                    a[r + 2, c - 2] = 0;
                }
                #endregion
                CleanBoardBlack(a, 0, 0);
                CleanBoardBlack(a, 1, 1);
                turn = 1;
            }
            #endregion
        }
        private void ShowBlack_KingOptions(int[,] a, int r, int c)
        {
            int CheckButton = 1, col = c;
            #region אילו מלכים שחורים יכולים ללכת
            for (; r < a.GetLength(0); r = r + 2)
            {
                for (c = col; c < a.GetLength(1); c = c + 2)
                {
                    if (a[r, c] == -2)
                    {
                        if (r + CheckButton < a.GetLength(1) && c - CheckButton > -1 && a[r + CheckButton, c - CheckButton] == 0)
                        {
                            a[r, c] = 5;
                        }
                        if (r + CheckButton + 1 < a.GetLength(1) && c - CheckButton - 1 > -1 && (a[r + CheckButton, c - CheckButton] == 1 || a[r + CheckButton, c - CheckButton] == 2) && a[r + CheckButton + 1, c - CheckButton - 1] == 0)
                        {
                            a[r, c] = 5;
                        }
                        if (r - CheckButton > -1 && c - CheckButton > -1 && a[r - CheckButton, c - CheckButton] == 0)
                        {
                            a[r, c] = 5;
                        }
                        if (r - CheckButton - 1 > -1 && c - CheckButton - 1 > -1 && (a[r - CheckButton, c - CheckButton] == 1 || a[r - CheckButton, c - CheckButton] == 2) && a[r - CheckButton - 1, c - CheckButton - 1] == 0)
                        {
                            a[r, c] = 5;
                        }
                        if (r + CheckButton < a.GetLength(0) && c + CheckButton < a.GetLength(1) && a[r + CheckButton, c + CheckButton] == 0)
                        {
                            a[r, c] = 5;
                        }
                        if (r + CheckButton + 1 < a.GetLength(0) && c + CheckButton + 1 < a.GetLength(1) && (a[r + CheckButton, c + CheckButton] == 1 || a[r + CheckButton, c + CheckButton] == 2) && a[r + CheckButton + 1, c + CheckButton + 1] == 0)
                        {
                            a[r, c] = 5;
                        }
                        if (r - CheckButton > -1 && c + CheckButton < a.GetLength(1) && a[r - CheckButton, c + CheckButton] == 0)
                        {
                            a[r, c] = 5;
                        }
                        if (r - CheckButton - 1 > -1 && c + CheckButton + 1 < a.GetLength(1) && (a[r - CheckButton, c + CheckButton] == 1 || a[r - CheckButton, c + CheckButton] == 2) && a[r - CheckButton - 1, c + CheckButton + 1] == 0)
                        {
                            a[r, c] = 5;
                        }
                    }

                }
            }
            #endregion
        }
        private void OptionsForBlack_King(int[,] a, int r, int c)
        {
            #region סימון משבצות למלך שחור
            if (a[r, c] == 5)
            {
                CleanBoardBlack(a, 0, 0);
                CleanBoardBlack(a, 1, 1);
                a[r, c] = -3;
                int CheckButton = 1, CheckEat = 0;
                for (CheckButton = 0, CheckEat = 0; r + CheckButton < a.GetLength(1) && c - CheckButton > -1 && a[r + CheckButton, c - CheckButton] != -1 && a[r + CheckButton, c - CheckButton] != -2; CheckButton++)
                {
                    if ((a[r + CheckButton, c - CheckButton] == 1 || a[r + CheckButton, c - CheckButton] == 2) && CheckEat == 1)
                    {
                        break;
                    }
                    if (a[r + CheckButton, c - CheckButton] == 0)
                    {
                        a[r + CheckButton, c - CheckButton] = 6;
                    }
                    if (r + CheckButton + 1 < a.GetLength(1) && c - CheckButton - 1 > -1 && (a[r + CheckButton, c - CheckButton] == 1 || a[r + CheckButton, c - CheckButton] == 2) && a[r + CheckButton + 1, c - CheckButton - 1] == 0 && CheckEat == 0)
                    {
                        if (a[r + CheckButton, c - CheckButton] == 1)
                        {
                            a[r + CheckButton, c - CheckButton] = -4;
                        }
                        if (a[r + CheckButton, c - CheckButton] == 2)
                        {
                            a[r + CheckButton, c - CheckButton] = -5;
                        }

                        CheckEat++;
                    }
                    if (r + CheckButton + 1 < a.GetLength(1) && c - CheckButton - 1 > -1 && (a[r + CheckButton, c - CheckButton] == 1 || a[r + CheckButton, c - CheckButton] == 2) && a[r + CheckButton + 1, c - CheckButton - 1] != 0 && CheckEat == 0)
                    {
                        CheckEat = 1;
                    }
                }
                for (CheckButton = 1, CheckEat = 0; r - CheckButton > -1 && c - CheckButton > -1 && a[r - CheckButton, c - CheckButton] != -1 && a[r - CheckButton, c - CheckButton] != -2; CheckButton++)
                {
                    if ((a[r - CheckButton, c - CheckButton] == 1 || a[r - CheckButton, c - CheckButton] == 2) && CheckEat == 1)
                    {
                        break;
                    }
                    if (a[r - CheckButton, c - CheckButton] == 0)
                    {
                        a[r - CheckButton, c - CheckButton] = 6;
                    }
                    if (r - CheckButton - 1 > -1 && c - CheckButton - 1 > -1 && (a[r - CheckButton, c - CheckButton] == 1 || a[r - CheckButton, c - CheckButton] == 2) && a[r - CheckButton - 1, c - CheckButton - 1] == 0 && CheckEat == 0)
                    {
                        if (a[r - CheckButton, c - CheckButton] == 1)
                        {
                            a[r - CheckButton, c - CheckButton] = -4;
                        }
                        if (a[r - CheckButton, c - CheckButton] == 2)
                        {
                            a[r - CheckButton, c - CheckButton] = -5;
                        }
                        CheckEat++;
                    }
                    if (r - CheckButton - 1 > -1 && c - CheckButton - 1 > -1 && (a[r - CheckButton, c - CheckButton] == 1 || a[r - CheckButton, c - CheckButton] == 2) && a[r - CheckButton - 1, c - CheckButton - 1] != 0 && CheckEat == 0)
                    {
                        CheckEat++;
                    }
                }
                for (CheckButton = 1, CheckEat = 0; r + CheckButton < a.GetLength(0) && c + CheckButton < a.GetLength(1) && a[r + CheckButton, c + CheckButton] != -1 && a[r + CheckButton, c + CheckButton] != -2; CheckButton++)
                {
                    if ((a[r + CheckButton, c + CheckButton] == 1 || a[r + CheckButton, c + CheckButton] == 2) && CheckEat == 1)
                    {
                        break;
                    }
                    if (a[r + CheckButton, c + CheckButton] == 0)
                    {
                        a[r + CheckButton, c + CheckButton] = 6;
                    }
                    if (r + CheckButton + 1 < a.GetLength(0) && c + CheckButton + 1 < a.GetLength(1) && (a[r + CheckButton, c + CheckButton] == 1 || a[r + CheckButton, c + CheckButton] == 2) && a[r + CheckButton + 1, c + CheckButton + 1] == 0 && CheckEat == 0)
                    {
                        if (a[r + CheckButton, c + CheckButton] == 1)
                        {
                            a[r + CheckButton, c + CheckButton] = -4;
                        }
                        if (a[r + CheckButton, c + CheckButton] == 2)
                        {
                            a[r + CheckButton, c + CheckButton] = -5;
                        }
                        CheckEat++;
                    }
                    if (r + CheckButton + 1 < a.GetLength(0) && c + CheckButton + 1 < a.GetLength(1) && (a[r + CheckButton, c + CheckButton] == 1 || a[r + CheckButton, c + CheckButton] == 2) && a[r + CheckButton + 1, c + CheckButton + 1] != 0 && CheckEat == 0)
                    {
                        CheckEat++;
                    }
                }
                for (CheckButton = 1, CheckEat = 0; r - CheckButton > -1 && c + CheckButton < a.GetLength(1) && a[r - CheckButton, c + CheckButton] != -1 && a[r - CheckButton, c + CheckButton] != -2; CheckButton++)
                {
                    if ((a[r - CheckButton, c + CheckButton] == 1 || a[r - CheckButton, c + CheckButton] == 2) && CheckEat == 1)
                    {
                        break;
                    }
                    if (a[r - CheckButton, c + CheckButton] == 0)
                    {
                        a[r - CheckButton, c + CheckButton] = 6;
                    }
                    if (r - CheckButton - 1 > -1 && c + CheckButton + 1 < a.GetLength(1) && (a[r - CheckButton, c + CheckButton] == 1 || a[r - CheckButton, c + CheckButton] == 2) && a[r - CheckButton - 1, c + CheckButton + 1] == 0 && CheckEat == 0)
                    {
                        if (a[r - CheckButton, c + CheckButton] == 1)
                        {
                            a[r - CheckButton, c + CheckButton] = -4;
                        }
                        if (a[r - CheckButton, c + CheckButton] == 2)
                        {
                            a[r - CheckButton, c + CheckButton] = -5;
                        }
                        CheckEat++;
                    }
                    if (r - CheckButton - 1 > -1 && c + CheckButton + 1 < a.GetLength(1) && (a[r - CheckButton, c + CheckButton] == 1 || a[r - CheckButton, c + CheckButton] == 2) && a[r - CheckButton - 1, c + CheckButton + 1] != 0 && CheckEat == 0)
                    {
                        CheckEat++;
                    }
                }
                LeftorRight = c;
                UporDown = r;
            }
            #endregion
        }
        private void Black_KingMovement(int[,] a, int UoD, int LoR, int r, int c)
        {
            #region תזוזה למלך השחור      
            if (a[r, c] == 6)
            {
                a[r, c] = -2;
                #region תזוזה למטה לצד שמאל
                if (UoD < r && LoR > c)
                {
                    for (c = c + 1, r = r - 1; r > -1 && c < a.GetLength(1); r--, c++)
                    {
                        if (a[r, c] == -4 || a[r, c] == -5)
                        {
                        }
                        if (a[r, c] == -3)
                        {
                            a[r, c] = 0;
                            break;
                        }
                        a[r, c] = 0;
                    }
                }
                #endregion
                #region תזוזה למעלה לצד שמאל
                if (UoD > r && LoR > c)
                {
                    for (c = c + 1, r = r + 1; r < a.GetLength(0) && c < a.GetLength(1); r++, c++)
                    {
                        if (a[r, c] == -4 || a[r, c] == -5)
                        {
                        }
                        if (a[r, c] == -3)
                        {
                            a[r, c] = 0;
                            break;
                        }
                        a[r, c] = 0;
                    }
                }
                #endregion
                #region תזוזה למטה לצד ימין
                if (UoD < r && LoR < c)
                {
                    for (r = r - 1, c = c - 1; r > -1 && c > -1; r--, c--)
                    {
                        if (a[r, c] == -4 || a[r, c] == -5)
                        {
                        }
                        if (a[r, c] == -3)
                        {
                            a[r, c] = 0;
                            break;
                        }
                        a[r, c] = 0;
                    }
                }
                #endregion
                #region תזוזה למעלה לצד ימין
                if (UoD > r && LoR < c)
                {
                    for (r = r + 1, c = c - 1; r < a.GetLength(0) && c > -1; r++, c--)
                    {

                        if (a[r, c] == -4 || a[r, c] == -5)
                        {
                        }
                        if (a[r, c] == -3)
                        {
                            a[r, c] = 0;
                            break;
                        }
                        a[r, c] = 0;
                    }
                }
                #endregion
                CleanBoardBlack(a, 0, 0);
                CleanBoardBlack(a, 1, 1);
                turn = 1;
            }
            #endregion
        }
        #endregion
        #region White
        private void ShowWhiteOptions(int[,] a, int r, int c)
        {
            #region אילו חיילים לבנים יכולים ללכת
            int col = c;
            for (; r < a.GetLength(0); r = r + 2)
            {
                for (c = col; c < a.GetLength(0); c = c + 2)
                {
                    if (a[r, c] == 1)
                    {
                        if (r < a.GetLength(1) - 1 && c > 0 && a[r + 1, c - 1] == 0)
                        {
                            a[r, c] = 3;
                        }
                        if (r < a.GetLength(1) - 2 && c > 1 && (a[r + 1, c - 1] == -1 || a[r + 1, c - 1] == -2) && a[r + 2, c - 2] == 0)
                        {
                            a[r, c] = 3;
                        }
                        if (r < a.GetLength(1) - 1 && c < a.GetLength(1) - 1 && a[r + 1, c + 1] == 0)
                        {
                            a[r, c] = 3;
                        }
                        if (r < a.GetLength(1) - 2 && c < a.GetLength(1) - 2 && (a[r + 1, c + 1] == -1 || a[r + 1, c + 1] == -2) && a[r + 2, c + 2] == 0)
                        {
                            a[r, c] = 3;
                        }
                    }
                }
            }
            #endregion
        }
        private void OptionsForWhite(int[,] a, int r, int c)
        {
            #region סימון משבצות לחייל לבן רגיל
            if (a[r, c] == 3)
            {
                CleanBoardWhite(a, 0, 0);
                CleanBoardWhite(a, 1, 1);
                a[r, c] = -3;
                if (r < a.GetLength(1) - 1 && c > 0 && a[r + 1, c - 1] == 0)
                {
                    a[r + 1, c - 1] = 4;
                }
                if (r < a.GetLength(1) - 2 && c > 1 && (a[r + 1, c - 1] == -1 || a[r + 1, c - 1] == -2) && a[r + 2, c - 2] == 0)
                {
                    if (a[r + 1, c - 1] == -1)
                    {
                        a[r + 1, c - 1] = -4;
                    }
                    if (a[r + 1, c - 1] == -2)
                    {
                        a[r + 1, c - 1] = -5;
                    }
                    a[r + 2, c - 2] = 4;
                }
                if (r < a.GetLength(1) - 1 && c < a.GetLength(1) - 1 && a[r + 1, c + 1] == 0)
                {
                    a[r + 1, c + 1] = 4;
                }
                if (r < a.GetLength(1) - 2 && c < a.GetLength(1) - 2 && (a[r + 1, c + 1] == -1 || a[r + 1, c + 1] == -2) && a[r + 2, c + 2] == 0)
                {
                    if (a[r + 1, c + 1] == -1)
                    {
                        a[r + 1, c + 1] = -4;
                    }
                    if (a[r + 1, c + 1] == -2)
                    {
                        a[r + 1, c + 1] = -5;
                    }
                    a[r + 2, c + 2] = 4;
                }
                LeftorRight = c;
            }
            #endregion
        }
        private void WhiteMovement(int[,] a, int side, int r, int c)
        {
            #region תזוזה לחייל לבן
            if (a[r, c] == 4)
            {
                if (r == a.GetLength(1) - 1)
                {
                    a[r, c] = 2;
                }
                else
                {
                    a[r, c] = 1;
                }
                #region תזוזה שמאלה לחייל לבן
                if (r > 0 && side > c && a[r - 1, c + 1] == -3)
                {
                    a[r - 1, c + 1] = 0;
                }
                #endregion
                #region אכילה שמאלה לחייל לבן
                if (r > 1 && c < a.GetLength(1) - 2 && side > c && a[r - 2, c + 2] == -3)
                {
                    a[r - 1, c + 1] = 0;
                    a[r - 2, c + 2] = 0;
                }
                #endregion
                #region תזוזה ימינה לחייל לבן
                if (r > 0 && side < c && a[r - 1, c - 1] == -3)
                {
                    a[r - 1, c - 1] = 0;
                }
                #endregion
                #region אכילה ימינה לחייל לבן
                if (c > 1 && r > 1 && side < c && a[r - 2, c - 2] == -3)
                {
                    a[r - 1, c - 1] = 0;
                    a[r - 2, c - 2] = 0;
                }
                #endregion
                CleanBoardWhite(a, 0, 0);
                CleanBoardWhite(a, 1, 1);
                turn = 0;
            }
            #endregion
        }
        private void ShowWhite_KingOptions(int[,] a, int r, int c)
        {
            #region אילו מלכים לבנים יכולים ללכת
            int col = c;
            for (; r < a.GetLength(0); r = r + 2)
            {
                for (c = col; c < a.GetLength(1); c = c + 2)
                {

                    if (a[r, c] == 2)
                    {
                        int CheckButton = 1;
                        if (r + CheckButton < a.GetLength(0) && c - CheckButton > -1 && a[r + CheckButton, c - CheckButton] == 0)
                        {
                            a[r, c] = 5;
                        }
                        if (r + CheckButton + 1 < a.GetLength(0) && c - CheckButton - 1 > -1 && (a[r + CheckButton, c - CheckButton] == -1 || a[r + CheckButton, c - CheckButton] == -2) && a[r + CheckButton + 1, c - CheckButton - 1] == 0)
                        {
                            a[r, c] = 5;
                        }
                        if (r - CheckButton > -1 && c - CheckButton > -1 && a[r - CheckButton, c - CheckButton] == 0)
                        {
                            a[r, c] = 5;
                        }
                        if (r - CheckButton - 1 > -1 && c - CheckButton - 1 > -1 && (a[r - CheckButton, c - CheckButton] == -1 || a[r - CheckButton, c - CheckButton] == -2) && a[r - CheckButton - 1, c - CheckButton - 1] == 0)
                        {
                            a[r, c] = 5;
                        }
                        if (r + CheckButton < a.GetLength(0) && c + CheckButton < a.GetLength(1) && a[r + CheckButton, c + CheckButton] == 0)
                        {
                            a[r, c] = 5;
                        }
                        if (r + CheckButton + 1 < a.GetLength(0) && c + CheckButton + 1 < a.GetLength(1) && (a[r + CheckButton, c + CheckButton] == -1 || a[r + CheckButton, c + CheckButton] == -2) && a[r + CheckButton + 1, c + CheckButton + 1] == 0)
                        {
                            a[r, c] = 5;
                        }
                        if (r - CheckButton > -1 && c + CheckButton < a.GetLength(1) && a[r - CheckButton, c + CheckButton] == 0)
                        {
                            a[r, c] = 5;
                        }
                        if (r - CheckButton - 1 > -1 && c + CheckButton + 1 < a.GetLength(1) && (a[r - CheckButton, c + CheckButton] == -1 || a[r - CheckButton, c + CheckButton] == -2) && a[r - CheckButton - 1, c + CheckButton + 1] == 0)
                        {
                            a[r, c] = 5;
                        }
                    }
                }
            }
            #endregion
        }
        private void OptionsForWhite_King(int[,] a, int r, int c)
        {
            #region סימון משבצות למלך לבן
            if (a[r, c] == 5)
            {
                CleanBoardWhite(a, 0, 0);
                CleanBoardWhite(a, 1, 1);
                a[r, c] = -3;
                int CheckButton = 1, CheckEat = 0;
                for (CheckButton = 1, CheckEat = 0; r + CheckButton < a.GetLength(0) && c - CheckButton > -1 && a[r + CheckButton, c - CheckButton] != 1 && a[r + CheckButton, c - CheckButton] != 2; CheckButton++)
                {
                    if ((a[r + CheckButton, c - CheckButton] == -1 || a[r + CheckButton, c - CheckButton] == -2) && CheckEat == 1)
                    {
                        break;
                    }
                    if (a[r + CheckButton, c - CheckButton] == 0)
                    {
                        a[r + CheckButton, c - CheckButton] = 6;
                    }
                    if (r + CheckButton + 1 < a.GetLength(0) && c - CheckButton - 1 > -1 && (a[r + CheckButton, c - CheckButton] == -1 || a[r + CheckButton, c - CheckButton] == -2) && a[r + CheckButton + 1, c - CheckButton - 1] == 0 && CheckEat == 0)
                    {
                        if (a[r + CheckButton, c - CheckButton] == -1)
                        {
                            a[r + CheckButton, c - CheckButton] = -4;
                        }
                        if (a[r + CheckButton, c - CheckButton] == -2)
                        {
                            a[r + CheckButton, c - CheckButton] = -5;
                        }
                        CheckEat++;
                    }
                    if (r + CheckButton + 1 < a.GetLength(0) && c - CheckButton - 1 > -1 && (a[r + CheckButton, c - CheckButton] == -1 || a[r + CheckButton, c - CheckButton] == -2) && a[r + CheckButton + 1, c - CheckButton - 1] != 0 && CheckEat == 0)
                    {
                        CheckEat++;
                    }
                }
                for (CheckButton = 1, CheckEat = 0; r - CheckButton > -1 && c - CheckButton > -1 && a[r - CheckButton, c - CheckButton] != 1 && a[r - CheckButton, c - CheckButton] != 2; CheckButton++)
                {
                    if ((a[r - CheckButton, c - CheckButton] == -1 || a[r - CheckButton, c - CheckButton] == -2) && CheckEat == 1)
                    {
                        break;
                    }
                    if (a[r - CheckButton, c - CheckButton] == 0)
                    {
                        a[r - CheckButton, c - CheckButton] = 6;
                    }
                    if (r - CheckButton - 1 > -1 && c - CheckButton - 1 > -1 && (a[r - CheckButton, c - CheckButton] == -1 || a[r - CheckButton, c - CheckButton] == -2) && a[r - CheckButton - 1, c - CheckButton - 1] == 0 && CheckEat == 0)
                    {
                        if (a[r - CheckButton, c - CheckButton] == -1)
                        {
                            a[r - CheckButton, c - CheckButton] = -4;
                        }
                        if (a[r - CheckButton, c - CheckButton] == -2)
                        {
                            a[r - CheckButton, c - CheckButton] = -5;
                        }
                        CheckEat++;
                    }
                    if (r - CheckButton - 1 > -1 && c - CheckButton - 1 > -1 && (a[r - CheckButton, c - CheckButton] == -1 || a[r - CheckButton, c - CheckButton] == -2) && a[r - CheckButton - 1, c - CheckButton - 1] != 0 && CheckEat == 0)
                    {
                        CheckEat++;
                    }
                }
                for (CheckButton = 1, CheckEat = 0; r + CheckButton < a.GetLength(0) && c + CheckButton < a.GetLength(1) && a[r + CheckButton, c + CheckButton] != 1 && a[r + CheckButton, c + CheckButton] != 2; CheckButton++)
                {
                    if (a[r + CheckButton, c + CheckButton] == -2 && CheckEat == 1 || a[r + CheckButton, c + CheckButton] == -2 && CheckEat == 1)
                    {
                        break;
                    }
                    if (a[r + CheckButton, c + CheckButton] == 0)
                    {
                        a[r + CheckButton, c + CheckButton] = 6;
                    }
                    if (r + CheckButton + 1 < a.GetLength(0) && c + CheckButton + 1 < a.GetLength(1) && (a[r + CheckButton, c + CheckButton] == -1 || a[r + CheckButton, c + CheckButton] == -2) && a[r + CheckButton + 1, c + CheckButton + 1] == 0 && CheckEat == 0)
                    {
                        if (a[r + CheckButton, c + CheckButton] == -1)
                        {
                            a[r + CheckButton, c + CheckButton] = -4;
                        }
                        if (a[r + CheckButton, c + CheckButton] == -2)
                        {
                            a[r + CheckButton, c + CheckButton] = -5;
                        }
                        CheckEat++;
                    }
                    if (r + CheckButton + 1 < a.GetLength(0) && c + CheckButton + 1 < a.GetLength(1) && (a[r + CheckButton, c + CheckButton] == -1 || a[r + CheckButton, c + CheckButton] == -2) && a[r + CheckButton + 1, c + CheckButton + 1] != 0 && CheckEat == 0)
                    {
                        CheckEat++;
                    }
                }
                for (CheckButton = 1, CheckEat = 0; r - CheckButton > -1 && c + CheckButton < a.GetLength(1) && a[r - CheckButton, c + CheckButton] != 1 && a[r - CheckButton, c + CheckButton] != 2; CheckButton++)
                {
                    if (a[r - CheckButton, c + CheckButton] == -2 && CheckEat == 1 || a[r - CheckButton, c + CheckButton] == -2 && CheckEat == 1)
                    {
                        break;
                    }
                    if (a[r - CheckButton, c + CheckButton] == 0)
                    {
                        a[r - CheckButton, c + CheckButton] = 6;
                    }
                    if (r - CheckButton - 1 > -1 && c + CheckButton + 1 < a.GetLength(1) && (a[r - CheckButton, c + CheckButton] == -1 || a[r - CheckButton, c + CheckButton] == -2) && a[r - CheckButton - 1, c + CheckButton + 1] == 0 && CheckEat == 0)
                    {
                        if (a[r - CheckButton, c + CheckButton] == -1)
                        {
                            a[r - CheckButton, c + CheckButton] = -4;
                        }
                        if (a[r - CheckButton, c + CheckButton] == -2)
                        {
                            a[r - CheckButton, c + CheckButton] = -5;
                        }
                        CheckEat++;
                    }
                    if (r - CheckButton - 1 > -1 && c + CheckButton + 1 < a.GetLength(1) && (a[r - CheckButton, c + CheckButton] == -1 || a[r - CheckButton, c + CheckButton] == -2) && a[r - CheckButton - 1, c + CheckButton + 1] != 0 && CheckEat == 0)
                    {
                        CheckEat++;
                    }
                }
                LeftorRight = c;
                UporDown = r;
            }
            #endregion
        }
        private void White_KingMovement(int[,] a, int UoD, int LoR, int r, int c)
        {
            #region תזוזה למלך לבן
            if (a[r, c] == 6)
            {
                a[r, c] = 2;
                if (UoD < r && LoR > c)
                {
                    for (c = c + 1, r = r - 1; r > -1 && c < a.GetLength(1); r--, c++)
                    {
                        if (a[r, c] == -4 || a[r, c] == -5)
                        {
                        }
                        if (a[r, c] == -3)
                        {
                            a[r, c] = 0;
                            break;
                        }
                        a[r, c] = 0;
                    }
                }
                if (UoD > r && LoR > c)
                {
                    for (c = c + 1, r = r + 1; r < a.GetLength(0) && c < a.GetLength(1); r++, c++)
                    {
                        if (a[r, c] == -4 || a[r, c] == -5)
                        {
                        }
                        if (a[r, c] == -3)
                        {
                            a[r, c] = 0;
                            break;
                        }
                        a[r, c] = 0;
                    }
                }
                if (UoD < r && LoR < c)
                {
                    for (c = c - 1, r = r - 1; r > -1 && c > -1; r--, c--)
                    {
                        if (a[r, c] == -4 || a[r, c] == -5)
                        {
                        }
                        if (a[r, c] == -3)
                        {
                            a[r, c] = 0;
                            break;
                        }
                        a[r, c] = 0;
                    }
                }
                if (UoD > r && LoR < c)
                {
                    for (r = r + 1, c = c - 1; r < a.GetLength(0) && c > -1; c--, r++)
                    {
                        if (a[r, c] == -4 || a[r, c] == -5)
                        {
                        }
                        if (a[r, c] == -3)
                        {
                            a[r, c] = 0;
                            break;
                        }
                        a[r, c] = 0;
                    }
                }
                CleanBoardWhite(a, 0, 0);
                CleanBoardWhite(a, 1, 1);
                turn = 0;
            }
            #endregion
        }
        #endregion
        #region Computer
        private void first(int[,] a, int r, int c)
        {
            {
                int col = c;
                CopyBoard(a, CopySemi);
                for (; r < Copy.GetLength(0); r = r + 2)
                {
                    for (c = col; c < Copy.GetLength(1); c = c + 2)
                    {
                        if (CopySemi[r, c] == 3)
                        {
                            OptionsForWhite(CopySemi, r, c);
                            CopyBoard(CopySemi, Copy);
                            if (c < Copy.GetLength(1) - 1)
                            {
                                if (Copy[r + 1, c + 1] == 4)
                                {
                                    WhiteMovement(Copy, LeftorRight, r + 1, c + 1);
                                    ShowBlackOptions(Copy, 0, 0);
                                    ShowBlackOptions(Copy, 1, 1);
                                    ShowBlack_KingOptions(Copy, 0, 0);
                                    ShowBlack_KingOptions(Copy, 1, 1);
                                    min0 = 5555;
                                    second(Copy, 0, 0);
                                    second(Copy, 1, 1);
                                    if (min0 != 5555 && min0 > max0)
                                    {
                                        max0 = min0;
                                        FR = r;
                                        FC = c;
                                        FinalRow = r + 1;
                                        FinalCol = c + 1;
                                    }
                                    if (min0 == 5555)
                                    {
                                        points = TheScoreThatTheBoardReturns(Copy);
                                        if (points > max0)
                                        {
                                            max0 = points;
                                            FR = r;
                                            FC = c;
                                            FinalRow = r + 1;
                                            FinalCol = c + 1;
                                        }
                                    }
                                    CopyBoard(CopySemi, Copy);
                                }
                                if (Copy[r + 1, c + 1] == -4 || Copy[r + 1, c + 1] == -5)
                                {
                                    WhiteMovement(Copy, LeftorRight, r + 2, c + 2);
                                    ShowBlackOptions(Copy, 0, 0);
                                    ShowBlackOptions(Copy, 1, 1);
                                    ShowBlack_KingOptions(Copy, 0, 0);
                                    ShowBlack_KingOptions(Copy, 1, 1);
                                    min0 = 5555;
                                    second(Copy, 0, 0);
                                    second(Copy, 1, 1);
                                    if (min0 != 5555 && min0 > max0)
                                    {
                                        max0 = min0;
                                        FR = r;
                                        FC = c;
                                        FinalRow = r + 2;
                                        FinalCol = c + 2;
                                    }
                                    if (min0 == 5555)
                                    {
                                        points = TheScoreThatTheBoardReturns(Copy);
                                        if (points > max0)
                                        {
                                            max0 = points;
                                            FR = r;
                                            FC = c;
                                            FinalRow = r + 2;
                                            FinalCol = c + 2;
                                        }
                                    }
                                    CopyBoard(CopySemi, Copy);
                                }
                            }
                            if (c > 0)
                            {
                                if (Copy[r + 1, c - 1] == 4)
                                {
                                    WhiteMovement(Copy, LeftorRight, r + 1, c - 1);
                                    ShowBlackOptions(Copy, 0, 0);
                                    ShowBlackOptions(Copy, 1, 1);
                                    ShowBlack_KingOptions(Copy, 0, 0);
                                    ShowBlack_KingOptions(Copy, 1, 1);
                                    min0 = 5555;
                                    second(Copy, 0, 0);
                                    second(Copy, 1, 1);
                                    if (min0 != 5555 && min0 > max0)
                                    {
                                        max0 = min0;
                                        FR = r;
                                        FC = c;
                                        FinalRow = r + 1;
                                        FinalCol = c - 1;
                                    }
                                    if (min0 == 5555)
                                    {
                                        points = TheScoreThatTheBoardReturns(Copy);
                                        if (points > max0)
                                        {
                                            max0 = points;
                                            FR = r;
                                            FC = c;
                                            FinalRow = r + 1;
                                            FinalCol = c - 1;
                                        }
                                    }
                                    CopyBoard(CopySemi, Copy);
                                }
                                if (Copy[r + 1, c - 1] == -4 || Copy[r + 1, c - 1] == -5)
                                {
                                    WhiteMovement(Copy, LeftorRight, r + 2, c - 2);
                                    ShowBlackOptions(Copy, 0, 0);
                                    ShowBlackOptions(Copy, 1, 1);
                                    ShowBlack_KingOptions(Copy, 0, 0);
                                    ShowBlack_KingOptions(Copy, 1, 1);
                                    min0 = 5555;
                                    second(Copy, 0, 0);
                                    second(Copy, 1, 1);
                                    if (min0 != 5555 && min0 > max0)
                                    {
                                        max0 = min0;
                                        FR = r;
                                        FC = c;
                                        FinalRow = r + 2;
                                        FinalCol = c - 2;
                                    }
                                    if (min0 == 5555)
                                    {
                                        points = TheScoreThatTheBoardReturns(Copy);
                                        if (points > max0)
                                        {
                                            max0 = points;
                                            FR = r;
                                            FC = c;
                                            FinalRow = r + 2;
                                            FinalCol = c - 2;
                                        }
                                    }
                                    CopyBoard(CopySemi, Copy);
                                }
                            }
                            CopyBoard(a, CopySemi);
                        }
                        if (CopySemi[r, c] == 5)
                        {
                            OptionsForWhite_King(CopySemi, r, c);
                            CopyBoard(CopySemi, Copy);
                            for (int rK = r + 1, cK = c + 1; rK < Copy.GetLength(0) && cK < Copy.GetLength(1); rK++, cK++)
                            {
                                if (Copy[rK, cK] != 6 && Copy[rK, cK] != -4 && Copy[rK, cK] != -5)
                                {
                                    break;
                                }
                                if (Copy[rK, cK] == -4 || Copy[rK, cK] == -5)
                                {
                                    rK++;
                                    cK++;
                                    White_KingMovement(Copy, UporDown, LeftorRight, rK, cK);
                                    ShowBlackOptions(Copy, 0, 0);
                                    ShowBlackOptions(Copy, 1, 1);
                                    ShowBlack_KingOptions(Copy, 0, 0);
                                    ShowBlack_KingOptions(Copy, 1, 1);
                                    min0 = 5555;
                                    second(Copy, 0, 0);
                                    second(Copy, 1, 1);
                                    if (min0 != 5555 && min0 > max0)
                                    {
                                        max0 = min0;
                                        FR = r;
                                        FC = c;
                                        FinalRow = rK;
                                        FinalCol = cK;
                                    }
                                    if (min0 == 5555)
                                    {
                                        points = TheScoreThatTheBoardReturns(Copy);
                                        if (points > max0)
                                        {
                                            max0 = points;
                                            FR = r;
                                            FC = c;
                                            FinalRow = rK;
                                            FinalCol = cK;
                                        }
                                    }
                                    CopyBoard(CopySemi, Copy);
                                }
                                if (Copy[rK, cK] == 4)
                                {
                                    White_KingMovement(Copy, UporDown, LeftorRight, rK, cK);
                                    ShowBlackOptions(Copy, 0, 0);
                                    ShowBlackOptions(Copy, 1, 1);
                                    ShowBlack_KingOptions(Copy, 0, 0);
                                    ShowBlack_KingOptions(Copy, 1, 1);
                                    min0 = 5555;
                                    second(Copy, 0, 0);
                                    second(Copy, 1, 1);
                                    if (min0 != 5555 && min0 > max0)
                                    {
                                        max0 = min0;
                                        FR = r;
                                        FC = c;
                                        FinalRow = rK;
                                        FinalCol = cK;
                                    }
                                    if (min0 == 5555)
                                    {
                                        points = TheScoreThatTheBoardReturns(Copy);
                                        if (points > max0)
                                        {
                                            max0 = points;
                                            FR = r;
                                            FC = c;
                                            FinalRow = rK;
                                            FinalCol = cK;
                                        }
                                    }
                                    CopyBoard(CopySemi, Copy);
                                }
                            }
                            for (int rK = r + 1, cK = c - 1; rK < Copy.GetLength(0) && cK > 0; rK++, cK--)
                            {
                                if (Copy[rK, cK] != 6 && Copy[rK, cK] != -4 && Copy[rK, cK] != -5)
                                {
                                    break;
                                }
                                if (Copy[rK, cK] == -4 || Copy[rK, cK] == -5)
                                {
                                    rK++;
                                    cK--;
                                    White_KingMovement(Copy, UporDown, LeftorRight, rK, cK);
                                    ShowBlackOptions(Copy, 0, 0);
                                    ShowBlackOptions(Copy, 1, 1);
                                    ShowBlack_KingOptions(Copy, 0, 0);
                                    ShowBlack_KingOptions(Copy, 1, 1);
                                    min0 = 5555;
                                    second(Copy, 0, 0);
                                    second(Copy, 1, 1);
                                    if (min0 != 5555 && min0 > max0)
                                    {
                                        max0 = min0;
                                        FR = r;
                                        FC = c;
                                        FinalRow = rK;
                                        FinalCol = cK;
                                    }
                                    if (min0 == 5555)
                                    {
                                        points = TheScoreThatTheBoardReturns(Copy);
                                        if (points > max0)
                                        {
                                            max0 = points;
                                            FR = r;
                                            FC = c;
                                            FinalRow = rK;
                                            FinalCol = cK;
                                        }
                                    }
                                    CopyBoard(CopySemi, Copy);
                                }
                                if (Copy[rK, cK] == 4)
                                {
                                    White_KingMovement(Copy, UporDown, LeftorRight, rK, cK);
                                    ShowBlackOptions(Copy, 0, 0);
                                    ShowBlackOptions(Copy, 1, 1);
                                    ShowBlack_KingOptions(Copy, 0, 0);
                                    ShowBlack_KingOptions(Copy, 1, 1);
                                    min0 = 5555;
                                    second(Copy, 0, 0);
                                    second(Copy, 1, 1);
                                    if (min0 != 5555 && min0 > max0)
                                    {
                                        max0 = min0;
                                        FR = r;
                                        FC = c;
                                        FinalRow = rK;
                                        FinalCol = cK;
                                    }
                                    if (min0 == 5555)
                                    {
                                        points = TheScoreThatTheBoardReturns(Copy);
                                        if (points > max0)
                                        {
                                            max0 = points;
                                            FR = r;
                                            FC = c;
                                            FinalRow = rK;
                                            FinalCol = cK;
                                        }
                                    }
                                    CopyBoard(CopySemi, Copy);
                                }
                            }
                            for (int rK = r - 1, cK = c + 1; rK > 0 && cK < Copy.GetLength(1); rK--, cK++)
                            {
                                if (Copy[rK, cK] != 6 && Copy[rK, cK] != -4 && Copy[rK, cK] != -5)
                                {
                                    break;
                                }
                                if (Copy[rK, cK] == -4 || Copy[rK, cK] == -5)
                                {
                                    rK--;
                                    cK++;
                                    White_KingMovement(Copy, UporDown, LeftorRight, rK, cK);
                                    ShowBlackOptions(Copy, 0, 0);
                                    ShowBlackOptions(Copy, 1, 1);
                                    ShowBlack_KingOptions(Copy, 0, 0);
                                    ShowBlack_KingOptions(Copy, 1, 1);
                                    min0 = 5555;
                                    second(Copy, 0, 0);
                                    second(Copy, 1, 1);
                                    if (min0 != 5555 && min0 > max0)
                                    {
                                        max0 = min0;
                                        FR = r;
                                        FC = c;
                                        FinalRow = rK;
                                        FinalCol = cK;
                                    }
                                    if (min0 == 5555)
                                    {
                                        points = TheScoreThatTheBoardReturns(Copy);
                                        if (points > max0)
                                        {
                                            max0 = points;
                                            FR = r;
                                            FC = c;
                                            FinalRow = rK;
                                            FinalCol = cK;
                                        }
                                    }
                                    CopyBoard(CopySemi, Copy);
                                }
                                if (Copy[rK, cK] == 4)
                                {
                                    White_KingMovement(Copy, UporDown, LeftorRight, rK, cK);
                                    ShowBlackOptions(Copy, 0, 0);
                                    ShowBlackOptions(Copy, 1, 1);
                                    ShowBlack_KingOptions(Copy, 0, 0);
                                    ShowBlack_KingOptions(Copy, 1, 1);
                                    min0 = 5555;
                                    second(Copy, 0, 0);
                                    second(Copy, 1, 1);
                                    if (min0 != 5555 && min0 > max0)
                                    {
                                        max0 = min0;
                                        FR = r;
                                        FC = c;
                                        FinalRow = rK;
                                        FinalCol = cK;
                                    }
                                    if (min0 == 5555)
                                    {
                                        points = TheScoreThatTheBoardReturns(Copy);
                                        if (points > max0)
                                        {
                                            max0 = points;
                                            FR = r;
                                            FC = c;
                                            FinalRow = rK;
                                            FinalCol = cK;
                                        }
                                    }
                                    CopyBoard(CopySemi, Copy);
                                }
                            }
                            for (int rK = r - 1, cK = c - 1; rK > 0 && cK > 0; rK--, cK--)
                            {
                                if (Copy[rK, cK] != 6 && Copy[rK, cK] != -4 && Copy[rK, cK] != -5)
                                {
                                    break;
                                }
                                if (Copy[rK, cK] == -4 || Copy[rK, cK] == -5)
                                {
                                    rK--;
                                    cK--;
                                    White_KingMovement(Copy, UporDown, LeftorRight, rK, cK);
                                    ShowBlackOptions(Copy, 0, 0);
                                    ShowBlackOptions(Copy, 1, 1);
                                    ShowBlack_KingOptions(Copy, 0, 0);
                                    ShowBlack_KingOptions(Copy, 1, 1);
                                    min0 = 5555;
                                    second(Copy, 0, 0);
                                    second(Copy, 1, 1);
                                    if (min0 != 5555 && min0 > max0)
                                    {
                                        max0 = min0;
                                        FR = r;
                                        FC = c;
                                        FinalRow = rK;
                                        FinalCol = cK;
                                    }
                                    if (min0 == 5555)
                                    {
                                        points = TheScoreThatTheBoardReturns(Copy);
                                        if (points > max0)
                                        {
                                            max0 = points;
                                            FR = r;
                                            FC = c;
                                            FinalRow = rK;
                                            FinalCol = cK;
                                        }
                                    }
                                    CopyBoard(CopySemi, Copy);
                                }
                                if (Copy[rK, cK] == 4)
                                {
                                    White_KingMovement(Copy, UporDown, LeftorRight, rK, cK);
                                    ShowBlackOptions(Copy, 0, 0);
                                    ShowBlackOptions(Copy, 1, 1);
                                    ShowBlack_KingOptions(Copy, 0, 0);
                                    ShowBlack_KingOptions(Copy, 1, 1);
                                    min0 = 5555;
                                    second(Copy, 0, 0);
                                    second(Copy, 1, 1);
                                    if (min0 != 5555 && min0 > max0)
                                    {
                                        max0 = min0;
                                        FR = r;
                                        FC = c;
                                        FinalRow = rK;
                                        FinalCol = cK;
                                    }
                                    if (min0 == 5555)
                                    {
                                        points = TheScoreThatTheBoardReturns(Copy);
                                        if (points > max0)
                                        {
                                            max0 = points;
                                            FR = r;
                                            FC = c;
                                            FinalRow = rK;
                                            FinalCol = cK;
                                        }
                                    }
                                    CopyBoard(CopySemi, Copy);
                                }
                            }
                            CopyBoard(a, CopySemi);
                        }
                    }
                }
            }
        }
        private void second(int[,] a, int r, int c)
        {
            int col = c;
            CopyBoard(a, CopySemi1);
            for (; r < Copy1.GetLength(0); r = r + 2)
            {
                for (c = col; c < Copy1.GetLength(1); c = c + 2)
                {
                    if (CopySemi1[r, c] == 3)
                    {
                        OptionsForBlack(CopySemi1, r, c);
                        CopyBoard(CopySemi1, Copy1);
                        if (c < Copy1.GetLength(1) - 1)
                        {
                            if (Copy1[r - 1, c + 1] == 4)
                            {
                                BlackMovement(Copy1, LeftorRight, r - 1, c + 1);
                                ShowWhiteOptions(Copy1, 0, 0);
                                ShowWhiteOptions(Copy1, 1, 1);
                                ShowWhite_KingOptions(Copy1, 0, 0);
                                ShowWhite_KingOptions(Copy1, 1, 1);
                                max1 = -6666;
                                third(Copy1, 0, 0);
                                third(Copy1, 1, 1);
                                if (max1 != -6666 && max1 < min0)
                                {
                                    min0 = max1;
                                }
                                CopyBoard(CopySemi1, Copy1);
                            }
                            if (Copy1[r - 1, c + 1] == -4 || Copy1[r - 1, c + 1] == -5)
                            {
                                BlackMovement(Copy1, LeftorRight, r - 2, c + 2);
                                ShowWhiteOptions(Copy1, 0, 0);
                                ShowWhiteOptions(Copy1, 1, 1);
                                ShowWhite_KingOptions(Copy1, 0, 0);
                                ShowWhite_KingOptions(Copy1, 1, 1);
                                max1 = -6666;
                                third(Copy1, 0, 0);
                                third(Copy1, 1, 1);
                                if (max1 != -6666 && max1 < min0)
                                {
                                    min0 = max1;
                                }
                                CopyBoard(CopySemi1, Copy1);
                            }
                        }
                        if (c > 0)
                        {
                            if (Copy1[r - 1, c - 1] == 4)
                            {
                                BlackMovement(Copy1, LeftorRight, r - 1, c - 1);
                                ShowWhiteOptions(Copy1, 0, 0);
                                ShowWhiteOptions(Copy1, 1, 1);
                                ShowWhite_KingOptions(Copy1, 0, 0);
                                ShowWhite_KingOptions(Copy1, 1, 1);
                                max1 = -6666;
                                third(Copy1, 0, 0);
                                third(Copy1, 1, 1);
                                if (max1 != -6666 && max1 < min0)
                                {
                                    min0 = max1;
                                }
                                CopyBoard(CopySemi1, Copy1);
                            }
                            if (Copy1[r - 1, c - 1] == -4 || Copy1[r - 1, c - 1] == -5)
                            {
                                BlackMovement(Copy1, LeftorRight, r - 2, c - 2);
                                ShowWhiteOptions(Copy1, 0, 0);
                                ShowWhiteOptions(Copy1, 1, 1);
                                ShowWhite_KingOptions(Copy1, 0, 0);
                                ShowWhite_KingOptions(Copy1, 1, 1);
                                max1 = -6666;
                                third(Copy1, 0, 0);
                                third(Copy1, 1, 1);
                                if (max1 < min0)
                                {
                                    min0 = max1;
                                }
                                CopyBoard(CopySemi1, Copy1);
                            }
                        }
                        CopyBoard(a, CopySemi1);
                    }
                    if (CopySemi1[r, c] == 5)
                    {
                        OptionsForBlack_King(CopySemi1, r, c);
                        CopyBoard(CopySemi1, Copy1);
                        for (int rK = r + 1, cK = c + 1; rK < Copy1.GetLength(0) && cK < Copy1.GetLength(1); rK++, cK++)
                        {
                            if (Copy1[rK, cK] != 6 && Copy1[rK, cK] != -4 && Copy1[rK, cK] != -5)
                            {
                                break;
                            }
                            if (Copy1[rK, cK] == -4 || Copy1[rK, cK] == -5)
                            {
                                rK++;
                                cK++;
                                Black_KingMovement(Copy1, UporDown, LeftorRight, rK, cK);
                                ShowWhiteOptions(Copy1, 0, 0);
                                ShowWhiteOptions(Copy1, 1, 1);
                                ShowWhite_KingOptions(Copy1, 0, 0);
                                ShowWhite_KingOptions(Copy1, 1, 1);
                                max1 = -6666;
                                third(Copy1, 0, 0);
                                third(Copy1, 1, 1);
                                if (max1 != -6666 && max1 < min0)
                                {
                                    min0 = max1;
                                }
                                CopyBoard(CopySemi1, Copy1);
                            }
                            if (Copy1[rK, cK] == 4)
                            {
                                Black_KingMovement(Copy1, UporDown, LeftorRight, rK, cK);
                                ShowWhiteOptions(Copy1, 0, 0);
                                ShowWhiteOptions(Copy1, 1, 1);
                                ShowWhite_KingOptions(Copy1, 0, 0);
                                ShowWhite_KingOptions(Copy1, 1, 1);
                                max1 = -6666;
                                third(Copy1, 0, 0);
                                third(Copy1, 1, 1);
                                if (max1 != -6666 && max1 < min0)
                                {
                                    min0 = max1;
                                }
                                CopyBoard(CopySemi1, Copy1);
                            }
                        }
                        for (int rK = r + 1, cK = c - 1; rK < Copy1.GetLength(0) && cK > 0; rK++, cK--)
                        {
                            if (Copy1[rK, cK] != 6 && Copy1[rK, cK] != -4 && Copy1[rK, cK] != -5)
                            {
                                break;
                            }
                            if (Copy1[rK, cK] == -4 || Copy1[rK, cK] == -5)
                            {
                                rK++;
                                cK--;
                                Black_KingMovement(Copy1, UporDown, LeftorRight, rK, cK);
                                ShowWhiteOptions(Copy1, 0, 0);
                                ShowWhiteOptions(Copy1, 1, 1);
                                ShowWhite_KingOptions(Copy1, 0, 0);
                                ShowWhite_KingOptions(Copy1, 1, 1);
                                max1 = -6666;
                                third(Copy1, 0, 0);
                                third(Copy1, 1, 1);
                                if (max1 != -6666 && max1 < min0)
                                {
                                    min0 = max1;
                                }
                                CopyBoard(CopySemi1, Copy1);
                            }
                            if (Copy1[rK, cK] == 4)
                            {
                                Black_KingMovement(Copy1, UporDown, LeftorRight, rK, cK);
                                ShowWhiteOptions(Copy1, 0, 0);
                                ShowWhiteOptions(Copy1, 1, 1);
                                ShowWhite_KingOptions(Copy1, 0, 0);
                                ShowWhite_KingOptions(Copy1, 1, 1);
                                max1 = -6666;
                                third(Copy1, 0, 0);
                                third(Copy1, 1, 1);
                                if (max1 != -6666 && max1 < min0)
                                {
                                    min0 = max1;
                                }
                                CopyBoard(CopySemi1, Copy1);
                            }
                        }
                        for (int rK = r - 1, cK = c + 1; rK > 0 && cK < Copy1.GetLength(1); rK--, cK++)
                        {
                            if (Copy1[rK, cK] != 6 && Copy1[rK, cK] != -4 && Copy1[rK, cK] != -5)
                            {
                                break;
                            }
                            if (Copy1[rK, cK] == -4 || Copy1[rK, cK] == -5)
                            {
                                rK--;
                                cK++;
                                Black_KingMovement(Copy1, UporDown, LeftorRight, rK, cK);
                                ShowWhiteOptions(Copy1, 0, 0);
                                ShowWhiteOptions(Copy1, 1, 1);
                                ShowWhite_KingOptions(Copy1, 0, 0);
                                ShowWhite_KingOptions(Copy1, 1, 1);
                                max1 = -6666;
                                third(Copy1, 0, 0);
                                third(Copy1, 1, 1);
                                if (max1 != -6666 && max1 < min0)
                                {
                                    min0 = max1;
                                }
                                CopyBoard(CopySemi1, Copy1);
                            }
                            if (Copy1[rK, cK] == 4)
                            {
                                Black_KingMovement(Copy1, UporDown, LeftorRight, rK, cK);
                                ShowWhiteOptions(Copy1, 0, 0);
                                ShowWhiteOptions(Copy1, 1, 1);
                                ShowWhite_KingOptions(Copy1, 0, 0);
                                ShowWhite_KingOptions(Copy1, 1, 1);
                                max1 = -6666;
                                third(Copy1, 0, 0);
                                third(Copy1, 1, 1);
                                if (max1 != -6666 && max1 < min0)
                                {
                                    min0 = max1;
                                }
                                CopyBoard(CopySemi1, Copy1);
                            }
                        }
                        for (int rK = r - 1, cK = c - 1; rK > 0 && cK > 0; rK--, cK--)
                        {
                            if (Copy1[rK, cK] != 6 && Copy1[rK, cK] != -4 && Copy1[rK, cK] != -5)
                            {
                                break;
                            }
                            if (Copy1[rK, cK] == -4 || Copy1[rK, cK] == -5)
                            {
                                rK--;
                                cK--;
                                Black_KingMovement(Copy1, UporDown, LeftorRight, rK, cK);
                                ShowWhiteOptions(Copy1, 0, 0);
                                ShowWhiteOptions(Copy1, 1, 1);
                                ShowWhite_KingOptions(Copy1, 0, 0);
                                ShowWhite_KingOptions(Copy1, 1, 1);
                                max1 = -6666;
                                third(Copy1, 0, 0);
                                third(Copy1, 1, 1);
                                if (max1 != -6666 && max1 < min0)
                                {
                                    min0 = max1;
                                }
                                CopyBoard(CopySemi1, Copy1);
                            }
                            if (Copy1[rK, cK] == 4)
                            {
                                Black_KingMovement(Copy1, UporDown, LeftorRight, rK, cK);
                                ShowWhiteOptions(Copy1, 0, 0);
                                ShowWhiteOptions(Copy1, 1, 1);
                                ShowWhite_KingOptions(Copy1, 0, 0);
                                ShowWhite_KingOptions(Copy1, 1, 1);
                                max1 = -6666;
                                third(Copy1, 0, 0);
                                third(Copy1, 1, 1);
                                if (max1 != -6666 && max1 < min0)
                                {
                                    min0 = max1;
                                }
                                CopyBoard(CopySemi1, Copy1);
                            }
                        }
                        CopyBoard(a, CopySemi1);
                    }
                }
            }
        }
        private void third(int[,] a, int r, int c)
        {
            int col = c;
            CopyBoard(a, CopySemi2);
            for (; r < Copy2.GetLength(0); r = r + 2)
            {
                for (c = col; c < Copy2.GetLength(1); c = c + 2)
                {
                    if (CopySemi2[r, c] == 3)
                    {
                        OptionsForWhite(CopySemi2, r, c);
                        CopyBoard(CopySemi2, Copy2);
                        if (c < Copy2.GetLength(1) - 1)
                        {
                            if (Copy2[r + 1, c + 1] == 4)
                            {
                                WhiteMovement(Copy2, LeftorRight, r + 1, c + 1);
                                points = TheScoreThatTheBoardReturns(Copy2);
                                if (points > max1)
                                {
                                    max1 = points;
                                }
                                CopyBoard(CopySemi2, Copy2);
                            }
                            if (Copy2[r + 1, c + 1] == -4)
                            {
                                WhiteMovement(Copy2, LeftorRight, r + 2, c + 2);
                                points = TheScoreThatTheBoardReturns(Copy2);
                                if (points > max1)
                                {
                                    max1 = points;
                                }
                                CopyBoard(CopySemi2, Copy2);
                            }
                        }
                        if (c > 0)
                        {
                            if (Copy2[r + 1, c - 1] == 4)
                            {
                                WhiteMovement(Copy2, LeftorRight, r + 1, c - 1);
                                points = TheScoreThatTheBoardReturns(Copy2);
                                if (points > max1)
                                {
                                    max1 = points;
                                }
                                CopyBoard(CopySemi2, Copy2);
                            }
                            if (Copy2[r + 1, c - 1] == -4)
                            {
                                WhiteMovement(Copy2, LeftorRight, r + 2, c - 2);
                                points = TheScoreThatTheBoardReturns(Copy2);
                                if (points > max1)
                                {
                                    max1 = points;
                                }
                                CopyBoard(CopySemi2, Copy2);
                            }
                        }
                        CopyBoard(a, CopySemi2);
                    }
                    if (CopySemi2[r, c] == 5)
                    {
                        OptionsForWhite_King(CopySemi2, r, c);
                        CopyBoard(CopySemi2, Copy2);
                        for (int rK = r + 1, cK = c + 1; rK < Copy2.GetLength(0) && cK < Copy2.GetLength(1); rK++, cK++)
                        {
                            if (Copy2[rK, cK] != 6 && Copy2[rK, cK] != -4 && Copy2[rK, cK] != -5)
                            {
                                break;
                            }
                            if (Copy2[rK, cK] == -4 || Copy2[rK, cK] == -5)
                            {
                                rK++;
                                cK++;
                                White_KingMovement(Copy2, UporDown, LeftorRight, rK, cK);
                                points = TheScoreThatTheBoardReturns(Copy2);
                                if (points > max1)
                                {
                                    max1 = points;
                                }
                                CopyBoard(CopySemi2, Copy2);
                            }
                            if (Copy2[rK, cK] == 4)
                            {
                                White_KingMovement(Copy2, UporDown, LeftorRight, rK, cK);
                                points = TheScoreThatTheBoardReturns(Copy2);
                                if (points > max1)
                                {
                                    max1 = points;
                                }
                                CopyBoard(CopySemi2, Copy2);
                            }
                        }
                        for (int rK = r + 1, cK = c - 1; rK < Copy2.GetLength(0) && cK > 0; rK++, cK--)
                        {
                            if (Copy2[rK, cK] != 6 && Copy2[rK, cK] != -4 && Copy2[rK, cK] != -5)
                            {
                                break;
                            }
                            if (Copy2[rK, cK] == -4 || Copy2[rK, cK] == -5)
                            {
                                rK++;
                                cK--;
                                White_KingMovement(Copy2, UporDown, LeftorRight, rK, cK);
                                points = TheScoreThatTheBoardReturns(Copy2);
                                if (points > max1)
                                {
                                    max1 = points;
                                }
                                CopyBoard(CopySemi2, Copy2);
                            }
                            if (Copy2[rK, cK] == 4)
                            {
                                White_KingMovement(Copy2, UporDown, LeftorRight, rK, cK);
                                points = TheScoreThatTheBoardReturns(Copy2);
                                if (points > max1)
                                {
                                    max1 = points;
                                }
                                CopyBoard(CopySemi2, Copy2);
                            }
                        }
                        for (int rK = r - 1, cK = c + 1; rK > 0 && cK < Copy2.GetLength(1); rK--, cK++)
                        {
                            if (Copy2[rK, cK] != 6 && Copy2[rK, cK] != -4 && Copy2[rK, cK] != -5)
                            {
                                break;
                            }
                            if (Copy2[rK, cK] == -4 || Copy2[rK, cK] == -5)
                            {
                                rK--;
                                cK++;
                                White_KingMovement(Copy2, UporDown, LeftorRight, rK, cK);
                                points = TheScoreThatTheBoardReturns(Copy2);
                                if (points > max1)
                                {
                                    max1 = points;
                                }
                                CopyBoard(CopySemi2, Copy2);
                            }
                            if (Copy2[rK, cK] == 4)
                            {
                                White_KingMovement(Copy2, UporDown, LeftorRight, rK, cK);
                                points = TheScoreThatTheBoardReturns(Copy2);
                                if (points > max1)
                                {
                                    max1 = points;
                                }
                                CopyBoard(CopySemi2, Copy2);
                            }
                        }
                        for (int rK = r - 1, cK = c - 1; rK > 0 && cK > 0; rK--, cK--)
                        {
                            if (Copy2[rK, cK] != 6 && Copy2[rK, cK] != -4 && Copy2[rK, cK] != -5)
                            {
                                break;
                            }
                            if (Copy2[rK, cK] == -4 || Copy2[rK, cK] == -5)
                            {
                                rK--;
                                cK--;
                                White_KingMovement(Copy2, UporDown, LeftorRight, rK, cK);
                                points = TheScoreThatTheBoardReturns(Copy2);
                                if (points > max1)
                                {
                                    max1 = points;
                                }
                                CopyBoard(CopySemi2, Copy2);
                            }
                            if (Copy2[rK, cK] == 4)
                            {
                                White_KingMovement(Copy2, UporDown, LeftorRight, rK, cK);
                                points = TheScoreThatTheBoardReturns(Copy2);
                                if (points > max1)
                                {
                                    max1 = points;
                                }
                                CopyBoard(CopySemi2, Copy2);
                            }
                        }
                        CopyBoard(a, CopySemi2);
                    }
                }
            }
        }
        #endregion
        #endregion
        private void CheckWin(int[,] a)
        {
            int CheckIfTheBlackLost = 0;
            int CheckIfTheWhiteLost = 0;
            for (int r = 0; r < a.GetLength(0); r = r + 2)
            {
                for (int c = 0; c < a.GetLength(0); c = c + 2)
                {
                    if (a[r, c] == 1 || a[r, c] == 2)
                    {
                        CheckIfTheWhiteLost = 1;
                    }
                    if (a[r, c] == -1 || a[r, c] == -2)
                    {
                        CheckIfTheBlackLost = 1;
                    }
                    if (CheckIfTheBlackLost == 1 && CheckIfTheWhiteLost == 1)
                    {
                        break;
                    }
                }
                if (CheckIfTheBlackLost == 1 && CheckIfTheWhiteLost == 1)
                {
                    break;
                }
            }
            for (int r = 1; r < a.GetLength(0); r = r + 2)
            {
                for (int c = 1; c < a.GetLength(0); c = c + 2)
                {
                    if (a[r, c] == 1 || a[r, c] == 2)
                    {
                        CheckIfTheWhiteLost = 1;
                    }
                    if (a[r, c] == -1 || a[r, c] == -2)
                    {
                        CheckIfTheBlackLost = 1;
                    }
                    if (CheckIfTheBlackLost == 1 && CheckIfTheWhiteLost == 1)
                    {
                        break;
                    }
                }
                if (CheckIfTheBlackLost == 1 && CheckIfTheWhiteLost == 1)
                {
                    break;
                }
            }
            if (CheckIfTheWhiteLost == 0)
            {
                MessageBox.Show("The black won!");
                this.Close();
            }
            if (CheckIfTheBlackLost == 0)
            {
                MessageBox.Show("The white won!");
                this.Close();
            }
        }
        private void Aginst_Computer_Load(object sender, EventArgs e)
        {
            this.Controls.Remove(button2);
            this.Controls.Remove(button3);
        }
    }
}
