

namespace GUI
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        //BasePage<>
        public MainPage()
        {
            InitializeComponent();

        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count+=5;

            if (count == 1)
                CounterBtn.Text = $"Kliknięto {count} raz";
            else
                CounterBtn.Text = $"Kliknięto {count} razy";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }

}
