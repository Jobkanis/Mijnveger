using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mijnveger
{


    class leveldata
    {
        public List<Tile> positions;

        Game_Manager gamemanager;
        Tile emptypos;
        public int x_num;
        public int y_num;

        public Position realposition = new Position(0,0);
        public Size realsize = new Size(0,0);

        public leveldata(Game_Manager gamemanager, Dificulty dificulty)
        {
            positions = new List<Tile>();
            emptypos = new Tile(gamemanager, new Position(-1, -1), false);
            this.gamemanager = gamemanager;
            this.emptypos = gamemanager.EmptyTile;

            int bomb_num;

            Random rnd;
            switch (dificulty)
            {
                case (Dificulty.normal):
                    {
                        x_num = 20;
                        y_num = 15;
                        bomb_num = 30;
                        break;
                    }

                default:
                    {
                        x_num = 0;
                        y_num = 0;
                        bomb_num = 0;
                        break;
                    }
            }

            // creating tiles

            rnd = new Random();
            int tiles_amount = x_num * y_num;

            int tiles_left = tiles_amount;
            int bombs_left = bomb_num;

            for (int y = 0; y < y_num; y++)
            {
                for (int x = 0; x < x_num; x++)
                {
                    bool IsBomb = false;
                    if (bombs_left > 0)
                    {
                        int LocalRand = rnd.Next(0, (int)(tiles_left / bombs_left));
                        if (LocalRand == 0) { IsBomb = true; bombs_left--; }
                    }



                    positions.Add(new Tile(gamemanager, new Position(x, y), IsBomb));

                    tiles_left--;
                }
            }

            foreach (Tile tile in positions)
            {
                int tile_num = 0;
                // bomb = false on emptypos

                if (FindPosition(tile.position.x + 1, tile.position.y - 1).isbomb) { tile_num++; }
                if (FindPosition(tile.position.x + 1, tile.position.y + 0).isbomb) { tile_num++; }
                if (FindPosition(tile.position.x + 1, tile.position.y + 1).isbomb) { tile_num++; }

                if (FindPosition(tile.position.x - 1, tile.position.y - 1).isbomb) { tile_num++; }
                if (FindPosition(tile.position.x - 1, tile.position.y + 0).isbomb) { tile_num++; }
                if (FindPosition(tile.position.x - 1, tile.position.y + 1).isbomb) { tile_num++; }

                if (FindPosition(tile.position.x + 0, tile.position.y - 1).isbomb) { tile_num++; }
                if (FindPosition(tile.position.x + 0, tile.position.y + 1).isbomb) { tile_num++; }

                tile.VisualNum = tile_num;
            }

        }

        public Tile FindPosition(int x, int y)
        {
            foreach (Tile tile in positions)
            {
                if (tile.position.x == x && tile.position.y == y)
                {
                    return tile;
                }
            }
            return emptypos;
        }

        public Tile GetIntersectingtile(Vector2 vector2)
        {
            foreach (Tile tile in positions)
            {
                if (vector2.X > tile.real_position.x && vector2.Y > tile.real_position.y &&
                vector2.X < tile.real_position.x + tile.real_size.x && vector2.Y < tile.real_position.y + tile.real_size.y)
                {
                    return tile;
                }
            }
            return emptypos;
        }

        public void HandleHighlights(ReturnInput input, bool controllsenabled)
        {
            Vector2 vector2 = input.cursor;
            foreach (Tile tile in positions)
            {
                if (vector2.X > tile.real_position.x && vector2.Y > tile.real_position.y &&
                 vector2.X < tile.real_position.x + tile.real_size.x && vector2.Y < tile.real_position.y + tile.real_size.y && controllsenabled && gamemanager.SelectedTile == gamemanager.EmptyTile) 
                {
                    tile.MouseOn();
                }
                else
                {
                    tile.MouseOff();
                }

            }
        }

        public bool CheckIfDone()
        {
            int to_go = 0;
            foreach (Tile tile in positions)
            {
                if (tile.opened == false && tile.isbomb == false)
                {
                    to_go++;
                }
            }

            return (to_go == 0);
        }


        public bool OnBomb(float time, float dt)
        {
            int tilesPerSec = (int)(0.5 + ((x_num * y_num) / (time / dt)));

            foreach (Tile tile in positions)
            {
                if (tile.opened == false)
                {
                    tile.marked = false;
                    tile.opened = true;
                    tilesPerSec--;
                    if (tilesPerSec <= 0) { return false; }
                }
            }
            return true;
            /*
            // x-line moving down
            bool Check = true;
            for (int y = 0; y < y_num && Check; y++)
            {
                for (int x = 0; x < x_num; x++)
                {
                    Tile tile = FindPosition(x, y);
                    if (tile.Checked == false) { Check = false; }
                    tile.Checked = true;
                    tile.marked = false;
                    tile.opened = true;
                }
                if (Check == false) { return false; }
            }
            
            return true;
            */

            /*
             * // comming from all sides;
            int Currentx = 0;
            int Currenty = 0;

            int max_x = x_num - 1;
            int min_x = 0;
            int max_y = y_num - 1;
            int min_y = 0;

            Tile tile;

            while (max_x >= min_x || max_y >= min_y)
            { 
               while(Currentx < max_x)
               {
                    tile = FindPosition(Currentx, Currenty);
                    tile.marked = false;
                    if (tile.opened == false)
                    {
                        tile.opened = true;
                        return false;
                    }
                    Currentx++;
                }
                max_x--;

                while (Currenty < max_y)
                {
                    tile = FindPosition(Currentx, Currenty);
                    tile.marked = false;
                    if (tile.opened == false)
                    {
                        tile.opened = true;
                        return false;
                    }
                    Currenty++;
                }
                max_y--;

                while (Currentx > min_x)
                {
                    tile = FindPosition(Currentx, Currenty);
                    tile.marked = false;
                    if (tile.opened == false)
                    {
                        tile.opened = true;
                        return false;
                    }
                    Currentx--;
                }
                min_x++;

                while (Currenty > min_y)
                {
                    tile = FindPosition(Currentx, Currenty);
                    tile.marked = false;
                    if (tile.opened == false)
                    {
                        tile.opened = true;
                        return false;
                    }
                    Currenty--;
                }
                min_y++;
            }
            return true;
            */
        }


        public void Draw(Drawvisitor drawvisitor, ReturnInput input)
        {
            drawvisitor.DrawGameScreen(this, input);
        }

        public void Update(float dt)
        {
            foreach (Tile tile in positions)
            {
                tile.Update(dt);
            }
        }
    }
}
