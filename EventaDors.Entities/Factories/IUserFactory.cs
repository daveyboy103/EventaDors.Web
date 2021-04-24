using System.Collections.Generic;
using EventaDors.Entities.Interfaces;

namespace EventaDors.Entities.Factories
{
    public interface IUserFactory
    {
        IUser Create(long? id);
        bool Update(IUser user);
        bool Delete(IUser user);
        IList<IUser> List();
    }
}