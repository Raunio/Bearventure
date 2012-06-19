
namespace Bearventure
{
#if WINDOWS || XBOX
    static class EntryPoint
    {
        static void Main(string[] args)
        {
            using (BearventureImpl game = new BearventureImpl())
            {
                game.Run();
            }
        }
    }
#endif
}

