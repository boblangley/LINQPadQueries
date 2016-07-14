<Query Kind="Program">
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Microsoft.Win32</Namespace>
</Query>

void Main()
{
	var subKey64 = @"SOFTWARE\WOW6432Node\ParticularSoftware";
	var subKey32 = @"SOFTWARE\ParticularSoftware";
	
	var enable = LINQPad.Util.ReadLine<bool>("Enable?", true);
	
	ToggleSubkey(Registry.LocalMachine, subKey32, enable);
	ToggleSubkey(Registry.CurrentUser, subKey32, enable);
	ToggleSubkey(Registry.LocalMachine, subKey64, enable);
	ToggleSubkey(Registry.CurrentUser, subKey64, enable);
}

// Define other methods and classes here
void ToggleSubkey(RegistryKey parentKey, string subKey, bool enable)
{
	var key = parentKey.OpenSubKey(subKey, true);

	if (key == null) return;

	var values = key.GetValueNames();

	if (values.Contains("License") && !enable)
	{
		key.SetValue("LicenseDisabled", key.GetValue("License"));
		key.DeleteValue("License");
	}
	else if (values.Contains("LicenseDisabled") && enable)
	{
		key.SetValue("License", key.GetValue("LicenseDisabled"));
		key.DeleteValue("LicenseDisabled");
	}
}