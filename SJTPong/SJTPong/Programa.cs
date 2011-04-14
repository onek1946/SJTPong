using System;

namespace SJTPongGame
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SJTPong game = new SJTPong())
            {
                game.Run();
            }
        }
    }
#endif
}

