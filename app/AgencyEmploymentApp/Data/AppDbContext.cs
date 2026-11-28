using System;
using System.Collections.Generic;
using AgencyEmploymentApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AgencyEmploymentApp.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<ApplicationStatus> ApplicationStatuses { get; set; }

    public virtual DbSet<Authorization> Authorizations { get; set; }

    public virtual DbSet<DesiredPosition> DesiredPositions { get; set; }

    public virtual DbSet<EmploymentType> EmploymentTypes { get; set; }

    public virtual DbSet<PositionTitle> PositionTitles { get; set; }

    public virtual DbSet<Resume> Resumes { get; set; }

    public virtual DbSet<ResumeStatus> ResumeStatuses { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vacancy> Vacancies { get; set; }

    public virtual DbSet<WorkMode> WorkModes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pgcrypto");

        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(e => e.ApplicationId).HasName("pk_applications");

            entity.ToTable("applications", tb => tb.HasComment("Отклики пользователей на вакансии"));

            entity.HasIndex(e => e.ResponseDate, "idx_applications_response_date");

            entity.HasIndex(e => e.ResumeId, "idx_applications_resume");

            entity.HasIndex(e => e.ApplicationStatusId, "idx_applications_status");

            entity.HasIndex(e => e.VacancyId, "idx_applications_vacancy");

            entity.HasIndex(e => new { e.ResumeId, e.VacancyId }, "uq_applications_resume_vacancy").IsUnique();

            entity.Property(e => e.ApplicationId).HasColumnName("application_id");
            entity.Property(e => e.ApplicationStatusId).HasColumnName("application_status_id");
            entity.Property(e => e.ResponseDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasComment("Дата отклика")
                .HasColumnName("response_date");
            entity.Property(e => e.ResumeId).HasColumnName("resume_id");
            entity.Property(e => e.VacancyId).HasColumnName("vacancy_id");

            entity.HasOne(d => d.ApplicationStatus).WithMany(p => p.Applications)
                .HasForeignKey(d => d.ApplicationStatusId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_applications_status");

            entity.HasOne(d => d.Resume).WithMany(p => p.Applications)
                .HasForeignKey(d => d.ResumeId)
                .HasConstraintName("fk_applications_resume");

            entity.HasOne(d => d.Vacancy).WithMany(p => p.Applications)
                .HasForeignKey(d => d.VacancyId)
                .HasConstraintName("fk_applications_vacancy");
        });

        modelBuilder.Entity<ApplicationStatus>(entity =>
        {
            entity.HasKey(e => e.ApplicationStatusId).HasName("pk_application_statuses");

            entity.ToTable("application_statuses", tb => tb.HasComment("Справочник статусов отклика на вакансию"));

            entity.HasIndex(e => e.Name, "uq_application_statuses_name").IsUnique();

            entity.Property(e => e.ApplicationStatusId).HasColumnName("application_status_id");
            entity.Property(e => e.Name)
                .HasMaxLength(80)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Authorization>(entity =>
        {
            entity.HasKey(e => e.AuthorizationId).HasName("pk_authorizations");

            entity.ToTable("authorizations", tb => tb.HasComment("Учётные данные для входа в систему"));

            entity.HasIndex(e => e.Login, "uq_authorizations_login").IsUnique();

            entity.Property(e => e.AuthorizationId).HasColumnName("authorization_id");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .HasComment("Уникальный логин пользователя")
                .HasColumnName("login");
            entity.Property(e => e.PasswordHash)
                .HasComment("bcrypt-хеш пароля (pgcrypto, crypt/gen_salt)")
                .HasColumnName("password_hash");
        });

        modelBuilder.Entity<DesiredPosition>(entity =>
        {
            entity.HasKey(e => e.DesiredPositionId).HasName("pk_desired_positions");

            entity.ToTable("desired_positions", tb => tb.HasComment("Справочник желаемых должностей и специализаций"));

            entity.HasIndex(e => e.Specialization, "uq_desired_positions_specialization").IsUnique();

            entity.Property(e => e.DesiredPositionId).HasColumnName("desired_position_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Salary)
                .HasPrecision(12, 2)
                .HasComment("Ожидаемая заработная плата, руб.")
                .HasColumnName("salary");
            entity.Property(e => e.Specialization)
                .HasMaxLength(150)
                .HasColumnName("specialization");
        });

        modelBuilder.Entity<EmploymentType>(entity =>
        {
            entity.HasKey(e => e.EmploymentTypeId).HasName("pk_employment_types");

            entity.ToTable("employment_types", tb => tb.HasComment("Справочник типов занятости"));

            entity.HasIndex(e => e.Name, "uq_employment_types_name").IsUnique();

            entity.Property(e => e.EmploymentTypeId).HasColumnName("employment_type_id");
            entity.Property(e => e.Name)
                .HasMaxLength(80)
                .HasColumnName("name");
        });

        modelBuilder.Entity<PositionTitle>(entity =>
        {
            entity.HasKey(e => e.PositionTitleId).HasName("pk_position_titles");

            entity.ToTable("position_titles", tb => tb.HasComment("Справочник названий позиций"));

            entity.HasIndex(e => e.Name, "uq_position_titles_name").IsUnique();

            entity.Property(e => e.PositionTitleId).HasColumnName("position_title_id");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Resume>(entity =>
        {
            entity.HasKey(e => e.ResumeId).HasName("pk_resumes");

            entity.ToTable("resumes", tb => tb.HasComment("Резюме пользователей"));

            entity.HasIndex(e => e.DesiredPositionId, "idx_resumes_desired_position");

            entity.HasIndex(e => e.ResumeStatusId, "idx_resumes_status");

            entity.HasIndex(e => e.UserId, "idx_resumes_user");

            entity.Property(e => e.ResumeId).HasColumnName("resume_id");
            entity.Property(e => e.DesiredPositionId).HasColumnName("desired_position_id");
            entity.Property(e => e.Experience)
                .HasComment("Описание опыта работы")
                .HasColumnName("experience");
            entity.Property(e => e.PersonalDataConsent)
                .HasDefaultValue(false)
                .HasComment("Согласие на обработку персональных данных")
                .HasColumnName("personal_data_consent");
            entity.Property(e => e.PublishedAt)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("published_at");
            entity.Property(e => e.ResumeStatusId).HasColumnName("resume_status_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.DesiredPosition).WithMany(p => p.Resumes)
                .HasForeignKey(d => d.DesiredPositionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_resumes_desired_position");

            entity.HasOne(d => d.ResumeStatus).WithMany(p => p.Resumes)
                .HasForeignKey(d => d.ResumeStatusId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_resumes_status");

            entity.HasOne(d => d.User).WithMany(p => p.Resumes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_resumes_user");
        });

        modelBuilder.Entity<ResumeStatus>(entity =>
        {
            entity.HasKey(e => e.ResumeStatusId).HasName("pk_resume_statuses");

            entity.ToTable("resume_statuses", tb => tb.HasComment("Справочник статусов резюме"));

            entity.HasIndex(e => e.Name, "uq_resume_statuses_name").IsUnique();

            entity.Property(e => e.ResumeStatusId).HasColumnName("resume_status_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("pk_roles");

            entity.ToTable("roles", tb => tb.HasComment("Роли пользователей системы"));

            entity.HasIndex(e => e.Name, "uq_roles_name").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasComment("Наименование роли")
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("pk_users");

            entity.ToTable("users", tb => tb.HasComment("Пользователи системы"));

            entity.HasIndex(e => e.RoleId, "idx_users_role");

            entity.HasIndex(e => e.AuthorizationId, "uq_users_authorization").IsUnique();

            entity.HasIndex(e => e.Email, "uq_users_email").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.AuthorizationId)
                .HasComment("Ссылка на учётные данные (связь 1:1)")
                .HasColumnName("authorization_id");
            entity.Property(e => e.Email)
                .HasMaxLength(254)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.HasMedicalBook)
                .HasDefaultValue(false)
                .HasComment("Наличие медицинской книжки")
                .HasColumnName("has_medical_book");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(100)
                .HasColumnName("middle_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Authorization).WithOne(p => p.User)
                .HasForeignKey<User>(d => d.AuthorizationId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_users_authorization");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_users_role");
        });

        modelBuilder.Entity<Vacancy>(entity =>
        {
            entity.HasKey(e => e.VacancyId).HasName("pk_vacancies");

            entity.ToTable("vacancies", tb => tb.HasComment("Вакансии, размещённые пользователями"));

            entity.HasIndex(e => e.EmploymentTypeId, "idx_vacancies_employment_type");

            entity.HasIndex(e => e.OpenedAt, "idx_vacancies_opened_at");

            entity.HasIndex(e => e.PositionTitleId, "idx_vacancies_position_title");

            entity.HasIndex(e => e.UserId, "idx_vacancies_user");

            entity.HasIndex(e => e.WorkModeId, "idx_vacancies_work_mode");

            entity.Property(e => e.VacancyId).HasColumnName("vacancy_id");
            entity.Property(e => e.ClosedAt)
                .HasComment("Дата закрытия вакансии; NULL — вакансия открыта")
                .HasColumnName("closed_at");
            entity.Property(e => e.EmploymentTypeId).HasColumnName("employment_type_id");
            entity.Property(e => e.OpenedAt)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("opened_at");
            entity.Property(e => e.PositionTitleId).HasColumnName("position_title_id");
            entity.Property(e => e.Salary)
                .HasPrecision(12, 2)
                .HasColumnName("salary");
            entity.Property(e => e.UserId)
                .HasComment("Пользователь, разместивший вакансию")
                .HasColumnName("user_id");
            entity.Property(e => e.WorkModeId).HasColumnName("work_mode_id");

            entity.HasOne(d => d.EmploymentType).WithMany(p => p.Vacancies)
                .HasForeignKey(d => d.EmploymentTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_vacancies_employment_type");

            entity.HasOne(d => d.PositionTitle).WithMany(p => p.Vacancies)
                .HasForeignKey(d => d.PositionTitleId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_vacancies_position_title");

            entity.HasOne(d => d.User).WithMany(p => p.Vacancies)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_vacancies_user");

            entity.HasOne(d => d.WorkMode).WithMany(p => p.Vacancies)
                .HasForeignKey(d => d.WorkModeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_vacancies_work_mode");
        });

        modelBuilder.Entity<WorkMode>(entity =>
        {
            entity.HasKey(e => e.WorkModeId).HasName("pk_work_modes");

            entity.ToTable("work_modes", tb => tb.HasComment("Справочник принципов работы (офис, удалённо, гибрид)"));

            entity.HasIndex(e => e.Name, "uq_work_modes_name").IsUnique();

            entity.Property(e => e.WorkModeId).HasColumnName("work_mode_id");
            entity.Property(e => e.Name)
                .HasMaxLength(80)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
