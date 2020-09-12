using Piranha.AttributeBuilder;
using Piranha.Models;

namespace BodyGenesis.Presentation.Website.Cms.Models
{
    [PageType(Title = "Agreement Page", UseExcerpt = false, UsePrimaryImage = false)]
    [PageTypeRoute(Title = "Default", Route = "/agreement-page")]
    public class AgreementPage : Page<AgreementPage>
    {
    }
}
