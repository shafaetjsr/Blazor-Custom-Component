using Microsoft.Extensions.Configuration;

namespace GGAPost.Data.DbObject
{
    public static class DbConnection
    {
        public static string CName
        {
            get => cName;
        }

        public static string DbProvidrType
        {
            get => DbProvidrConnectionName;
        }

        static IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());

        public static string cName = conf["ConnectionStrings:ConnctionStringName"].ToString();

        public static string DbProvidrConnectionName = conf["DbProviderFactories"].ToString();
    }
}
