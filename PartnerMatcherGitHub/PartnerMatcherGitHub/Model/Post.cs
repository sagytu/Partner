using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace partnersMatcherPart4.Model
{
    public class Post
    {
        //Post Class
        public string id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string publisherEmail { get; set; }
        public string city { get; set; }
        public bool isPublished { get; set; }

        public List<string> postAreas { get; set; }

        /// <summary>
        /// post constrcutor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="areas"></param>
        /// <param name="publisher"></param>
        /// <param name="city"></param>
        public Post(string title, string content, List<string> areas, string publisher, string city, bool isPublished)
        {
            //this.id = id;
            this.title = title;
            this.content = content;
            postAreas = new List<string>(areas);
            publisherEmail = publisher;
            this.city = city;
            this.isPublished = isPublished;
        }

        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="p"></param>
        public Post(Post p)
        {

        }
    }
}
