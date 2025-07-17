// State.cs - Represents a single state in a workflow
// Each state can be an initial, final, or intermediate state.

namespace WorkflowEngine.Models;

public class State
{
    // Unique identifier for the state (e.g., "s1")
    public string Id { get; set; } = default!;
    // Human-readable name for the state (e.g., "Start")
    public string Name { get; set; } = default!;
    // True if this is the initial state of the workflow
    public bool IsInitial { get; set; }
    // True if this is a final (terminal) state
    public bool IsFinal { get; set; }
    // If false, this state is disabled and cannot be entered
    public bool Enabled { get; set; } = true;
    // Optional description for documentation or UI
    public string? Description { get; set; }
}