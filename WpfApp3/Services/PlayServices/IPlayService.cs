using System.Windows.Controls;
using TestMediaPlayer.DataObjects;
using System.Collections.Generic;

namespace TestMediaPlayer.Services.PlayServices
{
    public interface IPlayService
    {
        void playSchedule(List<ScheduleDataObject> schedule, MediaElement player);
        void stopAll();
        void nextPlay(MediaElement player);
    }
}
