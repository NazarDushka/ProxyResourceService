using ProxyResource.Models;

namespace ProxyResource.Interfaces
{
    public interface IResourceService
    {
        Task<Resource> GetResourceById(int id);
    }
}
