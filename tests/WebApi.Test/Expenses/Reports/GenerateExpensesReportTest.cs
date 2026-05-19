using FluentAssertions;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace WebApi.Test.Expenses.Reports
{
    public class GenerateExpensesReportTest : CashFlowClassFixture
    {
        private const string METHOD = "api/Report";

        private readonly string _adminToken;
        private readonly string _teamMemberToken;
        private readonly string _expenseDate;

        public GenerateExpensesReportTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _adminToken = webApplicationFactory.User_Admin.GetToken();
            _teamMemberToken = webApplicationFactory.User_Team_Member.GetToken();
            var date = webApplicationFactory.Expense_Admin.GetDate();
            _expenseDate = date.ToString("yyyy-MM", CultureInfo.InvariantCulture);
        }

        [Fact]
        public async Task Success_Pdf()
        {
            var result = await DoGet(requestUri: $"{METHOD}/pdf?month={_expenseDate:Y}", token: _adminToken);

            result.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Content.Headers.ContentType.Should().NotBeNull();
            result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Pdf);
        }

        [Fact]
        public async Task Success_Excel()
        {
            var result = await DoGet(requestUri: $"{METHOD}/excel?month={_expenseDate:Y}", token: _adminToken);

            result.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Content.Headers.ContentType.Should().NotBeNull();
            result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Octet);
        }

        [Fact]
        public async Task Error_Forbidden_User_Not_Allowed_Pdf()
        {
            var result = await DoGet(requestUri: $"{METHOD}/pdf?month={_expenseDate:Y}", token: _teamMemberToken);

            result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Error_Forbidden_User_Not_Allowed_Excel()
        {
            var result = await DoGet(requestUri: $"{METHOD}/excel?month={_expenseDate:Y}", token: _teamMemberToken);

            result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
