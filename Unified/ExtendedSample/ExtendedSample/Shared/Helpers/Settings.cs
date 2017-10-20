// Copyright 2016 Scandit AG
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

// Helpers/Settings.cs
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Scandit.BarcodePicker.Unified;
using Scandit.BarcodePicker.Unified.Abstractions;

namespace ExtendedSample.Helpers
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters.
	/// </summary>
	public static class Settings
	{
		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

		private static IBarcodePicker picker = ScanditService.BarcodePicker;
		private static ScanSettings scanSettings = picker.GetDefaultScanSettings();

		public const string SymbologyPrefix = "Sym_";
		public const string InvSymbologyPrefix = "Inv_Sym_";

		// Checksum
		public const string MsiPlesseyChecksumString = "MsiPlesseyChecksum";
		public const string MsiPlesseyChecksumString_None = "None";
		public const string MsiPlesseyChecksumString_Mod10 = "Mod 10";
		public const string MsiPlesseyChecksumString_Mod11 = "Mod 11";
		public const string MsiPlesseyChecksumString_Mod1010 = "Mod 1010";
		public const string MsiPlesseyChecksumString_Mod1110 = "Mod 1110";

		// Feedback
		public const string BeepString = "Overlay_BeepEnabled";
		public const string VibrateString = "Overlay_VibrateEnabled";

		// Torch button
		public const string TorchButtonString = "Overlay_TorchButtonVisible";
		public const string TorchButtonXString = "Overlay_TorchButtonX"; // Unused as not supported yet.
		public const string TorchButtonYString = "Overlay_TorchButtonY"; // Unused as not supported yet.

		// Camera button
		public const string CameraButtonString = "Overlay_CameraButton";
		public const string CameraButtonString_Always = "Overlay_CameraButton_Always";
		public const string CameraButtonString_Never = "Overlay_CameraButton_Never";
		public const string CameraButtonString_OnlyTablet = "Overlay_CameraButton_OnlyTablets";
		public const string CameraButtonXString = "Overlay_CameraButtonX"; // Unused as not supported yet.
		public const string CameraButtonYString = "Overlay_CameraButtonY"; // Unused as not supported yet.

		// Hotspot
		public const string RestrictedAreaString = "ScanSettings_RestrictedAreaScanningEnabled";
		public const string HotSpotHeightString = "ScanOverlay_HotSpotHeight";
		public const string HotSpotWidthString = "ScanOverlay_HotSpotWidth";
		public const string HotSpotYString = "ScanOverlay_HotSpotY";

		// ViewFinder
		public const string ViewFinderPortraitWidthString = "Overlay_ViewFinderSizePortrait_Width";
		public const string ViewFinderPortraitHeightString = "Overlay_ViewFinderSizePortrait_Height";
		public const string ViewFinderLandscapeWidthString = "Overlay_ViewFinderSizeLandscape_Width";
		public const string ViewFinderLandscapeHeightString = "Overlay_ViewFinderSizeLandscape_Height";

		public static readonly string[] SliderStrings = {
			ViewFinderPortraitWidthString,
			ViewFinderPortraitHeightString,
			ViewFinderLandscapeWidthString,
			ViewFinderLandscapeHeightString
		};

		public const string GuiStyleString = "Overlay_GuiStyle";
		public const string GuiStyleString_Frame = "Overlay_GuiStyle_Frame";
		public const string GuiStyleString_Laser = "Overlay_GuiStyle_Laser";
		public const string GuiStyleString_None = "Overlay_GuiStyle_None";
		public const string GuiStyleString_LocationsOnly = "Overlay_GuiStyle_LocationsOnly";

		public static bool hasInvertedSymbology(string symbology)
		{
			return (symbology == "Sym_Qr" || symbology == "Sym_DataMatrix");
		}

		public static string getInvertedSymboloby(string symbology)
		{
			if (hasInvertedSymbology(symbology))
			{
				return ("Inv_" + symbology);
			}
			else {
				throw new Exception("has no inversion");
			}
		}

		public static bool getBoolSetting(string setting)
		{
			return AppSettings.GetValueOrDefault(setting, defaultBool(setting));
		}

		public static void setBoolSetting(string setting, bool value)
		{
			AppSettings.AddOrUpdateValue(setting, value);
		}

		private static bool defaultBool(string setting)
		{
			if (Array.IndexOf(Convert.EnabledSettings, setting) >= 0)
			{
				return true;
			} else
			{
				return false;
			}
		}

		public static int getIntSetting(string setting)
		{
			return AppSettings.GetValueOrDefault(setting, defaultInt(setting));
		}

		public static void setIntSetting(string setting, int value)
		{
			AppSettings.AddOrUpdateValue(setting, value);
		}

		private static int defaultInt(string setting)
		{
			return 15;
		}

		public static Double getDoubleSetting(string setting)
		{
			return AppSettings.GetValueOrDefault(setting, defaultDouble(setting));
		}

		public static void setDoubleSetting(string setting, Double value)
		{
			AppSettings.AddOrUpdateValue(setting, value);
		}

		private static Double defaultDouble(string setting)
		{
			switch (setting)
			{
				case HotSpotHeightString:
				return 0.25;
				case HotSpotWidthString:
					return 1.0;
				case HotSpotYString:
					return scanSettings.ScanningHotSpot.Y;
					
				case ViewFinderPortraitWidthString:
					return picker.ScanOverlay.ViewFinderSizePortrait.Width;
				case ViewFinderPortraitHeightString:
					return picker.ScanOverlay.ViewFinderSizePortrait.Height;
				case ViewFinderLandscapeWidthString:
					return picker.ScanOverlay.ViewFinderSizeLandscape.Width;
				case ViewFinderLandscapeHeightString:
					return picker.ScanOverlay.ViewFinderSizePortrait.Height;
					
				default:
					throw (new Exception("No such Double setting: " + setting));
			}
		}

		public static string getStringSetting(string setting)
		{
			return AppSettings.GetValueOrDefault(setting, defaultString(setting));
		}

		public static void setStringSetting(string setting, string value)
		{
			AppSettings.AddOrUpdateValue(setting, value);
		}

		private static string defaultString(string setting)
		{
			switch (setting)
			{
				case MsiPlesseyChecksumString:
					return MsiPlesseyChecksumString_Mod10;
				case CameraButtonString:
					return CameraButtonString_Always;
				case GuiStyleString:
					return GuiStyleString_Frame;
				default:
					throw new Exception("No default setting for " + setting);
			}
		}
	}
}
