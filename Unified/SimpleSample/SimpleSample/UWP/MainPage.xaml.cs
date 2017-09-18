using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace SimpleSample.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            // required if you are scanning codes with more "exotic" encodings such as Kanji. 
            // Without this call, the data can't be converted to UTF-8.
            this.SetupAdditionalEncodingProviders();
            LoadApplication(new SimpleSample.App());
        }


        // setup additional encoding providers using reflection. In your own application, it's typically sufficient to just 
        // call Encoding.RegisterProvider(CodePagesEncodingProvider.Instance). We use reflection to make this also work with 
        // older .NET versions that don't yet have the functionality.
        private void SetupAdditionalEncodingProviders()
        {
            var encodingType = Type.GetType("System.Text.Encoding, Windows, ContentType=WindowsRuntime");
            var codePagesEncodingProviderType = Type.GetType("System.Text.CodePagesEncodingProvider, Windows, ContentType=WindowsRuntime");
            if (encodingType == null || codePagesEncodingProviderType == null)
            {
                return;
            }
            var registerProviderMethod = encodingType.GetRuntimeMethod("RegisterProvider", new Type[] { codePagesEncodingProviderType });
            var instanceProperty = codePagesEncodingProviderType.GetRuntimeProperty("Instance");
            if (registerProviderMethod == null || instanceProperty == null)
            {
                return;
            }
            try
            {
                var theInstance = instanceProperty.GetValue(null);
                if (theInstance == null)
                {
                    return;
                }
                registerProviderMethod.Invoke(null, new object[] { theInstance });
            }
            catch (TargetInvocationException)
            {
                return;
            }
        }
    }
}
