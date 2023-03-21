using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLogic.DTO
{
    public class NewJobReqeust
    {
        public string AudioUrl { get; set; }
        public List<string> sentences { get; set; }
    }
}
