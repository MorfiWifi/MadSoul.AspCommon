using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace MadSoul.AspCommon;

public interface IEndpointRoute
{
    public void RegisterRoute(IEndpointRouteBuilder app);
}
