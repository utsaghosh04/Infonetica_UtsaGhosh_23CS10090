// WorkflowInstance.cs - Represents a running instance of a workflow
// Tracks the current state and the history of actions taken.

namespace WorkflowEngine.Models;

public class WorkflowInstance
{
    // Unique identifier for this workflow instance
    public string Id { get; set; } = default!;
    // The ID of the workflow definition this instance is based on
    public string DefinitionId { get; set; } = default!;
    // The current state ID of this instance
    public string CurrentStateId { get; set; } = default!;
    // List of actions taken (with timestamps and state changes)
    public List<InstanceHistoryEntry> History { get; set; } = new();
}

// Represents a single action taken in the instance's history
public class InstanceHistoryEntry
{
    // The ID of the action performed
    public string ActionId { get; set; } = default!;
    // When the action was performed
    public DateTime Timestamp { get; set; }
    // The state ID before the action
    public string FromStateId { get; set; } = default!;
    // The state ID after the action
    public string ToStateId { get; set; } = default!;
}