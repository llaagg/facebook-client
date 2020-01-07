﻿using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Client
{
    public interface IFacebookService
    {
        Task<Account> GetAccountAsync(string accessToken);
        Task PostOnWallAsync(string accessToken, string message);
    }

    public class FacebookService : IFacebookService
    {
        private readonly IFacebookClient _facebookClient;

        public FacebookService(IFacebookClient facebookClient)
        {
            _facebookClient = facebookClient;
        }

        public async Task<Account> GetAccountAsync(string accessToken)
        {
            var result = await _facebookClient.GetAsync<dynamic>(
                accessToken, "me", "fields=id,name,email,first_name,last_name,age_range,birthday,gender,locale");

            //if (result == null)
            //{
            //    throw new MissingFieldException();
            //}

            //var account = new Account
            //{
            //    Id = result.id,
            //    Email = result.email,
            //    Name = result.name,
            //    UserName = result.username,
            //    FirstName = result.first_name,
            //    LastName = result.last_name,
            //    Locale = result.locale
            //};

            return new Account();
        }

        public async Task PostOnWallAsync(string accessToken, string message)
            => await _facebookClient.PostAsync(accessToken, "me/feed", new {message});
    }
}
