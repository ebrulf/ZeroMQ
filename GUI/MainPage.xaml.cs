//using Com.Google.Android.Exoplayer2.Source;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Storage;
namespace GUI
{
    public partial class MainPage : ContentPage
    {
        public ZeroMQ.Giera gra; //to chyba najlepsze miejsce, wróć, gdzie jest instancja?
        public MainPage()
        {
            InitializeComponent();

        }
        //Shell.Current.DisplayAlert
        void OnClicked(object sender, EventArgs args)
        {
            Console.Beep(220,500);
            Wyłącz((Button)sender);
            
        }
        void Wyłącz(Button s)
        {
            s.IsEnabled = false; //to rozkracza plus zamienienie sendera z obiektu na przycisk
            //rzutowanie załatwiło sprawę
            s.BackgroundColor = new Color(64, 64, 64);//kolor wyłączonego przycisku
            s.TextColor = new Color(200, 200, 200);//kolor tekstu ma się nie zmieniać
            //dla porządku, niekliknięty tekst na razie ma biały kolor
        }
    }
    

}
