using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class TokenDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string TokenValue { get; set; }

        public DateTime Expiration { get; set; }

    }

}