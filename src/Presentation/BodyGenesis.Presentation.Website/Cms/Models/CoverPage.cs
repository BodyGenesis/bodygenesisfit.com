using Piranha.AttributeBuilder;
using Piranha.Models;

namespace BodyGenesis.Presentation.Website.Cms.Models
{
    [PageType(Title = "Cover Page", UseExcerpt = false, UsePrimaryImage = true)]
    [PageTypeRoute(Title = "Default", Route = "/cover-page")]
    public class CoverPage : Page<CoverPage>
    {
    }
}
