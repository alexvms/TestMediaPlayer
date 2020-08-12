using System;
using System.Collections.Generic;
using System.Text;

namespace TestMediaPlayer.Services
{
    public interface IDialogService
    {
        void ShowMessage(string message);
        string FilePath { get; set; } 
        bool OpenFileDialog();
    }
}
