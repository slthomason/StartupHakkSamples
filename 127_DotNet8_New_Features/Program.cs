//System.Text.Json Improvements (or Fixes)

// Code snippet to demonstrate the annotation format
[JsonConverter(typeof(JsonStringEnumConverter<MyEnum>))]
public enum MyEnum { Value1, Value2, Value3 }
[JsonSerializable(typeof(MyEnum))]
public partial class MyContext : JsonSerializerContext { }


// Code snippet showcasing the use of JsonConverter.Type
Dictionary<Type, JsonConverter> CreateDictionary(IEnumerable<JsonConverter> converters)
=> converters.Where(converter => converter.Type != null).ToDictionary(converter => converter.Type!);


//ZipFile.CreateFromDirectory usages


// Get a stream to use as a destination
Stream destinationStream = GetStreamFromSomewhere()

// Use the new overload to zip your directory of files and store it in your destination stream
ZipFile.CreateFromDirectory(
    sourceDirectoryName: "/home/username/sourcedirectory/",
    destination: destinationStream,
    compressionLevel: CompressionLevel.Optimal,
    includeBaseDirectory: true,
    entryNameEncoding: Encoding.UTF8);


//ZipFile.ExtractToDirectory usage

// Get a stream with your source zipped file from somewhere
Stream sourceStream = GetStreamFromSomewhere();

// Here's how you use the method to extract the contents of your zipped stream into a directory in the filesystem
ZipFile.ExtractToDirectory(
    source: sourceStream,
    destinationDirectoryName: "/home/username/destinationdirectory/",
    entryNameEncoding: Encoding.UTF8,
    overwriteFiles: true);

//MetricCollector Metrics API

// Define the name for your counter
const string CounterName = "MyCounter";

// Set up your time provider
var now = DateTimeOffset.Now;
var timeProvider = new FakeTimeProvider(now);
// Initialise your meter and counter
using var meter = new Meter(Guid.NewGuid().ToString());
var counter = meter.CreateCounter<long>(CounterName);
// Set up your metric collector
using var collector = new MetricCollector<long>(counter, timeProvider);
// Check that nothing has been recorded initially
Assert.Empty(collector.GetMeasurementSnapshot());
Assert.Null(collector.LastMeasurement);
// Add a measurement value to your counter
counter. Add(3);
// Verify that the measurement update was recorded
Assert.Equal(counter, collector.Instrument);
Assert.NotNull(collector.LastMeasurement);
Assert.Single(collector.GetMeasurementSnapshot());
Assert.Same(collector.GetMeasurementSnapshot().Last(), collector.LastMeasurement);
Assert.Equal(3, collector.LastMeasurement.Value);
Assert.Empty(collector.LastMeasurement.Tags);
Assert.Equal(now, collector.LastMeasurement.Timestamp);


//Understanding Options Validation Usage

public class FirstModelNoNamespace
{
    [Required] // Property P1 is required and should have minimum length of 5
    [MinLength(5)]
    public string P1 { get; set; } = string. Empty;

// P2 and P3 properties are validated using custom validators
    [Microsoft.Extensions.Options.ValidateObjectMembers(typeof(SecondValidatorNoNamespace))]
    public SecondModelNoNamespace? P2 { get; set; }
    [Microsoft.Extensions.Options.ValidateObjectMembers]
    public ThirdModelNoNamespace? P3 { get; set; }
}

[OptionsValidator] // Indicates that this class is to be used for options validation
public partial class FirstValidatorNoNamespace : IValidateOptions<FirstModelNoNamespace>
{
}
[OptionsValidator] // Indicates that this class is to be used for options validation
public partial class SecondValidatorNoNamespace : IValidateOptions<SecondModelNoNamespace>
{
}

var builder = WebApplication.CreateBuilder(args);
// Configuring services
builder.Services.AddControllersWithViews();
builder.Services.Configure<FirstModelNoNamespace>(builder.Configuration.GetSection(...));

// Adding singleton services for options validation
builder.Services.AddSingleton<IValidateOptions<FirstModelNoNamespace>, FirstValidatorNoNamespace>();
builder.Services.AddSingleton<IValidateOptions<SecondModelNoNamespace>, SecondValidatorNoNamespace>();


//LoggerMessageAttribute’s Extended Constructor Overloads


// Specifying only the LogLevel and message
    public LoggerMessageAttribute(LogLevel level, string message);

// Specifying only the LogLevel
    public LoggerMessageAttribute(LogLevel level);
    // Specifying only the message
    public LoggerMessageAttribute(string message);


//LogLevel and Message parameters are declared
    [LoggerMessage(Level = LogLevel.Warning, Message = "{p1} should be valid")]
    public partial void LogWarning(string p1);


//Source Generated COM Interop

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

[GeneratedComInterface] // We've marked the interface with the given attribute, which indicates that this is a COM interfate.
[Guid("5401c312-ab23-4dd3-aa40-3cb4b3a4683e")]
interface IComInterface
{
    void DoWork();
}
internal class MyNativeLib
{
    [LibraryImport(nameof(MyNativeLib))]
    public static partial void GetComInterface(out IComInterface comInterface); // The source 
    //generator part to enable the interoperability
}

MyNativeLib.GetComInterface(out IComInterface comInterface); // Getting the COM interface
comInterface.RegisterCallbacks(new MyCallbacks()); // Registering the callbacks, which is exposed to unmanaged code
comInterface.DoWork(); 

//... More code ...
[GeneratedComClass]
internal class MyCallbacks : ICallbacks // COM class attribute being used for the class implementing the interface
{
    public void Callback() // A callback implementation
    {
        Console.WriteLine("Callback called");
    }
}

//SHA-3 Support

// Hashing example
// Check if SHA-3-256 is supported on the current platform.
if (SHA3_256.IsSupported)
{
    byte[] hash = SHA3_256.HashData(dataToHash); // Hashing made easier.
}
else
{
    // Determine what to do if SHA-3 is not supported.
    // Backup plan?
}

// Signing Example
// Check if SHA-3-256 is supported on the current platform.
if (SHA3_256.IsSupported)
{
     using ECDsa ec = ECDsa.Create(ECCurve.NamedCuves.nistP256); // Creating an elliptic curve.
     byte[] signature = ec.SignData(dataToBeSigned, HashAlgorithmName.SHA3_256); // Signing data with SHA-3-256.
}
else
{
    // Determine what to do if SHA-3 is not supported.
    // Maybe an alternative hash algorithm?
}


if (Shake128.IsSupported)
{
    using Shake128 shake = new Shake128(); // Create a new instance.
    shake.AppendData("Hello .NET!"u8); // Data to shake.
    byte[] digest = shake.GetHashAndReset(outputLength: 32); // Shake it up!

// You can also use it like this:
    digest = Shake128.HashData("Hello .NET!"u8, outputLength: 32);
}
else
{
    // Determine what to do if SHAKE is not supported.
    // Time to shake up the plan. 
}