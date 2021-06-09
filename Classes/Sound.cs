using Windows.Media.Core;
using Windows.Media.Playback;

namespace LudoGame.Classes
{
    public partial class Sound
    {
        public static MediaPlayer backgroundMusic;
        private static bool playing = false;

        /// <summary>
        /// Plays and pauses the background music
        /// </summary>
        public static void PlayBGMusic()
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