namespace ApiDigitalDesign.Models.AuthModels
{
    public class TokenModel
    {
        public string RefreshToken { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
    }
}
