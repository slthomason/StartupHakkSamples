using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DockerCSharp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Define Docker API client configuration
            var dockerConfig = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine"));

            // Create Docker API client
            var dockerClient = dockerConfig.CreateClient();

            // Define container configuration
            var containerConfig = new Config()
            {
                Image = "mcr.microsoft.com/dotnet/sdk:5.0",
                Cmd = new[] { "dotnet", "new", "console", "-o", "/app", "--no-https" },
                HostConfig = new HostConfig()
                {
                    AutoRemove = true,
                    Binds = new[] { $"{Directory.GetCurrentDirectory()}/app:/app" }
                }
            };

            // Create container
            var containerCreateResponse = await dockerClient.Containers.CreateContainerAsync(containerConfig);
            var containerId = containerCreateResponse.ID;

            // Start container
            await dockerClient.Containers.StartContainerAsync(containerId, new ContainerStartParameters());

            // Wait for container to finish executing
            var containerWaitResponse = await dockerClient.Containers.WaitContainerAsync(containerId);

            // Inspect container logs
            var containerLogsStream = await dockerClient.Containers.GetContainerLogsAsync(containerId, new ContainerLogsParameters()
            {
                ShowStdout = true,
                ShowStderr = true,
                Follow = false
            });
            using (var containerLogsReader = new StreamReader(containerLogsStream))
            {
                Console.WriteLine(containerLogsReader.ReadToEnd());
            }

            // Remove container
            await dockerClient.Containers.RemoveContainerAsync(containerId, new ContainerRemoveParameters());
        }
    }
}

using Docker.DotNet;
using Docker.DotNet.Models;

public async Task RunContainer()
{
    var endpoint = "npipe://./pipe/docker_engine"; // This is the default endpoint for Docker on Windows
    
    var dockerClient = new DockerClientConfiguration(new Uri(endpoint)).CreateClient();
    
    var containerConfig = new Config()
    {
        Image = "microsoft/dotnet",
        Cmd = new List<string>() { "run", "ConsoleApp.dll" } // Replace with the name of your application's entry point
    };
    
    var containerHostConfig = new HostConfig()
    {
        PortBindings = new Dictionary<string, IList<PortBinding>>()
        {
            { "5000/tcp", new List<PortBinding>(){ new PortBinding { HostPort = "5000" } } }
        }
    };
    
    var container = await dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters(containerConfig)
    {
        HostConfig = containerHostConfig,
        Name = "my-dotnet-app"
    });
    
    await dockerClient.Containers.StartContainerAsync(container.ID, new ContainerStartParameters());
}