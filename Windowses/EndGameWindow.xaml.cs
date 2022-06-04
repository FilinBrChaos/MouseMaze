using GameParts.GameCore.OtherSoftware;
using System.Windows;
using System.Windows.Input;

namespace MouseMaze.Windowses
{
    public partial class EndGameWindow : Window
    {
        MainWindow MainWindow;

        public EndGameWindow(MainWindow mainWindow, MazeManager.Winer winer)
        {
            InitializeComponent();
            MainWindow = mainWindow;
            KeyDown += EndGameWindow_KeyDown;
            if (winer == MazeManager.Winer.Player) textBoxWiner.Text = "You won!";
            else textBoxWiner.Text = "You lose";
        }

        private void EndGameWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Close();
        }

        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Close();
            Close();
        }
    }
}
