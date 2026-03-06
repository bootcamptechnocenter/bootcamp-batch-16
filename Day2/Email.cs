namespace Day2
{
    public class Email(
        string name,
        string address,
        string phone,
        string emailAddress
        ) : Identity(name, address, phone)
    {
        public string EmailAddress { get; set; } = emailAddress;

        public override void TypeOfIdentity()
        {
            Console.WriteLine("EMAIL");
        }

        public void PrintEmail()
        {
            PrintIdentity();
            Console.WriteLine($"Email: {EmailAddress}");
        }
    }
}