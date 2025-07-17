// ActionTransition.cs - Represents a transition (action) between states in a workflow
// Each action can move an instance from one or more source states to a single target state.

namespace WorkflowEngine.Models;

public class ActionTransition
{
    // Unique identifier for the action (e.g., "a1")
    public string Id { get; set; } = default!;
    // Human-readable name for the action (e.g., "Approve")
    public string Name { get; set; } = default!;
    // If false, this action is disabled and cannot be executed
    public bool Enabled { get; set; } = true;
    // List of state IDs from which this action can be executed
    public List<string> FromStates { get; set; } = new();
    // The state ID to which this action transitions
    public string ToState { get; set; } = default!;
}
