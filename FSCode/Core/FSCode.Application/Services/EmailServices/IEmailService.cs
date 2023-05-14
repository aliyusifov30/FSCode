using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSCode.Application.Services.EmailServices
{
    public interface IEmailService
    {
        Task Send(string to, string subject, string html);
        string FormatHTML(string fileName, params string[] formats);
    }
}
