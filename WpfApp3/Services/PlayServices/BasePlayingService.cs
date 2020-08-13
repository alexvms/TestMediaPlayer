using System.Collections.Generic;
using System.Linq;
using System;
using TestMediaPlayer.DataObjects;
using TestMediaPlayer.Helpers;
using System.Security.Policy;

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
        public scheduleList schedule;
        public int currentScheduleIdent = 0;
        public ScheduleDataObject currentScheduleBg;
        public ScheduleDataObject currentScheduleIr;
        public bool stop = false;

        public void setSchedule(List<ScheduleDataObject> data)
        {
            Random rnd = new Random();
            var ident = rnd.Next();

            schedule = new scheduleList { list = data, ident = ident };
        }

        public string getCurrentFileName()
        {
            if (statusPlayingBg == StatusPlayingBg.playing)
                return currentFileBg.Value.name;
            else if (statusPlayingIr == StatusPlayingIr.playing)
                return currentFileIr.Value.name;
            else
                return "";
        }
        public void timeProcessing(TimeSpan position, TimeSpan timeSpan)
        {
            if(schedule != null)
            {
                var justStartPlaying = false;
                var proveSchedule = schedule.list.Where(i => i.typePlaying == TypePlaying.background && DateTime.Now >= i.startTime && DateTime.Now < i.stopTime).FirstOrDefault();
                if (proveSchedule != null)
                {
                    if (currentScheduleBg != null)
                    {
                        if(schedule.ident == currentScheduleIdent)
                        {
                            if (currentScheduleBg.id != proveSchedule.id)
                            {
                                if (statusPlayingIr != StatusPlayingIr.playing && statusPlayingBg != StatusPlayingBg.playing && statusPlayingBg != StatusPlayingBg.paused)
                                {
                                    currentScheduleBg = proveSchedule;
                                    currentScheduleIdent = schedule.ident;
                                    playSchedule();
                                    justStartPlaying = true;
                                }
                                else if (!stop)
                                {
                                    stop = true;
                                }
                            }
                        }
                        else
                        {
                            stop = true;
                        }
                    }
                    else
                    {
                        currentScheduleBg = proveSchedule;
                        if (statusPlayingIr != StatusPlayingIr.playing)
                        {
                            currentScheduleIdent = schedule.ident;
                            playSchedule();
                            justStartPlaying = true;
                        }
                        else
                        {
                            statusPlayingBg = StatusPlayingBg.paused;
                        }
                    }
                }

                proveSchedule = schedule.list.Where(i => i.typePlaying == TypePlaying.interrupt && DateTime.Now.AddMilliseconds(-500) <= i.startTime && DateTime.Now.AddMilliseconds(500) >= i.startTime).FirstOrDefault();
                if (proveSchedule != null)
                {
                    if (currentScheduleIr != null)
                    {
                        if (schedule.ident == currentScheduleIdent)
                        {
                            if (currentScheduleIr.id != proveSchedule.id)
                            {
                                if (statusPlayingIr != StatusPlayingIr.playing && statusPlayingBg != StatusPlayingBg.playing && statusPlayingBg != StatusPlayingBg.paused)
                                {
                                    currentScheduleIr = proveSchedule;
                                    playInterrupt(position.TotalSeconds);
                                    justStartPlaying = true;
                                }
                                else if (!stop)
                                {
                                    stop = true;
                                }
                            }
                        }
                        else
                        {
                            stop = true;
                        }
                    }
                    else
                    {
                        currentScheduleIr = proveSchedule;
                        playInterrupt(position.TotalSeconds);
                        justStartPlaying = true;
                    }
                }

                if (!justStartPlaying)
                {
                    if (statusPlayingBg == StatusPlayingBg.playing)
                    {
                        if (position == timeSpan)
                        {
                            if (stop)
                            {
                                stop = false; 
                                proveSchedule = schedule.list.Where(i => i.typePlaying == TypePlaying.background && DateTime.Now >= i.startTime && DateTime.Now < i.stopTime).FirstOrDefault();
                                currentScheduleBg = proveSchedule;
                                currentScheduleIdent = schedule.ident;
                                statusPlayingBg = StatusPlayingBg.stopped;
                                playSchedule();
                            }
                            else
                            {
                                nextPlay(player, playlistBg, currentFileBg, true);
                            }

                        }
                    }
                    else if (statusPlayingIr == StatusPlayingIr.playing)
                    {
                        if (position == timeSpan)
                        {
                            if (stop)
                            {
                                statusPlayingIr = StatusPlayingIr.stopped;
                                if(statusPlayingBg == StatusPlayingBg.stopped)
                                {
                                    proveSchedule = schedule.list.Where(i => i.typePlaying == TypePlaying.background && DateTime.Now >= i.startTime && DateTime.Now < i.stopTime).FirstOrDefault();
                                    currentScheduleBg = proveSchedule;
                                    currentScheduleIdent = schedule.ident;
                                    playSchedule();
                                }
                            }
                            else
                            {
                                nextPlay(player, playlistIr, currentFileIr, false);
                            }
                        }
                    }
                    else
                    {
                        if (statusPlayingBg == StatusPlayingBg.paused)
                        {
                            continuePlay();
                        }
                    }
                }
            }
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

        public void playSchedule()
        {
            if (statusPlayingBg == StatusPlayingBg.stopped)
            {
                playlistBg = FileTools.GetFilesList(currentScheduleBg.path);
                stop = false;
                currentFileBg = playlistBg.First;
                play(player, currentFileBg.Value.name);
                statusPlayingBg = StatusPlayingBg.playing;
            }
        }

        public void playInterrupt(double totalSeconds)
        {
            playlistIr = FileTools.GetFilesList(currentScheduleIr.path);
            currentFileIr = playlistIr.First;
            stop = false;

            positionBg = totalSeconds;
            statusPlayingIr = StatusPlayingIr.playing;
            statusPlayingBg = StatusPlayingBg.paused;

            play(player, currentFileIr.Value.name);
        }

        public void continuePlay()
        {
            play(player, currentFileBg.Value.name, positionBg);
        }

    }
}
