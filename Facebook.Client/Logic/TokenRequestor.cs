using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Facebook.Client.Logic
{
    public class TokenRequestor
    {
        /// <summary>
        /// get token
        /// </summary>
        /// <param name="config"></param>
        /// <param name="fbcode">if empty then we are asking for our website token</param>
        /// <returns></returns>
        public string GetToken(FacbookClientConfig config, string fbcode)
        {
            string baseurl = "https://graph.facebook.com/v6.0/oauth/access_token?";
            var url = QueryHelpers.AddQueryString(baseurl,
                new Dictionary<string, string>()
                {
                    { "client_id" , config.ClientId},
                    { "redirect_uri" , config.RedirectUri},
                    { "client_secret" , config.ClientSecret},
                }
            );

            if (!string.IsNullOrEmpty(fbcode))
            {
                url = QueryHelpers.AddQueryString(url, new Dictionary<string, string>()
                {
                    {"code", fbcode }
                });
            }
            else
            {
                //// get credentials for application
                url = QueryHelpers.AddQueryString(url, new Dictionary<string, string>()
                {
                    {"grant_type", "client_credentials" }
                });
            }

            using (var client = new WebClient())
            {
                try
                {
                    var response = client.OpenRead(url);
                    using (var reader = new StreamReader(response, Encoding.UTF8))
                    {
                        string value = reader.ReadToEnd();
                        var token = System.Text.Json.JsonSerializer.Deserialize<TokenResponse>(value);
                        return token.access_token;
                    }
                }
                catch (WebException e)
                {
                    var resposne = new StreamReader(e.Response.GetResponseStream()).ReadToEnd();
                    ///{"error":{"message":"This authorization code has been used.","type":"OAuthException","code":100,"error_subcode":36009,"fbtrace_id":"AHtohT4VFYAhX-Ii6g-6xL7"}}
                }

            }
            return null;
        }

        public FacebookToken InterpretToken(string token, string apptoken)
        {
            string baseurl = "https://graph.facebook.com/debug_token?";
            var url = QueryHelpers.AddQueryString(baseurl,
                new Dictionary<string, string>()
                {
                    { "input_token" , token},
                    { "access_token" , apptoken}
                }
            );
            using (var client = new WebClient())
            {
                var response = client.OpenRead(url);
                using (var reader = new StreamReader(response, Encoding.UTF8))
                {
                    string value = reader.ReadToEnd();
                    return System.Text.Json.JsonSerializer.Deserialize<FacebookToken>(value);
                    
                }
            }

            throw new Exception("Failed to parse toke");
        }
    }
}