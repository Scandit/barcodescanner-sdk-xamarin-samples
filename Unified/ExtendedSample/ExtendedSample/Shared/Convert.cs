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

using System;
using System.Collections.Generic;
using ExtendedSample.Helpers;
using Scandit.BarcodePicker.Unified;

namespace ExtendedSample
{
    public class Convert
    {
        public Convert()
        {
        }

        // Associates the key (string) for permanent storage with the list of symbologies in ScanSettings
        public static IReadOnlyDictionary<string, Symbology[]> settingToSymbologies = new Dictionary<string, Symbology[]> {
            { "Sym_Ean13Upca", new Symbology[] { Symbology.Ean13, Symbology.Upca } },
            { "Sym_Ean8", new Symbology[] { Symbology.Ean8 } },
            { "Sym_Upce", new Symbology[] { Symbology.Upce } },
            { "Sym_TwoDigitAddOn", new Symbology[] { Symbology.TwoDigitAddOn } },
            { "Sym_FiveDigitAddOn", new Symbology[] { Symbology.FiveDigitAddOn } },
            { "Sym_Code11", new Symbology[] { Symbology.Code11 } },
            { "Sym_Code25", new Symbology[] { Symbology.Code25 } },
            { "Sym_Code39", new Symbology[] { Symbology.Code39 } },
            { "Sym_Code93", new Symbology[] { Symbology.Code93 } },
            { "Sym_Code128", new Symbology[] { Symbology.Code128 } },
            { "Sym_Interleaved2Of5", new Symbology[] { Symbology.Interleaved2Of5 } },
            { "Sym_MsiPlessey", new Symbology[] { Symbology.MsiPlessey } },
            { "Sym_Gs1Databar", new Symbology[] { Symbology.Gs1Databar } },
            { "Sym_Gs1DatabarExpanded", new Symbology[] { Symbology.Gs1DatabarExpanded } },
            { "Sym_Gs1DatabarLimited", new Symbology[] { Symbology.Gs1DatabarLimited } },
            { "Sym_Codabar", new Symbology[] { Symbology.Codabar } },
            { "Sym_Qr", new Symbology[] { Symbology.Qr } },
            { "Sym_DataMatrix", new Symbology[] { Symbology.DataMatrix } },
            { Settings.DpmModeString, null },
            { "Sym_Pdf417", new Symbology[] { Symbology.Pdf417 } },
            { "Sym_MicroPdf417", new Symbology[] { Symbology.MicroPdf417 } },
            { "Sym_Aztec", new Symbology[] { Symbology.Aztec } },
            { "Sym_MaxiCode", new Symbology[] { Symbology.MaxiCode } },
            { "Sym_Rm4scc", new Symbology[] { Symbology.Rm4scc } },
            { "Sym_Kix", new Symbology[] { Symbology.Kix } },
            { "Sym_DotCode", new Symbology[] { Symbology.DotCode } },
            { "Sym_MicroQR", new Symbology[] { Symbology.MicroQr } },
        };

        // Associates the key (string) for permanent storage with the text displayed in the GUI
        public static IReadOnlyDictionary<string, string> settingToDisplay = new Dictionary<string, string> {
            { "Sym_Ean13Upca", "EAN-13 & UPC-A" },
            { "Sym_Ean8", "EAN-8" },
            { "Sym_Upce", "UPC-E" },
            { "Sym_TwoDigitAddOn", "Two-Digit-Add-On" },
            { "Sym_FiveDigitAddOn", "Five-Digit-Add-On" },
            { "Sym_Code11", "Code 11" },
            { "Sym_Code25", "Code 25" },
            { "Sym_Code39", "Code 39" },
            { "Sym_Code93", "Code 93" },
            { "Sym_Code128", "Code 128" },
            { "Sym_Interleaved2Of5", "Interleaved-Two-of-Five (ITF)" },
            { "Sym_MsiPlessey", "MSI Plessey" },
            { "Sym_Gs1Databar", "GS1 DataBar" },
            { "Sym_Gs1DatabarExpanded", "GS1 DataBar Expanded" },
            { "Sym_Gs1DatabarLimited", "GS1 DataBar Limited" },
            { "Sym_Codabar", "Codabar" },
            { "Sym_Qr", "QR Code" },
            { "Inv_Sym_Qr", "Color-inverted QR Code" },
            { "Sym_DataMatrix", "Data Matrix" },
            { "Inv_Sym_DataMatrix", "Color-Inverted Data Matrix" },
            { Settings.DpmModeString, "DPM Mode" },
            { "Sym_Pdf417", "PDF417" },
            { "Sym_MicroPdf417", "MicroPDF417" },
            { "Sym_Aztec", "Aztec Code" },
            { "Sym_MaxiCode", "MaxiCode" },
            { "Sym_Rm4scc", "RM4SCC" },
            { "Sym_Kix", "KIX" },
            { "Sym_DotCode", "DotCode" },
            { "Sym_MicroQR", "MicroQr" },
        };

        // Associates an index with the storage key (string)
        public static Dictionary<int, string> indexToMsiPlesseyChecksum = new Dictionary<int, string> {
            { 0, Settings.MsiPlesseyChecksumString_None },
            { 1, Settings.MsiPlesseyChecksumString_Mod10 },
            { 2, Settings.MsiPlesseyChecksumString_Mod11 },
            { 3, Settings.MsiPlesseyChecksumString_Mod1010 },
            { 4, Settings.MsiPlesseyChecksumString_Mod1110 }
        };

        // Associates the storage key (string) with an index
        public static Dictionary<string, int> msiPlesseyChecksumToIndex = new Dictionary<string, int> {
            { Settings.MsiPlesseyChecksumString_None, 0 },
            { Settings.MsiPlesseyChecksumString_Mod10, 1 },
            { Settings.MsiPlesseyChecksumString_Mod11, 2 },
            { Settings.MsiPlesseyChecksumString_Mod1010, 3 },
            { Settings.MsiPlesseyChecksumString_Mod1110, 4 }
        };

        // Associates an index with the enum in the picker
        public static Dictionary<string, Checksum> msiPlesseyChecksumToScanSetting = new Dictionary<string, Checksum> {
            { Settings.MsiPlesseyChecksumString_None, Checksum.None },
            { Settings.MsiPlesseyChecksumString_Mod10, Checksum.Mod10 },
            { Settings.MsiPlesseyChecksumString_Mod11, Checksum.Mod11 },
            { Settings.MsiPlesseyChecksumString_Mod1010, Checksum.Mod1010 },
            { Settings.MsiPlesseyChecksumString_Mod1110, Checksum.Mod1110 }
        };

        // Associates the index in the picker with the storage key (string)
        public static Dictionary<int, string> indexToCameraButton = new Dictionary<int, string> {
            { 0, Settings.CameraButtonString_Always },
            { 1, Settings.CameraButtonString_Never },
            { 2, Settings.CameraButtonString_OnlyTablet }
        };

        // Associates the storage key (string) with the index in the picker
        public static Dictionary<string, int> cameraButtonToIndex = new Dictionary<string, int>
        {
            { Settings.CameraButtonString_Always, 0 },
            { Settings.CameraButtonString_Never, 1 },
            { Settings.CameraButtonString_OnlyTablet, 2 }
        };

        // Associates the storage key (string) with ScanSettings enum
        public static Dictionary<string, CameraSwitchVisibility> cameraToScanSetting = new Dictionary<string, CameraSwitchVisibility>
        {
            { Settings.CameraButtonString_Always, CameraSwitchVisibility.Always },
            { Settings.CameraButtonString_Never, CameraSwitchVisibility.Never },
            { Settings.CameraButtonString_OnlyTablet, CameraSwitchVisibility.OnTablets }
        };

        // Associates the index in the picker with the storage key (string)
        public static Dictionary<int, string> indexToGuiStyle = new Dictionary<int, string> {
            { 0, Settings.GuiStyleString_Frame },
            { 1, Settings.GuiStyleString_Laser },
            { 2, Settings.GuiStyleString_None },
            { 3, Settings.GuiStyleString_LocationsOnly }
        };

        // Associates the storage key (string) with the index in the picker
        public static Dictionary<string, int> guiStyleToIndex = new Dictionary<string, int>
        {
            { Settings.GuiStyleString_Frame, 0 },
            { Settings.GuiStyleString_Laser, 1 },
            { Settings.GuiStyleString_None, 2 },
            { Settings.GuiStyleString_LocationsOnly, 3 }
        };

        // Associates the storage key (string) with ScanSettings enum
        public static Dictionary<string, GuiStyle> guiStyleToScanSetting = new Dictionary<string, GuiStyle>
        {
            { Settings.GuiStyleString_Frame, GuiStyle.Default },
            { Settings.GuiStyleString_Laser, GuiStyle.Laser },
            { Settings.GuiStyleString_None, GuiStyle.None },
            { Settings.GuiStyleString_LocationsOnly, GuiStyle.LocationsOnly }
        };

        // Associates the index in the picker with the storage key (string)
        public static Dictionary<int, string> indexToResolution = new Dictionary<int, string> {
            { 0, Settings.ResolutionString_HD },
            { 1, Settings.ResolutionString_FullHD }
        };

        // Associates the storage key (string) with the index in the picker
        public static Dictionary<string, int> resolutionToIndex = new Dictionary<string, int>
        {
            { Settings.ResolutionString_HD, 0 },
            { Settings.ResolutionString_FullHD, 1 }
        };

        // Associates the storage key (string) with ScanSettings enum
        public static Dictionary<string, VideoResolution> resolutionToScanSetting = new Dictionary<string, VideoResolution>
        {
            { Settings.ResolutionString_HD, VideoResolution.Medium},
            { Settings.ResolutionString_FullHD, VideoResolution.High }
        };

        // List of Settings that are enabled by default
        public static readonly string[] EnabledSettings = {
            "Sym_Ean13Upca", "Sym_Ean13Upca", "Sym_Ean8", "Sym_Upce", "Sym_Code39", "Sym_Code128",
            "Sym_Interleaved2Of5", "Sym_Qr", "Sym_DataMatrix", Settings.BeepString,
            Settings.TorchButtonString
        };
    }
}

