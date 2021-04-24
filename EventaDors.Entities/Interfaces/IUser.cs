using System;
using System.Collections.Generic;

namespace EventaDors.Entities.Interfaces
{
    public interface IUser
    {
        long Id { get; }
        string UserName { get; set; }
        DateTime Created { get; }
        DateTime Modified { get; }
        Guid Uuid { get; }
        IList<IUserPasswordHistory> Passwords { get; }
        IList<IQuoteRequest> QuoteRequests { get; }
    }
}