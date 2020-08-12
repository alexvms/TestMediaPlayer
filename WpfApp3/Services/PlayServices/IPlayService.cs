using System.Windows.Controls;
using TestMediaPlayer.DataObjects;
using System.Collections.Generic;
using System;

namespace TestMediaPlayer.Services.PlayServices
{
    public interface IPlayService<T>
    {
        void playSchedule(List<ScheduleDataObject> schedule);
        void stopAll();
        void nextPlay(T player, LinkedList<FileDataObject> playlist, LinkedListNode<FileDataObject> currentFile, bool repeat);
        void continuePlay();
        void playInterrupt();
        void Initialization(T player);
        void timeProcessing(TimeSpan position, TimeSpan timeSpan);
    }
}
