// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Diagnostics.Metrics;



AsyncTest asyncTest = new();

asyncTest.AsyncOperations();


while (true)
{
    Console.WriteLine("Enter a value to finish async loop");
    var value = Console.ReadKey();

    if (value.KeyChar == 'a')
    {
        if (asyncTest.cts is not null)
        {
            asyncTest.cts.Cancel();
        }
    }

    if (value.KeyChar == 'b')
    {
        asyncTest.cts = new CancellationTokenSource();
        asyncTest.AsyncOperations();
    }


}

class AsyncTest
{
    public CancellationTokenSource cts = new();

    public async Task AsyncOperations()
    {

        try
        {
            Task.Run(async() =>
            {

                while (!cts.Token.IsCancellationRequested)
                {

                    var TerminalReadingTask = AsyncOperation1();

                    var IFF45TsReadingTask = AsyncOperation2();

                    await Task.WhenAll(TerminalReadingTask, IFF45TsReadingTask);

                    double error = IFF45TsReadingTask.Result - TerminalReadingTask.Result;

                    Console.WriteLine($"Error: {error}" );
                }


            } ,cts.Token);


        }

        catch (Exception ex)
        {


        }

    }


    public async Task<double> AsyncOperation1()
    {

        Random rnd = new();
        Console.WriteLine(DateTime.Now + "  Async Operation-1 Started !!!");
        await Task.Delay(1000);

        var rndNumber = rnd.NextDouble();
        Console.WriteLine(DateTime.Now + "  Async Operation-1 Finished generated number: "  + rndNumber);
        return rndNumber;
    }

    public async Task<double> AsyncOperation2()
    {
        Random rnd = new();
        Console.WriteLine(DateTime.Now + "  Async Operation-2 Started !!!");
        await Task.Delay(3000);

        var rndNumber = rnd.NextDouble();
        Console.WriteLine(DateTime.Now + "  Async Operation-2 Finished generated number: " + rndNumber);
        return rndNumber;
    }

}