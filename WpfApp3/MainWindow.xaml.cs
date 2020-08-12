using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using TestMediaPlayer.Services.DataServices;
using TestMediaPlayer.DataObjects;
using TestMediaPlayer.Services.DialogServices;
using TestMediaPlayer.Services.PlayServices;

namespace TestMediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
		public ScheduleDataObject itemScheduleObject {get; set;}

		public List<ScheduleDataObject> listScheduleObject { get; set; }

		public MainWindow()
        {
            InitializeComponent();

			DataServices.Init();
			DialogServices.Init();
			PlayService.Init();
			
			DispatcherTimer timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += timer_Tick;
			timer.Start();
		}

		async Task loadData(string fileName)
        {
			var result = await DataServices.Main.GetScheduleDataObject(fileName);
			if (result.IsValid)
			{
				listScheduleObject = result.Data;
			}
			else
            {
				DialogServices.Main.ShowMessage("Wrong file format");
			}

		}
		void btnLoadSchedule_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (DialogServices.Main.OpenFileDialog() == true)
				{
					loadData(DialogServices.Main.FilePath);
					PlayService.Main.playSchedule(listScheduleObject, mePlayerBg);
				}
			}
			catch (Exception ex)
			{
				DialogServices.Main.ShowMessage(ex.Message);
			}

		}
		void btnInterrupt_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				PlayService.Main.playInterrupt(listScheduleObject, mePlayerIr);
			}
			catch (Exception ex)
			{
				DialogServices.Main.ShowMessage(ex.Message);
			}

		}
		void timer_Tick(object sender, EventArgs e)
		{
			if (mePlayerBg.Source != null)
			{
				if (mePlayerBg.NaturalDuration.HasTimeSpan)
                {
					lblStatus.Content = String.Format("{0} / {1}", mePlayerBg.Position.ToString(@"mm\:ss"), mePlayerBg.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
					if (mePlayerBg.Position == mePlayerBg.NaturalDuration.TimeSpan)
                    {
						PlayService.Main.nextPlay(mePlayerBg);
                    }
				}
			}
			else
				lblStatus.Content = "No file selected...";


		}
	}
}
