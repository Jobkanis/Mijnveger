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
    class Drawvisitor
    {
        Platform platform;
        public SpriteBatch spritebatch;
        public Size TileSize = new Size(24,24);
        // textures
        Texture2D emptyTexture;
        Texture2D Black_Transparent_Texture;
        Texture2D White_transparent_texture;

        SpriteFont arial;

        int gamescreen_x;
        int gamescreen_y;

        int width;
        int height;
        int smallestsize;
        int biggestsize;



        public Drawvisitor(SpriteBatch spritebatch, Platform platform, int width, int height, Texture2D emptyTexture, Texture2D Black_Transparent_Texture, Texture2D White_transparent_texture, SpriteFont arial)
        {
            this.spritebatch = spritebatch;
            this.platform = platform;
            this.emptyTexture = emptyTexture;
            this.Black_Transparent_Texture = Black_Transparent_Texture;
            this.White_transparent_texture = White_transparent_texture;

            this.arial = arial;

            this.width = width;
            this.height = height;
            if (width > height ) { smallestsize = height; biggestsize = width; }
            else { smallestsize = width; biggestsize = height; }
        }

        public void Begin()
        {
            spritebatch.Begin();
        }


        public void End() { spritebatch.End(); }

        public void DrawGameScreen(leveldata level, ReturnInput input)
        {
            int squaresize = 1;

            if ((width / level.x_num) > height / level.y_num) { squaresize = height / level.y_num; }
            else { squaresize = width / level.x_num; }

            squaresize = ConvertInt(squaresize);
            gamescreen_x = ConvertInt(level.x_num * squaresize);
            gamescreen_y = ConvertInt(level.y_num * squaresize);

            level.realsize = new Size(gamescreen_x, gamescreen_y);

            level.realposition = ConvertPosition(new Position((width - gamescreen_x) / 2, (height - gamescreen_y) / 2));

            Size tilesize = new Size(squaresize, squaresize);

            foreach (Tile tile in level.positions)
            {
                tile.real_position = ConvertPosition(new Position((level.realposition.x) + tile.position.x * tilesize.x, (level.realposition.y) + tile.position.y * tilesize.y));
                tile.real_size = tilesize;

                DrawTile(tile);
            }
        }

        public void DrawTileSelectionButtons(SelectionButtons selectionbuttons)
        {

            /*
            int local_square_size = 0;
            if ((width / 2) > (height))
            {
                local_square_size = ((height / 5));
            }
            else {
                local_square_size = ((width / 5));
            }
            */
            int squareheight = (height / 3);
            int squarewidth = (width / 5) * 2;

            selectionbuttons.real_mark_position = new Position(0, height - (int)(squareheight* 1.3));
            selectionbuttons.real_mark_size = new Size(squarewidth, squareheight);
            selectionbuttons.real_open_position = new Position(width - squarewidth, height - (int)(squareheight * 1.3));
            selectionbuttons.real_open_size = new Size(squarewidth, squareheight);

            Color markcolor = Color.Gray;
            Color opencolor = Color.Gray;

            if (selectionbuttons.mark_highlighted) { markcolor = Color.LightGray; }
            if (selectionbuttons.open_highlighted) { opencolor = Color.LightGray; }

            spritebatch.Draw(White_transparent_texture, new Rectangle(selectionbuttons.real_mark_position.x, selectionbuttons.real_mark_position.y, selectionbuttons.real_mark_size.x, selectionbuttons.real_mark_size.y), markcolor);
            if (selectionbuttons.openvisible)
            {
                spritebatch.Draw(White_transparent_texture, new Rectangle(selectionbuttons.real_open_position.x, selectionbuttons.real_open_position.y, selectionbuttons.real_open_size.x, selectionbuttons.real_open_size.y), opencolor);
            }
        }

        public void DrawCursor(Vector2 cursor)
        {
            spritebatch.Draw(emptyTexture, new Rectangle((int)cursor.X, (int)cursor.Y, 4, 4), Color.Yellow);
        }

        public void DrawTile(Tile tile)
        {
            Color buttoncolor;
            string buttontext;
            Color textcolor = Color.Black;

            buttontext = tile.VisualNum.ToString();
            switch (tile.VisualNum)
            {
                case (0):
                    {
                        if (tile.highlighted) { buttoncolor = Color.White; }
                        else { buttoncolor = Color.White; }
                        buttontext = " ";
                        break;
                    }
                case (1):
                    {
                        if (tile.highlighted) { buttoncolor = Color.DeepSkyBlue; }
                        else { buttoncolor = Color.DeepSkyBlue; }
                        buttontext = "1";
                        break;
                    }
                case (2):
                    { 
                        if (tile.highlighted) { buttoncolor = Color.LightGreen; }
                        else { buttoncolor = Color.LightGreen; }
                        buttontext = "2";
                        break;
                    }
                case (3):
                    {
                        if (tile.highlighted) { buttoncolor = Color.LightPink; }
                        else { buttoncolor = Color.LightPink; }
                        buttontext = "3";
                        break;
                    }
                case (4):
                    {
                        if (tile.highlighted) { buttoncolor = Color.LightCyan; }
                        else { buttoncolor = Color.LightCyan; }
                        buttontext = "4";
                        break;
                    }
                case (5):
                    {
                        if (tile.highlighted) { buttoncolor = Color.LightSalmon; }
                        else { buttoncolor = Color.LightSalmon; }
                        buttontext = "5";
                        break;
                    }
                case (6):
                    {
                        if (tile.highlighted) { buttoncolor = Color.LightYellow; }
                        else { buttoncolor = Color.LightYellow; }
                        buttontext = "6";
                        break;
                    }
                case (7):
                    {
                        if (tile.highlighted) { buttoncolor = Color.LightSteelBlue; }
                        else { buttoncolor = Color.LightSteelBlue; }
                        buttontext = "7";
                        break;
                    }
                case (8):
                    {
                        if (tile.highlighted) { buttoncolor = Color.LightSlateGray; }
                        else { buttoncolor = Color.LightSlateGray; }
                        buttontext = "8";
                        break;
                    }
                default:
                    {
                        buttoncolor = Color.Pink;
                        buttontext = "E";
                        break;
                    }
            }

            if (tile.isbomb)
            {
                buttoncolor = Color.Red;
                buttontext = "b";
            }

            if (!tile.opened)
            {

                if (tile.highlighted) { buttoncolor = Color.Gray; }
                else { buttoncolor = Color.Black; }

                if (tile.marked)
                { 
                    buttontext = "m";
                    textcolor = Color.White;
                }
                else
                {
                    buttontext = " ";
                }
            }

            if (tile.selected)
            {
                buttontext = "?";
                textcolor = Color.Black;
                buttoncolor = Color.White;
            }

            spritebatch.Draw(emptyTexture, new Rectangle(tile.real_position.x, tile.real_position.y, tile.real_size.x, tile.real_size.y), buttoncolor);

            Vector2 textsize = arial.MeasureString(buttontext);
            int textsize_x = (int)textsize.X;
            int textsize_y = (int)textsize.Y;
            spritebatch.DrawString(arial, buttontext, new Vector2(tile.real_position.x + ((tile.real_size.x - textsize_x) / 2), tile.real_position.y + ((tile.real_size.y - textsize_y) / 2)), textcolor);
        }

        public int ConvertInt(int Int)
        {
            return Int;
        }

        public Size ConvertSize(Size size)
        {
            return size;
        }

        public Position ConvertPosition(Position position)
        {
            return position;
        }

        public Vector2 ConvertVector2(Vector2 vector2)
        {
            return vector2;
        }
    }
}
