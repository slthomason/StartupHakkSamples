# Simple RAG .NET

A simple Retrieval-Augmented Generation (RAG) implementation in .NET that uses Ollama for embeddings and completions.

## Overview

This project demonstrates a basic RAG system that:

1. Stores documents in a simple in-memory vector store
2. Converts text to embeddings using Ollama
3. Performs similarity search to find relevant documents
4. Generates answers based on retrieved context

## Prerequisites

- .NET 8.0 SDK
- Access to an Ollama server (local or remote)

## Configuration

Before running, update the Ollama URL and model in `Program.cs`:

```csharp
string ollamaUrl = "http://localhost:11434"; // Change to your Ollama server URL
string model = "phi4"; // Change to your preferred model
```

## How It Works

1. The application starts by adding sample documents about animals to the vector store
2. Each document is converted to an embedding vector using Ollama
3. When you ask a question, it:
   - Converts your question to an embedding
   - Finds similar documents in the vector store
   - Creates a prompt with the relevant context
   - Sends the prompt to Ollama for completion
   - Returns the answer

## Running the Application

```
dotnet run
```

## Example Usage

```
=== Simple RAG Example ===

Adding documents...
Ready! Ask questions about animals.

Question: What are cats known for?
Answer: Cats are known for their independence and hunting skills.

Question: quit
Goodbye!
```

## Project Structure

- `Program.cs` - Main application entry point
- `Services/OllamaService.cs` - Handles API calls to Ollama
- `Services/RagService.cs` - Implements the RAG pattern
- `Services/VectorStore.cs` - Simple vector store implementation

## License

MIT 