using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace BusTrackBackEnd.API.Shared.Infrastructure.Interfaces.ASP.Configuration.Extensions;

public class KebabCaseRouteNamingConvention : IApplicationModelConvention
{
    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            controller.ControllerName = ToKebabCase(controller.ControllerName.Replace("Controller", ""));
            foreach (var action in controller.Actions)
            {
                action.ActionName = ToKebabCase(action.ActionName);
            }
        }
    }

    private static string ToKebabCase(string input)
    {
        return string.Concat(
                input.Select((x, i) =>
                    i > 0 && char.IsUpper(x)
                        ? "-" + x
                        : x.ToString()))
            .ToLower();
    }
}