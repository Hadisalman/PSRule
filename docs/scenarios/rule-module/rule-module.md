# Packaging rules in a module

PSRule supports distribution of rules within modules.
Using a module, rules can be published and installed using standard PowerShell cmdlets.

You should consider packaging rules into a module to:

- Version rules. PowerShell modules support semantic versioning (semver).
- Reuse rules across projects, pipelines or teams.
- Publish rules to external consumers via the PowerShell Gallery.

This scenario covers the following:

- Creating a module manifest
- Including rules and baselines
- Including documentation

## Creating a module manifest

When creating a PowerShell module, a module manifest is an optional file that stores module metadata.
Module manifests use the `.psd1` file extension.
When packaging rules in a module, a module manifest is required for PSRule discover the module.

### Creating the manifest file

A module manifest can be created from PowerShell using the `New-ModuleManifest` cmdlet.
Additionally, Visual Studio Code and many other tools also include snippets for creating a module manifest.

For example:

```powershell
# Create a directory for the module
md ./Enterprise.Rules;

# Create the manifest
New-ModuleManifest -Path ./Enterprise.Rules/Enterprise.Rules.psd1 -Tags 'PSRule-rules';
```

The example above creates a module manifest for a module named _Enterprise.Rules_ tagged with `PSRule-rules`.
The use of the `PSRule-rules` tag is explained in the following section.

### Setting module tags

When PSRule cmdlets are used with the `-Module` parameter, PSRule discovers rule modules.
If the module is already imported, that module is used.
If the module is not imported, PSRule will import the highest version of the module automatically.

For a module to be discovered by PSRule, tag the module with `PSRule-rules`.
To tag modules, find the `Tags` section the `PSData` hashtable in the module manifest and add `PSRule-rules`.

An updated module manifest may look like this:

```powershell
# Private data to pass to the module specified in RootModule/ModuleToProcess. This may also contain a PSData hashtable with additional module metadata used by PowerShell.
PrivateData = @{
    PSData = @{
        # Tags applied to this module. These help with module discovery in online galleries.
        Tags = @('PSRule-rules')
    }
}
```

### Setting a default baseline

Optionally, baselines can be included in rule modules.
If a baseline contains configuration or binding options then setting a default baseline is often desirable.
When a default baseline is set, PSRule will use the named baseline automatically when processing rules from that module.
This feature removes the need for users to specify it manually.

To set a default baseline, add a new property after the `PSData` hashtable ends called `PSRule`.
The `PSRule` property is a hashtable of configuration for PSRule.
Within the hashtable specify the `Baseline` property with the name of the baseline.

For example:

```powershell
# Private data to pass to the module specified in RootModule/ModuleToProcess. This may also contain a PSData hashtable with additional module metadata used by PowerShell.
PrivateData = @{
    PSData = @{
        # Tags applied to this module. These help with module discovery in online galleries.
        Tags = @('PSRule-rules')
    }
    PSRule = @{
        Baseline = 'Enterprise.Default'
    }
}
```

This examples set the default baseline to `Enterprise.Default`.
The default baseline must be included in file ending with `.Rule.yaml` within the module directory structure.

## Including rules and baselines

Rules and baselines can be included anywhere within the module directory structure.
Such as in the root directory of the module or in a nested sub-directory.

By convention, consider including rules and baselines within a `rules` sub-directory within the module.

For example:

- Enterprise.Rules/
  - rules/
    - NamingStandards.Rule.ps1
    - ComplianceStandards.Rule.ps1
    - Baseline.Rule.yaml
  - Enterprise.Rules.psd1

### File names

For PSRule to find rules included in a module, rule file names must end with the `.Rule.ps1` suffix.
We recommend using the exact case `.Rule.ps1`.
This is because some file systems are case-sensitive.
For example, on Linux `NamingStandards.rule.ps1` would be ignored by PSRule.

Similarly, when including baselines within a module use the `.Rule.yaml` suffix.

## Including documentation

PSRule supports write and packaging rule modules with markdown documentation.
Markdown documentation is automatically interpreted by PSRule and included in output.

When including markdown, files are copied into a directory structure based on the target culture.

For example, store documentation targeted to the culture `en-US` in a directory named `en-US`.
Similarly, documentation for cultures such as `en-AU`, `en-GB` and `fr-FR` would be in separate directories.

If a directory for the exact culture `en-US` doesn't exist, PSRule will attempt to find the parent culture.
For example, documentation would be read from a directory named `en`.

When naming directories for their culture, use exact case.
This is because some file systems are case-sensitive.
For example on Linux `en-us` would not match.

For example:

- Enterprise.Rules/
  - en/
    - Metadata.Name.md
  - en-US/
    - Metadata.Name.md
  - fr-FR/
    - Metadata.Name.md
  - rules/
    - NamingStandards.Rule.ps1
    - ComplianceStandards.Rule.ps1
    - Baseline.Rule.yaml
  - Enterprise.Rules.psd1

## More information

- [Enterprise.Rules.psd1](Enterprise.Rules/Enterprise.Rules.psd1) - An example module manifest.
