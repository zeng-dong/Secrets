namespace InClassLib
{
    public class Secrets
    {
        public Address Address { get; set; }
        public string Password { get; set; }
        public string ConnectionStringOne { get; set; }
    }

    public class Address
    {
        public string Email { get; set; }
        public string Shipping { get; set; }
    }
}
