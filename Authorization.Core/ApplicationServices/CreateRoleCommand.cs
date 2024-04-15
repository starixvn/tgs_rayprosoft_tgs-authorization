using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tgs.Authorization.ApplicationServices;
public record CreateRoleCommand(
	string System,
	string RoleName);
