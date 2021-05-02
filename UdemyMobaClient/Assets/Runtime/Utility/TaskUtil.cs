using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public static class TaskUtil
{
    #region public-method
    public static CancellationToken RefreshToken(ref CancellationTokenSource tokenSource)
    {
        tokenSource?.Cancel();
        tokenSource?.Dispose();
        tokenSource = new CancellationTokenSource();
        return tokenSource.Token;
    }

    public static void CancelToken(ref CancellationTokenSource tokenSource)
    {
        tokenSource?.Cancel();
        tokenSource?.Dispose();
        tokenSource = null;
    }
    #endregion public-method
}