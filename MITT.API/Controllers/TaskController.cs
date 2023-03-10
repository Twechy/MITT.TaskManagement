using Microsoft.AspNetCore.Mvc;
using MITT.API.Shared;
using MITT.Services.Abstracts;
using MITT.Services.TaskServices;

namespace MITT.API.Controllers;

public class TaskController : BaseController
{
    private readonly ITaskService _taskService;
    private readonly AssignmentService _taskAssignmentService;
    private readonly ReviewService _reviewService;

    public TaskController(ITaskService taskService, AssignmentService taskAssignmentService, ReviewService reviewService)
    {
        _taskService = taskService;
        _taskAssignmentService = taskAssignmentService;
        _reviewService = reviewService;
    }

    [HttpGet]
    public async Task<List<TaskVm>> Tasks(string? projectId, string? developerId, bool activeOnly = true, CancellationToken cancellationToken = default)
        => await _taskService.Tasks(projectId, developerId, activeOnly, cancellationToken);

    [HttpGet]
    public async Task<List<TaskNamesVm>> TaskNames(CancellationToken cancellationToken = default)
        => await _taskService.TaskNames(cancellationToken);

    [HttpPost]
    public async Task<OperationResult> AddOrUpdateTask(TaskDto taskDto, CancellationToken cancellationToken = default)
        => await _taskService.AddOrUpdateTask(taskDto, cancellationToken);

    [HttpPost]
    public async Task<OperationResult> AssignTask(AssignTaskDto assignTaskDto, CancellationToken cancellationToken = default) => assignTaskDto.AssignDevType switch
    {
        AssignDevType.Be => await _taskAssignmentService.AssignBETask(assignTaskDto, cancellationToken),
        AssignDevType.Qa => await _taskAssignmentService.AssignQATask(assignTaskDto, cancellationToken),
    };

    [HttpPost]
    public async Task<OperationResult> AddReview(AddReviewDto addReviewDro, CancellationToken cancellationToken = default) => addReviewDro.AssignDevType switch
    {
        AssignDevType.Be => await _reviewService.AddBeReview(addReviewDro, cancellationToken),
        AssignDevType.Qa => await _reviewService.AddQaReview(addReviewDro, cancellationToken),
    };
}