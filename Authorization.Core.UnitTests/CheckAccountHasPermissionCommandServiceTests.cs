using System;
using Tgs.Authorization;
using Tgs.Authorization.ApplicationServices;
using Xunit;

namespace Authorization.Core.UnitTests;

public class CheckAccountHasPermissionCommandServiceTests
{
    [Fact]
    public async Task CheckAccountHasPermissionCommandService_檢查帳號是否有權限_如果沒有任何設定應該回傳沒有權限()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new CheckAccountHasPermissionCommandService(fakeTgsRepository);

        var command = new CheckAccountHasPermissionCommand(Guid.NewGuid(), Guid.NewGuid());

        // Act
        var actual = await sut.GetAsync(command);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public async Task CheckAccountHasPermissionCommandService_檢查帳號是否有權限_只有設定帳號Allow會回傳有權限()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new CheckAccountHasPermissionCommandService(fakeTgsRepository);

        var command = new CheckAccountHasPermissionCommand(Guid.NewGuid(), Guid.NewGuid());

        _ = fakeTgsRepository.GetFunctionGrantAccountsAsync(Arg.Is(command.FunctionId))
            .Returns(new[]
            {
                new AccountGrant(command.FunctionId, command.AccountId, PermissionType.Allow)
            }.ToAsyncEnumerable());

        // Act
        var actual = await sut.GetAsync(command);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public async Task CheckAccountHasPermissionCommandService_檢查帳號是否有權限_會檢查帳號所屬的所有角色權限()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new CheckAccountHasPermissionCommandService(fakeTgsRepository);

        var command = new CheckAccountHasPermissionCommand(Guid.NewGuid(), Guid.NewGuid());

        var accountRoles = new[]
        {
            new Role(Guid.NewGuid(), "test1", "test")
        };

        _ = fakeTgsRepository.GetAccountAllRolesAsync(Arg.Is(command.AccountId), Arg.Is(true))
            .Returns(accountRoles.ToAsyncEnumerable());

        _ = fakeTgsRepository.GetFunctionGrantRolesAsync(Arg.Is(command.FunctionId))
            .Returns(new[]
            {
                new RoleGrant(command.FunctionId, accountRoles[0].Id, PermissionType.Allow)
            }.ToAsyncEnumerable());

        // Act
        var actual = await sut.GetAsync(command);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public async Task CheckAccountHasPermissionCommandService_檢查帳號是否有權限_如果帳號已經Deny_就算角色是Allow也沒有權限()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new CheckAccountHasPermissionCommandService(fakeTgsRepository);

        var command = new CheckAccountHasPermissionCommand(Guid.NewGuid(), Guid.NewGuid());

        _ = fakeTgsRepository.GetFunctionGrantAccountsAsync(Arg.Is(command.FunctionId))
            .Returns(new[]
            {
                new AccountGrant(command.FunctionId, command.AccountId, PermissionType.Deny)
            }.ToAsyncEnumerable());

        var accountRoles = new[]
        {
            new Role(Guid.NewGuid(), "test1", "test")
        };

        _ = fakeTgsRepository.GetAccountAllRolesAsync(Arg.Is(command.AccountId), Arg.Is(true))
            .Returns(accountRoles.ToAsyncEnumerable());

        _ = fakeTgsRepository.GetFunctionGrantRolesAsync(Arg.Is(command.FunctionId))
            .Returns(new[]
            {
                new RoleGrant(command.FunctionId, accountRoles[0].Id, PermissionType.Allow)
            }.ToAsyncEnumerable());

        // Act
        var actual = await sut.GetAsync(command);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public async Task CheckAccountHasPermissionCommandService_檢查帳號是否有權限_如果帳號設定Allow_算角色Deny也是有權限()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new CheckAccountHasPermissionCommandService(fakeTgsRepository);

        var command = new CheckAccountHasPermissionCommand(Guid.NewGuid(), Guid.NewGuid());

        _ = fakeTgsRepository.GetFunctionGrantAccountsAsync(Arg.Is(command.FunctionId))
            .Returns(new[]
            {
                new AccountGrant(command.FunctionId, command.AccountId, PermissionType.Allow)
            }.ToAsyncEnumerable());

        var accountRoles = new[]
        {
            new Role(Guid.NewGuid(), "test1", "test")
        };

        _ = fakeTgsRepository.GetAccountAllRolesAsync(Arg.Is(command.AccountId), Arg.Is(true))
            .Returns(accountRoles.ToAsyncEnumerable());

        _ = fakeTgsRepository.GetFunctionGrantRolesAsync(Arg.Is(command.FunctionId))
            .Returns(new[]
            {
                new RoleGrant(command.FunctionId, accountRoles[0].Id, PermissionType.Deny)
            }.ToAsyncEnumerable());

        // Act
        var actual = await sut.GetAsync(command);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public async Task CheckAccountHasPermissionCommandService_檢查帳號是否有權限_角色權限判斷會先以Role所屬層級再以優先順序依序處理()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new CheckAccountHasPermissionCommandService(fakeTgsRepository);

        var command = new CheckAccountHasPermissionCommand(Guid.NewGuid(), Guid.NewGuid());

        var accountRoles = new[]
        {
            new Role(Guid.NewGuid(), "test1", "test", 1, 100),
            new Role(Guid.NewGuid(), "test2", "test", 2, 200),
            new Role(Guid.NewGuid(), "test3", "test", 1, 200)
        };

        _ = fakeTgsRepository.GetAccountAllRolesAsync(Arg.Is(command.AccountId), Arg.Is(true))
            .Returns(accountRoles.ToAsyncEnumerable());

        _ = fakeTgsRepository.GetFunctionGrantRolesAsync(Arg.Is(command.FunctionId))
            .Returns(new[]
            {
                new RoleGrant(command.FunctionId, accountRoles[0].Id, PermissionType.Deny),
                new RoleGrant(command.FunctionId, accountRoles[1].Id, PermissionType.Deny),
                new RoleGrant(command.FunctionId, accountRoles[2].Id, PermissionType.Allow)
            }.ToAsyncEnumerable());

        // Act
        var actual = await sut.GetAsync(command);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public async Task CheckAccountHasPermissionCommandService_檢查帳號是否有權限_角色權限判斷同層級會以優先權越大越優先()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new CheckAccountHasPermissionCommandService(fakeTgsRepository);

        var command = new CheckAccountHasPermissionCommand(Guid.NewGuid(), Guid.NewGuid());

        var accountRoles = new[]
        {
            new Role(Guid.NewGuid(), "test1", "test", 1, 100),
            new Role(Guid.NewGuid(), "test3", "test", 1, 200)
        };

        _ = fakeTgsRepository.GetAccountAllRolesAsync(Arg.Is(command.AccountId), Arg.Is(true))
            .Returns(accountRoles.ToAsyncEnumerable());

        _ = fakeTgsRepository.GetFunctionGrantRolesAsync(Arg.Is(command.FunctionId))
            .Returns(new[]
            {
                new RoleGrant(command.FunctionId, accountRoles[0].Id, PermissionType.Deny),
                new RoleGrant(command.FunctionId, accountRoles[1].Id, PermissionType.Allow)
            }.ToAsyncEnumerable());

        // Act
        var actual = await sut.GetAsync(command);

        // Assert
        Assert.True(actual);
    }
}
