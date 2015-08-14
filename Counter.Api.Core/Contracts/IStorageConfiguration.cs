namespace Counter.Api.Core.Contracts
{
    public interface IStorageConfiguration
    {
        string GetConnectionString();
        string GetContainer();
    }
}