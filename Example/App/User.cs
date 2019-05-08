using System.Collections.Generic;

namespace App
{
    public class User
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public long Id { get; set; }
    }

    public class SearchUsers
    {
        public IEnumerable<User> Results { get; set; }
    }
}
