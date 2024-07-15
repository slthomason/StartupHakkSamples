using Xunit;

namespace NuGetDemo.Tests
{
    public class MathTests
    {
        [Fact]
        public void CanAdd()
        {
            var math = new Math();
            var result = math.Add(2, 2);
            Assert.Equal(4, result);
        }
    }
}
