using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Mijnveger
{
    public enum ClickTypes { none, open, mark }
    public enum NavigationType { none, exit }

    class ReturnInput
    {
        public Vector2 cursor;
        public ClickTypes clicktype;
        public NavigationType navigation;
        public ReturnInput(Vector2 cursor, ClickTypes clicktype, NavigationType navigation)
        {
            this.cursor = cursor;
            this.clicktype = clicktype;
            this.navigation = navigation;
        }

    }

     interface iInputhandler
    {
        ReturnInput getinput();
    }

    class WindowsInputHandler: iInputhandler
    {
        public WindowsInputHandler()
        {

        }

        public ReturnInput getinput()
        {
            MouseState mouse = Mouse.GetState();
            ClickTypes clicktype = ClickTypes.none;
            NavigationType navigation = NavigationType.none;

            if (mouse.LeftButton == ButtonState.Pressed ) { clicktype = ClickTypes.open;  }
            else if (mouse.RightButton == ButtonState.Pressed) { clicktype = ClickTypes.mark; }

            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Escape)) { navigation = NavigationType.exit; }

            return new ReturnInput(new Vector2(mouse.X, mouse.Y), clicktype, navigation);
        }
    }

    class AndroidInputHandler : iInputhandler
    {
        public AndroidInputHandler()
        {

        }

        public ReturnInput getinput()
        {
            int touchex = -1;
            int touchey = -1;
            bool pressed = false;
            TouchCollection touchCollection = TouchPanel.GetState();

            NavigationType navigation = NavigationType.none;

            foreach (TouchLocation touch1 in touchCollection) // for loop and checkign each touch
            {
                touchex = (int)touch1.Position.X;
                touchey = (int)touch1.Position.Y;
                pressed = true;
            }
            ClickTypes action = ClickTypes.none;
            if (pressed) { action = ClickTypes.open; }
            return new ReturnInput(new Vector2(touchex, touchey), action, navigation);
        }
    }
}
