using System.Device.Gpio;
using UnitsNet;

namespace CutilloRigby.Device.STWD100;

public sealed class STWD100
{
    private readonly GpioController _gpioController;
    private readonly int _wdiPin;
    private readonly int _wdoPin;
    private readonly TimeSpan _pollInterval;
    private readonly Timer _timer;
    private readonly CancellationTokenSource _timeoutToken;

    public STWD100(GpioController gpioController, int wdiPin, int wdoPin, 
        Duration pollInterval, CancellationTokenSource timeoutToken, DeviceVersion deviceVersion = DeviceVersion.Y)
    {
        _gpioController = gpioController ?? throw new ArgumentNullException(nameof(gpioController));
        _wdiPin = wdiPin;
        _wdoPin = wdoPin;
        _pollInterval = pollInterval.ToTimeSpan();
        _timeoutToken = timeoutToken;

        TWD = deviceVersion.GetTWD();
        TPW = deviceVersion.GetTPW();

        _gpioController.OpenPin(_wdiPin, PinMode.Output, PinValue.Low);
        _gpioController.OpenPin(_wdoPin, PinMode.InputPullUp, PinValue.High);
        _gpioController.RegisterCallbackForPinValueChangedEvent(_wdoPin, PinEventTypes.Falling, (s, e) => timeoutToken.Cancel());

        _timer = new Timer(async (state) => 
        {
            if (AssertInput)
                _gpioController.Write(wdiPin, PinValue.High);
            await Task.Delay(1);
            _gpioController.Write(wdiPin, PinValue.Low);
        }, null, _pollInterval, _pollInterval);
    }

    public bool AssertInput { get; set; } = true;

    public TimeSpan PollInterval => _pollInterval;

    public Duration TWD { get; }
    public Duration TPW { get; }
}
