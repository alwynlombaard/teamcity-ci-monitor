namespace website.Application.Services.TeamCity.Dto
{
    public class Change
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Version { get; set; }
        public VcsRootInstance VcsRootInstance { get; set; }
    }
}