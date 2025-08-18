@echo off
echo ====================================
echo Simple RAG .NET with FAISS Integration (Persistent)
echo ====================================
echo.

echo Installing/updating Python dependencies...
pip install -r requirements.txt -q
echo.

echo Starting FAISS Vector Database Service with Persistence...
echo Service will be available at: http://localhost:8001
echo Data will be saved to: faiss_index.bin and documents.json
echo.

start "FAISS Service" cmd /k "python start_faiss_service.py"

echo Waiting for FAISS service to start up...
timeout /t 5 /nobreak > nul

echo.
echo Testing FAISS service connection...
python test_faiss_service.py

echo.
echo Starting .NET RAG Application with Persistence...
echo Data will be cached locally in: vector_cache.json
echo.

dotnet run

echo.
echo Application finished. FAISS data has been automatically saved.
echo Press any key to close FAISS service...
pause > nul

echo Stopping FAISS service...
taskkill /f /im python.exe /fi "WINDOWTITLE eq FAISS Service*" 2>nul

echo Done!
