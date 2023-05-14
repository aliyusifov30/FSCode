using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSCode.Application.DTOs.ReminderDTOs
{
    public class ReminderEditDto
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public DateTime SendAt { get; set; }
        public string Content { get; set; }
        public string Method { get; set; }
    }
}
