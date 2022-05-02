using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public static partial class TaskUtility
{
	#region public-method
	public static CancellationToken RefreshToken(ref CancellationTokenSource token) 
	{
		token?.Cancel();
		token?.Dispose();
		token = new CancellationTokenSource();
		return token.Token;
	}

	public static void CancelToken(ref CancellationTokenSource token)
	{
		token?.Cancel();
		token?.Dispose();
		token = null;
	}
	#endregion public-method
}
