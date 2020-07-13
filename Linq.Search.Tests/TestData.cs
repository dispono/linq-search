using System.Collections.Generic;

namespace Linq.Search.Tests
{
    public class Person
    {
        [MakeSearchable]
        public string Name { get; set; }

        [MakeSearchable]
        public string Email { get; set; }

        [MakeSearchable]
        public Facility Facility { get; set; }
    }

    public class Facility
    {
        [MakeSearchable]
        public string Name { get; set; }
    }

    public static class TestData
    {
        public static List<Person> Persons()
        {
            var namn = new Person {Name = "Namn", Email = "namn@mail.com", Facility = new Facility {Name = "Anläggning1"}};
            var name = new Person {Name = "name", Email = "name@mail.com", Facility = new Facility {Name = "Anläggning2"}};
            return new List<Person> {name, namn};
        }
    }
}