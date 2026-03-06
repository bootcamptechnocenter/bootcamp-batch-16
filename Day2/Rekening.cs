namespace Day2
{
    public class Rekening
    {
        private decimal _saldo;

        public decimal Saldo
        {
            get { return _saldo; }
            private set { _saldo = value; }
        }

        public Rekening(decimal saldoAwal)
        {
            if (saldoAwal < 0)
            {
                throw new ArgumentException("Saldo awal tidak boleh negatif.");
            }
            Saldo = saldoAwal;
        }

        public void TampilkanSaldo()
        {
            Console.WriteLine($"Saldo saat ini: {Saldo:C}");
        }

        public void Setor(decimal jumlah)
        {
            if (jumlah > 0)
            {
                Saldo += jumlah;
                Console.WriteLine($"Setor sebesar {jumlah:C} berhasil. Saldo saat ini: {Saldo:C}");
            }
            else
            {
                throw new ArgumentException("Jumlah setor harus lebih besar dari 0.");
            }
        }

        public void Tarik(decimal jumlah)
        {
            if (jumlah > 0)
            {
                if (jumlah <= Saldo)
                {
                    Saldo -= jumlah;
                    Console.WriteLine($"Tarik sebesar {jumlah:C} berhasil. Saldo saat ini: {Saldo:C}");
                }
                else
                {
                    throw new InvalidOperationException("Saldo tidak cukup untuk melakukan penarikan.");
                }
            }
            else
            {
                throw new ArgumentException("Jumlah tarik harus lebih besar dari 0.");
            }
        }
    }
}