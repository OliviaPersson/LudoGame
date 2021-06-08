using Windows.Media.Core;
using Windows.Media.Playback;

namespace LudoGame.Classes
{
    public partial class Sound
    {
        public static MediaPlayer backgroundMusic;
        private static bool playing = false;

        public static async void PlayBGMSound()
        {
            //https://pixabay.com/music/search/genre/ambient/
            if (playing)
            {
                backgroundMusic.Pause();
                playing = false;
            }
            else
            {
                playing = true;
                backgroundMusic.Play();
            }
        }
    }
}