using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Model
{
    public partial class Token
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string TokenValue { get; set; }

        public DateTime Expiration { get; set; }

        public virtual User User { get; set; }
    }
}