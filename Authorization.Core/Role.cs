namespace Tgs.Authorization;
public record Role(
	Guid Id,
	string Name,
	string System,
	int Level = 1,
	int Priority = 1);
