using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DNWS
{
    public class TwitterApiPlugin : TwitterPlugin
    {

        private List<User> ListUsers()
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
        private bool ChackListUsers(string name)
        {
            using (var context = new TweetContext())
            {
                try
                {
                    List<User> users = context.Users.Where(b => b.Name.Equals(name)).ToList();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        private List<Following> ListFollow(string F)
        {
            using (var context = new TweetContext())
            {
                try
                {
                    List<User> ListFollow = context.Users.Where(b => b.Name.Equals(F)).Include(b => b.Following).ToList();
                    return ListFollow[0].Following;
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
            string time = request.getRequestByKey("timeline");
            string[] name = request.Filename.Split("?");
           
            if (name[0] == "API") {
                Twitter twitter = new Twitter(user);
                if (request.Method == "GET")
                {
                    string json = JsonConvert.SerializeObject(ListUsers());
                    response.body = Encoding.UTF8.GetBytes(json);
                }
                else if (request.Method == "POST")
                {
                    try
                    {
                        Twitter.AddUser(user, password);
                        response.body = Encoding.UTF8.GetBytes("SUCCESS");
                    }
                    catch (Exception)
                    {
                        response.status = 403;
                        response.body = Encoding.UTF8.GetBytes("ERROR");
                    }
                }
                else if (request.Method == "DELETE")
                {
                
                try
                {
                    twitter.RemoveUser(user);
                    response.body = Encoding.UTF8.GetBytes("OK^_^");
                }
                catch (Exception)
                {
                    response.body = Encoding.UTF8.GetBytes("DELETE ERROR");
                }
                 }
            }
            else if (name[0] == "FOLLOW")
            {
                Twitter twitter = new Twitter(user);
                if (request.Method == "GET")
                {
                    string json = JsonConvert.SerializeObject(ListFollow(user));
                    response.body = Encoding.UTF8.GetBytes(json);
                }
                else if (request.Method == "POST")
                {
                    try
                    {
                        Twitter Friend = new Twitter(user);
                        Friend.AddFollowing(following);
                        response.body = Encoding.UTF8.GetBytes("SUCCESS");
                    }
                    catch (Exception)
                    {
                        response.status = 403;
                        response.body = Encoding.UTF8.GetBytes("ERROR");
                    }
                }
                else if (request.Method == "DELETE")
                {
              
                    try
                    {
                        twitter.RemoveFollowing(following);
                        response.body = Encoding.UTF8.GetBytes("OK^_^");
                    }
                    catch (Exception)
                    {
                        response.body = Encoding.UTF8.GetBytes("DELETE ERROR");
                    }
                }
            }
            else if (name[0] == "TWEET")
            {
                Twitter twitter = new Twitter(user);
                if (request.Method == "GET")
                {
                    if (time == "FOLLOW")
                    {
                        string json = JsonConvert.SerializeObject(twitter.GetFollowingTimeline());
                        response.body = Encoding.UTF8.GetBytes(json);
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(twitter.GetFollowingTimeline());
                        response.body = Encoding.UTF8.GetBytes(json);
                    }
                }
                else if (request.Method == "POST")
                {
                    try
                    {
                        twitter.PostTweet(message);
                        response.body = Encoding.UTF8.GetBytes("SUCCESS");
                    }
                    catch (Exception)
                    {
                        response.status = 403;
                        response.body = Encoding.UTF8.GetBytes("ERROR");
                    }
                }
                else if (request.Method == "DELETE")
                {
                    try
                    {
                        
                        response.body = Encoding.UTF8.GetBytes("OK^_^");
                    }
                    catch (Exception)
                    {
                        response.body = Encoding.UTF8.GetBytes("DELETE ERROR");
                    }
                }
            }
                response.type = "application/json";
            return response;
        }
    }
}