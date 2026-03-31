namespace Spelers_API.ViewModels
{
    public class SpelerVM
    {
        public string Naam { get; set; }
        public int Leeftijd { get; set; }
        public string PositieNaam { get; set; }  
        public string TeamNaam { get; set; }
    }

    public class SpelerPostVM
    {
        public string? Naam { get; set; }
        public int Leeftijd { get; set; }
        public int? PositieId { get; set; }  
        public int? TeamId { get; set; }
    }
}
