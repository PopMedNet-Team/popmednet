param(
	[string]$root = '.\'
)

$moveTags = '(Compile|EmbeddedResource)'
$moveMask = '(\.cs|\.css|\.less|(app|web)\.config|\.sql|\.cshtml|\.ts)'
$selfClosingToMove = "<$moveTags [^/>]+/>[\s\n]*"
$fullToMove = "<(?<fullTag>$moveTags)\s+[^/>]+>[\s\n]*((?!</\k<fullTag>>).|\s|\n)*</\k<fullTag>>"
$maskedToMove = "<[a-zA-Z]+ Include=`"((?!$moveMask|(`")).)*$moveMask`"[^/>]*/>[\s\n]*"
$fullMasked = "<(?<tag>[a-zA-Z]+) Include=`"((?!$moveMask|(`")).)*$moveMask`"[^/>]*>[\s\n]*((?!</\k<tag>>).|\s|\n)*</\k<tag>>"
$allToMove = "($selfClosingToMove)|($fullToMove)|($maskedToMove)|($fullMasked)"

dir -include *.csproj -recurse |
where-object { -not ($_.fullname -match '(\\net45\\)|(\.vsext)') } |
foreach-object {
	$dir = [System.IO.Path]::GetDirectoryName($_.fullname);
	$fn = [System.IO.Path]::GetFileName($_.fullname);
	$targetDir = "$dir\net45";
	$targetName = "$targetDir\$fn";
	write-host "$_ -> $targetName"

	if( !(test-path $targetDir -pathtype container) ) { new-item $targetDir -type directory }
	if( !(test-path "$targetDir\packages.config" -pathtype leaf) -and (test-path "$dir\packages.config" -pathtype leaf) ) { copy-item "$dir\packages.config" -destination "$targetDir\packages.config" }

	$content = get-content $_ | out-string

	if ( test-path $targetName -pathtype leaf ) {

		$tcontent = get-content $targetName | out-string

		$replaceWith = [System.Text.RegularExpressions.Regex]::Matches( $content, $allToMove ) | foreach-object { $_.Value } | out-string
		$replaceWith = $replaceWith `
						-replace "<(?<tag>[a-zA-Z]+)\s*Include=`"(?<path>[^`"]+)`"\s*>[\s\n]*(?<inside>((?!</\k<tag>>).|\s|\n)*)</\k<tag>>", '<${tag} Include="..\${path}"><Link>${path}</Link>${inside}</${tag}>' `
						-replace "<([a-zA-Z]+)\s*Include=`"([^`"]+)`"\s*/>", '<$1 Include="..\$2"><Link>$2</Link></$1>'

		$res = $tcontent `
				-replace $allToMove, '' `
				-replace '</Project>(\s\n)*$', "<ItemGroup>$replaceWith</ItemGroup>`n</Project>"

	} else {
	
		$res = $content `
				-replace '(<Project [^>]+>\s*\n*\s*)(<Import[^/]+/>\s*\n*\s*)(<PropertyGroup>((?!</PropertyGroup>)(.+\s*\n*\s*))+</PropertyGroup>)', '$1$3$2' `
				-replace '<Import Project="..\\Lpp.Mvc.Composition.targets"\s*/>', '<Import Project="..\..\Lpp.Mvc.Composition.targets" />' `
				-replace '<HintPath>(\.\.[^\<]+)</HintPath>', '<HintPath>..\$1</HintPath>' `
				-replace '<(Compile|EmbeddedResource) Include="([^"]+)"', '<$1 Include="..\$2"' `
				-replace '<(ProjectReference Include=")(\.\.\\[^\\]+\\)(((?!\.csproj).)+)\.csproj"', '<$1..\$2net45\$3.csproj"';

		if ($content -match '<TargetFrameworkVersion>') { 
			$res = $res -replace '<TargetFrameworkVersion>[^\<]+</TargetFrameworkVersion>', '<TargetFrameworkVersion>v4.5</TargetFrameworkVersion>'
		} else {
			$res = $res -replace '<Project [^\>]+>\s*\n\s*', "`$0<PropertyGroup><TargetFrameworkVersion>v4.5</TargetFrameworkVersion></PropertyGroup>`n"
		}
	}

	set-content $targetName $res
}