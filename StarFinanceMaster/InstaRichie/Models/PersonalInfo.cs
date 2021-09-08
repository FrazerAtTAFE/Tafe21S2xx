using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace StartFinance.Models
{
    class PersonalInfo
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [NotNull]
        public string FirstName { get; set; }

        [NotNull]
        public string LastName { get; set; }

        [NotNull]
        public string DateOfBirth { get; set; }

        [NotNull]
        public String Gender { get; set; }

        [NotNull]
        public String Email { get; set; }

        [NotNull]
        public String Phone { get; set; }
    }
}
