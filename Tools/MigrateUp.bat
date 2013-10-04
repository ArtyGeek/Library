@echo off
set /p InsertTestData="Insert test data? (Y/N) "

IF %InsertTestData% == Y GOTO MigrateWithTestData
IF %InsertTestData% == N GOTO MigrateWithoutTestData
GOTO End

:MigrateWithTestData
..\Sources\Library\packages\FluentMigrator.Tools.1.1.1.0\tools\AnyCPU\40\Migrate.exe --db=sqlserver --configPath=..\Sources\Library\Library.Web\Web.config --connectionString="LibraryConnectionString" --assembly=..\Sources\Library\Library.Migrations\bin\Debug\Library.Migrations.dll --verbose=true --profile=TestData
GOTO End

:MigrateWithoutTestData
..\Sources\Library\packages\FluentMigrator.Tools.1.1.1.0\tools\AnyCPU\40\Migrate.exe --db=sqlserver --configPath=..\Sources\Library\Library.Web\Web.config --connectionString="LibraryConnectionString" --assembly=..\Sources\Library\Library.Migrations\bin\Debug\Library.Migrations.dll --verbose=true
GOTO End

:End
pause