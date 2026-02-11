using System.ComponentModel.Composition;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace BulkPowerAutomateActivator
{
    [Export(typeof(IXrmToolBoxPlugin))]
    [ExportMetadata("Name", "Bulk Power Automate Activator")]
    [ExportMetadata("Description", "Bulk activate Power Automate cloud flows from Dynamics 365 solutions")]
    [ExportMetadata("SmallImageBase64", null)]
    [ExportMetadata("BigImageBase64", null)]
    [ExportMetadata("BackgroundColor", "White")]
    [ExportMetadata("PrimaryFontColor", "Black")]
    [ExportMetadata("SecondaryFontColor", "Gray")]
    public class BulkPowerAutomateActivatorPlugin : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new BulkPowerAutomateActivatorControl();
        }
    }
}
