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
   
    public enum Dificulty { normal }

    class Game_Manager
    {
        Platform current_platform;

        public Tile EmptyTile;
        public Tile SelectedTile;

        public SelectionButtons selectionbuttons;

        Drawvisitor drawvisitor;
        leveldata level;
        iInputhandler inputhandler;

        ReturnInput latestinput;

        bool EndingLevel = false;
        public float ClickCooldown = 0;
        public float EndingLevelCooldown = 0;

        public bool Exit = false;
        public bool TileControllEnabled = true;

        public bool SelectionButtonsEnabled;

        public Game_Manager(Platform current_platform, Drawvisitor drawvisitor, iInputhandler inputhandler)
        {
            this.EmptyTile = new Tile(this, new Position(-1, -1), false);
            this.drawvisitor = drawvisitor;
            this.level = new leveldata(this, Dificulty.normal);
            this.inputhandler = inputhandler;
            latestinput = inputhandler.getinput();
            this.SelectedTile = EmptyTile;
            this.selectionbuttons = new SelectionButtons(this);
            this.current_platform = current_platform;
            if (this.current_platform == Platform.windows)
            {
                SelectionButtonsEnabled = false;
            }
            else
            {
                SelectionButtonsEnabled = true;
            }
        }


        public void Draw(float dt)
        {
            drawvisitor.Begin();

            if (level != null)
            {
                level.Draw(drawvisitor, latestinput);
            }

            selectionbuttons.Draw(drawvisitor);

            drawvisitor.DrawCursor(latestinput.cursor);

            drawvisitor.End();
        }

        public void increaseclickcooldown()
        {
            ClickCooldown = 0.3f;
        }

        public bool isIntersecting(Position pos, Size size, Vector2 vector2)
        {
            return vector2.X > pos.x && vector2.Y > pos.y &&
            vector2.X < pos.x + size.x && vector2.Y < pos.y + size.y;
        }

        public void OnBomb(Tile Pressed_tile)
        {
            this.EndingLevel = true;
            Pressed_tile.opened = true;
            this.EndingLevelCooldown = 1f;
            TileControllEnabled = false;

        }

        public void OnTile(Tile Pressed_tile)
        {
            if (Pressed_tile != this.EmptyTile)
            {
                int pressedx = Pressed_tile.position.x;
                int pressedy = Pressed_tile.position.y;

                if (Pressed_tile.opened == false)
                {
                    Pressed_tile.opened = true;

                    if (Pressed_tile.isbomb == false && Pressed_tile.VisualNum == 0)
                    {
                        OnTile(level.FindPosition(pressedx - 1, pressedy - 1));
                        OnTile(level.FindPosition(pressedx - 1, pressedy + 0));
                        OnTile(level.FindPosition(pressedx - 1, pressedy + 1));

                        OnTile(level.FindPosition(pressedx + 0, pressedy - 1));
                        OnTile(level.FindPosition(pressedx + 0, pressedy + 1));

                        OnTile(level.FindPosition(pressedx + 1, pressedy - 1));
                        OnTile(level.FindPosition(pressedx + 1, pressedy + 0));
                        OnTile(level.FindPosition(pressedx + 1, pressedy + 1));
                    }
                }
            }
        }

        public void HandleInputs()
        {
            bool clicked = false;

            if (level != null)
            {
                level.HandleHighlights(latestinput, TileControllEnabled);

                if (ClickCooldown <= 0 && TileControllEnabled) //clickactions
                {
                    if (latestinput.clicktype == ClickTypes.open)
                    {
                        Tile PressedTile = level.GetIntersectingtile(latestinput.cursor);

                        if (SelectionButtonsEnabled)
                        {
                            if (this.SelectedTile == this.EmptyTile && PressedTile.opened == false)
                            {
                                this.SelectedTile = PressedTile;
                                this.TileControllEnabled = false;
                            }
                        }
                        else
                        {
                            PressedTile.Open();
                        }
                        clicked = true;
                    }

                    else if (latestinput.clicktype == ClickTypes.mark)
                    {
                        Tile PressedTile = level.GetIntersectingtile(latestinput.cursor);
                        if (SelectionButtonsEnabled)
                        {
                            if (this.SelectedTile == this.EmptyTile && PressedTile.opened == false)
                            {
                                this.SelectedTile = PressedTile;
                                this.TileControllEnabled = false;
                            }
                        }
                        else
                        {
                            PressedTile.Mark();
                        }

                        clicked = true;
                    }
                }

            }

            if (clicked)
            {
                ClickCooldown = 0.3f;
            }
        }
        public void NewLevel()
        {
            // animation done
            this.EndingLevel = false;
            this.level = new leveldata(this, Dificulty.normal);
            this.TileControllEnabled = true;
            this.SelectedTile = this.EmptyTile;
        }

        public void Update(float dt)
        {
            if (ClickCooldown > 0) { ClickCooldown -= dt; } else { ClickCooldown = 0; }
            if (EndingLevelCooldown > 0) { EndingLevelCooldown -= dt; } else { EndingLevelCooldown = 0; }

            latestinput = inputhandler.getinput();

            HandleInputs();
            selectionbuttons.Update(dt, latestinput);
            
            if (level != null)
            {
                level.Update(dt);

                if (EndingLevel)
                {
                    if (EndingLevelCooldown <= 0)
                    { 
                        if (level.OnBomb(3f, dt))
                        {
                            NewLevel();
                        }
                    }
                }
                else if (level.CheckIfDone())
                {
                    NewLevel();
                }
            }

            if (latestinput.navigation == NavigationType.exit)
            {
                this.Exit = true;
            }

        }
    }
}
