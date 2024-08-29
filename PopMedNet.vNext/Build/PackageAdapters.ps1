Write-Host $PSScriptRoot\

$Paths = @(("Lpp.Dns.DataMart.Model.QueryComposer","$PSScriptRoot\..\Lpp.Adapters\Lpp.Dns.DataMart.Model.QueryComposer\bin\*"),
				("Lpp.Dns.DataMart.Model.ESPQueryBuilder","$PSScriptRoot\..\Lpp.Adapters\Lpp.Dns.DataMart.Model.ESPQueryBuilder\bin\*"),
				("Lpp.Dns.DataMart.Model.DataChecker","$PSScriptRoot\..\Lpp.Adapters\Lpp.Dns.DataMart.Model.DataChecker\bin\*"),
				("Lpp.Dns.DataMart.Model.ESPQueryBuilder","$PSScriptRoot\..\Lpp.Adapters\Lpp.Dns.DataMart.Model.ESPQueryBuilder\bin\*"),
				("Lpp.Dns.DataMart.Model.ESPQueryBuilder.Conditions","$PSScriptRoot\..\Lpp.Adapters\Lpp.Dns.DataMart.Model.ESPQueryBuilder.Conditions\bin\*"),
				("Lpp.Dns.DataMart.Model.ESPQueryBuilder.I2B2","$PSScriptRoot\..\Lpp.Adapters\Lpp.Dns.DataMart.Model.ESPQueryBuilder.I2B2\bin\*"),
				("Lpp.Dns.DataMart.Model.Metadata","$PSScriptRoot\..\Lpp.Adapters\Lpp.Dns.DataMart.Model.Metadata\bin\*"),
				("Lpp.Dns.DataMart.Model.Processors","$PSScriptRoot\..\Lpp.Adapters\Lpp.Dns.DataMart.Model.Processors\bin\*"))

$attr = (get-content "$PSScriptRoot\CommonAssemblyInfo.cs" | select-string "AssemblyFileVersion").ToString()
# Parse the attribute to get the 3 digit version
$s = $attr.IndexOf("`"")+1
$e = $attr.LastIndexOf(".")
$attr.Substring($s,$e-$s+2)
$version = [System.Version]::Parse($attr.Substring($s,$e-$s+2))

$includeBaseDirectory = $false
$compressionLevel = [IO.Compression.CompressionLevel]::Optimal
Add-Type -Assembly "System.IO.Compression.FileSystem" ;
foreach($p in $Paths)
{
    try{
	    $zipName = $p[0]
	    $zipPath = $p[1]
	    $zipPath = $zipPath.Substring(0,$zipPath.length - 2)
	    if((Test-Path -Path "$PSScriptRoot\..\Lpp.Dns.Api\App_Data\$zipName.$version.zip" )){
			    Remove-Item "$PSScriptRoot\..\Lpp.Dns.Api\App_Data\$zipName.$version.zip" -Force
	    }
        [System.IO.Compression.ZipFile]::CreateFromDirectory($zipPath, "$PSScriptRoot\..\Lpp.Dns.Api\App_Data\$zipName.$version.zip", $compressionLevel, $includeBaseDirectory);
    }
    catch{
        Write-Host "the adpater $zipName was not zipped correctly. Below is the error"
        throw $_.Exception
    }
}	