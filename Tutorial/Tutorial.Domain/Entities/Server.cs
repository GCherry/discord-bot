using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tutorial.Domain.Entities
{
    public class Server
    {
        public ulong Id { get; set; }
        public string Prefix { get; set; }
        public string Name { get; set; }
    }
}
