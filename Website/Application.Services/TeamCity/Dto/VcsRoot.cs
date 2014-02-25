namespace website.Application.Services.TeamCity.Dto
{
    public class VcsRoot
    {
        public VcsRoot()
        {
            Properties = new Properties();
        }
        public string Id { get; set; }
        public Properties Properties { get; set; }
    }
}