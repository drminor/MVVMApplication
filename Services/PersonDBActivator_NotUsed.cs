using System;

namespace MVVMApplication.Services
{
    public class PersonDBActivator_NotUsed 
    {
        public PersonDB DbContext { get; }

        public PersonDBActivator_NotUsed()
        {
            DbContext = new PersonDB();
        }

        public PersonDBActivator_NotUsed(string dataDirPath)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", dataDirPath);
            DbContext = new PersonDB();
        }

        public PersonDBActivator_NotUsed(Environment.SpecialFolder specialFolder)
        {
            string dataDirPath = Environment.GetFolderPath(specialFolder);
            AppDomain.CurrentDomain.SetData("DataDirectory", dataDirPath);
            DbContext = new PersonDB();
        }
    }
}
