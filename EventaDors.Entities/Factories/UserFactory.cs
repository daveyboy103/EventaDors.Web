using System.Collections.Generic;
using EventaDors.Entities.Interfaces;

namespace EventaDors.Entities.Factories
{
    public class UserFactory : IUserFactory
    {
        public IUser Create(long? id)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(IUser user)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(IUser user)
        {
            throw new System.NotImplementedException();
        }

        public IList<IUser> List()
        {
            throw new System.NotImplementedException();
        }
    }
}