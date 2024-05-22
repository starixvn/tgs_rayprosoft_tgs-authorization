using Microsoft.EntityFrameworkCore;
using Tgs.Authorization;
using Tgs.Authorization.Repositories;
using Tgs.Authorization.Repositories.Entities;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddTgsAuthorizationRepositories(
		this IServiceCollection services,
		Action<DbContextOptionsBuilder> dbConfigure)
		=> services
			.AddDbContext<TgsContext>()
			.Configure(dbConfigure)
			.AddScoped<ITgsRepository, TgsRepository>();
}
