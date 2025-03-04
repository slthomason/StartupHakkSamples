Ollama Structured Chat Client
This project is a .NET console application that interacts with a locally running Ollama AI model.
It allows users to send structured prompts and receive formatted AI responses.

Setup Instructions
	Verify Ollama Installation

	Run: ollama
	If not installed, download it from Ollama’s official site.
	Start Ollama

	Run: ollama serve
	Pull the AI Model

	Run: ollama pull tinyllama:latest(can use any other model as well)
	Run the AI Model

	Run: ollama run tinyllama:latest
	Restore Dependencies

	Run: dotnet restore
	Run the Console Application

	Run: dotnet run
	Once running, the application will prompt you to select a demo (Country Information, Recipe, Product Catalog, Weather Forecast) and return structured responses based on the AI model’s output.