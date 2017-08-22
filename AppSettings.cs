namespace birds
{
    public class AppSettings
    {
        public AppSettings()
        {
     
        }
        public string FlickrUserId { get; set; }
        public string FlickrApiKey { get; set; }
        public string Github { get; set; }
        public string UserName { get; set; }
        public bool IsTestRun { get; set; }
    }
}