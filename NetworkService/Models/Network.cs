namespace NetworkService.Models
{
    public class Network
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public bool IsConnected { get; set; }
    }
}