namespace KSiwiak_Urzad_API.Data
{
    public interface IUrzadDBContextProvider
    {
        UrzadDBContext GetUrzadDBContext(string connectionString);
    }
}
