using UnitsNet;

namespace CutilloRigby.Device.STWD100;

public static class Extensions
{
    internal static Duration GetTWD(this DeviceVersion version)  
    {
        return new Duration(version switch
        {
            DeviceVersion.P => 3.4,
            DeviceVersion.W => 6.3,
            DeviceVersion.X => 102,
            DeviceVersion.Y => 1600,
            _ => throw new ArgumentException("Unknown DeviceVersion", nameof(version)),
        }, UnitsNet.Units.DurationUnit.Millisecond);
    }

    internal static Duration GetTPW(this DeviceVersion version)  
    {
        return new Duration(version switch
        {
            DeviceVersion.P => 3.4,
            DeviceVersion.W => 210,
            DeviceVersion.X => 210,
            DeviceVersion.Y => 210,
            _ => throw new ArgumentException("Unknown DeviceVersion", nameof(version)),
        }, UnitsNet.Units.DurationUnit.Millisecond);
    }
}