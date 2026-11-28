using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using AgencyEmploymentApp.Services;

namespace AgencyEmploymentApp;

public partial class LoginWindow : Window
{
    private static readonly Brush ErrorBrush = new SolidColorBrush(Color.FromRgb(214, 69, 69));
    private static readonly Brush InfoBrush = new SolidColorBrush(Color.FromRgb(107, 122, 144));

    public LoginWindow()
    {
        InitializeComponent();
        Loaded += (_, _) => LoginBox.Focus();
    }

    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        await LoginAsync();
    }

    private async void PasswordBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            await LoginAsync();
        }
    }

    private async Task LoginAsync()
    {
        var login = LoginBox.Text.Trim();
        var password = PasswordBox.Password;

        StatusText.Foreground = ErrorBrush;
        StatusText.Text = string.Empty;

        if (string.IsNullOrWhiteSpace(login) || string.IsNullOrEmpty(password))
        {
            StatusText.Text = "Введите логин и пароль.";
            return;
        }

        LoginButton.IsEnabled = false;
        StatusText.Foreground = InfoBrush;
        StatusText.Text = "Проверка учётных данных...";

        try
        {
            var result = await AuthService.AuthenticateAsync(login, password);

            if (!result.Success)
            {
                StatusText.Foreground = ErrorBrush;
                StatusText.Text = result.Error ?? "Не удалось выполнить вход.";
                PasswordBox.Clear();
                PasswordBox.Focus();
                return;
            }

            var mainWindow = new MainWindow(result.User!, result.Login!);
            mainWindow.Show();
            Close();
        }
        catch (Exception ex)
        {
            StatusText.Foreground = ErrorBrush;
            StatusText.Text = "Не удалось подключиться к базе данных: " + ex.Message;
        }
        finally
        {
            LoginButton.IsEnabled = true;
        }
    }
}
