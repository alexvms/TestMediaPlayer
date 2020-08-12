using System.Windows.Controls;
using TestMediaPlayer.DataObjects;
using System.Collections.Generic;

namespace TestMediaPlayer.Services.PlayServices
{
    public interface IPlayService<T>
    {
        void playSchedule(List<ScheduleDataObject> schedule, T player);
        void stopAll();
        void nextPlay(T player);
        void playInterrupt(List<ScheduleDataObject> listScheduleObject, T mePlayerIr);
    }
}
