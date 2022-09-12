
param (
    # Physical path for the app.config file
    [string] $path, #"C:\Users\Rick\Documents\PsTests"

    # Key to add/update
    [string] $key,

    # Value for key
    [string] $value
    )

Write-Output ("Setting app.config settings...")
$appConfig = Join-Path $path "app.config"
if(!(Test-Path $appConfig))
{
    New-Item -path $path -name "app.config" -type "file";
    Set-Content $appConfig "<?xml version='1.0' encoding='utf-8' ?><configuration><appSettings></appSettings></configuration>"

}

[bool] $found = $false

if (Test-Path $appConfig)
{
    $xml = [xml](get-content $appConfig);
    $root = $xml.DocumentElement;

    foreach ($item in $root.appSettings.add)
    {
        if ($item.key -eq $key)
        {
            $item.SetAttribute("value",$value);
            $found = $true;
        }
    }

    if (-not $found)
    {
        $newElement = $xml.CreateElement("add");
        $nameAtt1 = $xml.CreateAttribute("key");
        $nameAtt1.psbase.value = $key;
        $newElement.SetAttributeNode($nameAtt1);

        $nameAtt2 = $xml.CreateAttribute("value");
        $nameAtt2.psbase.value = $value;
        $newElement.SetAttributeNode($nameAtt2);

        $xml.configuration["appSettings"].AppendChild($newElement);
    }

    $xml.Save($appConfig)
}
else
{
    Write-Error -Message "Error: File not found '$appConfig'"
}
