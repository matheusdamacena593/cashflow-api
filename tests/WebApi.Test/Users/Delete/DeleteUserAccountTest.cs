using FluentAssertions;
using System.Net;

namespace WebApi.Test.Users.Delete
{
    public class DeleteUserAccountTest : CashFlowClassFixture
    {
        private const string METHOD = "api/User";

        private readonly string _tokenTeamMember;
        private readonly string _tokenAdmin;

        public DeleteUserAccountTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _tokenTeamMember = webApplicationFactory.User_Team_Member.GetToken();
            _tokenAdmin = webApplicationFactory.User_Admin.GetToken();
        }

        [Fact]
        public async Task Success_Team_Member()
        {
            var result = await DoDelete(requestUri: METHOD, token: _tokenTeamMember);

            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Success_Admin()
        {
            var result = await DoDelete(requestUri: METHOD, token: _tokenAdmin);

            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
