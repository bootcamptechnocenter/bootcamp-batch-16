namespace Day2
{
    public class WhatsApp(
        string name,
        string address,
        string phone,
        string whatsAppNumber
        ) : Identity(name, address, phone)
    {
        public string WhatsAppNumber { get; set; } = whatsAppNumber;

        public override void TypeOfIdentity()
        {
            Console.WriteLine("WHATSAPP");
        }

        public void PrintWhatsApp()
        {
            PrintIdentity();
            Console.WriteLine($"WhatsApp: {WhatsAppNumber}");
        }
    }
}