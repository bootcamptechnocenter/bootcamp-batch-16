using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace day_2
{
    public class Rekening
    {
        private decimal _saldo;

        public decimal Saldo => _saldo;

        public void Setor(decimal jumlah)
        {
            if (jumlah <= 0)
            {
                throw new ArgumentException("Jumlah setor harus positif.");
            }

            _saldo += jumlah;
        }

        public bool Tarik(decimal jumlah)
        {
            if (jumlah <= 0)
            {
                throw new ArgumentException("Jumlah tarik harus positif.");
            }

            if (jumlah > _saldo)
            {
                throw new ArgumentException("Saldo tidak mencukupi untuk melakukan penarikan.");
            }

            _saldo -= jumlah;
            return true;
        }
    }
}