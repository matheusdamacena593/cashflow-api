using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Login.DoLogin
{
    public class DoLoginUseCase : IDoLoginUseCase
    {
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IPasswordEncripter _passwordEncripter;
        private readonly IAccessTokenGenerator _tokenGenerator;

        public DoLoginUseCase(
            IUserReadOnlyRepository userReadOnlyRepository,
            IPasswordEncripter passwordEncripter,
            IAccessTokenGenerator tokenGenerator)
        {
            _userReadOnlyRepository = userReadOnlyRepository;
            _passwordEncripter = passwordEncripter;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
        {
            var user = await _userReadOnlyRepository.GetUserByEmail(request.Email);

            if (user is null)
            {
                throw new InvalidLoginException();
            }

            var passwordMatch = _passwordEncripter.VerifyPassword(request.Password, user.Password);

            if (passwordMatch is false)
            {
                throw new InvalidLoginException();
            }

            return new ResponseRegisteredUserJson
            {
                Name = user.Name,
                Token = _tokenGenerator.Generate(user)
            };
        }
    }
}
