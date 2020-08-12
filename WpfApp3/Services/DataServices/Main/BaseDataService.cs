using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestMediaPlayer.DataObjects;
using TestMediaPlayer.Helpers;

namespace TestMediaPlayer.Services.DataServices.Main
{
    public class BaseDataService
    {
		protected async Task<RequestResult<List<T>>> GetDataListFromCsv<T>(string fileName, Func<string[], T> convertFunc) where T : class
		{
			var result = DataTools.ParseCsv<T>(fileName, convertFunc, ' ');
			return new RequestResult<List<T>>(result, result != null ? RequestStatus.Ok : RequestStatus.SerializationError);
		}
	}
}
