using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TestMediaPlayer.DataObjects;
using TestMediaPlayer.Helpers;

namespace TestMediaPlayer.Services.DataServices.Main
{
    public class MainDataService : BaseDataService, IMainDataService
    {
        public Task<RequestResult<List<ScheduleDataObject>>> GetScheduleDataObject(string fileName)
        {
            Func<string[], ScheduleDataObject> convertLine = DataTools.ConvertLine;
            return GetDataListFromCsv<ScheduleDataObject>(fileName, convertLine);
        }
    }
}