// WorkflowService.cs - Core business logic for managing workflows and their instances
// This service keeps all data in memory and enforces workflow rules and validation.

using WorkflowEngine.Models;

namespace WorkflowEngine.Services;

public class WorkflowService
{
    // In-memory store for workflow definitions (keyed by definition ID)
    private readonly Dictionary<string, WorkflowDefinition> _definitions = new();
    // In-memory store for workflow instances (keyed by instance ID)
    private readonly Dictionary<string, WorkflowInstance> _instances = new();

    // Add or update a workflow definition
    // Validates the definition for uniqueness, initial state, and references
    public (bool Success, string? Error) AddOrUpdateDefinition(WorkflowDefinition def)
    {
        // Ensure all state IDs are unique
        if (def.States.GroupBy(s => s.Id).Any(g => g.Count() > 1))
            return (false, "Duplicate state IDs.");
        // Ensure all action IDs are unique
        if (def.Actions.GroupBy(a => a.Id).Any(g => g.Count() > 1))
            return (false, "Duplicate action IDs.");
        // There must be exactly one initial state
        if (def.States.Count(s => s.IsInitial) != 1)
            return (false, "There must be exactly one initial state.");
        // State IDs must not be empty
        if (def.States.Any(s => string.IsNullOrWhiteSpace(s.Id)))
            return (false, "State IDs cannot be empty.");
        // Action IDs must not be empty
        if (def.Actions.Any(a => string.IsNullOrWhiteSpace(a.Id)))
            return (false, "Action IDs cannot be empty.");
        // All action target states must exist
        if (def.Actions.Any(a => !def.States.Any(s => s.Id == a.ToState)))
            return (false, "Action points to unknown toState.");
        // All action source states must exist
        if (def.Actions.Any(a => a.FromStates.Any(fs => !def.States.Any(s => s.Id == fs))))
            return (false, "Action has unknown fromState.");

        // If all checks pass, add or update the definition
        _definitions[def.Id] = def;
        return (true, null);
    }

    // Retrieve a workflow definition by ID
    public WorkflowDefinition? GetDefinition(string id) =>
        _definitions.TryGetValue(id, out var def) ? def : null;

    // List all workflow definitions
    public IEnumerable<WorkflowDefinition> ListDefinitions() => _definitions.Values;

    // Start a new workflow instance for a given definition
    public (WorkflowInstance? Instance, string? Error) StartInstance(string definitionId)
    {
        // Check if the definition exists
        if (!_definitions.TryGetValue(definitionId, out var def))
            return (null, "Definition not found.");
        // Find the enabled initial state
        var initial = def.States.First(s => s.IsInitial && s.Enabled);
        if (initial == null)
            return (null, "No enabled initial state.");

        // Create a new instance starting at the initial state
        var instance = new WorkflowInstance
        {
            Id = Guid.NewGuid().ToString(),
            DefinitionId = definitionId,
            CurrentStateId = initial.Id,
            History = new()
        };
        _instances[instance.Id] = instance;
        return (instance, null);
    }

    // Retrieve a workflow instance by ID
    public WorkflowInstance? GetInstance(string id) =>
        _instances.TryGetValue(id, out var inst) ? inst : null;

    // List all workflow instances
    public IEnumerable<WorkflowInstance> ListInstances() => _instances.Values;

    // Execute an action on a workflow instance
    // Moves the instance to a new state if the action is valid
    public (bool Success, string? Error) ExecuteAction(string instanceId, string actionId)
    {
        // Check if the instance exists
        if (!_instances.TryGetValue(instanceId, out var inst))
            return (false, "Instance not found.");
        // Check if the definition exists
        if (!_definitions.TryGetValue(inst.DefinitionId, out var def))
            return (false, "Definition not found.");

        // Get the current state of the instance
        var currentState = def.States.FirstOrDefault(s => s.Id == inst.CurrentStateId);
        if (currentState == null)
            return (false, "Current state not found.");
        // Prevent actions on final states
        if (currentState.IsFinal)
            return (false, "Cannot execute actions on a final state.");

        // Find the action in the definition
        var action = def.Actions.FirstOrDefault(a => a.Id == actionId);
        if (action == null)
            return (false, "Action not found in definition.");
        // Action must be enabled
        if (!action.Enabled)
            return (false, "Action is disabled.");
        // The current state must be a valid source for this action
        if (!action.FromStates.Contains(inst.CurrentStateId))
            return (false, "Action not valid from current state.");

        // The target state must exist and be enabled
        var toState = def.States.FirstOrDefault(s => s.Id == action.ToState && s.Enabled);
        if (toState == null)
            return (false, "Target state is unknown or disabled.");

        // Record the action in the instance's history and update state
        var historyEntry = new InstanceHistoryEntry
        {
            ActionId = action.Id,
            Timestamp = DateTime.UtcNow,
            FromStateId = inst.CurrentStateId,
            ToStateId = toState.Id
        };
        inst.CurrentStateId = toState.Id;
        inst.History.Add(historyEntry);
        return (true, null);
    }
}
