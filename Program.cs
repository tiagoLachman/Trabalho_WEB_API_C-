//VERSAO WEBAPI COM DATABASE

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Trabalho
{

    class Especialidade
    {
        public int id { get; set; }
        public string? nome { get; set; }
    }

    class Paciente
    {
        public int id { get; set; }
        public string? nome { get; set; }
        public string? email { get; set; }
        public string? cpf { get; set; }
        public string? endereco { get; set; }
    }

    class Medico
    {
        public int id { get; set; }
        public string? nome { get; set; }
        public string? email { get; set; }
        public string? crm { get; set; }
        public Especialidade? especialidade { get; set; }
    }

    class Agendamento
    {
        public int id { get; set; }
        public Medico? medico { get; set; }
        public Paciente? paciente { get; set; }
        public float? valor { get; set; }
    }

    class BaseDeDados : DbContext
    {
        public BaseDeDados(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Paciente> Pacientes { get; set; } = null!;
        public DbSet<Medico> Medicos { get; set; } = null!;
        public DbSet<Agendamento> Agendamentos { get; set; } = null!;
        public DbSet<Especialidade> Especialidades { get; set; } = null!;
    }

    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("Consultorio") ?? "Data Source=Consultorio.db";
            builder.Services.AddSqlite<BaseDeDados>(connectionString);

            var app = builder.Build();

            PacienteRoutes.CreateRoutes(app);
            MedicoRoutes.CreateRoutes(app);
            EspecialidadeRoutes.CreateRoutes(app);
            AgendamentoRoutes.CreateRoutes(app);
            

            app.Run();
        }
    }
}
