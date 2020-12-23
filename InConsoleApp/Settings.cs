namespace InConsoleApp
{
    public class Settings
    {
        public AddressSettings Address { get; set; }

        public string Password { get; set; }
        public string ConnectionStringOne { get; set; }
    }

    public class AddressSettings
    {
        public string Email { get; set; }
        public string Shipping { get; set; }
    }
}
