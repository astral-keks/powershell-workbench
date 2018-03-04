if (([System.Management.Automation.PSTypeName]'System.Runtime.Loader.AssemblyLoadContext').Type) 
{
	$action = 
	{ 
		param ($loadContext, $assemblyName)
		$assembly = $null
		
		$assemblyRedirecting = ('Newtonsoft.Json', 'AstralKeks.Workbench.Common').Contains($assemblyName.Name)
		$assemblyPath = [System.IO.Path]::Combine($PSScriptRoot, "$($assemblyName.Name).dll")
		$assemblyNames = [System.AppDomain]::CurrentDomain.GetAssemblies() | foreach { $_.FullName }
		if ($assemblyRedirecting -and (Test-Path $assemblyPath) -and !$assemblyNames.Contains($assemblyName.FullName))
		{
			$assemblyBytes = [System.IO.File]::ReadAllBytes($assemblyPath)
			$assembly = [System.Reflection.Assembly]::Load($assemblyBytes)
		}
		
		return $assembly
	}
	
	$context = [System.Runtime.Loader.AssemblyLoadContext]::Default
	$handler = [Func[System.Runtime.Loader.AssemblyLoadContext, System.Reflection.AssemblyName, System.Reflection.Assembly]]$action
	$context.GetType().GetEvent('Resolving').AddEventHandler($context, $handler)
}


