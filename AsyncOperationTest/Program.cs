using System;
using System.Threading;
using System.Threading.Tasks;

AsyncTest asyncTest = new();

asyncTest.StartAsyncOperations();

while (true)
{
    Console.WriteLine("Press 'a' to cancel, 'b' to restart the async loop.");
    var value = Console.ReadKey();

    if (value.KeyChar == 'a')
    {
        asyncTest.CancelAsyncOperations();
    }

    if (value.KeyChar == 'b')
    {
        asyncTest.RestartAsyncOperations();
    }
}

class AsyncTest
{
    public CancellationTokenSource cts = new();
    private readonly Random rnd = new();

    public void StartAsyncOperations()
    {
        cts = new CancellationTokenSource();
        AsyncOperations(cts.Token);
    }

    public void CancelAsyncOperations()
    {
        if (cts is not null)
        {
            cts.Cancel();
            Console.WriteLine(DateTime.Now + " Async operations cancellation requested.");
        }
    }

    public void RestartAsyncOperations()
    {
        CancelAsyncOperations();
        StartAsyncOperations();
    }

    private async Task AsyncOperations(CancellationToken cancellationToken)
    {
        Task.Run(async() =>
        {

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var terminalReadingTask = AsyncOperation1(cancellationToken);
                    var iff45TsReadingTask = AsyncOperation2(cancellationToken);

                    await Task.WhenAll(terminalReadingTask, iff45TsReadingTask);

                    double error = iff45TsReadingTask.Result - terminalReadingTask.Result;
                    Console.WriteLine($"Error: {error}");
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine(DateTime.Now + " Async operation cancelled!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }


        });
    }

    public async Task<double> AsyncOperation1(CancellationToken cancellationToken)
    {
        Console.WriteLine(DateTime.Now + " Async Operation-1 Started.");
        await Task.Delay(1000, cancellationToken);
        var rndNumber = rnd.NextDouble();
        Console.WriteLine(DateTime.Now + " Async Operation-1 Finished, generated number: " + rndNumber);
        return rndNumber;
    }

    public async Task<double> AsyncOperation2(CancellationToken cancellationToken)
    {
        Console.WriteLine(DateTime.Now + " Async Operation-2 Started.");
        await Task.Delay(3000, cancellationToken);
        var rndNumber = rnd.NextDouble();
        Console.WriteLine(DateTime.Now + " Async Operation-2 Finished, generated number: " + rndNumber);
        return rndNumber;
    }
}