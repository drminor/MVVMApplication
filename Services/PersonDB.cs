using MVVMApplication.Model;
using System.Data.Entity;

namespace MVVMApplication.Services
{
    public class PersonDB:DbContext
    {
        public PersonDB():base("name=DefaultConnection")
        {

        }

        DbSet<Person> _person;
        public DbSet<Person> Person
        {
            get
            {
                return _person;
            }
            set
            {
                _person = value;
            }
        }
    }
}
