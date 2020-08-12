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
					PlayService.Main.playSchedule(listScheduleObject, mePlayer);
				}
			}
			catch (Exception ex)
			{
				DialogServices.Main.ShowMessage(ex.Message);
			}

		}
		void timer_Tick(object sender, EventArgs e)
		{
			if (mePlayer.Source != null)
			{
				if (mePlayer.NaturalDuration.HasTimeSpan)
                {
					lblStatus.Content = String.Format("{0} / {1}", mePlayer.Position.ToString(@"mm\:ss"), mePlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
					if (mePlayer.Position == mePlayer.NaturalDuration.TimeSpan)
                    {
						PlayService.Main.nextPlay(mePlayer);
                    }
				}
			}
			else
				lblStatus.Content = "No file selected...";


		}

		private void btnPlay_Click(object sender, RoutedEventArgs e)
		{
			mePlayer.Play();
		}

		private void btnPause_Click(object sender, RoutedEventArgs e)
		{
			mePlayer.Pause();
		}

		private void btnStop_Click(object sender, RoutedEventArgs e)
		{
			mePlayer.Stop();
		}
	}
}
