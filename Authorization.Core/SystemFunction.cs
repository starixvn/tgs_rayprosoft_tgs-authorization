using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tgs.Authorization;
public record SystemFunction(
	Guid Id,
	string Name,
	string System);
