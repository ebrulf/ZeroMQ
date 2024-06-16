//using Com.Google.Android.Exoplayer2.Source;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Storage;
namespace GUI
{
    public partial class MainPage : ContentPage
    {
        //int count = 0;
        public MainPage()
        {
            InitializeComponent();

        }

        //private void OnCounterClicked(object sender, EventArgs e)
        //{
        //    count+=5;

        //    if (count == 1)
        //        CounterBtn.Text = $"Kliknięto {count} raz";
        //    else
        //        CounterBtn.Text = $"Kliknięto {count} razy";

        //    SemanticScreenReader.Announce(CounterBtn.Text);
        //}
        private async void SelectSound(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Wybierz plik dźwiękowy",
                    //FileTypes = FilePickerFileType.Images
                });
                if (result != null)
                {
                    if (result.FileName.EndsWith("wav", StringComparison.OrdinalIgnoreCase) ||
                        result.FileName.EndsWith("mp3", StringComparison.OrdinalIgnoreCase))
                    {
                        //using var stream = await result.OpenReadAsync();
                        odtwarzacz.Stop();
                        var Sos = MediaSource.FromFile(result.FullPath); //FileName narzeka, gdy
                        await DisplayAlert("info", "Plik załadowano", " Ok");
                        outputText.Text = result.FullPath;
                        odtwarzacz.Source = Sos;
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Alert", " Wybrano niewłaściwy plik", " Ok");
                    }
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Błąd", ex.Message, "OK");
                throw;
            }
            // https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/storage/file-picker?view=net-maui-8.0&tabs=windows
            //var Sos = result.FullPath.ToString();
            //odtwarzacz.Source = Sos;
            //outputText.Text = imageSource;
        }
        //
    }

}
