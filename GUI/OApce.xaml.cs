using CommunityToolkit.Maui;
using GUI.WinUI;
namespace GUI;

public partial class OApce : ContentPage
{
	public OApce()
	{
		InitializeComponent();
	}
    private void LearnMore_Clicked(object sender, EventArgs e)
    {
        // Navigate to the specified URL in the system browser.
        Console.Beep(440, 500);
        //await Launcher.Default.OpenAsync("https://aka.ms/maui");
    }
    void OnColorsRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        //((RadioButton)sender) //ma wyboldowa� tekst, ale to da si� zrobi� w xaml
        IP.IsEnabled = (Serwer.IsChecked==true);//o to chodzi�o!
        //RadioButtonGroup.SelectedValueProperty.
        //Serwer.
    }
    void OnEntryCompleted(object sender, EventArgs e)
    {
        //((RadioButton)sender) //ma wyboldowa� tekst, ale to da si� zrobi� w xaml
        Console.Beep(440, 500);
        DisplayAlert("info", IP.Text, " Ok");//to nie dzia�a itd.
        GUI.MainPage.gra = new ZeroMQ.Giera();
        Programik.JoinGame(new ZeroMQ.Giera(), IP.Text);
    }
    void Add(object sender, CheckedChangedEventArgs e)
    {
        //narzeka, �e tego nie ma
    }
}