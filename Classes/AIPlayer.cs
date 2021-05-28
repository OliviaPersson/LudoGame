namespace LudoGame.Classes
{
    internal class AIPlayer
    {
        //Color for wich team the ai is playing as
        public Player Player { get; set; }

        public AIPlayer(Player player)
        {
            Player = player;
        }

        public void Play()
        {
        }
    }
}