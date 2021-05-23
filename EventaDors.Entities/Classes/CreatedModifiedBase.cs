using System;
using System.Collections.Generic;

namespace EventaDors.Entities.Classes
{
    public abstract class CreatedModifiedBase : ProcessStatus
    {
        protected CreatedModifiedBase()
        {
            HelpMessages = new Dictionary<string, string>();
        }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        
        public IDictionary<string, string> HelpMessages { get; }
    }
}