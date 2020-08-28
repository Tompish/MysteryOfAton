using Microsoft.Xna.Framework;
using System;

namespace MysteryOfAtonClient
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Client())
            {
                game.IsFixedTimeStep = true;
                game.TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 17);
                game.Run();
            }
        }
    }
}
