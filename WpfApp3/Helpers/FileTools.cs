using TestMediaPlayer.DataObjects;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TestMediaPlayer.Helpers
{
    class FileTools
    {
        public static LinkedList<FileDataObject> GetFilesList(string path)
        {
            var files = Directory.GetFiles(path);
            LinkedList<FileDataObject> result = new LinkedList<FileDataObject>();
            foreach(var file in files)
            {
                FileDataObject fileObject = new FileDataObject { name = file };
                result.AddLast(fileObject);
            }
            return result;
        }
    }
}
