using System.Windows.Controls;
using TestMediaPlayer.DataObjects;

namespace TestMediaPlayer.Services.PlayServices
{
    public static class PlayService
    {
		public static IPlayService Main { get; private set; }

		public static void Init()
		{
			Main = new DefaultPlayService();
			Main.stopAll();
		}
	}
}
