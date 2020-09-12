using Piranha.AttributeBuilder;
using Piranha.Models;

namespace BodyGenesis.Presentation.Website.Cms.Models
{
    [PageType(Title = "Basic Page", UseExcerpt = false, UsePrimaryImage = false)]
    [PageTypeRoute(Title = "Default", Route = "/basic-page")]
    public class BasicPage : Page<BasicPage>
    {
    }
}
