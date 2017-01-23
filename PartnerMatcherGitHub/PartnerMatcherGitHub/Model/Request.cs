using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace partnersMatcherPart4.Model
{
    public class Request
    {
        public string senderID { get; set; }
        public int postID { get; set; }
        public string status { get; set; }
        public int rank { get; set; }
        // public int requestID { get; set; }

        public Request(int postID, string senderID, string status, int rank)
        {
            this.postID = postID;
            this.senderID = senderID;
            this.status = status;
            this.rank = rank;
        }
    }
}