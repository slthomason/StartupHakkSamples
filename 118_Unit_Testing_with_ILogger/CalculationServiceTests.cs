using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ClassLibrary.UnitTests
{

    /// <summary>
    /// This TestClass demonstrates Unit Testing with <see cref="ILogger&lt;T&gt;"/>
    /// </summary>
    [TestClass]
    public class CalculationServiceTests
    {

        [TestMethod]
        public void TestWithNullLogger()
        {
            var svc = new CalculationService(new NullLogger<CalculationService>());
            var result = svc.AddTwoPositiveNumbers(1, 2);
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void TestWithConsoleLogger()
        {
            using var loggerFactory = LoggerFactory.Create(c => c.AddConsole());
            var logger = loggerFactory.CreateLogger<CalculationService>();
            var svc = new CalculationService(logger);
            var result = svc.AddTwoPositiveNumbers(1, 2);
            Assert.AreEqual(3, result);

            // If you are using .NET Core 2.1.
            //using (var loggerFactory = new LoggerFactory().AddConsole())   // Need to use "using" in order to flush Console output
            //{
            //    var logger = loggerFactory.CreateLogger<CalculationService>();
            //    var svc = new CalculationService(logger);
            //    var result = svc.AddTwoPositiveNumbers(1, 2);
            //    Assert.AreEqual(3, result);
            //}
        }

        [TestMethod]
        public void TestWithDependencyInjectionLogger()
        {
            var services = new ServiceCollection()
                .AddLogging(config => config.AddConsole())      // can add any other logger(s)
                .BuildServiceProvider();
            using var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            var svc = new CalculationService(loggerFactory.CreateLogger<CalculationService>());
            var result = svc.AddTwoPositiveNumbers(1, 2);
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void TestWithMockLogger()
        {
            var loggerMock = new Mock<ILogger<CalculationService>>();
            var svc = new CalculationService(loggerMock.Object);

            var result = svc.AddTwoPositiveNumbers(1, 2);
            Assert.AreEqual(3, result);
            loggerMock.Verify(
                m => m.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v!.ToString()!.Contains("Adding 1 and 2")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

            result = svc.AddTwoPositiveNumbers(-1, 1);
            Assert.AreEqual(0, result);
            loggerMock.Verify(
                m => m.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v!.ToString()!.Contains("Arguments should be both positive.")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}
