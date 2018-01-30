// Copyright Malachi Griffie
// 
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BLELocalization.util;
using nexus.core;
using nexus.core.text;
using nexus.protocols.ble.scan;
using nexus.protocols.ble.scan.advertisement;
using Xamarin.Forms;
using BLELocalization.Abstractions;

namespace BLELocalization.viewmodel
{
   public class BlePeripheralViewModel
      : BaseViewModel,
        IEquatable<IBlePeripheral>
   {

        //public IBlePeripheral Model { get; private set; }
        public BleDevice Device;
        public BlePeripheralViewModel( BleDevice device/*IBlePeripheral model*/ )
      {
         Device = device;
         
      }

      public String Address =>  Device.abs.Address.Select(b => b.EncodeToBase16String()).Join(":");
      public String DeviceName => Device.abs.Advertisement.DeviceName;
      public String Name => Device.abs.Advertisement.DeviceName ?? Address;
      public Int32 Rssi => Device.abs.Rssi;
      public Double RssiAvg => Device.rssiList.Average();

      public override Boolean Equals( Object other )
      {
         return Device.abs.Equals( other );
      }

      public Boolean Equals( IBlePeripheral other )
      {
         return Device.abs.Equals( other );
      }

      public override Int32 GetHashCode()
      {
         // ReSharper disable once NonReadonlyMemberInGetHashCode
         return Device.abs.GetHashCode();
      }

      public void Update( IBlePeripheral model )
      {
            //if(!Equals(Device.abs, model ))
            //{
            //       Device.abs = model;
            //}


         Device.rssiList[0] = model.Rssi;
         Device.UpdateRssi();
         RaisePropertyChanged( nameof(RssiAvg) );
         RaisePropertyChanged( nameof(Address) );
         RaisePropertyChanged( nameof(DeviceName) );
         RaisePropertyChanged( nameof(Device.abs) );
         RaisePropertyChanged( nameof(Name) );
         RaisePropertyChanged( nameof(Rssi) );
      }
   }
}