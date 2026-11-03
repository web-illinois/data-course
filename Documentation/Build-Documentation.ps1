# Converts every Markdown file under Documentation to static HTML in ProgramInformationV2/wwwroot/documentation.
# Run with pwsh: ./Documentation/Build-Documentation.ps1
$src = $PSScriptRoot
$out = Join-Path (Split-Path $src -Parent) 'ProgramInformationV2\wwwroot\documentation'
if (Test-Path $out) { Remove-Item $out -Recurse -Force }
New-Item -ItemType Directory -Path $out | Out-Null

$template = @'
<!DOCTYPE html>
<html dir="ltr" lang="en">
<head>
	<meta charset="utf-8" />
	<meta name="viewport" content="width=device-width, initial-scale=1.0" />
	<title>{TITLE} | Documentation | Illinois</title>
	<link rel="dns-prefetch" href="//cdn.brand.illinois.edu">
	<link rel="dns-prefetch" href="//cdn.toolkit.illinois.edu">
	<link rel="dns-prefetch" href="//cdn.disability.illinois.edu">
	<link rel="shortcut icon" href="https://cdn.brand.illinois.edu/favicon.ico">
	<link rel="apple-touch-icon" href="https://cdn.brand.illinois.edu/touch-icon-40x40.png">
	<link rel="stylesheet" href="//cdn.toolkit.illinois.edu/3/toolkit.css">
	<script type="module" src="//cdn.toolkit.illinois.edu/3/toolkit.js" async></script>
	<script src="https://cdn.disability.illinois.edu/skipto.min.js"></script>
	<script>var SkipToConfig = { 'settings': { 'skipTo': { displayOption: 'static', colorTheme: 'illinois', headings: 'h1, h2, h3', } } };</script>
	<link href="/css/site.css?cache=1" rel="stylesheet" />
</head>
<body>
<ilw-page>
	<ilw-header slot="header">
		<a slot="primary-unit" href="https://wigg.illinois.edu/">Web Implementation Guidelines Group</a>
		<a slot="site-name" href="/">WIGG Course Application</a>
		<div slot="links">
			<a href="/">Back to the Application</a>
		</div>
	</ilw-header>

	<main id="app-main">
		<ilw-content width="page">
{BODY}
		</ilw-content>
		<ilw-spacer></ilw-spacer>
	</main>

	<ilw-footer slot="footer">
		<a slot="primary-unit" href="https://wigg.illinois.edu/">Web Implementation Guidelines Group</a>
		<a slot="site-name" href="/">WIGG Course Application</a>
		<div slot="actions">
			<a href="https://go.illinois.edu/course-toolkit-help">Report a Problem</a>
		</div>
	</ilw-footer>
</ilw-page>
</body>
</html>
'@

Get-ChildItem $src -Recurse -Filter *.md | ForEach-Object {
	$rel = $_.FullName.Substring($src.Length).TrimStart('\')
	$relHtml = [IO.Path]::ChangeExtension($rel, '.html')
	if ($rel -ieq 'README.md') { $relHtml = 'index.html' }
	elseif ($_.Name -ieq 'README.md') { $relHtml = Join-Path (Split-Path $rel -Parent) 'index.html' }
	$html = (ConvertFrom-Markdown -Path $_.FullName).Html
	$html = [regex]::Replace($html, 'href="([^"#:]+?)\.md(#[^"]*)?"', {
		param($m)
		$p = $m.Groups[1].Value
		if ($p -match '(^|/)README$') { $p = $p -replace 'README$', 'index' }
		'href="' + $p + '.html' + $m.Groups[2].Value + '"'
	})
	$title = if ($html -match '<h1[^>]*>(.*?)</h1>') { $Matches[1] -replace '<[^>]+>', '' } else { $_.BaseName }
	$page = $template.Replace('{TITLE}', $title).Replace('{BODY}', $html)
	$target = Join-Path $out $relHtml
	New-Item -ItemType Directory -Path (Split-Path $target -Parent) -Force | Out-Null
	Set-Content -Path $target -Value $page -Encoding UTF8
}
Write-Host "Generated $((Get-ChildItem $out -Recurse -Filter *.html).Count) files in $out"
