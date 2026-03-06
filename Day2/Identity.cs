namespace Day2
{
    public class Identity(
        string name,
        string address,
        string phone
        )
    {
        public string Name { get; set; } = name;
        public string Address { get; set; } = address;
        public string Phone { get; set; } = phone;

        public virtual void TypeOfIdentity()
        {
            Console.WriteLine("IDENTITY");
        }

        public void PrintIdentity()
        {
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Address: {Address}");
            Console.WriteLine($"Phone: {Phone}");
        }
    }
}