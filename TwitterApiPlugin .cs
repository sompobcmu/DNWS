using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

/// <summary>
/// A part of code from 600611030
/// </summary>

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
                    List<User> users = context.Users.ToList(); //list users store in users .
                    return users;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        private List<Following> ListFollow(string F)
        {
            using (var context = new TweetContext())
            {
                try
                {
                    List<User> ListFollow = context.Users.Where(b => b.Name.Equals(F)).Include(b => b.Following).ToList(); //ListFollow users store in ListFollow 
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
            string[] name = request.Filename.Split("?");        //split part 
           
            if (name[0] == "API") {
                Twitter twitter = new Twitter(user);
                if (request.Method == "GET")
                {
                    string json = JsonConvert.SerializeObject(ListUsers()); //show list user 
                    response.body = Encoding.UTF8.GetBytes(json);
                }
                else if (request.Method == "POST")
                {
                    try
                    {
                        Twitter.AddUser(user, password); //add usersby use name and password.
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
                    twitter.RemoveUser(user); //remove user by use name  but sill bug if user have following it can not delete.
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
                    string json = JsonConvert.SerializeObject(ListFollow(user)); //list all following.
                    response.body = Encoding.UTF8.GetBytes(json);
                }
                else if (request.Method == "POST")
                {
                    try
                    {
                        if (twitter.chack(following)) { //chack it have follow in list if not add it.
                        Twitter Friend = new Twitter(user);
                        Friend.AddFollowing(following);   //AddFollowing
                        response.body = Encoding.UTF8.GetBytes("SUCCESS");
                        }
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
                        twitter.RemoveFollowing(following);  //RemoveFollowing.
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
                        string json = JsonConvert.SerializeObject(twitter.GetFollowingTimeline()); //get timeline Following if FOLLOW
                        response.body = Encoding.UTF8.GetBytes(json);
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(twitter.GetFollowingTimeline()); //get timeline Following
                        response.body = Encoding.UTF8.GetBytes(json);
                    }
                }
                else if (request.Method == "POST")
                {
                    try
                    {
                        twitter.PostTweet(message);    //POst message
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