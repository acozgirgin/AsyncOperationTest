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
    public CancellationTokenSource cts = new CancellationTokenSource();

    public async Task AsyncOperations()
    {

        try
        {
            Task.Run(async () =>
            {

                while (!cts.Token.IsCancellationRequested)
                {

                    var TerminalReadingTask = AsyncOperation1();

                    var IFF45TsReadingTask = AsyncOperation2();

                    await Task.WhenAll(TerminalReadingTask, IFF45TsReadingTask);

                }


            } ,cts.Token);


        }

        catch (Exception ex)
        {


        }

    }


    public async Task<string> AsyncOperation1()
    {
        Console.WriteLine(DateTime.Now + "  Async Operation-1 Started !!!");
        await Task.Delay(1000);
        Console.WriteLine(DateTime.Now + "  Async Operation-1 Finished");
        return "Async Operation 1 Result : AAAAAAAAAAAA";
    }

    public async Task<string> AsyncOperation2()
    {
        Console.WriteLine(DateTime.Now + "  Async Operation-2 Started !!!");
        await Task.Delay(3000);
        Console.WriteLine(DateTime.Now + "  Async Operation-2 Finished");
        return "Async Operation 2 Result : BBBBBBBBBBBBB";
    }

}