using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using StackExchange.Profiling.Data;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Extensions;

namespace Webwonders.Baseline.Redirects.Controllers;

public class RedirectController : RenderController
{
    public RedirectController(ILogger<RedirectController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor)
        : base(logger, compositeViewEngine, umbracoContextAccessor)
    {
    }

    public override IActionResult Index()
    {
        var redirect = CurrentPage;

        if (redirect != null && redirect.HasProperty("redirectTo") && redirect.HasValue("redirectTo"))
        {
            var redirectLink = redirect.Value<Link>("redirectTo");
            var redirectUrl = redirectLink?.Url;
            if (redirectLink is { Type: Umbraco.Cms.Core.Models.LinkType.External, Url: not null } && !redirectLink.Url.StartsWith("http"))
            {
                redirectUrl = "https://" + redirectUrl;
            }

            if (redirectUrl != null)
            {
                return RedirectPermanent(redirectUrl);
            }
        }
        else
        {
            var root = CurrentPage?.Root();
            return Redirect(root?.Url()!);
        }

        // Fallback to hardset root if no valid redirect found
        return Redirect("/");
    }
}