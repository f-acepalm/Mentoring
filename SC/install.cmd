for %%A in (..\ImageJoinerService\bin\Debug\ImageJoinerService.exe) DO set P=%%~fA
sc create ImageJoinerService binPath="%P%"
