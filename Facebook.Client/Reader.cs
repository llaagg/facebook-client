using System;
using System.Threading.Tasks;

namespace Facebook.Client
{
    public class Reader
    {
        public void GO()
        {
            var accessToken = "";
            var facebookClient = new FacebookClient();
            var facebookService = new FacebookService(facebookClient);
            var getAccountTask = facebookService.GetAccountAsync(accessToken);

            Task.WaitAll(getAccountTask);
            var account = getAccountTask.Result;
            Console.WriteLine($"{account.Id} {account.Name}");

            var postOnWallTask = facebookService.PostOnWallAsync(accessToken, "Hello from C# .NET Core!");
            Task.WaitAll(postOnWallTask);
        }

    }
}