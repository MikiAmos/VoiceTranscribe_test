using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLogic
{
    public class Job
    {
        public Guid id { get; init; } = Guid.NewGuid();
        public Guid JobId { get; init; }

        public JobStatus Status { get; set; }

        public string AudioUrl { get; set; }

       public List<SentenceResult> Sentences { get; set; }

    }
}
