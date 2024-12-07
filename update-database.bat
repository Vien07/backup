@echo off
set PROJECT_PATH="\MasterAdmin"

REM Define list of DbContexts
set CONTEXTS="
AuthorizationContext 
CollectionContext 
EmailContext"

cd %PROJECT_PATH%

for %%C in (%CONTEXTS%) do (
    echo Updating database for context: %%C
    dotnet ef database update -c %%C
    if %errorlevel% neq 0 (
        echo Error updating database for context %%C
        exit /b 1
    )
)

echo All contexts updated successfully!
pause
