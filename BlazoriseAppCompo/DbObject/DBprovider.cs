using System.Data.Common;

namespace GGAPost.Data.DbObject
{
    public class DBprovider
    {
        private static DbProviderFactory factory = null;

        public static DbProviderFactory Create()
        {

            factory = DbProviderFactories.GetFactory(DbConnection.DbProvidrConnectionName);
            return factory;
        }
    }
}
