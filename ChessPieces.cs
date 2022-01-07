namespace Chess
{
    public enum PieceColor
    {
        white,
        black,
        none //沒有棋子
    }

    public abstract class ChessPieces
    {
        protected int x, y;
        protected PieceColor color;

        public int X => x;
        public int Y => y;
        public PieceColor Color => color;

        public ChessPieces(int set_x, int set_y, PieceColor set_color) =>
            (x, y, color) = (set_x, set_y, set_color);

        //可以走
        public abstract bool[,] AbleToMove(ChessPieces[,] table);

        //轉字串
        public override string ToString() =>
            (color == PieceColor.white ? "W" : "B") + GetType().Name;
    }

    //國王
    public class King : ChessPieces
    {
        private bool can_castle; //可否入堡

        public King(int set_x, int set_y, PieceColor set_color)
            : base(set_x, set_y, set_color)
        {
            can_castle = false;
        }

        public override bool[,] AbleToMove(ChessPieces[,] table)
        {
            bool[,] able_to_move = Tools.GenerateBoolArray();

            //走訪自身為中心3x3
            for (int i = x - 1; i < x + 1; i++)
                for (int j = y - 1; j < y + 1; j++)
                    if (Tools.InTable(i, j) && (i != x || j != y)) //在棋盤內 且不可和棋子本身同一格
                        able_to_move[i, j] = table[i, j].Color != color; //如果顏色和我的顏色不一樣 (沒有棋子是none) 就可以走

            if (can_castle) //如果可以入堡
                able_to_move[x - 2, y] = able_to_move[x + 2, y] = true;

            return able_to_move;
        }

        public bool CanCastle
        {
            set => can_castle = value;
        }
    }

    //王后
    public class Queen : ChessPieces
    {
        public Queen(int set_x, int set_y, PieceColor set_color)
            : base(set_x, set_y, set_color) { }

        public override bool[,] AbleToMove(ChessPieces[,] table)
        {
            bool[,] able_to_move = Tools.GenerateBoolArray();

            int i, j; //走訪要用
            //向左上走
            for (i = x - 1, j = y - 1; i >= 0 && j >= 0; i--, j--)
            {
                if (table[i, j].Color != color) //如果顏色和我的顏色不一樣 (沒有棋子是none)
                    able_to_move[i, j] = true; //可以走
                if (table[i, j].Color != PieceColor.none) //有棋子擋著
                    break; //結束這個迴圈
            }
            //向上走
            for (j = y - 1; j >= 0; j--)
            {
                if (table[x, j].Color != color) //如果顏色和我的顏色不一樣 (沒有棋子是none)
                    able_to_move[x, j] = true; //可以走
                if (table[x, j].Color != PieceColor.none) //有棋子擋著
                    break; //結束這個迴圈
            }
            //向右上走
            for (i = x + 1, j = y - 1; i <= 7 && j >= 0; i++, j--)
            {
                if (table[i, j].Color != color) //如果顏色和我的顏色不一樣 (沒有棋子是none)
                    able_to_move[i, j] = true; //可以走
                if (table[i, j].Color != PieceColor.none) //有棋子擋著
                    break; //結束這個迴圈
            }
            //向右走
            for (i = x + 1; i <= 7; i++)
            {
                if (table[i, y].Color != color) //如果顏色和我的顏色不一樣 (沒有棋子是none)
                    able_to_move[i, y] = true; //可以走
                if (table[i, y].Color != PieceColor.none) //有棋子擋著
                    break; //結束這個迴圈
            }
            //向右下走
            for (i = x + 1, j = y + 1; i <= 7 && j <= 7; i++, j++)
            {
                if (table[i, j].Color != color) //如果顏色和我的顏色不一樣 (沒有棋子是none)
                    able_to_move[i, j] = true; //可以走
                if (table[i, j].Color != PieceColor.none) //有棋子擋著
                    break; //結束這個迴圈
            }
            //向下走
            for (j = y + 1; j <= 7; j++)
            {
                if (table[x, j].Color != color) //如果顏色和我的顏色不一樣 (沒有棋子是none)
                    able_to_move[x, j] = true; //可以走
                if (table[x, j].Color != PieceColor.none) //有棋子擋著
                    break; //結束這個迴圈
            }
            //向左下走
            for (i = x - 1, j = y + 1; i >= 0 && j <= 7; i--, j++)
            {
                if (table[i, j].Color != color) //如果顏色和我的顏色不一樣 (沒有棋子是none)
                    able_to_move[i, j] = true; //可以走
                if (table[i, j].Color != PieceColor.none) //有棋子擋著
                    break; //結束這個迴圈
            }
            //向左走
            for (i = x - 1; i >= 0; i--)
            {
                if (table[i, y].Color != color) //如果顏色和我的顏色不一樣 (沒有棋子是none)
                    able_to_move[i, y] = true; //可以走
                if (table[i, y].Color != PieceColor.none) //有棋子擋著
                    break; //結束這個迴圈
            }

            return able_to_move;
        }
    }

    public class Bishop : ChessPieces
    {
        public Bishop(int set_x, int set_y, PieceColor set_color)
            : base(set_x, set_y, set_color) { }

        public override bool[,] AbleToMove(ChessPieces[,] table)
        {
            bool[,] able_to_move = Tools.GenerateBoolArray();

            int i, j; //走訪要用
            //向左上走
            for (i = x - 1, j = y - 1; i >= 0 && j >= 0; i--, j--)
            {
                if (table[i, j].Color != color) //如果顏色和我的顏色不一樣 (沒有棋子是none)
                    able_to_move[i, j] = true; //可以走
                if (table[i, j].Color != PieceColor.none) //有棋子擋著
                    break; //結束這個迴圈
            }
            //向右上走
            for (i = x + 1, j = y - 1; i <= 7 && j >= 0; i++, j--)
            {
                if (table[i, j].Color != color) //如果顏色和我的顏色不一樣 (沒有棋子是none)
                    able_to_move[i, j] = true; //可以走
                if (table[i, j].Color != PieceColor.none) //有棋子擋著
                    break; //結束這個迴圈
            }
            //向右下走
            for (i = x + 1, j = y + 1; i <= 7 && j <= 7; i++, j++)
            {
                if (table[i, j].Color != color) //如果顏色和我的顏色不一樣 (沒有棋子是none)
                    able_to_move[i, j] = true; //可以走
                if (table[i, j].Color != PieceColor.none) //有棋子擋著
                    break; //結束這個迴圈
            }
            //向左下走
            for (i = x - 1, j = y + 1; i >= 0 && j <= 7; i--, j++)
            {
                if (table[i, j].Color != color) //如果顏色和我的顏色不一樣 (沒有棋子是none)
                    able_to_move[i, j] = true; //可以走
                if (table[i, j].Color != PieceColor.none) //有棋子擋著
                    break; //結束這個迴圈
            }

            return able_to_move;
        }
    }

    public class Knight : ChessPieces
    {
        public Knight(int set_x, int set_y, PieceColor set_color)
            : base(set_x, set_y, set_color) { }

        public override bool[,] AbleToMove(ChessPieces[,] table)
        {
            bool[,] able_to_move = Tools.GenerateBoolArray();

            int i, j;
            //左上左
            i = x - 2;
            j = y - 1;
            if (Tools.InTable(i, j))
                able_to_move[i, j] = table[i, j].Color != color; //如果顏色和我的顏色不一樣 (沒有棋子是none)
            //左上上
            i = x - 1;
            j = y - 2;
            if (Tools.InTable(i, j))
                able_to_move[i, j] = table[i, j].Color != color;
            //右上上
            i = x + 1;
            j = y - 2;
            if (Tools.InTable(i, j))
                able_to_move[i, j] = table[i, j].Color != color;
            //右上右
            i = x + 2;
            j = y - 1;
            if (Tools.InTable(i, j))
                able_to_move[i, j] = table[i, j].Color != color;
            //右下右
            i = x + 2;
            j = y + 1;
            if (Tools.InTable(i, j))
                able_to_move[i, j] = table[i, j].Color != color;
            //右下下
            i = x + 1;
            j = y + 2;
            if (Tools.InTable(i, j))
                able_to_move[i, j] = table[i, j].Color != color;
            //左下下
            i = x - 1;
            j = y + 2;
            if (Tools.InTable(i, j))
                able_to_move[i, j] = table[i, j].Color != color;
            //左下左
            i = x - 2;
            j = y + 1;
            if (Tools.InTable(i, j))
                able_to_move[i, j] = table[i, j].Color != color;

            return able_to_move;
        }
    }

    public class Rook : ChessPieces
    {
        public Rook(int set_x, int set_y, PieceColor set_color)
            : base(set_x, set_y, set_color) { }

        public override bool[,] AbleToMove(ChessPieces[,] table)
        {
            bool[,] able_to_move = Tools.GenerateBoolArray();

            int i, j;
            //向左走
            for (i = x - 1; i >= 0; i--)
            {
                if (table[i, y].Color != color) //如果顏色和我的顏色不一樣 (沒有棋子是none)
                    able_to_move[i, y] = true; //可以走
                if (table[i, y].Color != PieceColor.none) //有棋子擋著
                    break; //結束這個迴圈
            }
            //向上走
            for (j = y - 1; j >= 0; j--)
            {
                if (table[x, j].Color != color) //如果顏色和我的顏色不一樣 (沒有棋子是none)
                    able_to_move[x, j] = true; //可以走
                if (table[x, j].Color != PieceColor.none) //有棋子擋著
                    break; //結束這個迴圈
            }
            //向右走
            for (i = x + 1; i <= 7; i++)
            {
                if (table[i, y].Color != color) //如果顏色和我的顏色不一樣 (沒有棋子是none)
                    able_to_move[i, y] = true; //可以走
                if (table[i, y].Color != PieceColor.none) //有棋子擋著
                    break; //結束這個迴圈
            }
            //向下走
            for (j = y + 1; j <= 7; j++)
            {
                if (table[x, j].Color != color) //如果顏色和我的顏色不一樣 (沒有棋子是none)
                    able_to_move[x, j] = true; //可以走
                if (table[x, j].Color != PieceColor.none) //有棋子擋著
                    break; //結束這個迴圈
            }

            return able_to_move;
        }
    }

    public class Pawn : ChessPieces
    {
        private bool can_jump_two;

        public Pawn(int set_x, int set_y, PieceColor set_color)
            : base(set_x, set_y, set_color)
        {
            can_jump_two = true;
        }

        public override bool[,] AbleToMove(ChessPieces[,] table)
        {
            bool[,] able_to_move = Tools.GenerateBoolArray();

            if (color == PieceColor.white)
            {
                //如果前面一格是存在的
                if (Tools.InTable(x, y - 1))
                {
                    //如果還沒走過第一步
                    if (can_jump_two) //檢查前兩格 第二格 = (第一格 = 第一格是不是空的) && 第二格是不是空的
                        able_to_move[x, y - 2] = (able_to_move[x, y - 1] = table[x, y - 1].Color == PieceColor.none) && (table[x, y - 2].Color == PieceColor.none);
                    else //檢查前面一格
                        able_to_move[x, y - 1] = table[x, y - 1].Color == PieceColor.none;
                }

                if (Tools.InTable(x - 1, y - 1)) //左前方有敵人
                    able_to_move[x - 1, y - 1] = table[x - 1, y - 1].Color == PieceColor.black;

                if (Tools.InTable(x + 1, y - 1)) //右前方有敵人
                    able_to_move[x + 1, y - 1] = table[x + 1, y - 1].Color == PieceColor.black;
            }
            else //if (color == PieceColor.black)
            {
                //如果前面一格是存在的
                if (Tools.InTable(x, y + 1))
                {
                    //如果還沒走過第一步
                    if (can_jump_two) //檢查前兩格 第二格 = (第一格 = 第一格是不是空的) && 第二格是不是空的
                        able_to_move[x, y + 2] = (able_to_move[x, y + 1] = table[x, y + 1].Color == PieceColor.none) && (table[x, y + 2].Color == PieceColor.none);
                    else //檢查前面一格
                        able_to_move[x, y + 1] = table[x, y + 1].Color == PieceColor.none;
                }

                if (Tools.InTable(x - 1, y + 1)) //左前方有敵人
                    able_to_move[x - 1, y + 1] = table[x - 1, y + 1].Color == PieceColor.black;

                if (Tools.InTable(x + 1, y + 1)) //右前方有敵人
                    able_to_move[x + 1, y + 1] = table[x + 1, y + 1].Color == PieceColor.black;

            }

            return able_to_move;
        }

        public bool CanJumpTwo
        {
            set => can_jump_two = value;
        }
    }

    public class None : ChessPieces
    {
        public None(int set_x, int set_y, PieceColor set_color)
            : base(set_x, set_y, set_color) { }

        public override bool[,] AbleToMove(ChessPieces[,] table)
        {
            return null;
        }
    }

    internal class Tools
    {
        //產生棋盤boolean陣列
        //儲存每個點是否可以下
        //預設全為false
        internal static bool[,] GenerateBoolArray()
        {
            bool[,] new_array = new bool[8, 8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    new_array[i, j] = false;
            return new_array;
        }

        //檢測傳入的座標是否在棋盤內
        //0 <= x <= 7 && 0 <= y <= 7
        internal static bool InTable(int x, int y) =>
            x >= 0 && x <= 7 && y >= 0 && y <= 7;
    }
}
