using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataFormCustomTextEditor;
using DataFormCustomTextEditor.UWP;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace DataFormCustomTextEditor.UWP
{
    public class CustomEntryRenderer : EntryRenderer
    {
        internal TextBox NativeTextBox { get; set; }
        internal string PlaceholderText { get; set; }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null && Element is CustomEntry)
            {
                PlaceholderText = Control.PlaceholderText;
                Control.Foreground = new SolidColorBrush(Colors.White);
                Control.TextWrapping = TextWrapping.Wrap;
                Control.FontSize = 12;
                Control.Style = (Windows.UI.Xaml.Style)Windows.UI.Xaml.Application.Current.Resources["CustomTextBoxStyle"];
                Control.PointerEntered += Control_PointerEntered;
                NativeTextBox = Control as TextBox;
                this.Control.PointerExited += Control_PointerExited;

                NativeTextBox.GotFocus += NativeTextBox_GotFocus;
                NativeTextBox.LostFocus += NativeTextBox_LostFocus;

                var color = Element.PlaceholderColor;
                var windowsColor = Windows.UI.Color.FromArgb((byte)(color.A * 255), (byte)(color.R * 255), (byte)(color.G * 255), (byte)(color.B * 255));
            
                windowsColor = Windows.UI.Color.FromArgb((byte)(color.A * 255), (byte)(color.R * 255), (byte)(color.G * 255), (byte)(color.B * 255));
                NativeTextBox.BorderBrush = new SolidColorBrush(windowsColor);

                if (!string.IsNullOrEmpty(NativeTextBox.Text))
                    NativeTextBox.Header = NativeTextBox.PlaceholderText;
            }
        }

        private void Control_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            this.Control.Foreground = new SolidColorBrush(Colors.White);
        }

        private void Control_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            this.Control.Foreground = new SolidColorBrush(Colors.Yellow);
        }

        private void NativeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            NativeTextBox.PlaceholderText = PlaceholderText;
            if (string.IsNullOrEmpty(NativeTextBox.Text))
                NativeTextBox.Header = string.Empty;
        }

        private void NativeTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var color = Element.PlaceholderColor;
            var windowsColor = Windows.UI.Color.FromArgb((byte)(color.A * 255), (byte)(color.R * 255), (byte)(color.G * 255), (byte)(color.B * 255));
            NativeTextBox.Header = new TextBlock() { Text = Element.Placeholder, Foreground = new SolidColorBrush(windowsColor) };
            NativeTextBox.PlaceholderText = string.Empty;

            if (string.IsNullOrEmpty(NativeTextBox.Text))
                NativeTextBox.Text = string.Empty;
        }
    }
}