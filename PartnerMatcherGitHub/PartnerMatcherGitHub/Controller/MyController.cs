using partnersMatcherPart4.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace partnersMatcherPart4.Controller
{
    public class MyController
    {
        public string currentUserEmail;
        public string currentFullName;
        MyModel model;
        public List<string> areas;

        public MyController()
        {
            model = new MyModel();
            areas = model.getListOfAreas();
            currentUserEmail = "";
        }

        public bool addNewUser(User user)
        {
            return model.add_user_record(user);
        }

        public bool login(string email, string pass)
        {
            if (model.userConnect(email, pass)) // if login true
            {
                currentUserEmail = email;
                return true;
            }

            return false; // return login result
        }



        public Dictionary<int, Post> search(string city, string area)
        {
            Dictionary<int, Post> res = new Dictionary<int, Post>();


            return res;
        }


        public void addNewPost(Post post, string area)
        {
            if (area != "" && !areas.Contains(area))
            {
                model.addNewArea(area);
                areas = model.getListOfAreas();

            }

            model.add_post_record(post);
        }


        public void publishPost(int postID, bool publish)
        {
            model.PublishPost(postID, publish);
        }


        public Dictionary<int, Post> getUsersPosts()
        {
            return model.getUserPosts(currentUserEmail);
        }

        public Dictionary<int, Post> getPublishedPost()
        {
            return model.getAllPublishedPosts();
        }


        public void setCurrentFullName()
        {
            List<string> name = model.getUserFirstnameLastnameByEmail(currentUserEmail);
            currentFullName = name[0] + " " + name[1];

        }

        public void setRequestStatus(string senderID, int postID, int rank, string status)
        {
            model.updateRequestStatusRank(senderID, postID, rank, status);
        }



        public List<Request> getRequests()
        {
            return model.GetRequests(currentUserEmail);
        }

        public void sendRequest(Request request)
        {
            model.SendRequest(request);
        }
    }
}
