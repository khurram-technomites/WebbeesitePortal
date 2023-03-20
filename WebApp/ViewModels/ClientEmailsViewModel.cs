namespace WebApp.ViewModels
{
    public class ClientEmailsViewModel
    {
        public long Id { get; set; }

        public long ClientId { get; set; }


        public string Email { get; set; }


        public GarageViewModel Garage { get; set; }
    }
}
