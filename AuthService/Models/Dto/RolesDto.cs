namespace AuthService.Models.Dto
{
    public class RolesDto
    {
        public string username { get; set; }
        public IList<string> roles { get; set; }
    }
}