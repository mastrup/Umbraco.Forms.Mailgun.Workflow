param($installPath, $toolsPath, $package, $project)

$appPluginsFolder = $project.ProjectItems | Where-Object { $_.Name -eq "App_Plugins" }
$contentFolder = $appPluginsFolder.ProjectItems | Where-Object { $_.Name -eq "UmbracoForms.Mailgun.Template.Workflow" }

if (!$contentFolder)
{
	$newPackageFiles = "$installPath\Content\App_Plugins\UmbracoForms.Mailgun.Template.Workflow"

	$projFile = Get-Item ($project.FullName)
	$projDirectory = $projFile.DirectoryName
	$projectPath = Join-Path $projDirectory -ChildPath "App_Plugins"
	$projectPathExists = Test-Path $projectPath

	if ($projectPathExists) {
		Write-Host "Updating UmbracoForms Mailgun Template Workflow App_Plugin files using PS as they have been excluded from the project"
		Copy-Item $newPackageFiles $projectPath -Recurse -Force
	}
}
