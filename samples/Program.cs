using System.Device.Gpio;
using CutilloRigby.Device.STWD100;
using UnitsNet;

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) => cts.Cancel();

var gpioController = new GpioController();

var _stwd100 = new STWD100(gpioController, 15, 18, Duration.FromMilliseconds(500), cts);

while(!cts.IsCancellationRequested)
{
    await Task.Delay(500);
}
