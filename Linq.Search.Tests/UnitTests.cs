using System.Linq;
using Xunit;

namespace Linq.Search.Tests
{
    public class Tests
    {
        [Fact]
        public void TestNamn()
        {
            var results = TestData.Persons()
                                  .AsQueryable()
                                  .Search("namn@mail.com");

            Assert.True(results.Count() == 1);
        }
    }
}