using Microsoft.EntityFrameworkCore;

namespace KSiwiak_Urzad_API.Data
{
    public class UrzadDBContextProvider : IUrzadDBContextProvider
    {
        UrzadDBContext IUrzadDBContextProvider.GetUrzadDBContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UrzadDBContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new UrzadDBContext(optionsBuilder.Options);
        }
    }
}
