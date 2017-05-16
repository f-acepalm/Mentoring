for %%A in (..\ServerService\bin\Debug\ServerService.exe) DO set P=%%~fA
sc create ServerService binPath="%P%"
