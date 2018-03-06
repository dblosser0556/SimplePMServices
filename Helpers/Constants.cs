namespace SimplePMServices.Helpers
{
    public static class Constants
    {
        public static class Strings
        {
            public static class JwtClaimIdentifiers
            {
                public const string Rol = "rol", Id = "id";
            }

            public static class JwtClaims
            {
                public const string ApiAccess = "apiAccess";
                public const string Admin = "admin";
                public const string EditPrograms = "editPrograms";
                public const string EditProjects = "editProjects";
            }
        }
    }
}
