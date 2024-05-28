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

	public async ValueTask DeleteAccountFunctionGrantAsync(Guid functionId, Guid accountId, CancellationToken cancellationToken = default)
	{
		var row = await dbContext.GrantRows.FindAsync(
			[accountId, functionId],
			cancellationToken).ConfigureAwait(false);

		if (row is not null)
		{
			dbContext.GrantRows.Remove(row);

			_ = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}
	}

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

	public async ValueTask DeleteRoleFunctionGrantAsync(Guid roleId, Guid functionId, CancellationToken cancellationToken = default)
	{
		var row = await dbContext.RoleGrantRows.FindAsync(
			[roleId, functionId],
			cancellationToken).ConfigureAwait(false);

		if (row is not null)
		{
			dbContext.RoleGrantRows.Remove(row);

			_ = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}
	}

	public IAsyncEnumerable<Role> GetAccountAllRolesAsync(Guid accountId, CancellationToken cancellationToken = default)
	{
		var query = from p in dbContext.AccountRoles
					from role in dbContext.RoleRows
					where p.AccountId == accountId
						&& role.Id == p.RoleId
					select role;

		return query.ToAsyncEnumerable()
			.Do(_ => cancellationToken.ThrowIfCancellationRequested())
			.Select(row => new Role(row.Id, row.Name, row.System));
	}

	public IAsyncEnumerable<AccountGrant> GetFunctionGrantAccountsAsync(Guid functionId, CancellationToken cancellationToken = default)
		=> dbContext.GrantRows
			.Where(row => row.FunctionId == functionId)
			.ToAsyncEnumerable()
			.Do(_ => cancellationToken.ThrowIfCancellationRequested())
			.Select(row => new AccountGrant(
				row.FunctionId,
				row.AccountId,
				Enum.Parse<PermissionType>(row.Type)));

	public IAsyncEnumerable<RoleGrant> GetFunctionGrantRolesAsync(Guid functionId, CancellationToken cancellationToken = default)
		=> dbContext.RoleGrantRows
			.Where(row => row.FunctionId == functionId)
			.ToAsyncEnumerable()
			.Do(_ => cancellationToken.ThrowIfCancellationRequested())
			.Select(row => new RoleGrant(
				row.FunctionId,
				row.RoleId,
				Enum.Parse<PermissionType>(row.Type),
				100));

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

	public async ValueTask UpsertAccountFunctionGrantAsync(AccountGrant grant, CancellationToken cancellationToken = default)
	{
		var row = await dbContext.GrantRows.FindAsync(
			[grant.AccountId, grant.FunctionId],
			cancellationToken).ConfigureAwait(false)
			?? (await dbContext.GrantRows.AddAsync(
				new GrantRow
				{
					AccountId = grant.AccountId,
					FunctionId = grant.FunctionId
				},
				cancellationToken).ConfigureAwait(false)).Entity;

		row.Type = grant.Type.ToString();

		_ = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}

	public async ValueTask UpsertRoleFunctionGrantAsync(RoleGrant grant, CancellationToken cancellationToken = default)
	{
		var row = await dbContext.RoleGrantRows.FindAsync(
			[grant.RoleId, grant.FunctionId],
			cancellationToken).ConfigureAwait(false)
			?? (await dbContext.RoleGrantRows.AddAsync(
				new RoleGrantRow
				{
					RoleId = grant.RoleId,
					FunctionId = grant.FunctionId,
				},
				cancellationToken).ConfigureAwait(false)).Entity;

		row.Type = grant.Type.ToString();

		_ = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}

	public async ValueTask UpsertAccountInRoleAsync(Guid accountId, Guid roleId, int priority, CancellationToken cancellationToken = default)
	{
		var row = await dbContext.AccountRoles.FindAsync(
			[accountId, roleId],
			cancellationToken).ConfigureAwait(false)
			?? (await dbContext.AccountRoles.AddAsync(
				new AccountRole
				{
					AccountId = accountId,
					RoleId = roleId,
				},
				cancellationToken).ConfigureAwait(false)).Entity;

		row.Priority = priority;

		_ = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}

	public async ValueTask RemoveAccountInRoleAsync(Guid accountId, Guid roleId, CancellationToken cancellationToken = default)
	{
		var row = await dbContext.AccountRoles.FindAsync(
			[accountId, roleId],
			cancellationToken).ConfigureAwait(false);

		if (row is not null)
		{
			dbContext.AccountRoles.Remove(row);

			_ = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}
	}

	public async ValueTask UpsertRoleInRoleAsync(Guid roleId, Guid parentRoleId, int priority, CancellationToken cancellationToken = default)
	{
		var row = await dbContext.RoleInRoles.FindAsync(
			[roleId, parentRoleId],
			cancellationToken).ConfigureAwait(false)
			?? (await dbContext.RoleInRoles.AddAsync(
				new RoleInRole
				{
					RoleId = roleId,
					ParentRoleId = parentRoleId
				},
				cancellationToken).ConfigureAwait(false)).Entity;

		row.Priority = priority;

		_ = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}

	public async ValueTask RemoveRoleInRoleAsync(Guid roleId, Guid parentRoleId, CancellationToken cancellationToken = default)
	{
		var row = await dbContext.RoleInRoles.FindAsync(
			[roleId, parentRoleId],
			cancellationToken).ConfigureAwait(false);

		if (row is not null)
		{
			dbContext.RoleInRoles.Remove(row);

			_ = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}
	}
}