﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSCode.Application.DTOs.ReminderDTOs
{
    public class ReminderCreateDto
    {
        //public string To { get; set; }
        public DateTime SendAt { get; set; }
        public string Content { get; set; }
        public string Method { get; set; }
    }
}
