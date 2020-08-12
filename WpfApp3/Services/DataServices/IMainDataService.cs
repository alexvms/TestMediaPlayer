using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestMediaPlayer.DataObjects;

namespace TestMediaPlayer.Services.DataServices
{
    public interface IMainDataService {
        Task<RequestResult<List<ScheduleDataObject>>> GetScheduleDataObject(string fileName);
    }
}
