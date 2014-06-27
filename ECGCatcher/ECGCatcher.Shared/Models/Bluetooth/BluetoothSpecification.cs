using System;
using System.Collections.Generic;
using System.Text;

namespace ECGCatcher.Models.Bluetooth
{
    public class BluetoothSpecification
    {
        // The Server's custom service Uuid: 34B1CF4D-1069-4AD6-89B6-E161D79BE4D8
        public static readonly Guid RfcommServiceUuid = Guid.Parse("34B1CF4D-1069-4AD6-89B6-E161D79BE4D8");

        // The custom data character which separate specifided provided data from bluetooth connection
        public static readonly Char CustomDataSeparator = ':';
    }
}
