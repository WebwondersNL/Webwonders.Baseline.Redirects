using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Webwonders.Baseline.Redirects.Models;

namespace Webwonders.Baseline.Redirects.Configuration;

public static class RedirectInitializer
{
    public static WebApplication ConfigureRedirects(this WebApplication app,
                                                     IWebHostEnvironment _env,
                                                     IConfiguration _config)
    {
        const string rewritesFile = "rewrites.xml";
        
        app.UseHttpsRedirection();

        if (_env.IsProduction())
        {
            var rewriteOptions = new RewriteOptions();

            var redirectSettings = new RedirectSettings();
            _config.GetSection("Webwonders:Redirects").Bind(redirectSettings);
            
            if (redirectSettings.RedirectUrls != null && redirectSettings.RedirectUrls.Count > 0)
            {
                foreach (var redirectUrl in redirectSettings.RedirectUrls)
                {
                    rewriteOptions.AddRedirectToWwwPermanent(redirectUrl);
                }
            }

            rewriteOptions.Add(context =>
            {
                var request = context.HttpContext.Request;
                var response = context.HttpContext.Response;
                
                // Normalize host and path for consistency
                var host = request.Host.Host.ToLower();
                var path = request.Path.Value?.TrimEnd('/').ToLower() ?? "";
                
                var isUmbracoHost = host.EndsWith(redirectSettings.UmbracoCloudIoSuffix);
                var isNotDevOrStage = !(host.StartsWith(redirectSettings.DevPrefix) || host.StartsWith(redirectSettings.StagingPrefix));
                var isNotUmbracoPath = !path.StartsWith(redirectSettings.UmbracoPath);
                var isNotAppPluginsPath = !path.StartsWith(redirectSettings.AppPluginsPath);
                var isNotSmidgeBundle = !path.StartsWith(redirectSettings.SmidgeBundlePath);
                var isCookieNotSet = request.Cookies[redirectSettings.ContextCookieName] == null;
                var isNotLocalhost = !host.Equals(redirectSettings.LocalhostName) && !host.StartsWith(redirectSettings.LocalhostName + ":");
                
                // Check all conditions before redirecting
                if (isUmbracoHost && isNotDevOrStage && isNotUmbracoPath && isNotAppPluginsPath &&
                    isNotSmidgeBundle && isCookieNotSet && isNotLocalhost)
                {
                    var rootDomain = redirectSettings.RootDomain ?? request.Host.Host;
                    
                    // Redirect to the specified domain
                    var newPath = $"{rootDomain}{request.Path}";
                    response.Redirect(newPath, permanent: true);
                    
                    // Stop further processing of this request
                    context.Result = RuleResult.EndResponse;
                }
            });
            app.UseRewriter(rewriteOptions);
        }

        return app;
    }
    
}