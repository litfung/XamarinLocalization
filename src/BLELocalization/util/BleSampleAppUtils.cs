// Copyright Malachi Griffie
// 
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;

namespace BLELocalization.util
{
   internal static class BleSampleAppUtils
   {
      public const Int32 SCAN_SECONDS_DEFAULT = 240;
      public const Int32 SCAN_SECONDS_MAX = 240;

      public static Double ClampSeconds( Double seconds )
      {
         return Math.Max( Math.Min( seconds, SCAN_SECONDS_MAX ), 0 );
      }
   }
}