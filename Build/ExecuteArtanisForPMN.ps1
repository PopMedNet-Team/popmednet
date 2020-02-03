$currentDirectory = (Get-Item -Path ".\" -Verbose).parent.FullName
cd Artanis

cmd.exe /c Lpp.Artanis.exe /Interface=true /Path=$currentDirectory /Config=PMNInterfaces
cmd.exe /c Lpp.Artanis.exe /ViewModel=true /Path=$currentDirectory /Config=PMNViewModels
cmd.exe /c Lpp.Artanis.exe /Api=true /Path=$currentDirectory /Config=PMNAPI
cmd.exe /c Lpp.Artanis.exe /NetClient=true /Path=$currentDirectory /Config=PMNNetClient
