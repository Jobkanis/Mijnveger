using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mijnveger
{
    class SelectionButtons
    {
        public Position real_mark_position = new Position(0,0);
        public Size real_mark_size = new Size(0,0);
        public Position real_open_position = new Position(0,0);
        public Size real_open_size = new Size(0,0);

        public bool mark_highlighted;
        public bool open_highlighted;

        public bool openvisible;

        Game_Manager gamemanager;
        
        public SelectionButtons(Game_Manager gamemanager)
        {
            this.gamemanager = gamemanager;
            this.mark_highlighted = false;
            this.open_highlighted = false;
            this.openvisible = true;
        }

        public void Draw(Drawvisitor drawvisitor)
        {
            if (gamemanager.SelectedTile != gamemanager.EmptyTile)
            {
                drawvisitor.DrawTileSelectionButtons(this);
            }
        }

        public void Update(float dt, ReturnInput input)
        {
            if (gamemanager.SelectedTile != gamemanager.EmptyTile)
            {
                if (gamemanager.ClickCooldown <= 0)
                {
                    this.mark_highlighted = (gamemanager.isIntersecting(real_mark_position, real_mark_size, input.cursor));
                    this.open_highlighted = (gamemanager.isIntersecting(real_open_position, real_open_size, input.cursor));
                }

                this.openvisible = !gamemanager.SelectedTile.marked;
            
                if (input.clicktype == ClickTypes.open && gamemanager.ClickCooldown <= 0)
                {
                    if (this.mark_highlighted)
                    {
                        gamemanager.SelectedTile.Mark();
                        gamemanager.SelectedTile = gamemanager.EmptyTile;
                        gamemanager.TileControllEnabled = true;
                        gamemanager.increaseclickcooldown();
                    }
                    else if (this.open_highlighted )
                    {
                        gamemanager.SelectedTile.Open();
                        gamemanager.SelectedTile = gamemanager.EmptyTile;
                        gamemanager.TileControllEnabled = true;
                        gamemanager.increaseclickcooldown();
                    }
                    else
                    {
                        gamemanager.TileControllEnabled = true;
                        gamemanager.SelectedTile = gamemanager.EmptyTile;
                    }
                 }
            }
            else
            {
            }
        }
    }
}
