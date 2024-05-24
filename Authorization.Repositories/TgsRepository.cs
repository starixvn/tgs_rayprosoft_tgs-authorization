using System.Text.Json;
using Tgs.Authorization.Repositories.Entities;

namespace Tgs.Authorization.Repositories;

internal class TgsRepository(TgsContext dbContext) : ITgsRepository
{
	public async ValueTask CreateFunctionAsync(SystemFunction function, CancellationToken cancellationToken = default)
	{
		_ = await dbContext.FunctionRows.AddAsync(
			new FunctionRow
			{
				Id = function.Id,
				Name = function.Name,
				System = function.System
			},
			cancellationToken).ConfigureAwait(false);

		_ = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}

	public async ValueTask CreateRoleAsync(Role newRole, CancellationToken cancellationToken = default)
	{
		_ = await dbContext.RoleRows.AddAsync(
			new RoleRow
			{
				Id = newRole.Id,
				Name = newRole.Name,
				System = newRole.System
			},
			cancellationToken).ConfigureAwait(false);

		_ = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}

	public ValueTask DeleteAccountFunctionGrantAsync(Guid functionId, Guid accountId, CancellationToken cancellationToken = default) => throw new NotImplementedException();

	public async ValueTask DeleteFunctionAsync(Guid functionId, CancellationToken cancellationToken = default)
	{
		var row = await dbContext.FunctionRows.FindAsync(
			[functionId],
			cancellationToken).ConfigureAwait(false);

		if (row is not null)
		{
			dbContext.FunctionRows.Remove(row);

			_ = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}
	}

	public async ValueTask DeleteFunctionFeatureAsync(Guid featureId, CancellationToken cancellationToken = default)
	{
		var row = await dbContext.FeatureRows.FindAsync(
			[featureId],
			cancellationToken).ConfigureAwait(false);

		if (row is not null)
		{
			dbContext.FeatureRows.Remove(row);

			_ = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}
	}

	public async ValueTask DeleteRoleAsync(Guid roleId, CancellationToken cancellationToken = default)
	{
		var row = await dbContext.RoleRows.FindAsync(
			[roleId],
			cancellationToken).ConfigureAwait(false);

		if (row is not null)
		{
			dbContext.RoleRows.Remove(row);

			_ = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}
	}

	public ValueTask DeleteRoleFunctionGrantAsync(Guid functionId, Guid roleId, CancellationToken cancellationToken = default) => throw new NotImplementedException();

	public IAsyncEnumerable<Role> GetAccountAllRolesAsync(Guid accountId, CancellationToken cancellationToken = default) => throw new NotImplementedException();

	public IAsyncEnumerable<AccountGrant> GetFunctionGrantAccountsAsync(Guid functionId, CancellationToken cancellationToken = default) => throw new NotImplementedException();

	public IAsyncEnumerable<RoleGrant> GetFunctionGrantRolesAsync(Guid functionId, CancellationToken cancellationToken = default) => throw new NotImplementedException();

	public IAsyncEnumerable<Role> GetSystemAllRolesAsync(string system, CancellationToken cancellationToken = default)
		=> dbContext.RoleRows
			.Where(row => row.System == system)
			.ToAsyncEnumerable()
			.Do(_ => cancellationToken.ThrowIfCancellationRequested())
			.Select(row => new Role(row.Id, row.Name, row.System));

	public IAsyncEnumerable<SystemFunctionFeature<JsonDocument>> GetSystemFunctionFeaturesAsync(
		Guid functionId,
		CancellationToken cancellationToken = default)
		=> dbContext.FeatureRows
			.Where(row => row.FunctionId == functionId)
			.ToAsyncEnumerable()
			.Do(_ => cancellationToken.ThrowIfCancellationRequested())
			.Select(row => new SystemFunctionFeature<JsonDocument>(
				row.Id,
				row.Name,
				row.Type,
				JsonDocument.Parse(row.Settings)));

	public IAsyncEnumerable<SystemFunction> GetSystemFunctionsAsync(string system, CancellationToken cancellationToken = default)
		=> dbContext.FunctionRows
			.Where(row => row.System == system)
			.ToAsyncEnumerable()
			.Do(_ => cancellationToken.ThrowIfCancellationRequested())
			.Select(row => new SystemFunction(
				row.Id,
				row.Name,
				row.System));

	public async ValueTask InsertFunctionFeatureAsync(
		Guid functionId,
		SystemFunctionFeature<string> feature,
		CancellationToken cancellationToken = default)
	{
		_ = await dbContext.FeatureRows.AddAsync(
			new FeatureRow
			{
				Id = feature.Id,
				Name = feature.Name,
				Type = feature.Type,
				Settings = feature.Settings,
				FunctionId = functionId
			},
			cancellationToken).ConfigureAwait(false);

		_ = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}

	public async ValueTask RenameFunctionAsync(Guid functionId, string newName, CancellationToken cancellationToken = default)
	{
		var row = await dbContext.FunctionRows.FindAsync(
			[functionId],
			cancellationToken).ConfigureAwait(false);

		if (row is not null)
		{
			row.Name = newName;

			_ = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}
	}

	public ValueTask UpsertAccountFunctionGrantAsync(AccountGrant grant, CancellationToken cancellationToken = default) => throw new NotImplementedException();

	public ValueTask UpsertRoleFunctionGrantAsync(RoleGrant grant, CancellationToken cancellationToken = default) => throw new NotImplementedException();
}