using CommunityToolkit.Maui;
namespace GUI;

public partial class OApce : ContentPage
{
	public OApce()
	{
		InitializeComponent();
	}
    private async void LearnMore_Clicked(object sender, EventArgs e)
    {
        // Navigate to the specified URL in the system browser.
        await Launcher.Default.OpenAsync("https://aka.ms/maui");
    }
}