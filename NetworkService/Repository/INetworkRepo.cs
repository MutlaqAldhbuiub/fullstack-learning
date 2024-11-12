using NetworkService.Models;

namespace NetworkService.Repository
{
    public interface INetworkRepo
    {
        Task<IEnumerable<Network>> GetAllNetworks();

        Task<Network> getNetworkById(int id);

        Task<bool> addNetwork(Network network);

        Task<bool> deleteNetwork(int id);

        Task<bool> changeNetworkStatus(int id, bool status);
    }
}