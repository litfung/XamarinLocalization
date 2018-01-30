// Copyright Malachi Griffie
// 
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using BLELocalization.util;
using nexus.core.logging;
using nexus.protocols.ble;
using nexus.protocols.ble.scan;
using Xamarin.Forms;
using BLELocalization.Abstractions;
using nexus.core.text;
using nexus.core;

namespace BLELocalization.viewmodel
{
   public class BleDeviceScannerViewModel : AbstractScanViewModel
   {
   
      private DateTime m_scanStopTime;

      public BleDeviceScannerViewModel( IBluetoothLowEnergyAdapter bleAdapter, IUserDialogs dialogs)
         : base( bleAdapter, dialogs )
      {
        
         FoundDevices = new ObservableCollection<BlePeripheralViewModel>();
         ScanForDevicesCommand =
            new Command( x => { StartScan( x as Double? ?? BleSampleAppUtils.SCAN_SECONDS_DEFAULT ); } );
      }

      public ObservableCollection<BlePeripheralViewModel> FoundDevices { get; }

      public ICommand ScanForDevicesCommand { get; }

      public Int32 ScanTimeRemaining =>
         (Int32)BleSampleAppUtils.ClampSeconds( (m_scanStopTime - DateTime.UtcNow).TotalSeconds );

     public void setupScan()
        {
            //FoundDevices.Add(new BlePeripheralViewModel());
        }

      private async void StartScan( Double seconds )
      {
         if(IsScanning)
         {
            return;
         }

         if(!IsAdapterEnabled)
         {
            m_dialogs.Toast( "Cannot start scan, Bluetooth is turned off" );
            return;
         }

         StopScan();
         IsScanning = true;
         seconds = BleSampleAppUtils.ClampSeconds( seconds );
         m_scanCancel = new CancellationTokenSource( TimeSpan.FromSeconds( seconds ) );
         m_scanStopTime = DateTime.UtcNow.AddSeconds( seconds );

         Log.Trace( "Beginning device scan. timeout={0} seconds", seconds );

         RaisePropertyChanged( nameof(ScanTimeRemaining) );
         
         Device.StartTimer(
            TimeSpan.FromSeconds( 1 ),
            () =>
            {
               RaisePropertyChanged( nameof(ScanTimeRemaining) );
               return IsScanning;
            } );

            await m_bleAdapter.ScanForBroadcasts(
           

            new ScanFilter().AddAdvertisedService("0000fe9a-0000-1000-8000-00805f9b34fb"),

            peripheral =>
            {
                Device.BeginInvokeOnMainThread(
                   () =>
                   {
                       var temp = new BleDevice(peripheral);
                       var existing = FoundDevices.FirstOrDefault(d => d.Address == peripheral.Address.Select(b => b.EncodeToBase16String()).Join(":"));
                       if (existing != null)
                       {
                           existing.Update(temp.abs);
                       }
                       else
                       {
                           
                           FoundDevices.Add(new BlePeripheralViewModel(temp));
                       }
                   });




            },
            m_scanCancel.Token );

         IsScanning = false;
      }
   }
}