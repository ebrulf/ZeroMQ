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
        //Shell.Current.DisplayAlert
        void OnClicked(object sender, EventArgs args)
        {
            Console.Beep(220,500);
            Button s = (Button)sender;
            s.IsEnabled = false; //to rozkracza plus zamienienie sendera z obiektu na przycisk
            //rzutowanie załatwiło sprawę
        }
    }
    

}
