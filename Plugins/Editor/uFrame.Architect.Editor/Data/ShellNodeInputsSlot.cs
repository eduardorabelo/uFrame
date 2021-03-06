using Invert.Core.GraphDesigner;
using Invert.Json;

public class ShellNodeInputsSlot : GenericReferenceItem<ShellSlotTypeNode>,IShellNodeItem
{
    [JsonProperty, InspectorProperty]
    public int Order { get; set; }

    public string ReferenceClassName
    {
        get { return SourceItem.ReferenceClassName; }
    }
}