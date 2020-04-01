using System.Collections.Generic;

namespace Facebook.Client.Logic
{
    public class FacebookTokenData
    {  
        public List<string>scopes { get; set; }
        public string user_id { get; set; }

        //{  "data": {
        //    "app_id": 138483919580948, 
        //    "type": "USER",
        //    "application": "Social Cafe", 
        //    "expires_at": 1352419328, 
        //    "is_valid": true, 
        //    "issued_at": 1347235328, 
        //    "metadata": {
        //        "sso": "iphone-safari"
        //    }, 
        //    "scopes": [
        //        "email",
        //        "publish_actions"
        //    ], 
        //    "user_id": "1207059"
        //}
    }
}