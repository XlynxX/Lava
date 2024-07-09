using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lava.Raknet.Utils
{
    public class HandshakeData
    {
        public string salt { get; set; }
        public string signedToken { get; set; }
    }
}
