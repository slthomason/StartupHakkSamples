#!/usr/bin/env python3
"""
Test script for the FAISS Vector Database Service
"""

import requests
import json
import numpy as np

FAISS_URL = "http://localhost:8001"

def test_faiss_service():
    """Test the FAISS service endpoints."""
    print("🧪 Testing FAISS Vector Database Service...")
    
    # Test health check
    print("\n1. Testing health check...")
    try:
        response = requests.get(f"{FAISS_URL}/health")
        print(f"   Status: {response.status_code}")
        print(f"   Response: {response.json()}")
    except Exception as e:
        print(f"   Error: {e}")
        return False
    
    # Test index creation
    print("\n2. Testing index creation...")
    try:
        # Create some test vectors (3 documents, 4 dimensions each)
        test_vectors = [
            [1.0, 0.5, 0.2, 0.8],
            [0.3, 1.0, 0.7, 0.1],
            [0.8, 0.2, 1.0, 0.6]
        ]
        
        response = requests.post(
            f"{FAISS_URL}/index",
            json={"vectors": test_vectors}
        )
        print(f"   Status: {response.status_code}")
        print(f"   Response: {response.json()}")
    except Exception as e:
        print(f"   Error: {e}")
        return False
    
    # Test querying
    print("\n3. Testing vector search...")
    try:
        # Query with a vector similar to the first one
        query_vector = [0.9, 0.4, 0.3, 0.7]
        
        response = requests.post(
            f"{FAISS_URL}/query",
            json={"query": query_vector, "k": 2}
        )
        print(f"   Status: {response.status_code}")
        result = response.json()
        print(f"   Response: {result}")
        
        if "indices" in result and "distances" in result:
            print(f"   Found {len(result['indices'])} similar vectors:")
            for i, (idx, dist) in enumerate(zip(result['indices'], result['distances'])):
                print(f"     {i+1}. Vector {idx} with distance {dist:.4f}")
        
    except Exception as e:
        print(f"   Error: {e}")
        return False
    
    # Test stats
    print("\n4. Testing stats endpoint...")
    try:
        response = requests.get(f"{FAISS_URL}/stats")
        print(f"   Status: {response.status_code}")
        print(f"   Response: {response.json()}")
    except Exception as e:
        print(f"   Error: {e}")
        return False
    
    print("\nAll tests completed successfully!")
    return True

if __name__ == "__main__":
    print("Make sure the FAISS service is running first:")
    print("  python start_faiss_service.py")
    print()
    
    test_faiss_service()
