using System.Collections.Generic;

namespace website.Application.Services.TeamCity.Dto
{
    public class Properties
    {
        public Properties()
        {
            Property = new List<Property>();
        }
        public IList<Property> Property { get; set; }    
    }
}