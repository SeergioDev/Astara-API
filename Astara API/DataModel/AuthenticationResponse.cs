using System.Text.Json.Serialization;

namespace Astara_API.DataModel
{
    public class AuthenticationResponse
    {
        [JsonIgnore]
        public string User { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public string Token { get; set; }  
    }
}
