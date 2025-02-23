using System;
using System.Collections;
using System.Collections.Generic;

namespace Util.Collections;

public class AdaptingEnumerator<X,Y> : IEnumerator<Y>
{
    private readonly IEnumerator<X> Delegate;
    private readonly Func<X,Y> AdaptingFunction;

    public AdaptingEnumerator(IEnumerator<X> @delegate, Func<X,Y> adaptingFunction)
    {
        Delegate         = @delegate;
        AdaptingFunction = adaptingFunction;
    }

    public void Reset()    => Delegate.Reset();
    public bool MoveNext() => Delegate.MoveNext();
    public void Dispose()  => Delegate.Dispose();

    public Y Current => AdaptingFunction(Delegate.Current);

    object? IEnumerator.Current => Current;
}
