using System.Collections.Generic;

namespace EventaDors.Entities.Interfaces
{
    public interface IUserFactory
    {
        IUser Create(IUser user);
        bool Update(IUser user);
        bool Delete(IUser user);
        IList<IUser> List();
    }
}