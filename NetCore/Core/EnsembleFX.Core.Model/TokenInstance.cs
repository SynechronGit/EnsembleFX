namespace EnsembleFX.Core.Model
{
    public static class TokenInstance
    {
        public static ApiToken ApiToken { get; set; }

        static TokenInstance()
        {
            ApiToken = new ApiToken();
        }
    }
}
