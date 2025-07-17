// WorkflowDefinition.cs - Represents a full workflow configuration
// Contains all states and actions (transitions) for a workflow.

namespace WorkflowEngine.Models;

public class WorkflowDefinition
{
    // Unique identifier for the workflow definition (e.g., "wf1")
    public string Id { get; set; } = default!;
    // Human-readable name for the workflow (e.g., "Leave Request Workflow")
    public string Name { get; set; } = default!;
    // All states that make up this workflow
    public List<State> States { get; set; } = new();
    // All actions (transitions) that can occur in this workflow
    public List<ActionTransition> Actions { get; set; } = new();
}