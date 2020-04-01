using Facebook.Client.Logic;
using Galnet.Common;
using Galnet.Plugin.Base;
using Galnet.Plugin.Base.Types;
using System;

namespace Facebook.Client
{
    public class Service
    {
        private TokenRequestor tokenRequestor = new TokenRequestor();
        private UserProfileReader profileReader = new UserProfileReader();


        public void Register(PluginService ps)
        {
            ps.AddPlugin<Value<string>>("Facebook.Client.Service.Login", this.Login)
                .SetDefaultConfig<FacbookClientConfig>(new FacbookClientConfig());
            ps.AddProvider<Value<string>>("Facebook.Client.Service.AppToken", this.AppToken)
                .SetDefaultConfig<FacbookClientConfig>(new FacbookClientConfig()); 
            ps.AddPlugin<Object, Value<string>>("Facebook.Client.Service.LoginLink", this.LoginLink)
                 .SetDefaultConfig<FacbookClientConfig>(new FacbookClientConfig());
        }



        private Value<string> LoginLink(IContext context, Object payload)
        {
            var state = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

            var config = context.GetConfig<FacbookClientConfig>();
            var url = $"https://www.facebook.com/v6.0/dialog/oauth?client_id={config.ClientId}&redirect_uri={config.RedirectUri}&scope={profileReader.scopes}&state={{{state}}}";
            return new Value<string>()
            {
                Data = url
            };
        }

        private Value<string> AppToken(IContext context)
        {
            var config = context.GetConfig<FacbookClientConfig>();

            var d = tokenRequestor.GetToken(config, null);

            return new Value<string>()
            {
                Data = d
            };
        }

        private void Login(IContext context, Value<string> code)
        {
            var config = context.GetConfig<FacbookClientConfig>();
            var appToken = context.Call("Facebook.Client.Service.AppToken").Response<Value<string>>().Data;
            var tr = tokenRequestor.GetToken(config, code.Data);
            //// ensure token is ok
            var data = tokenRequestor.InterpretToken(tr, appToken);

            var userProfile = profileReader.GetUser(tr, data.data.user_id);

        }
    }
}