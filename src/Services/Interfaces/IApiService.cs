using BlazorTestClean.Models;

namespace BlazorTestClean.Services.Interfaces
{
    public interface IApiService
    {
        Task<Port> GetFleet();
    }
}
