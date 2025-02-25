namespace HMAC_Authentication.Models
{
    public static class ClientSecrets
    {
        public static Dictionary<string, string> Secrets = new Dictionary<string, string>
        {
            {"ClientId1", "SecretKey1" },
            {"ClientId2", "SecretKey2" },
            {"ClientId3", "SecretKey3" }
        };
    }
}
