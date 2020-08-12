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
        public StatusPlaying statusPlaying;
        public LinkedListNode<FileDataObject> currentFile;
        LinkedList<FileDataObject> playlist;

        public void nextPlay(MediaElement player)
        {
            if(currentFile.Next != null)
            {
                currentFile = currentFile.Next;
            }
            else
            {
                currentFile = playlist.First;
            }
            play(currentFile.Value.name, player);
        }

        public void playSchedule(List<ScheduleDataObject> schedule, MediaElement player)
        {
            if(statusPlaying == StatusPlaying.stopped)
            {
                playlist = FileTools.GetFilesList(schedule.Where(i => i.typePlaying == TypePlaying.background).FirstOrDefault().path);
                currentFile = playlist.First;
                play(currentFile.Value.name, player);
                statusPlaying = StatusPlaying.playing;
            }
        }

        public void stopAll()
        {
            statusPlaying = StatusPlaying.stopped;
        }

        public override void play(MediaElement player, string fileName)
        {
            player.Source = new Uri(fileName);
            player.Play();
        }

        public void playInterrupt(List<ScheduleDataObject> listScheduleObject, MediaElement mePlayerIr)
        {
            
        }

    }
}
