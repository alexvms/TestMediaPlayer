namespace TestMediaPlayer.Services.DataServices
{
    public static class DataServices
    {
		public static IMainDataService Main { get; private set; }

		public static void Init()
		{
			Main = new Main.MainDataService();
		}
    }
}
