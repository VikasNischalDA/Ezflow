using LeadManagementSystem.Shared.Infrastructure;

namespace LeadManagementSystem.MVC.Services
{
    public interface IBaseHttpClientService
    {
        Task<ActionResult<TResponse>> GetAsync<TResponse>(string endpoint, Dictionary<string, string> queryParams = null);
        Task<ActionResult<TResponse>> PostAsync<TRequest, TResponse>(TRequest request, string endpoint);
    }
}
