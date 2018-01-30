﻿// Copyright Malachi Griffie
// 
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Reflection;
using Acr.UserDialogs;
using BLELocalization.view;
using BLELocalization.viewmodel;
using nexus.core.logging;
using nexus.protocols.ble;
using Xamarin.Forms;

#if RELEASE
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;
#endif

namespace BLELocalization
{
   public partial class FormsApp
   {
      private readonly IUserDialogs m_dialogs;
      private readonly NavigationPage m_rootPage;

      public FormsApp( IBluetoothLowEnergyAdapter adapter, IUserDialogs dialogs )
      {
         InitializeComponent();

         m_dialogs = dialogs;
         var logsVm = new LogsViewModel();
         SystemLog.Instance.AddSink( logsVm );

         var bleAssembly = adapter.GetType().GetTypeInfo().Assembly.GetName();
         Log.Info( bleAssembly.Name + "@" + bleAssembly.Version );


         var bleScanViewModel = new BleDeviceScannerViewModel(
            bleAdapter: adapter,
            dialogs: dialogs);

         m_rootPage = new NavigationPage(
            new TabbedPage
            {
               Title = "BLE.net Sample App",
               Children = {new BleDeviceScannerPage( bleScanViewModel ), new LogsPage( logsVm )}
            } );

         MainPage = m_rootPage;
      }

      /// <inheritdoc />
      protected override void OnStart()
      {
         base.OnStart();
#if RELEASE
         MobileCenter.Start(
            "ios=6b6689d5-0d94-476a-a632-81145dde8706;android=56864a4d-0dc3-4ab8-819b-bb5d412ba595",
            typeof(Analytics),
            typeof(Crashes) );
#endif
         if(Device.RuntimePlatform == Device.Windows)
         {
            Device.StartTimer(
               TimeSpan.FromSeconds( 3 ),
               () =>
               {
                  m_dialogs.Alert(
                     "The UWP API can listen for advertisements but is not yet able to connect to devices.",
                     "Quick Note",
                     "Aww, ok" );
                  return false;
               } );
         }
      }
   }
}