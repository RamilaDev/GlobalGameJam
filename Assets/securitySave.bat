@echo off

:: Cambia estas variables según tu proyecto y repositorio

git init

:: Agregar archivos al índice de Git
git add Assets
git add Packages
git add ProjectSettings

git push



git commit -m "commit"
git push -u origin master --force

pause: