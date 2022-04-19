namespace KSiwiak_Urzad_API.Models
{
    public class Urzad_Woj
    {
        public int id { get; set; }

        public string nazwa_urzedu { get; set; }

        public int wojewodztwo_id { get; set; }

        public string nazwa_wojewodztwa { get; set; }
    }
}
