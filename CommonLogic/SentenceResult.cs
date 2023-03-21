using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLogic
{
    public class SentenceResult
    {
        public string PlainText { get; set; }
        public bool WasPresent { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
    }
}
