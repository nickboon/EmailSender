using System;
using System.Diagnostics;
using System.Threading;

namespace EmailSender.Integration
{
    static class Retry
    {
        public static void WithExponentialBackOff(Action assertion, int backoff = 300, int maxAttempts = 5)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int attempt = 0; attempt < maxAttempts; attempt++)
                try
                {
                    assertion();
                    return;
                }
                catch (Exception ex)
                {
                    if (attempt == maxAttempts - 1)
                        throw new TimeoutException($"Assertion failed in {maxAttempts} attempts in {stopwatch.Elapsed} ms", ex);

                    Thread.Sleep(TimeSpan.FromMilliseconds(backoff * Math.Pow(2, attempt)));
                }
        }
    }
}

