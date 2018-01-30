using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLELocalization.util;
using nexus.core;
using nexus.core.text;
using nexus.protocols.ble.scan;
using nexus.protocols.ble.scan.advertisement;
using BLELocalization;

namespace BLELocalization.Abstractions
{
    public class BleDevice : IEquatable<BleDevice>
    {


        public int[] rssiList = new int[9];
        public int[] Location = new int[2];
        public double Distance;
        public IBlePeripheral abs;


        public BleDevice() { 

        }

        public BleDevice(IBlePeripheral abs)
        {

            this.abs = abs;

        }


        public void UpdateRssi()
        {
            ShiftRight(this.rssiList, 1);
        }

        public static void ShiftRight<T>(T[] arr, int shifts)
        {
            Array.Copy(arr, 0, arr, shifts, arr.Length - shifts);
            Array.Clear(arr, 0, shifts);
        }


        public bool Equals(IBlePeripheral other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(BleDevice other)
        {
            throw new NotImplementedException();
        }
    }
}
