using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace IceNet.Shared;

internal class Util
{
    internal static IComponentRenderMode? GetRenderMode()
    {
        return Environment.GetEnvironmentVariable("App") == "Server" ? RenderMode.InteractiveServer : default;
    }
}