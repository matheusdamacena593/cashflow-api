using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public class RequestChangePasswordJsonBuilder
    {
        public static RequestChangePasswordJson Build()
        {
            return new Faker<RequestChangePasswordJson>()
                .RuleFor(user => user.Password, (faker, user) => faker.Internet.Password())
                .RuleFor(user => user.NewPassword, (faker, user) => faker.Internet.Password(prefix: "!Aa1"));
        }
    }
}
