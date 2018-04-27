using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mijnveger
{
    class Tile
    {
        public int VisualNum;
        public bool isbomb;
        public bool opened;
        public Position position;
        public bool highlighted;
        public bool marked;
        public bool Checked = false;
        public bool selected;

        public Position real_position = new Position(0,0);
        public Size real_size = new Size(0,0);
        public Game_Manager gamemanager;

        public Tile (Game_Manager gamemanager, Position position, bool isbomb)
        { 
            this.gamemanager = gamemanager;
            this.position = position;
            this.isbomb = isbomb;
            this.opened = false;
            this.highlighted = false;
            this.marked = false;
            this.selected = false;
        }

        public void Open()
        {
            if (!this.opened && !this.marked)
            {
                if(this.isbomb)
                {
                    gamemanager.OnBomb(this);
                }
                else
                {
                    gamemanager.OnTile(this);
                }

            }

        }

        public void Mark()
        {
            this.marked = !this.marked;
        }

        public void MouseOn()
        {
            if (!this.opened)
            {
                this.highlighted = true;
            }
        }

        public void MouseOff()
        {
            this.highlighted = false;
        }

        public void Update(float dt)
        {
            if (gamemanager.SelectedTile.position.x == this.position.x && gamemanager.SelectedTile.position.y == this.position.y)
            {
                this.selected = true;
            }
            else
            {
                this.selected = false;
            }
        }
    }
}
