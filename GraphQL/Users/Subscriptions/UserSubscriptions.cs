using LaundryCleaning.Common.Constants;
using LaundryCleaning.Common.Models.Entities;
using LaundryCleaning.Common.Models.Messages;

namespace LaundryCleaning.GraphQL.Users.Subscriptions
{
    [ExtendObjectType(ExtendObjectTypeConstants.Subscription)]
    public class UserSubscriptions
    {
        [Subscribe]
        [Topic]
        public UserCreated OnUserCreated([EventMessage] UserCreated user)
        {
            return user;
        }
    }
}
