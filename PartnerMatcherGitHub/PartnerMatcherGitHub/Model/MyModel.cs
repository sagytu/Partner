using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace partnersMatcherPart4.Model
{
    /// <summary>
    /// my model class, all the funcionality of PartnerMatcher.
    /// </summary>
    public class MyModel
    {
        public OleDbConnection dataset;
        List<string> emailToUser; //emails list
        /// <summary>
        /// constructor
        /// </summary>
        public MyModel()
        {
            //connect to dataset
            string accessFilePath = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));
            string folderBinPath = @"\partnersMatcherDB.accdb";
            string fullPath = accessFilePath + folderBinPath;
            //string fullPath = @"C:\Users\lior\Documents\visual studio 2015\Projects\partnersMatcherPart4\partnersMatcherPart4\DB";
            dataset = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fullPath + "; Persist Security Info=False");

            emailToUser = new List<string>();
        }
        /// <summary>
        /// the function adds a user record to users table
        /// </summary>
        /// <param name="userInsert">the user to add.</param>
        public bool add_user_record(User userInsert)
        {
            try
            {
                dataset.Open();
                string validMailQuery = "SELECT * FROM users WHERE [email] = '" + userInsert.email + "'";
                OleDbCommand checker = new OleDbCommand(validMailQuery, dataset);
                OleDbDataReader reader = checker.ExecuteReader();
                if (reader.Read())
                {
                    dataset.Close();
                    return false;
                }
                string query = "INSERT INTO users ([email], [password], [first name], [last name], [date of birth], [religion], [gender], [phone], [marital status], [city])" + " VALUES (@email,@password,@firstName,@lastName,@dateOfBirth,@religion,@gender,@phone,@maritalStatus, @city)";
                OleDbCommand command = new OleDbCommand(query, dataset);
                command.Parameters.AddWithValue("@email", userInsert.email);
                command.Parameters.AddWithValue("@password", userInsert.password);
                command.Parameters.AddWithValue("@first name", userInsert.firstName);
                command.Parameters.AddWithValue("@last name", userInsert.lastName);
                command.Parameters.AddWithValue("@date of birth", userInsert.dateOfBirth.ToString());
                command.Parameters.AddWithValue("@religion", userInsert.religion);
                command.Parameters.AddWithValue("@gender", userInsert.gender);
                command.Parameters.AddWithValue("@phone", userInsert.phone);
                command.Parameters.AddWithValue("@marital status", userInsert.maritalStatus);
                command.Parameters.AddWithValue("@city", userInsert.city);
                command.ExecuteNonQuery();
                dataset.Close();
                // add areas of user to users-areas
                dataset.Open();
                string email = userInsert.email;
                foreach (string area in userInsert.areas)
                {
                    string queryToAdd = "INSERT INTO usersAreas ([area], [userEmail])" + " VALUES (@area,@email)";
                    OleDbCommand postidAreasCmd = new OleDbCommand(queryToAdd, dataset);
                    postidAreasCmd.Parameters.AddWithValue("@area", area);
                    postidAreasCmd.Parameters.AddWithValue("@email", email);
                    postidAreasCmd.ExecuteNonQuery();
                }
                dataset.Close();
                //send email
                SendRegistrationCompletedMail(userInsert);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// the function sends registration mail
        /// </summary>
        /// <param name="user">the user to send mail to.</param>
        private void SendRegistrationCompletedMail(User user)
        {
            var fromAddress = new MailAddress("partnermatcher@gmail.com", "PartnerMatcher");
            var toAddress = new MailAddress(user.email, user.firstName + " " + user.lastName);
            const string fromPassword = "Sg504504";
            const string subject = "Registration Approval For Parter Matcher";
            string body = "Hello " + user.firstName + ",\n\nThank you for joining Partner Matcher family!";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
        /// <summary>
        /// the function adds post record to the posts table
        /// </summary>
        public void add_post_record(Post postInsert)
        {
            dataset.Open();
            string query = "INSERT INTO posts ([title], [content], [user posted], [city], [isPublished])" + " VALUES (@title,@content,@userPosted,@city,@isPublished)";
            OleDbCommand command = new OleDbCommand(query, dataset);
            //command.Parameters.AddWithValue("@post id", postInsert.id);
            command.Parameters.AddWithValue("@title", postInsert.title);
            command.Parameters.AddWithValue("@content", postInsert.content);
            command.Parameters.AddWithValue("@user posted", postInsert.publisherEmail);
            command.Parameters.AddWithValue("@city", postInsert.city);
            command.Parameters.AddWithValue("@isPublished", postInsert.isPublished);
            command.ExecuteNonQuery();
            dataset.Close();
            int postID = getLastAddedPostRecordID();
            dataset.Open();
            foreach (string area in postInsert.postAreas)
            {
                string queryToAdd = "INSERT INTO PostsAreas ([area], [postid])" + " VALUES (@area,@postid)";
                OleDbCommand postidAreasCmd = new OleDbCommand(queryToAdd, dataset);
                postidAreasCmd.Parameters.AddWithValue("@area", area);
                postidAreasCmd.Parameters.AddWithValue("@postid", postID);
                postidAreasCmd.ExecuteNonQuery();
            }
            dataset.Close();
        }
        /// <summary>
        /// this function connects a user
        /// </summary>
        /// <param name="mail">the user mail</param>
        /// <param name="pw">the user password</param>
        /// <returns>if succeded to connect or not.</returns>
        public bool userConnect(string mail, string pw)
        {
            dataset.Open();
            string validMailQuery = "SELECT * FROM users WHERE [email] = '" + mail
                + "' AND [password] = '" + pw + "'";
            OleDbCommand checker = new OleDbCommand(validMailQuery, dataset);
            OleDbDataReader reader = checker.ExecuteReader();
            if (reader.Read())
            {
                dataset.Close();
                return true;
            }
            dataset.Close();
            return false;
        }
        /// <summary>
        /// this functions returns the last added post id.
        /// </summary>
        /// <returns></returns>
        public int getLastAddedPostRecordID()
        {
            int location = 0;
            dataset.Open();
            string queryForMax = "SELECT MAX([post id]) FROM posts";
            OleDbCommand checker = new OleDbCommand(queryForMax, dataset);
            int maxId = Convert.ToInt32(checker.ExecuteScalar());
            location = maxId;
            dataset.Close();
            return location;
        }
        /// <summary>
        /// this function adds a new area to db.
        /// </summary>
        /// <param name="area">the area name.</param>
        public void addNewArea(string area)
        {
            dataset.Open();
            string queryToAdd = "INSERT INTO areas ([area name])" + " VALUES (@area)";
            OleDbCommand postidAreasCmd = new OleDbCommand(queryToAdd, dataset);
            postidAreasCmd.Parameters.AddWithValue("@area", area);
            postidAreasCmd.ExecuteNonQuery();
            dataset.Close();
        }
        /// <summary>
        /// func that returns list of strings of all areas
        /// </summary>
        /// <returns>a list of string with all the areas</returns>
        public List<string> getListOfAreas()
        {
            List<string> res = new List<string>();
            dataset.Open();
            string query = "SELECT * FROM areas";
            OleDbCommand checker = new OleDbCommand(query, dataset);
            OleDbDataReader reader = checker.ExecuteReader();
            while (reader.Read())
            {
                res.Add(reader.GetString(0));
            }
            dataset.Close();
            return res;
        }
        /// <summary>
        /// this function returns all the posts of a given user.
        /// </summary>
        /// <param name="email">the user email</param>
        /// <returns>the user posts.</returns>
        public Dictionary<int, Post> getUserPosts(string email)
        {
            Dictionary<int, Post> res = new Dictionary<int, Post>();// postid, post
            dataset.Open();
            string query = "SELECT * FROM posts WHERE [user posted] = '" + email + "'";
            OleDbCommand checker = new OleDbCommand(query, dataset);
            OleDbDataReader reader = checker.ExecuteReader();
            while (reader.Read())
            {
                Post postToAdd = new Post(reader.GetString(0), reader.GetString(1),
                    new List<string>(), reader.GetString(2), reader.GetString(3),
                    reader.GetBoolean(4));

                int postid = reader.GetInt32(5);
                postToAdd.id = postid.ToString();
                res.Add(postid, postToAdd);
            }
            dataset.Close();
            return res;
        }
        /// <summary>
        /// this function publishes/unpublishes a post that is in the db.
        /// </summary>
        /// <param name="postID">the post id</param>
        /// <param name="toPublish">to publish or to unpublish</param>
        public void PublishPost(int postID, bool toPublish)
        {
            dataset.Open();
            string postToUpdateQuery = "UPDATE posts SET [isPublished] = " + toPublish.ToString()
                + " WHERE [post id] = " + postID;
            OleDbCommand toPost = new OleDbCommand(postToUpdateQuery, dataset);
            toPost.ExecuteNonQuery();
            dataset.Close();
        }
        /// <summary>
        /// this function searches for posts in the db by city and area
        /// </summary>
        /// <param name="city">the city</param>
        /// <param name="area">the area</param>
        /// <returns>all the posts matching the criterias</returns>
        public Dictionary<int, Post> searchForPosts(string city, string area)
        {
            Dictionary<int, Post> res = new Dictionary<int, Post>();
            dataset.Open();
            string query = "SELECT PostsAreas.postid FROM posts, PostsAreas" +
             " WHERE posts.[post id] = PostsAreas.[postid] AND posts.[city] = '"
             + city + "' AND PostsAreas.[area] = '" + area + "' " +
             "AND posts.[isPublished] = " + true;
            OleDbCommand checker = new OleDbCommand(query, dataset);
            OleDbDataReader reader = checker.ExecuteReader();
            List<int> relevantPosts = new List<int>();
            while (reader.Read())
            {
                int postid = reader.GetInt32(0);
                relevantPosts.Add(postid);
            }
            dataset.Close();
            dataset.Open();
            foreach (int i in relevantPosts)
            {
                string newQuery = "SELECT * FROM posts WHERE [post id] = " + i;
                OleDbCommand checker1 = new OleDbCommand(newQuery, dataset);
                OleDbDataReader reader1 = checker1.ExecuteReader();
                while (reader1.Read())
                {
                    Post postToAdd = new Post(reader1.GetString(0), reader1.GetString(1),
                    new List<string>(), reader1.GetString(2), reader1.GetString(3),
                    reader1.GetBoolean(4));
                    res.Add(i, postToAdd);
                }
            }
            Dictionary<int, List<string>> postsidToAreas = new Dictionary<int, List<string>>();
            foreach (int i in relevantPosts)
            {
                List<string> areas = new List<string>();
                string newQuery = "SELECT area FROM PostsAreas WHERE [postid] = " + i;
                OleDbCommand checker1 = new OleDbCommand(newQuery, dataset);
                OleDbDataReader reader1 = checker1.ExecuteReader();
                while (reader1.Read())
                {
                    areas.Add(reader1.GetString(0));

                }
                postsidToAreas.Add(i, areas);
            }
            foreach (int i in res.Keys)
            {
                res[i].postAreas = postsidToAreas[i];
            }
            dataset.Close();
            return res;
        }
        /// <summary>
        /// this function returns the user fname and lname 
        /// </summary>
        /// <param name="email">the user email</param>
        /// <returns>the user fname and lname</returns>
        public List<string> getUserFirstnameLastnameByEmail(string email)
        {
            List<string> res = new List<string>();
            dataset.Open();
            string query = "SELECT * FROM users WHERE [email] = '" + email + "'";
            OleDbCommand checker = new OleDbCommand(query, dataset);
            OleDbDataReader reader = checker.ExecuteReader();
            while (reader.Read())
            {
                res.Add(reader.GetString(1));
                res.Add(reader.GetString(2));
            }
            dataset.Close();
            return res;
        }
        /// <summary>
        /// this function returns all published posts.
        /// </summary>
        /// <returns>all the posts that are published.</returns>
        public Dictionary<int, Post> getAllPublishedPosts()
        {
            Dictionary<int, Post> res = new Dictionary<int, Post>();
            dataset.Open();
            string query = "SELECT * FROM posts WHERE [isPublished] = " + true.ToString();
            OleDbCommand checker = new OleDbCommand(query, dataset);
            OleDbDataReader reader = checker.ExecuteReader();
            while (reader.Read())
            {
                Post postToAdd = new Post(reader.GetString(0), reader.GetString(1),
                    new List<string>(), reader.GetString(2), reader.GetString(3),
                    reader.GetBoolean(4));

                int postid = reader.GetInt32(5);
                postToAdd.id = postid.ToString();
                res.Add(postid, postToAdd);
            }
            dataset.Close();
            dataset.Open();
            foreach (int i in res.Keys)
            {
                string queryForAreas = "SELECT area FROM PostsAreas WHERE postid = " + i;
                OleDbCommand checker1 = new OleDbCommand(queryForAreas, dataset);
                OleDbDataReader reader1 = checker1.ExecuteReader();
                List<string> area = new List<string>();
                while (reader1.Read())
                {
                    area.Add(reader1.GetString(0));
                }
                res[i].postAreas = area;
            }
            dataset.Close();
            return res;
        }
        /// <summary>
        /// Adds a request for a post to the relation 'requests'
        /// </summary>
        /// <param name="request">The request to add</param>
        public void SendRequest(Request request)
        {
            string query = "INSERT INTO requests ([sender], [post id], [status], [rank])" + " VALUES (@sender,@postId,@status,@rank)";
            dataset.Open();
            OleDbCommand command = new OleDbCommand(query, dataset);
            command.Parameters.AddWithValue("@sender", request.senderID);
            command.Parameters.AddWithValue("@postId", request.postID);
            command.Parameters.AddWithValue("@status", request.status);
            command.Parameters.AddWithValue("@rank", request.rank);
            command.ExecuteNonQuery();
            dataset.Close();
        }
        /// <summary>
        /// Returns the requests for posts of a user
        /// </summary>
        /// <param name="userID">The user</param>
        /// <returns>Request list of the user</returns>
        public List<Request> GetRequests(string userID)
        {
            //find the publisher
            string query = "SELECT [post id] FROM posts WHERE [user posted] = '" + userID + "'";
            List<int> usersPostsIDsList = new List<int>();
            List<Request> res = new List<Request>();
            dataset.Open();
            OleDbCommand command = new OleDbCommand(query, dataset);
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                usersPostsIDsList.Add(reader.GetInt32(0));
            }
            dataset.Close();
            //find publisher's requests
            dataset.Open();
            foreach (int userPostID in usersPostsIDsList)
            {
                query = "SELECT * FROM requests WHERE [post id] = " + userPostID;
                command = new OleDbCommand(query, dataset);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    res.Add(new Request(reader.GetInt32(1), reader.GetString(0), reader.GetString(2), reader.GetInt32(3)));
                }
            }
            dataset.Close();
            return res;
        }
        /// <summary>
        /// Updates request's status
        /// </summary>
        /// <param name="senderID">sender of request</param>
        /// <param name="postID">post id</param>
        /// <param name="rank">rank for the request</param>
        /// <param name="status">the status to update</param>
        public void updateRequestStatusRank(string senderID, int postID, int rank, string status)
        {
            dataset.Open();
            string requestToUpdate = "UPDATE requests SET [rank] = " + rank + ", [status] = '" + status + "'"
                + " WHERE [post id] = " + postID + " AND [sender] = '" + senderID + "'";
            OleDbCommand toPost = new OleDbCommand(requestToUpdate, dataset);
            toPost.ExecuteNonQuery();
            dataset.Close();
        }
    }
}