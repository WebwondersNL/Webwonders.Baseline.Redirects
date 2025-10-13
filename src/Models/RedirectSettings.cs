namespace Webwonders.Baseline.Redirects.Models;

public class RedirectSettings
{
    public List<string>? DomainsToRewriteToWww { get; set; }
    public string? UmbracoIoUrlReplacement { get; set; }
    public string UmbracoCloudIoSuffix { get; set; } = ".umbraco.io";
    public string DevPrefix { get; set; } = "dev-";
    public string StagingPrefix { get; set; } = "stage-";
    public string UmbracoPath { get; set; } = "/umbraco";
    public string AppPluginsPath { get; set; } = "/app_plugins";
    public string SmidgeBundlePath { get; set; } = "/sb";
    public string LocalhostName { get; set; } = "localhost";
    public string ContextCookieName { get; set; } = "UMB_UCONTEXT";
}