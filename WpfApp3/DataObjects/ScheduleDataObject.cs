using System;
using System.Collections.Generic;
using System.Text;

namespace TestMediaPlayer.DataObjects
{
    public class ScheduleDataObject
    {
        public int id { get; set; }
        public TypePlaying typePlaying { get; set; }
        public DateTime startTime { get; set; }
        public DateTime? stopTime { get; set; }
        public string path { get; set; }
    }

    public class scheduleList
    {
        public List<ScheduleDataObject> list { get; set; }
        public int ident { get; set; }
    }
}
