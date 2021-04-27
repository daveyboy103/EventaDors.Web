using EventaDors.Entities.Classes;

namespace EventaDors.DataManagement
{
    public class LoginResult
    {
        public LoginResult(bool result, string notes, User user)
        {
            Result = result;
            Notes = notes;
            User = user;
        }

        public bool Result { get; }
        public string Notes { get; }
        public User User { get; }
    }
}