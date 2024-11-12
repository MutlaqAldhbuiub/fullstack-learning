using NetworkService.Models;
using Microsoft.EntityFrameworkCore;
using NetworkService.Data;

namespace NetworkService.Repository
{
    public class NetworkRepo : INetworkRepo
    {
        private readonly AppDbContext _context;

        public NetworkRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Network>> GetAllNetworks()
        {
            var result = await _context.networks.ToListAsync();
            return result;
        }

        public async Task<Network> getNetworkById(int id)
        {
            return await _context.networks.FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<bool> addNetwork(Network network)
        {
            try
            {
                _context.networks.Add(network);
                await _context.SaveChangesAsync();
                return true; // Task completed successfully
            }
            catch (Exception ex)
            {
                return false; // Task failed
            }
        }

        public async Task<bool> deleteNetwork(int id)
        {
            try
            {
                var network = _context.networks.FirstOrDefault(l => l.Id == id);
                _context.networks.Remove(network);
                await _context.SaveChangesAsync();
                return true; // Task completed successfully
            }
            catch (Exception ex)
            {
                return false; // Task failed
            }
        }

        public async Task<bool> changeNetworkStatus(int id, bool status)
        {
            try
            {
                var network = _context.networks.FirstOrDefault(l => l.Id == id);
                network.IsConnected = status;
                await _context.SaveChangesAsync();
                return true; // Task completed successfully
            }
            catch (Exception ex)
            {
                return false; // Task failed
            }
        }
    }
}