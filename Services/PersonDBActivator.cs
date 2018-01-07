using System;

namespace MVVMApplication.Services
{
    public class PersonDBActivator 
    {
        public PersonDB DbContext { get; }

        public PersonDBActivator()
        {
            DbContext = new PersonDB();
        }

        public PersonDBActivator(string dataDirPath)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", dataDirPath);
            DbContext = new PersonDB();
        }

        public PersonDBActivator(Environment.SpecialFolder specialFolder)
        {
            string dataDirPath = Environment.GetFolderPath(specialFolder);
            AppDomain.CurrentDomain.SetData("DataDirectory", dataDirPath);
            DbContext = new PersonDB();
        }
    }
}
