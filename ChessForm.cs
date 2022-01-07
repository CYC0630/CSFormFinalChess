using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chess;

namespace Final
{
    public partial class ChessForm : Form
    {
        private readonly ChessPieces[,] table; //棋盤
        private readonly Button[,] buttons; //按鈕
        private readonly List<Place> mark_as_green; //記錄有哪些格數被標為綠色 可以走
        private bool[,] can_move_point; //棋盤上有哪些點可以走
        private bool move_stage; //正在試圖移動棋子
        private PieceColor now_color; //現在輪到誰了
        private ChessPieces target_piece; //要動的棋子

        private const int SIZE_X = 8;
        private const int SIZE_Y = 8;

        public ChessForm()
        {
            InitializeComponent();

            table = new ChessPieces[SIZE_X, SIZE_Y]
            {
                {
                    new Rook(0, 0, PieceColor.black),
                    new Pawn(0, 1, PieceColor.black),
                    new None(0, 2, PieceColor.none),
                    new None(0, 3, PieceColor.none),
                    new None(0, 4, PieceColor.none),
                    new None(0, 5, PieceColor.none),
                    new Pawn(0, 6, PieceColor.white),
                    new Rook(0, 7, PieceColor.white)
                },
                {
                    new Knight(1, 0, PieceColor.black),
                    new Pawn(1, 1, PieceColor.black),
                    new None(1, 2, PieceColor.none),
                    new None(1, 3, PieceColor.none),
                    new None(1, 4, PieceColor.none),
                    new None(1, 5, PieceColor.none),
                    new Pawn(1, 6, PieceColor.white),
                    new Knight(1, 7, PieceColor.white)
                },
                {
                    new Bishop(2, 0, PieceColor.black),
                    new Pawn(2, 1, PieceColor.black),
                    new None(2, 2, PieceColor.none),
                    new None(2, 3, PieceColor.none),
                    new None(2, 4, PieceColor.none),
                    new None(2, 5, PieceColor.none),
                    new Pawn(2, 6, PieceColor.white),
                    new Bishop(2, 7, PieceColor.white)
                },
                {
                    new Queen(3, 0, PieceColor.black),
                    new Pawn(3, 1, PieceColor.black),
                    new None(3, 2, PieceColor.none),
                    new None(3, 3, PieceColor.none),
                    new None(3, 4, PieceColor.none),
                    new None(3, 5, PieceColor.none),
                    new Pawn(3, 6, PieceColor.white),
                    new Queen(3, 7, PieceColor.white)
                },
                {
                    new King(4, 0, PieceColor.black),
                    new Pawn(4, 1, PieceColor.black),
                    new None(4, 2, PieceColor.none),
                    new None(4, 3, PieceColor.none),
                    new None(4, 4, PieceColor.none),
                    new None(4, 5, PieceColor.none),
                    new Pawn(4, 6, PieceColor.white),
                    new King(4, 7, PieceColor.white)
                },
                {
                    new Bishop(5, 0, PieceColor.black),
                    new Pawn(5, 1, PieceColor.black),
                    new None(5, 2, PieceColor.none),
                    new None(5, 3, PieceColor.none),
                    new None(5, 4, PieceColor.none),
                    new None(5, 5, PieceColor.none),
                    new Pawn(5, 6, PieceColor.white),
                    new Bishop(5, 7, PieceColor.white)
                },
                {
                    new Knight(6, 0, PieceColor.black),
                    new Pawn(6, 1, PieceColor.black),
                    new None(6, 2, PieceColor.none),
                    new None(6, 3, PieceColor.none),
                    new None(6, 4, PieceColor.none),
                    new None(6, 5, PieceColor.none),
                    new Pawn(6, 6, PieceColor.white),
                    new Knight(6, 7, PieceColor.white)
                },
                {
                    new Rook(7, 0, PieceColor.black),
                    new Pawn(7, 1, PieceColor.black),
                    new None(7, 2, PieceColor.none),
                    new None(7, 3, PieceColor.none),
                    new None(7, 4, PieceColor.none),
                    new None(7, 5, PieceColor.none),
                    new Pawn(7, 6, PieceColor.white),
                    new Rook(7, 7, PieceColor.white)
                }
            };

            buttons = new Button[,]//[SIZE_X, SIZE_Y]
            {
                { A8,A7,A6,A5,A4,A3,A2,A1 },
                { B8,B7,B6,B5,B4,B3,B2,B1 },
                { C8,C7,C6,C5,C4,C3,C2,C1 },
                { D8,D7,D6,D5,D4,D3,D2,D1 },
                { E8,E7,E6,E5,E4,E3,E2,E1 },
                { F8,F7,F6,F5,F4,F3,F2,F1 },
                { G8,G7,G6,G5,G4,G3,G2,G1 },
                { H8,H7,H6,H5,H4,H3,H2,H1 }
            };

            mark_as_green = new List<Place>();

            move_stage = false; //一開始當然不在移動階段 是在選擇棋子階段

            now_color = PieceColor.white; //規則是白棋先走
        }

        private void Checkmate(PieceColor winner)
        {

        }

        private void OnPieceClick(object sender, EventArgs e)
        {
            Button button = sender as Button;

            //每個棋子都有他們的tag 記錄這個棋子的位址
            //左上角的是00
            //右上角的是70
            //左下角的是07
            //右下角的是77
            int x = int.Parse(button.Tag.ToString());
            int y = x % 10;
            x /= 10;

            ChessPieces piece = table[x, y]; //取得點擊的棋子

            if (move_stage) //落子階段
            {
                foreach (Place p in mark_as_green)
                    buttons[p.x, p.y].BackColor = p.color; //恢復棋盤底色

                //mark_as_green.Find()

                bool checkmate = false;
                foreach (Place p in mark_as_green) //走訪所有合法路徑
                {
                    if (x == p.x && y == p.y) //走了合法路徑
                    {
                        int last_x = target_piece.X; //選子階段點的棋子 X
                        int last_y = target_piece.Y; //選子階段點的棋子 Y

                        checkmate = piece is King; //如果被吃掉的是國王

                        table[x, y] = table[last_x, last_y]; //棋子走過去
                        buttons[x, y].BackgroundImage = buttons[last_x, last_y].BackgroundImage; //圖像轉移過去

                        table[last_x, last_y] = new None(last_x, last_y, PieceColor.none); //原本的變成無棋子
                        buttons[last_x, last_y].BackgroundImage = null; //原本的變成無圖像

                        break; //不須再比較 離開for迴圈
                    }
                }

                if (checkmate) //國王被吃掉
                    Checkmate(now_color);

                now_color = now_color == PieceColor.white ? PieceColor.black : PieceColor.white; //切換顏色
                move_stage = false; //回到選子階段

            }
            else //這個點擊是為了選擇棋子
            {
                if (piece.Color == PieceColor.none) //該點沒有棋子 顏色是none
                    return;

                //輪到白棋卻點了黑棋
                if (now_color == PieceColor.white && piece.Color == PieceColor.black)
                    return;

                //輪到黑棋卻點了白棋
                if (now_color == PieceColor.black && piece.Color == PieceColor.white)
                    return;

                target_piece = piece; //紀錄下是這個棋子要動
                can_move_point = piece.AbleToMove(table); //找出可以移動的點

                mark_as_green.Clear(); //清空list內的綠色標記
                for (int i = 0; i < SIZE_X; i++) //走訪棋盤上所有可以走的位置
                {
                    for (int j = 0; j < SIZE_Y; j++)
                    {
                        if (can_move_point[i, j])
                        {
                            mark_as_green.Add(new Place(i, j, buttons[i, j].BackColor)); //將綠色標記加入list
                            buttons[i, j].BackColor = Color.Green; //將棋盤上可以走的地方變成綠色
                        }
                    }
                }

                move_stage = true; //進入落子階段
            }
        }

        private void MenuRestart_Click(object sender, EventArgs e)
        {

        }

        private void MenuCancelSelect_Click(object sender, EventArgs e)
        {
            if (move_stage)
            {
                foreach (Place p in mark_as_green)
                    buttons[p.x, p.y].BackColor = p.color; //恢復棋盤底色
                move_stage = false;
            }
        }
    }

    internal class Place
    {
        public Place(int set_x, int set_y, Color set_color)
        {
            x = set_x;
            y = set_y;
            color = set_color;
        }

        public int x;
        public int y;
        internal Color color;
    }
}
