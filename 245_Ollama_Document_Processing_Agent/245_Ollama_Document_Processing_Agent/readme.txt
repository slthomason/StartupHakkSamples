# Document Analysis Agent

## Overview

This is a console application that demonstrates how to use local LLMs via Ollama to extract structured information from text documents. The application shows how to use AI to process document content, transforming it into structured JSON formats that can be used in business applications.

## Features

- Process text documents using Ollama's local LLMs
- Generate structured JSON summaries from document content
- Compare multiple documents for similarities and differences
- Save analysis results as JSON files

## Prerequisites

- .NET 8.0 SDK or later
- Ollama installed and running locally (https://ollama.ai)
- The phi3:mini model pulled in Ollama (or change the model name in Program.cs)

## Getting Started

1. Clone this repository
2. Run `ollama pull phi3:mini` to download the required model
3. Run the application using `dotnet run`
4. Follow the on-screen prompts to analyze documents

## How It Works

1. The application connects to the local Ollama API
2. It reads text documents and sends their content to the LLM
3. It uses JSON Schema to instruct the LLM to return structured data
4. It processes and displays the structured response

## Sample Documents

The repository includes sample text files:
- TechnicalDoc.txt - A technical specification document
- ResearchPaper.txt - A research paper on climate change adaptation
- FinancialReport.txt - A financial annual report

## Use Cases

- Document summarization for faster information extraction
- Comparative analysis of multiple documents
- Converting unstructured document data into structured formats for database storage
- Automated report generation from document analysis

## Configuration

The application uses the local Ollama endpoint at http://localhost:11434/api/chat. If your Ollama instance is running on a different address, update the OllamaEndpoint constant in Program.cs.

## Model Configuration

The application is configured to use the "phi3:mini" model, but you can change this to any model available in your Ollama installation by modifying the ModelName constant in Program.cs.