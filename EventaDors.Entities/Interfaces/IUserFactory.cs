using System.Collections.Generic;

namespace EventaDors.Entities.Interfaces
{
    public interface IUserFactory
    {
        IUser Create(long? id);
        bool Update(IUser user);
        bool Delete(IUser user);
        IList<IUser> List();
    }
}