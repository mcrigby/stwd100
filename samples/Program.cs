using System.Device.Gpio;
using CutilloRigby.Device.STWD100;
using UnitsNet;

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) => cts.Cancel();

var gpioController = new GpioController();
var watchdogTimeout = new CancellationTokenSource();

Console.WriteLine("Instancing Watchdog Timer. WDI 15, WDO 18");
var _stwd100 = new STWD100(gpioController, 15, 18, Duration.FromMilliseconds(500), watchdogTimeout)
{
    Log = s => Console.WriteLine(s)
};

var counter = 0;

while (!cts.IsCancellationRequested)
{
    Console.WriteLine("Waiting...");
    await Task.Delay(500);
    
    counter++;
    if (counter > 10)
        _stwd100.AssertInput = false;
        
    if (watchdogTimeout.IsCancellationRequested)
    {
        Console.WriteLine("Watchdog Timed Out");
        cts.Cancel();
    }
}
