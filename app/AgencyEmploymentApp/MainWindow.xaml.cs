using System.Windows;
using AgencyEmploymentApp.Data;
using AgencyEmploymentApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AgencyEmploymentApp;

public partial class MainWindow : Window
{
    private readonly User _user;
    private readonly string _login;
    private readonly bool _isAdmin;
    private readonly bool _isSeeker;
    private readonly bool _isEmployer;

    public MainWindow(User user, string login)
    {
        InitializeComponent();

        _user = user;
        _login = login;
        _isAdmin = user.Role.Name == "Администратор";
        _isSeeker = user.Role.Name == "Соискатель";
        _isEmployer = user.Role.Name == "Работодатель";

        UserNameText.Text = FullName(user);
        UserRoleText.Text = $"{user.Role.Name} · {login}";

        Loaded += async (_, _) => await LoadDataAsync();
    }

    private static string FullName(User user)
    {
        var middle = string.IsNullOrWhiteSpace(user.MiddleName) ? string.Empty : " " + user.MiddleName;
        return $"{user.LastName} {user.FirstName}{middle}";
    }

    private async Task LoadDataAsync()
    {
        try
        {
            await using var db = new AppDbContext();
            await LoadResumesAsync(db);
            await LoadVacanciesAsync(db);
            await LoadApplicationsAsync(db);

            StatusBarText.Text =
                "Подключение: ais_agency_employment · PostgreSQL 18.3 · " +
                $"пользователь {_login} · роль: {_user.Role.Name}";
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Не удалось загрузить данные из базы: " + ex.Message,
                "Ошибка подключения",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            StatusBarText.Text = "Ошибка подключения к базе данных.";
        }
    }

    private async Task LoadResumesAsync(AppDbContext db)
    {
        var query = db.Resumes
            .AsNoTracking()
            .Include(r => r.User)
            .Include(r => r.DesiredPosition)
            .Include(r => r.ResumeStatus)
            .AsQueryable();

        if (_isSeeker)
        {
            query = query.Where(r => r.UserId == _user.UserId);
        }

        var data = await query.OrderBy(r => r.ResumeId).ToListAsync();

        var rows = data.Select(r => new ResumeRow
        {
            Id = r.ResumeId,
            UserName = FullName(r.User),
            DesiredPosition = r.DesiredPosition.Specialization,
            Experience = r.Experience ?? "—",
            Status = r.ResumeStatus.Name,
            PublishedAt = r.PublishedAt.ToString("dd.MM.yyyy"),
            Consent = r.PersonalDataConsent ? "Да" : "Нет"
        }).ToList();

        ResumesGrid.ItemsSource = rows;
        ResumesSummary.Text = _isSeeker
            ? $"Мои резюме: {rows.Count}"
            : $"Резюме в системе: {rows.Count}";
    }

    private async Task LoadVacanciesAsync(AppDbContext db)
    {
        var query = db.Vacancies
            .AsNoTracking()
            .Include(v => v.User)
            .Include(v => v.PositionTitle)
            .Include(v => v.EmploymentType)
            .Include(v => v.WorkMode)
            .AsQueryable();

        if (_isEmployer)
        {
            query = query.Where(v => v.UserId == _user.UserId);
        }

        var data = await query.OrderBy(v => v.VacancyId).ToListAsync();

        var rows = data.Select(v => new VacancyRow
        {
            Id = v.VacancyId,
            UserName = FullName(v.User),
            PositionTitle = v.PositionTitle.Name,
            Salary = v.Salary.ToString("N0") + " ₽",
            EmploymentType = v.EmploymentType.Name,
            WorkMode = v.WorkMode.Name,
            OpenedAt = v.OpenedAt.ToString("dd.MM.yyyy"),
            Status = v.ClosedAt is null ? "Открыта" : "Закрыта"
        }).ToList();

        VacanciesGrid.ItemsSource = rows;
        VacanciesSummary.Text = _isEmployer
            ? $"Мои вакансии: {rows.Count}"
            : $"Вакансии в системе: {rows.Count}";
    }

    private async Task LoadApplicationsAsync(AppDbContext db)
    {
        var query = db.Applications
            .AsNoTracking()
            .Include(a => a.Resume).ThenInclude(r => r.User)
            .Include(a => a.Vacancy).ThenInclude(v => v.User)
            .Include(a => a.Vacancy).ThenInclude(v => v.PositionTitle)
            .Include(a => a.ApplicationStatus)
            .AsQueryable();

        if (_isSeeker)
        {
            query = query.Where(a => a.Resume.UserId == _user.UserId);
        }
        else if (_isEmployer)
        {
            query = query.Where(a => a.Vacancy.UserId == _user.UserId);
        }

        var data = await query.OrderBy(a => a.ApplicationId).ToListAsync();

        var rows = data.Select(a => new ApplicationRow
        {
            Id = a.ApplicationId,
            ResumeUser = FullName(a.Resume.User),
            PositionTitle = a.Vacancy.PositionTitle.Name,
            VacancyUser = FullName(a.Vacancy.User),
            ResponseDate = a.ResponseDate.ToString("dd.MM.yyyy"),
            Status = a.ApplicationStatus.Name
        }).ToList();

        ApplicationsGrid.ItemsSource = rows;
        ApplicationsSummary.Text = _isSeeker
            ? $"Мои отклики: {rows.Count}"
            : _isEmployer
                ? $"Отклики на мои вакансии: {rows.Count}"
                : $"Отклики в системе: {rows.Count}";
    }

    private void LogoutButton_Click(object sender, RoutedEventArgs e)
    {
        var loginWindow = new LoginWindow();
        loginWindow.Show();
        Close();
    }

    public sealed class ResumeRow
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string DesiredPosition { get; set; } = string.Empty;
        public string Experience { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string PublishedAt { get; set; } = string.Empty;
        public string Consent { get; set; } = string.Empty;
    }

    public sealed class VacancyRow
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PositionTitle { get; set; } = string.Empty;
        public string Salary { get; set; } = string.Empty;
        public string EmploymentType { get; set; } = string.Empty;
        public string WorkMode { get; set; } = string.Empty;
        public string OpenedAt { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public sealed class ApplicationRow
    {
        public int Id { get; set; }
        public string ResumeUser { get; set; } = string.Empty;
        public string PositionTitle { get; set; } = string.Empty;
        public string VacancyUser { get; set; } = string.Empty;
        public string ResponseDate { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
