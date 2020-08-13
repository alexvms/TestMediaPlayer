using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using TestMediaPlayer.DataObjects;

namespace TestMediaPlayer.Helpers
{
    class DataTools
    {
		public static List<T> ParseCsv<T>(string csvFile, Func<string[], T> convertFunc, params char[] seprators)
		{
			var result = new List<T>();
            try
            {
				using (var reader = new StreamReader(csvFile))
					while (reader.Peek() >= 0)
						result.Add(convertFunc(reader.ReadLine().Split(seprators)));
			}
			catch(Exception e)
            {
				return null;
            }
			return result;
		}

		public static ScheduleDataObject ConvertLine(string[] line) {
			TypePlaying typePlaying;
			if (line[0] == "Background")
			{
				typePlaying = TypePlaying.background;
			}
			else if (line[0] == "Interrupt")
			{
				typePlaying = TypePlaying.interrupt;
			}
			else
			{
				throw new Exception("Error format");
			}

			DateTime startTime; 
			DateTime? stopTime = null;
			var itemIndex = 3;

			if (typePlaying == TypePlaying.background)
            {
                try
                {
					startTime = DateTime.Parse(line[1]);
					stopTime = DateTime.Parse(line[2]);
				}
				catch(Exception e)
                {
					throw e;
                }
			}
            else
            {
				itemIndex = 2;
				try
				{
					startTime = DateTime.Parse(line[1]);
				}
				catch (Exception e)
				{
					throw e;
				}
			}
			string filePath;
			
            if (!String.IsNullOrEmpty(line[itemIndex]))
            {
				filePath = line[itemIndex];
            }
            else
            {
				throw new Exception("Error format");
            }

			var result = new ScheduleDataObject() { typePlaying = typePlaying, startTime = startTime, stopTime = stopTime, path = filePath };
			return result;
		}
	}
}
