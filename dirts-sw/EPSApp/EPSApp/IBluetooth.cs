using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPSApp
{
    public interface IBluetooth
    {
        bool getConnectedStatus();

        List<String> getAverageMeasurement();

        bool checkPairing();
        Task GetPlatformName();
    }
}
