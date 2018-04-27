using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mijnveger
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            int width = (int)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int height = (int)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            using (var game = new Game1(Platform.windows, width, height))
                game.Run();
        }
    }
#endif
}
