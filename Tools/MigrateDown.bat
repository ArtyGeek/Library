@echo off
set /p StepsToRollback="How much steps do you want to rollback? (0 means all) "

IF %StepsToRollback% == 0 GOTO RollbackAll ELSE GOTO RollbackWithSteps
GOTO End

:RollbackAll
..\Sources\Library\packages\FluentMigrator.Tools.1.1.1.0\tools\AnyCPU\40\Migrate.exe --db=sqlserver --configPath=..\Sources\Library\Library.Web\Web.config --connectionString="LibraryConnectionString" --assembly=..\Sources\Library\Library.Migrations\bin\Debug\Library.Migrations.dll --verbose=true --profile=TestData --task=rollback:all
GOTO End

:RollbackWithSteps
..\Sources\Library\packages\FluentMigrator.Tools.1.1.1.0\tools\AnyCPU\40\Migrate.exe --db=sqlserver --configPath=..\Sources\Library\Library.Web\Web.config --connectionString="LibraryConnectionString" --assembly=..\Sources\Library\Library.Migrations\bin\Debug\Library.Migrations.dll --verbose=true --profile=TestData --task=rollback:%StepsToRollback%
GOTO End

:End
pause