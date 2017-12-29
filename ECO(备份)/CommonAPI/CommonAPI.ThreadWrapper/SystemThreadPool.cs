using System;
using System.Collections.Generic;
namespace CommonAPI.ThreadWrapper
{
	public interface SystemThreadPool<Bean, Result>
	{
		List<Result> GetResults(HandleThread handler);
	}
}
