﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IEmailService
    {
        public bool SendEmail(string to, string subject, string body);
    }
}
