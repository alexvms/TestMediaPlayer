using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using TestMediaPlayer.DataObjects;
using TestMediaPlayer.Helpers;

namespace TestMediaPlayer.Services.PlayServices
{
    public class DefaultPlayService : IPlayService
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

        void IPlayService.playSchedule(List<ScheduleDataObject> schedule, MediaElement player)
        {
            if(statusPlaying == StatusPlaying.stopped)
            {
                playlist = FileTools.GetFilesList(schedule.Where(i => i.typePlaying == TypePlaying.background).FirstOrDefault().path);
                currentFile = playlist.First;
                play(currentFile.Value.name, player);
                statusPlaying = StatusPlaying.playing;
            }
        }

        void IPlayService.stopAll()
        {
            statusPlaying = StatusPlaying.stopped;
        }

        void play(string fileName, MediaElement player)
        {
            player.Source = new Uri(fileName);
            player.Play();
        }
    }
}
