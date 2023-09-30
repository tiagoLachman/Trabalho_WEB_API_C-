//VERSAO WEBAPI COM DATABASE

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Trabalho
{

    class Plano{
        public int id { get; set; }
        public string convenio { get; set; }
        public float desconto { get; set; }
    }
    class Especialidade
    {
        public int id { get; set; }
        public string nome { get; set; }
    }

    class Paciente
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string cpf { get; set; }
        public string endereco { get; set; }
    }

    class Medico
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string crm { get; set; }
        public Especialidade especialidade { get; set; }
    }

    class Agendamento
    {
        public int id { get; set; }
        public Medico medico { get; set; }
        public Paciente paciente { get; set; }
        public DateTime data { get; set; }
        public float valor { get; set; }

        public Agendamento()
        {

        }

        public Agendamento(Medico medico, Paciente paciente, DateTime data, float valor)
        {
            this.medico = medico;
            this.data = data;
            this.paciente = paciente;
            this.valor = valor;
        }

    }

    class Consultorio
    {
        static public void AgendarConsulta(BaseDeDados db, Agendamento agendamento)
        {
            db.Agendamentos.Add(agendamento);
            db.SaveChanges();
        }

        static public List<Agendamento> BuscarAgendamentosPorMedico(BaseDeDados db, Medico medico)
        {
            return db.Agendamentos.Where(agendamento => agendamento.medico != null && agendamento.medico.id == medico.id).ToList();
        }

        static public List<Agendamento> BuscarAgendamentosPorDia(BaseDeDados db, DateTime data)
        {
            return db.Agendamentos.Where(agendamento => agendamento.data.Date == data.Date).ToList();
        }

        static public List<Agendamento> BuscarAgendamentosPorMedicoDia(BaseDeDados db, Medico medico, DateTime data)
        {
            return db.Agendamentos.Where(agendamento => agendamento.medico != null && agendamento.medico.id == medico.id && agendamento.data.Date == data.Date).ToList();
        }

        static public List<DateTime> HorariosDisponiveisMedico(BaseDeDados db, Medico medico, DateTime data)
        {
            List<DateTime> listaHorasDisponiveis = new List<DateTime>();
            DateTime temp = data.Date;
            for (int i = 0; i < 48; i++)
            {
                listaHorasDisponiveis.Add(temp);
                temp = temp.AddMinutes(30);
            }
            var listaConsultas = db.Agendamentos.Where(agendamento => agendamento.medico != null && agendamento.medico.id == medico.id && agendamento.data.Date == data.Date).ToList();

            foreach (var agendamento in listaConsultas)
            {
                db.Entry(agendamento).Reference(a => a.medico).Load();
                db.Entry(agendamento).Reference(a => a.paciente).Load();
                if (agendamento.data.Minute > 30)
                {
                    agendamento.data.AddMinutes(60 - agendamento.data.Minute);
                }
                agendamento.data.AddMinutes(30 - agendamento.data.Minute);
                foreach (var item in listaHorasDisponiveis)
                {
                    var horaAgendamento = new DateTime(data.Year, data.Month, data.Day, agendamento.data.Hour, agendamento.data.Minute, 0);
                    listaHorasDisponiveis.Remove(horaAgendamento);
                }
            }
            return listaHorasDisponiveis;
        }
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
