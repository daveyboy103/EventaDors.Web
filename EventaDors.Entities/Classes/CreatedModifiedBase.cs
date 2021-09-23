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
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime Modified { get; set; } = DateTime.UtcNow;
        
        public IDictionary<string, string> HelpMessages { get; }
    }
}