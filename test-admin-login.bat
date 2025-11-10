@echo off
echo ========================================
echo Testing Admin API Login Endpoint
echo ========================================
echo.

echo Testing LOCAL endpoint...
curl -X POST http://localhost:5000/admin/api/account/login ^
  -H "Content-Type: application/json" ^
  -d "{\"email\":\"marki\",\"password\":\"Pa$$w0rd\"}"

echo.
echo.
echo ========================================
echo Testing AZURE endpoint...
curl -X POST https://sampleecom-btadfdbsasgcfabf.southeastasia-01.azurewebsites.net/admin/api/account/login ^
  -H "Content-Type: application/json" ^
  -d "{\"email\":\"marki\",\"password\":\"Pa$$w0rd\"}"

echo.
echo.
echo ========================================
echo Expected JSON Response:
echo {
echo   "email": "admin@marki.vn",
echo   "displayName": "Marki", 
echo   "token": "eyJhbGc..."
echo }
echo ========================================
pause
