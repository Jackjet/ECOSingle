using System;
using System.Collections.Generic;
namespace EcoDevice.AccessAPI
{
	public interface SystemThreadPool<Bean, Result>
	{
		System.Collections.Generic.List<Result> GetResults(HandleThread handler);
	}
}
