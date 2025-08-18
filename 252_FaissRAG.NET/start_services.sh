#!/bin/bash

echo "===================================="
echo "Simple RAG .NET with FAISS Integration (Persistent)"
echo "===================================="
echo

echo "Installing/updating Python dependencies..."
pip install -r requirements.txt -q
echo

echo "Starting FAISS Vector Database Service with Persistence..."
echo "Service will be available at: http://localhost:8001"
echo "Data will be saved to: faiss_index.bin and documents.json"
echo

# Start FAISS service in background
python start_faiss_service.py &
FAISS_PID=$!

echo "Waiting for FAISS service to start up..."
sleep 5

echo
echo "Testing FAISS service connection..."
python test_faiss_service.py

echo
echo "Starting .NET RAG Application with Persistence..."
echo "Data will be cached locally in: vector_cache.json"
echo

dotnet run

echo
echo "Application finished. FAISS data has been automatically saved."
echo "Stopping FAISS service..."
kill $FAISS_PID 2>/dev/null

echo "Done!"
