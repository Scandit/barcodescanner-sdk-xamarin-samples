using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TextRecognitionSample
{
    public partial class SettingsPage : ContentPage
    {
        private Settings settings;

        public SettingsPage()
        {
            InitializeComponent();
        }

        public SettingsPage(Settings settings)
        {
            this.settings = settings;

            InitializeComponent();

            setupTableView();
        }

        private void setupTableView()
        {
            ModePicker.SelectedIndex = (int)settings.Mode;
            ScanPositionPicker.SelectedIndex = (int)settings.ScanPosition;
        }

        public void UpdateSettings()
        {
            settings.Mode = (Mode)ModePicker.SelectedIndex;
            settings.ScanPosition = (ScanPosition)ScanPositionPicker.SelectedIndex;
        }
    }
}
