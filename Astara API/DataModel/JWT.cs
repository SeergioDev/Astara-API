namespace Astara_API.DataModel
{
    public class JWT
    {
        public string Audience { get; set; }
        public string Isuer  { get; set; }
        public string Secret { get; set; }
    }

}
