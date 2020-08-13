using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using TestMediaPlayer.Services.DataServices;
using TestMediaPlayer.DataObjects;
using TestMediaPlayer.Services.DialogServices;
using TestMediaPlayer.Services.PlayServices;
using System.Linq;

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
			PlayService.Init(mePlayerBg);
			
			DispatcherTimer timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += timer_Tick;
			timer.Start();
		}

		async Task loadData(string fileName)
        {
			var result = await DataServices.Main.GetScheduleDataObject(fileName);
			var incorrect = false;
			foreach(var item in result.Data)
            {
				if(result.Data.Where(i => i.typePlaying == TypePlaying.background && ((item.startTime > i.startTime && item.startTime < i.stopTime) || (item.stopTime > i.startTime && item.stopTime < i.stopTime))).Any()){
					incorrect = true;
                }
            }
			foreach (var item in result.Data)
			{
				if (result.Data.Where(i => i.typePlaying == TypePlaying.interrupt && item.startTime == i.startTime && item.id != i.id).Any())
				{
					incorrect = true;
				}
			}

			if (result.IsValid && !incorrect)
			{
				PlayService.Main.setSchedule(result.Data);
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
					//PlayService.Main.playSchedule();
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
				//PlayService.Main.playInterrupt();
			}
			catch (Exception ex)
			{
				DialogServices.Main.ShowMessage(ex.Message);
			}

		}
		void timer_Tick(object sender, EventArgs e)
		{
			var position = new TimeSpan(0);
			var timeSpan = new TimeSpan(0);
			if (mePlayerBg.Source != null)
			{
				if (mePlayerBg.NaturalDuration.HasTimeSpan)
                {
					lblStatus.Content = String.Format("{0} / {1}", mePlayerBg.Position.ToString(@"mm\:ss"), mePlayerBg.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));

					lblName.Content = PlayService.Main.getCurrentFileName();

					position = mePlayerBg.Position;
					timeSpan = mePlayerBg.NaturalDuration.TimeSpan;
				}
			}
			else
				lblStatus.Content = "No file selected...";

			PlayService.Main.timeProcessing(position, timeSpan);

		}
	}
}
