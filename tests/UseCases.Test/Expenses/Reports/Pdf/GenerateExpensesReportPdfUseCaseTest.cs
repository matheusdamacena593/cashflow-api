using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Application.UseCases.Expenses.Reports.Excel;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Expenses.Reports.Pdf
{
    public class GenerateExpensesReportPdfUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var loggedUser = UserBuilder.Build();
            var expenses = ExpenseBuilder.Collection(loggedUser);

            var useCase = CreateUseCase(loggedUser, expenses);

            var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today));

            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Success_Empty()
        {
            var loggedUser = UserBuilder.Build();

            var useCase = CreateUseCase(loggedUser, []);

            var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today));

            result.Should().BeEmpty();
        }

        private GenerateExpensesReportPdfUseCase CreateUseCase(User user, List<Expense> expenses)
        {
            var repository = new ExpensesReadOnlyRepositoryBuilder().FilterByMonth(user, expenses).Build();
            var loggerUser = LoggedUserBuilder.Build(user);

            return new GenerateExpensesReportPdfUseCase(repository, loggerUser);
        }
    }
}
