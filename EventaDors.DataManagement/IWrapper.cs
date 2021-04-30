using System;
using System.Collections.Generic;
using EventaDors.Entities.Classes;

namespace EventaDors.DataManagement
{
    public interface IWrapper
    {
        LoginResult Authenticate(string userName, string password);
        User CreateUser(long userId);
        bool DeleteUser(long userId, bool deactivateOnly);
        bool ChangePassword(long userId, string oldPassword, string newPassword);
        User UpdateUser(User user);
        QuoteRequest CreateRequestFromTemplate(int templateId, int userId, int attendees, DateTime dueDate);
        QuoteRequest LoadQuoteRequest(int quoteIdIdentity);
        User RegisterUser(User user);
        bool VerifyUser(Guid guid);
        bool BlockUser(int userId, int blockedUserId);
        bool UnblockUser(int userId, int blockedUserId);
        int GetUserTokenBalance(int userId);
        IList<Deadline> GetDeadlines(int quoteIdIdentity, int alarmThreshold);
        QuoteRequestElementResponse PickupQuoteRequestItem(QuoteRequestElementResponse response);
        bool AssignUserToQuoteElement(int userId, int quoteElementId, bool active);
        bool AssignUserToElementType(int userId, int quoteElementTypeId, bool active);
        QuoteElement AddUpdateQuoteElement(QuoteElement quoteElement);
        QuoteElementType AddUpdateQuoteElementType(QuoteElementType quoteElementType);
        QuoteType AddUpdateQuoteType(QuoteType quoteType);
        QuoteSubType AddUpdateQuoteSubType(QuoteSubType quoteSubType);
        QuoteTemplate AddUpdateQuoteTemplate(QuoteTemplate quoteTemplate);
        UserType AddUpdateUserType(UserType userType);
        long CreateNewUser(User user);
        QuoteRequestElement AddQuoteElementToQuoteRequest(QuoteRequestElement quoteElement);
        IEnumerable<User> ListUsers();
    }
}