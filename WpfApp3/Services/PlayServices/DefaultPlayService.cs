using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using TestMediaPlayer.DataObjects;
using TestMediaPlayer.Helpers;

namespace TestMediaPlayer.Services.PlayServices
{
    public class DefaultPlayService : BasePlayingService<MediaElement>, IPlayService<MediaElement>
    {
        public void Initialization(MediaElement mePlayerBg)
        {
            player = mePlayerBg;
        }

        public override void play(MediaElement player, string fileName, double position = 0)
        {
            player.Source = new Uri(fileName);
            if(statusPlayingBg == StatusPlayingBg.paused && position > 0)
            {
                player.Position = TimeSpan.FromSeconds(position);
                statusPlayingBg = StatusPlayingBg.playing;
                positionBg = 0;
            }
            player.Play();
        }
    }
}
