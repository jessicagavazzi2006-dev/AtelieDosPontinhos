namespace AtelieDosPontinhos.UI.Models
{
    public class LoginApiResponse
    {
        public bool Succeeded { get; set; }
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
    }
}