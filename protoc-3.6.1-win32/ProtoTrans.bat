@echo off

set "PROTO_EXE=%cd%\bin\protoc.exe"
set "SCR_DIR=%cd%\src"
set "CS_OUT_DIR=%cd%\cs"

for /f "delims=" %%i in ('dir /b src "src/*.proto"') do (
	echo gen src/%%i...
	%PROTO_EXE% --proto_path="%SCR_DIR%" --csharp_out="%CS_OUT_DIR%" "%SCR_DIR%\%%i"
)

echo finish..

pause