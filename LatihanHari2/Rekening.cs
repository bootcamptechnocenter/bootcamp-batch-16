using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatihanHari2
{
    public class Rekening
    {
        private decimal _saldo;
        public decimal Saldo => _saldo;
        public void Setor(decimal jumlah)
        {
            if (jumlah > 0)
            {
                _saldo += jumlah;
                Console.WriteLine($"Setor: {jumlah:C}, Saldo: {_saldo:C}");
            }
            else
            {
                Console.WriteLine("Jumlah setor harus positif.");
            }
        }

        public bool Tarik(decimal jumlah)
        {
            if (jumlah > _saldo)
            {
                throw new ArgumentException("Saldo tidak cukup untuk melakukan penarikan.");
            }
            else
            {
                _saldo -= jumlah;
                Console.WriteLine($"Tarik: {jumlah:C}, Saldo: {_saldo:C}");
                return true;
            }
        }
    }
}