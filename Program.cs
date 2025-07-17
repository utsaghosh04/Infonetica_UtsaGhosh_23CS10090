// Program.cs - Entry point for the Workflow State Machine API
// This file sets up the minimal API endpoints for managing workflows and their instances.

using WorkflowEngine.Models;
using WorkflowEngine.Services;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Create a singleton service to manage workflows and instances in memory
var workflowService = new WorkflowService();

// --- Workflow Definition Endpoints ---

// Create or update a workflow definition (states + actions)
app.MapPost("/workflows", (WorkflowDefinition def) =>
{
    var (success, error) = workflowService.AddOrUpdateDefinition(def);
    // Return 200 OK if successful, otherwise 400 Bad Request with error message
    return success ? Results.Ok(def) : Results.BadRequest(error);
});

// Retrieve a workflow definition by its ID
app.MapGet("/workflows/{id}", (string id) =>
{
    var def = workflowService.GetDefinition(id);
    return def is not null ? Results.Ok(def) : Results.NotFound();
});

// List all workflow definitions
app.MapGet("/workflows", () =>
    Results.Ok(workflowService.ListDefinitions()));

// --- Workflow Instance Endpoints ---

// Start a new workflow instance from a definition
app.MapPost("/instances", (string definitionId) =>
{
    var (instance, error) = workflowService.StartInstance(definitionId);
    // Returns the new instance or an error if the definition is invalid
    return instance is not null ? Results.Ok(instance) : Results.BadRequest(error);
});

// Retrieve a workflow instance by its ID (shows current state and history)
app.MapGet("/instances/{id}", (string id) =>
{
    var inst = workflowService.GetInstance(id);
    return inst is not null ? Results.Ok(inst) : Results.NotFound();
});

// List all workflow instances
app.MapGet("/instances", () =>
    Results.Ok(workflowService.ListInstances()));

// --- Action Execution ---

// Execute an action on a workflow instance (move to next state if valid)
app.MapPost("/instances/{id}/actions/{actionId}", (string id, string actionId) =>
{
    var (success, error) = workflowService.ExecuteAction(id, actionId);
    // Returns 200 OK if the action is valid and performed, otherwise 400 Bad Request
    return success ? Results.Ok() : Results.BadRequest(error);
});

// Start the web application
app.Run();