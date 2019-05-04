using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DNWS
{
    public class TwitterApiPlugin : TwitterPlugin
    {

        private List<User> GetUsers()
        {
            using (var context = new TweetContext())
            {
                try
                {
                    List<User> users = context.Users.ToList();
                    return users;
                }
                catch (Exception)
                {
                    return null;
                }
            }

        }
        public override HTTPResponse GetResponse(HTTPRequest request)
        {
            HTTPResponse response = new HTTPResponse(200);
            string user = request.getRequestByKey("user");
            string password = request.getRequestByKey("password");
            string following = request.getRequestByKey("follow");
            string message = request.getRequestByKey("message");
            string[] path = request.Filename.Split("?");          
                if (request.Method == "GET")
                {
                    string json = JsonConvert.SerializeObject(GetUsers());
                    response.body = Encoding.UTF8.GetBytes(json);
                }
                else if (request.Method == "POST")
                {
                    try
                    {
                        Twitter.AddUser(user, password);
                        response.body = Encoding.UTF8.GetBytes("200 OK");
                    }
                    catch (Exception)
                    {
                        response.status = 403;
                        response.body = Encoding.UTF8.GetBytes("403 User already exists");
                    }
                }                              
            
            response.type = "application/json";
            return response;
        }
    }
}