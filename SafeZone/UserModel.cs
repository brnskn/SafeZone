using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeZone
{
    class UserModel
    {
        public long id;
        public string username;
        public string password;

        public UserModel(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public UserModel(int id, string username, string password)
        {
            this.id = id;
            this.username = username;
            this.password = password;
        }
    }
}
