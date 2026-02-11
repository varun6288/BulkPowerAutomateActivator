# BulkPowerAutomateActivator
Bulk activate Power Automate flows (Cloud Flows) from Dynamics 365 solutions.  Save time by activating multiple flows at once instead of one-by-one.


KEY FEATURES:
- Select any solution (managed or unmanaged) from your connected environment
- View all Power Automate flows contained in the solution
- Activate multiple flows simultaneously with a single click
- Track activation progress with real-time status updates
- View detailed results and error messages for each flow
- Filter flows by activation state (active/inactive)
- Reducing manual repetitive tasks during solution management or Dataverse envrionment restore.
- ALM scenarios requiring quick flow activation


WHY USE THIS TOOL:
Manually activating flows one-by-one through the Power Automate interface is time-consuming and error-prone, especially when dealing with solutions 
containing dozens of flows. This plugin streamlines the process, allowing system admins to activate all flows in a solution with just a few clicks.

REQUIREMENTS:
- XrmToolBox version 1.2023.x or later
- Appropriate security roles to activate workflows in the target environment
- Dynamics 365 CE/Power Platform environment

SUPPORT:
For issues, feature requests, or contributions, visit: [https://github.com/varun6288/BulkPowerAutomateActivator]
================================================================================================================================

I'm developing an XrmToolBox plugin called "BulkPowerAutomateActivator" for 
Dynamics 365/Power Platform.

PROJECT DETAILS:
- Plugin Name: BulkPowerAutomateActivator
- Purpose: Bulk activate Power Automate flows (Cloud Flows) from Dynamics 365 solutions
- Framework: .NET Framework 4.6.2 or later
- IDE: Visual Studio

KEY REQUIREMENTS:
1. Inherit from XrmToolBox PluginControlBase
2. Connect to Dynamics 365 using IOrganizationService
3. Retrieve solutions from the environment
4. Get Power Automate flows from selected solution (workflows with category=5)
5. Bulk activate selected flows using SetStateRequest
6. Show progress and results to user

NUGET PACKAGES NEEDED:
- XrmToolBox.Extensibility
- Microsoft.CrmSdk.CoreAssemblies
- Microsoft.CrmSdk.XrmTooling.CoreAssembly

MAIN FUNCTIONALITY:
- Solution dropdown/selector
- DataGridView showing flows with checkboxes
- Activate Selected button
- Progress bar for bulk operations
- Status/log panel for results

Please help me build this plugin step by step, starting with project setup 
and structure.
