using System;
using System.Windows;
using System.Windows.Controls;
using TinifyAPI;

namespace TinyImage.ViewModels
{
    /// <summary>
    /// Logic for APIValidation.xaml.cs
    /// </summary>
    public class APIValidation
    {
        public async void validateAPIKey(Label errLabel,TextBox apiTextBox)
        {
            try
            {
                errLabel.Content = string.Empty;

                Tinify.Key = apiTextBox.Text;
                await Tinify.Validate();

                errLabel.Content = "The Key is Valid";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "An Error has occured");
            }
        }
    }
}
