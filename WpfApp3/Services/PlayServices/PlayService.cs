﻿using System.Windows.Controls;
using TestMediaPlayer.DataObjects;

namespace TestMediaPlayer.Services.PlayServices
{
    public static class PlayService
    {
		public static IPlayService<MediaElement> Main { get; private set; }

		public static void Init(MediaElement playerBg)
		{
			Main = new DefaultPlayService();
			Main.Initialization(playerBg);
			Main.stopAll();
		}
	}
}
