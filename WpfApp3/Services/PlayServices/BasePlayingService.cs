using System;
using System.Collections.Generic;
using System.Text;

namespace TestMediaPlayer.Services.PlayServices
{
    public class BasePlayingService<T>
    {
        public T playerBg;
        public T playerIr;

        public virtual void play(T player, string fileName)
        {

        }
    }
}
