using LeadManagementSystem.Contracts.Request;
using LeadManagementSystem.Contracts.Response;

namespace LeadManagementSystem.Providers.Interface
{
    public interface IDalasProvider
    {
        TurboServiceCall GetSoapEnvlope(LeadRequest leadRequest, LesResponse lesResponse, int brandId);
        CustomerStatusRequest GetSoapEnvlope(LeadRequest request, int brandId);
    }
}
