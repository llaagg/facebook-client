using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Facebook.Client.Logic
{
    public class UserProfileReader
    {
        public string scopes = "email";

        /// <summary>
        /// get token
        /// </summary>
        /// <param name="config"></param>
        /// <param name="fbcode">if empty then we are asking for our website token</param>
        /// <returns></returns>
        public UserProfile GetUser(string token, string id)
        {
            string baseurl =
                //$"https://graph.facebook.com/me";
                $"https://graph.facebook.com/v6.0/{id}/";
            var url = QueryHelpers.AddQueryString(baseurl,
                new Dictionary<string, string>()
                {
                    { "fields" , scopes},
                    { "access_token" , token},
                }
            );

            
            using (var client = new WebClient())
            {
                try
                {
                    var response = client.OpenRead(url);
                    using (var reader = new StreamReader(response, Encoding.UTF8))
                    {
                        string value = reader.ReadToEnd();
                        var profile =  System.Text.Json.JsonSerializer.Deserialize<UserProfile>(value);
                        profile.profile_pic = $"http://graph.facebook.com/{profile.id}/picture?type=small";
                        return profile;
                    }
                }
                catch(WebException e)
                {
                    var resposne = new StreamReader(e.Response.GetResponseStream()).ReadToEnd();
                }
                
            }
            throw new Exception("Failed to get user profile inoramtion token");
        }
    }
}