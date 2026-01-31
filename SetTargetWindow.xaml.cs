using System.Windows;

namespace atomic_clock
{
    public partial class SetTargetWindow : Window
    {
        public string TargetResult { get; private set; } = "";

        public SetTargetWindow()
        {
            InitializeComponent();
            TimeInput.Text = DateTime.Now.ToString("HH:mm:ss"); // Suggerisce l'ora attuale
            TimeInput.Focus();
            TimeInput.SelectAll();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            TargetResult = TimeInput.Text;
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => this.DialogResult = false;
    }
}