using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSCode.Application.Exceptions
{
    public class ReminderNotFoundException : Exception
    {
        public ReminderNotFoundException():base("Reminder Not Found")
        {
            
        }
    }
}
