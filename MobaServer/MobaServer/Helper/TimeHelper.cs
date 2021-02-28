using System;
using System.Collections;
using System.Collections.Generic;

public static partial class TimeHelper
{
	#region private-field
	private static readonly long _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
	#endregion private-field

	#region public-method
	public static long Now
		{
			get
			{
				return (DateTime.UtcNow.Ticks - _epoch) / 10000;
			}
		}

	public static long NowSeconds
		{
			get
			{
				return (DateTime.UtcNow.Ticks - _epoch) / 10000000;
			}
		}
	#endregion public-method
}
