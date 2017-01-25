using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace partnersMatcherPart4.Model
{
    [Serializable]
    public class User
    {
        //User
        public string firstName { get; set; }
        public string lastName { get; set; }
        public DateTime dateOfBirth;
        public string gender;
        public string email { get; set; }
        public string password;
        public string religion;
        public string city { get; set; }
        public string maritalStatus;
        public List<string> areas { get; set; }
        public string phone { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="fName"></param>
        /// <param name="lName"></param>
        /// <param name="dob"></param>
        /// <param name="gen"></param>
        /// <param name="mail"></param>
        /// <param name="pass"></param>
        /// <param name="relig"></param>
        /// <param name="cty"></param>
        /// <param name="marital"></param>
        /// <param name="area"></param>
        public User(string fName, string lName, DateTime dob, string gen, string mail,
            string pass, string relig, string cty, string marital, List<string> area, string phone)
        {
            firstName = fName;
            lastName = lName;
            dateOfBirth = dob;
            gender = gen;
            email = mail;
            password = pass;
            religion = relig;
            city = cty;
            maritalStatus = marital;
            areas = area;
            this.phone = phone;
        }

        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="toCopy"></param>
        public User(User toCopy)
        {
            firstName = toCopy.firstName;
            lastName = toCopy.lastName;
            dateOfBirth = toCopy.dateOfBirth;
            gender = toCopy.gender;
            email = toCopy.email;
            password = toCopy.password;
            religion = toCopy.religion;
            city = toCopy.city;
            maritalStatus = toCopy.maritalStatus;
            areas = new List<string>(toCopy.areas);
            this.phone = toCopy.phone;
        }



    }
}