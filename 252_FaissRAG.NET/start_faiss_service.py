#!/usr/bin/env python3
"""
FAISS Vector Database Service with Persistence
Stores FAISS index to disk for persistence across restarts.
"""

from fastapi import FastAPI
from pydantic import BaseModel
import faiss
import numpy as np
import uvicorn
import os
import json
from typing import List, Optional

app = FastAPI(title="FAISS Vector Database Service (Persistent)", version="1.1.0")

# Global variables
index = None
documents = []  # Store document texts for retrieval
index_file = "faiss_index.bin"
documents_file = "documents.json"

class IndexRequest(BaseModel):
    vectors: list[list[float]]
    documents: Optional[list[str]] = None  # Optional document texts

class QueryRequest(BaseModel):
    query: list[float]
    k: int = 5

def save_index_to_disk():
    """Save FAISS index and documents to disk."""
    global index, documents
    
    if index is not None:
        # Save FAISS index
        faiss.write_index(index, index_file)
        print(f"FAISS index saved to {index_file}")
        
        # Save documents
        with open(documents_file, 'w', encoding='utf-8') as f:
            json.dump(documents, f, ensure_ascii=False, indent=2)
        print(f"Documents saved to {documents_file}")

def load_index_from_disk():
    """Load FAISS index and documents from disk."""
    global index, documents
    
    try:
        if os.path.exists(index_file):
            # Load FAISS index
            index = faiss.read_index(index_file)
            print(f"FAISS index loaded from {index_file} ({index.ntotal} vectors)")
            
            # Load documents
            if os.path.exists(documents_file):
                with open(documents_file, 'r', encoding='utf-8') as f:
                    documents = json.load(f)
                print(f"Documents loaded from {documents_file} ({len(documents)} documents)")
            
            return True
    except Exception as e:
        print(f"Error loading from disk: {e}")
        index = None
        documents = []
        return False
    
    return False

@app.on_event("startup")
async def startup_event():
    """Load existing index on service startup."""
    print("Starting FAISS service with persistence...")
    load_index_from_disk()

@app.on_event("shutdown")
async def shutdown_event():
    """Save index before service shutdown."""
    print("Saving index before shutdown...")
    save_index_to_disk()

@app.post("/index")
def create_index(req: IndexRequest):
    """Create or recreate the FAISS index with the provided vectors."""
    global index, documents
    
    if not req.vectors:
        return {"error": "No vectors provided"}
    
    # Auto-detect dimension from first vector
    dim = len(req.vectors[0])
    print(f"Creating FAISS index with dimension: {dim}")
    
    # Create a new L2 distance index
    index = faiss.IndexFlatL2(dim)
    
    # Convert vectors to numpy array and add to index
    vectors_np = np.array(req.vectors).astype("float32")
    index.add(vectors_np)
    
    # Store documents if provided
    if req.documents:
        documents = req.documents
    else:
        # If no documents provided, create placeholder documents
        documents = [f"Document {i}" for i in range(len(req.vectors))]
    
    # Save to disk immediately
    save_index_to_disk()
    
    print(f"Added {len(req.vectors)} vectors to FAISS index")
    return {
        "message": "Index created successfully", 
        "total_vectors": index.ntotal, 
        "dimension": dim,
        "persisted": True
    }

@app.post("/query")
def query_index(req: QueryRequest):
    """Search the FAISS index for similar vectors."""
    if index is None:
        return {"error": "No index created yet"}
    
    # Convert query to numpy array
    query_np = np.array([req.query]).astype("float32")
    
    # Search the index
    distances, indices = index.search(query_np, req.k)
    
    # Get corresponding document texts
    result_documents = []
    for idx in indices[0]:
        if 0 <= idx < len(documents):
            result_documents.append(documents[idx])
        else:
            result_documents.append(f"Document {idx}")
    
    print(f"Query returned {len(indices[0])} results")
    return {
        "indices": indices[0].tolist(),
        "distances": distances[0].tolist(),
        "documents": result_documents
    }

@app.get("/health")
def health_check():
    """Health check endpoint for monitoring."""
    return {
        "status": "healthy",
        "index_created": index is not None,
        "total_vectors": index.ntotal if index else 0,
        "persistent_files": {
            "index_exists": os.path.exists(index_file),
            "documents_exists": os.path.exists(documents_file)
        }
    }

@app.post("/save")
def manual_save():
    """Manually trigger save to disk."""
    save_index_to_disk()
    return {"message": "Index and documents saved to disk"}

@app.post("/load")
def manual_load():
    """Manually trigger load from disk."""
    success = load_index_from_disk()
    return {
        "message": "Load completed", 
        "success": success,
        "total_vectors": index.ntotal if index else 0
    }

@app.delete("/clear")
def clear_data():
    """Clear all data and delete persistent files."""
    global index, documents
    
    index = None
    documents = []
    
    # Delete files
    for file in [index_file, documents_file]:
        if os.path.exists(file):
            os.remove(file)
            print(f"Deleted {file}")
    
    return {"message": "All data cleared"}

@app.get("/")
def root():
    """Root endpoint with service information."""
    return {
        "service": "FAISS Vector Database (Persistent)",
        "status": "running",
        "persistence": {
            "index_file": index_file,
            "documents_file": documents_file,
            "auto_save": True,
            "auto_load": True
        },
        "endpoints": {
            "POST /index": "Create/update vector index",
            "POST /query": "Search similar vectors",
            "GET /health": "Health check",
            "POST /save": "Manual save to disk",
            "POST /load": "Manual load from disk",
            "DELETE /clear": "Clear all data",
            "GET /": "Service info"
        }
    }

@app.get("/stats")
def get_stats():
    """Get index statistics."""
    if index is None:
        return {"error": "No index created"}
    
    return {
        "total_vectors": index.ntotal,
        "total_documents": len(documents),
        "dimension": index.d,
        "index_type": "IndexFlatL2",
        "persistent_files": {
            "index_file": index_file,
            "documents_file": documents_file,
            "index_size_mb": os.path.getsize(index_file) / (1024*1024) if os.path.exists(index_file) else 0,
            "documents_size_mb": os.path.getsize(documents_file) / (1024*1024) if os.path.exists(documents_file) else 0
        }
    }

if __name__ == "__main__":
    print("Starting FAISS Vector Database Service (Persistent)...")
    print("Index will be saved to:", os.path.abspath(index_file))
    print("Documents will be saved to:", os.path.abspath(documents_file))
    print("Service will be available at: http://localhost:8001")
    print("API documentation at: http://localhost:8001/docs")
    print("Health check at: http://localhost:8001/health")
    
    uvicorn.run(
        app, 
        host="0.0.0.0", 
        port=8001,
        log_level="info"
    )
