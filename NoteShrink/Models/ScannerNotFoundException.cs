using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteShrink.Models
{
    class ScannerNotFoundException : ScannerException
    {
        public ScannerNotFoundException()
            : base("Ошибка получения списка сканеров! Проверьте, включен ли ваш сканер или многофункциональный принтер.")
        {
        }
    }
}
