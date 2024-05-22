using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Tgs.Authorization.ApplicationServices;
using Tgs.Authorization.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.Authorization.Controllers;

[Route("[controller]")]
[ApiController]
public class FunctionController : ControllerBase
{
	[HttpGet("{system}", Name = "ListSystemFunctions")]
	public IAsyncEnumerable<SystemFunctionViewModel> ListSystemFunctionsAsync(
		[FromServices] IQueryService<ListSystemFunctionsQuery, SystemFunction> queryService,
		string system)
	{
		var query = new ListSystemFunctionsQuery(system);

		return queryService.QueryAsync(query, HttpContext.RequestAborted)
			.Select(sysFunc => new SystemFunctionViewModel
			{
				Id = sysFunc.Id,
				Name = sysFunc.Name,
				System = sysFunc.System
			});
	}

	[HttpPut(Name = "CreateFunction")]
	public ValueTask<Guid> CreateFunctionAsync(
		[FromServices] IGetService<CreateFunctionCommand, Guid> createService,
		[FromBody] CreateFunctionViewModel viewModel)
	{
		var command = new CreateFunctionCommand(viewModel.Name, viewModel.System);

		return createService.GetAsync(command, HttpContext.RequestAborted);
	}

	[HttpPatch("{functionId:Guid}", Name = "RenameFunction")]
	public ValueTask RenameFunctionAsync(
		[FromServices] ICommandService<RenameFunctionCommand> renameService,
		Guid functionId,
		[FromBody] string newName)
	{
		var command = new RenameFunctionCommand(functionId, newName);

		return renameService.ExecuteAsync(command, HttpContext.RequestAborted);
	}

	[HttpDelete("{functionId:guid}", Name = "DeleteFunction")]
	public ValueTask DeleteFunctionAsync(
		[FromServices] ICommandService<DeleteFunctionCommand> deleteService,
		Guid functionId)
	{
		var command = new DeleteFunctionCommand(functionId);

		return deleteService.ExecuteAsync(command, HttpContext.RequestAborted);
	}

	[HttpGet("feature/{functionId:guid}", Name = "GetFunctionFeatures")]
	public IAsyncEnumerable<SystemFunctionFeatureViewModel> ListFunctionFeaturesAsync(
		[FromServices] IQueryService<ListFunctionFeaturesQuery, SystemFunctionFeature<JsonDocument>> queryService,
		Guid functionId)
	{
		var query = new ListFunctionFeaturesQuery(functionId);

		return queryService.QueryAsync(query, HttpContext.RequestAborted)
			.Select(feat => new SystemFunctionFeatureViewModel
			{
				Id = feat.Id,
				Name = feat.Name,
				Type = feat.Type,
				Settings = feat.Settings,
			});
	}

	[HttpPut("feature/{functionId:Guid}", Name = "AddFunctionFeature")]
	public ValueTask<Guid> AddFunctionFeatureAsync(
		[FromServices] IGetService<AddFunctionFeatureCommand, Guid> addFeaturService,
		Guid functionId,
		[FromBody] CreateFunctionFeatureViewModel viewModel)
	{
		var command = new AddFunctionFeatureCommand(
			functionId,
			viewModel.Name,
			viewModel.Type,
			viewModel.Settings);

		return addFeaturService.GetAsync(command, HttpContext.RequestAborted);
	}

	[HttpDelete("feature/{featureId:Guid}", Name = "RemoveFunctionFeature")]
	public ValueTask RemoveFunctionFeatureAsync(
		[FromServices] ICommandService<RemoveFunctionFeatureCommand> removeService,
		Guid featureId)
	{
		var command = new RemoveFunctionFeatureCommand(featureId);

		return removeService.ExecuteAsync(command, HttpContext.RequestAborted);
	}
}