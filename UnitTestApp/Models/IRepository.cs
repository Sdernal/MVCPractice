using System.Collections.Generic;

namespace UnitTestApp.Models
{
    public interface IRepository
    {
        IEnumerable<Phone> GetAll();
        Phone Get(int id);
        void Create(Phone p);
    }
}
