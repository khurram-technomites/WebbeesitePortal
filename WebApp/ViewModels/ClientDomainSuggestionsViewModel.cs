namespace WebApp.ViewModels
{
    public class ClientDomainSuggestionsViewModel
    {
        public long Id { get; set; }

        public long ClientId { get; set; }


        public string Domain { get; set; }

        public int Position { get; set; }
        public GarageViewModel Garage { get; set; }
    }
}
