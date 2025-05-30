using System;

namespace Analyzers1.Sample;

public class Spaceship
{
    public void SetSpeed(long speed)
    {
        if (speed > 400_000_000)
            throw new ArgumentOutOfRangeException(nameof(speed));
    }
}