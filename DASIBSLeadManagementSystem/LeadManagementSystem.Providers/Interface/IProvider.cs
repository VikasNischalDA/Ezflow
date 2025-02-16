namespace LeadManagementSystem.Providers.Interface
{
    public interface IProvider<TRequest, TResponse>
    {
        TResponse GetSoapEnvlope(TRequest request);

    }
}
