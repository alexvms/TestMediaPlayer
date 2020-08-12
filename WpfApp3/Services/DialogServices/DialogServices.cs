using System;
using System.Collections.Generic;
using System.Text;

namespace TestMediaPlayer.Services.DialogServices
{
    public class DialogServices
    {
		public static IDialogService Main { get; private set; }

		public static void Init()
		{
			Main = new DefaultDialogService();
		}
	}
}
