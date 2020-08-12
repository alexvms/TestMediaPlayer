using System.Collections.Generic;
using System.Linq;
using System;
using TestMediaPlayer.DataObjects;
using TestMediaPlayer.Helpers;

namespace TestMediaPlayer.Services.PlayServices
{
    public class BasePlayingService<T>
    {
        public T player;
        public double positionBg;
        public StatusPlayingBg statusPlayingBg;
        public StatusPlayingIr statusPlayingIr;
        public LinkedListNode<FileDataObject> currentFileBg;
        public LinkedListNode<FileDataObject> currentFileIr;
        public LinkedList<FileDataObject> playlistBg;
        public LinkedList<FileDataObject> playlistIr;
        public List<ScheduleDataObject> schedule;

        public void timeProcessing(TimeSpan position, TimeSpan timeSpan)
        {
            if(statusPlayingBg == StatusPlayingBg.playing)
            {
                if (position == timeSpan)
                    nextPlay(player, playlistBg, currentFileBg, true);
            }
            else if(statusPlayingIr == StatusPlayingIr.playing)
            {
                if (position == timeSpan)
                    nextPlay(player, playlistIr, currentFileIr, false);
            }
            else
            {
                if(statusPlayingBg == StatusPlayingBg.paused) {
                    continuePlay();
                }
            }
        }

        public virtual void continuePlay()
        {
        }

        public void nextPlay(T player, LinkedList<FileDataObject> playlist, LinkedListNode<FileDataObject> currentFile, bool repeat)
        {
            if (currentFile.Next != null)
            {
                currentFile = currentFile.Next;
            }
            else if(repeat)
            {
                currentFile = playlist.First;
            }
            else {
                currentFile = null;
                statusPlayingIr = StatusPlayingIr.stopped;
            }
            
            if(currentFile != null)
            {
                if (statusPlayingBg == StatusPlayingBg.playing)
                {
                    currentFileBg = currentFile;
                }
                if (statusPlayingIr == StatusPlayingIr.playing)
                {
                    currentFileIr = currentFile;
                }
                play(player, currentFile.Value.name);
            }
        }

        public virtual void play(T player, string fileName, double position = 0)
        {
        }

        public void stopAll()
        {
            statusPlayingBg = StatusPlayingBg.stopped;
        }

        public void playSchedule(List<ScheduleDataObject> scheduleParam)
        {
            schedule = scheduleParam;
            if (statusPlayingBg == StatusPlayingBg.stopped)
            {
                playlistBg = FileTools.GetFilesList(schedule.Where(i => i.typePlaying == TypePlaying.background).FirstOrDefault().path);
                currentFileBg = playlistBg.First;
                play(player, currentFileBg.Value.name);
                statusPlayingBg = StatusPlayingBg.playing;
            }
        }
    }
}
