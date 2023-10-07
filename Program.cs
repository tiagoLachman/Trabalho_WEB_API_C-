//VERSAO WEBAPI COM DATABASE

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Trabalho
{

    class Plano
    {
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
        public Plano plano { get; set; }

        public string ehNulo()
        {
            if (endereco == null || endereco.Length <= 0)
            {
                return "endereco nulo ou em branco";
            }
            if (plano == null)
            {
                return "plano nulo ou em branco";
            }
            if (email == null || email.Length <= 0)
            {
                return "email nulo ou em branco";
            }
            if (cpf == null || cpf.Length <= 0)
            {
                return "cpf nulo ou em branco";
            }
            if (nome == null || nome.Length <= 0)
            {
                return "nome nulo ou em branco";
            }
            return "";
        }
    }

    class Medico
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string crm { get; set; }
        public Especialidade especialidade { get; set; }

        public string ehNulo()
        {
            if (nome == null || nome.Length <= 0)
            {
                return "nome nulo ou em branco";
            }
            if (especialidade == null)
            {
                return "especialidade nulo ou em branco";
            }
            if (crm == null || crm.Length <= 0)
            {
                return "crm nulo ou em branco";
            }
            if (email == null || email.Length <= 0)
            {
                return "email nulo ou em branco";
            }
            return "";
        }
    }

    class Agendamento
    {
        public int id { get; set; }
        public Medico medico { get; set; }
        public Paciente paciente { get; set; }
        public DateTime data { get; set; }
        public float valor { get; set; }
        public bool? cancelado {get; set;}

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

        public string ehNulo()
        {
            if (medico == null)
            {
                return "medico nulo ou em branco";
            }
            if (data == null)
            {
                return "data nulo ou em branco";
            }
            if (paciente == null)
            {
                return "paciente nulo ou em branco";
            }
            if (valor <= 0)
            {
                return "valor nulo ou em branco";
            }
            return "";
        }

    }

    class Consultorio
    {
        static public string AgendarConsulta(BaseDeDados db, Agendamento agendamento)
        {
            List<DateTime> datas = HorariosDisponiveisMedico(db, agendamento.medico, agendamento.data);

            if (datas.Count == 0)
            {
                return "Sem horarios disponiveis";
            }

            DateTime dataMaisProxima = datas[0];
            TimeSpan diferencaMaisProxima = dataMaisProxima - agendamento.data;

            foreach (DateTime data in datas)
            {
                TimeSpan diferencaAtual = data - agendamento.data;
                if (diferencaAtual.Duration() < diferencaMaisProxima.Duration())
                {
                    dataMaisProxima = data;
                    diferencaMaisProxima = diferencaAtual;
                }
            }
            string retorno = "M";
            if (dataMaisProxima.CompareTo(agendamento.data) != 0)
            {
                retorno = "Horário não disponível, m";
            }
            agendamento.data = dataMaisProxima;
            db.Agendamentos.Add(agendamento);
            db.SaveChanges();

            return retorno + "arcado para: " + agendamento.data.ToString();
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

        static public List<DateTime> HorariosDisponiveisMedico(BaseDeDados db, Medico medico, DateTime dataConsulta)
        {
            int tempoConsulta = 30;
            List<DateTime> listaHorasDisponiveis = new List<DateTime>();

            DateTime horaInicioExpediente = new DateTime(dataConsulta.Year, dataConsulta.Month, dataConsulta.Day, 8, 0, 0);
            DateTime horaFimExpediente = new DateTime(dataConsulta.Year, dataConsulta.Month, dataConsulta.Day, 17, 0, 0);

            List<DateTime> intervalos = new List<DateTime>();
            DateTime horaAtual = horaInicioExpediente;

            while (horaAtual < horaFimExpediente)
            {
                intervalos.Add(horaAtual);
                horaAtual = horaAtual.AddMinutes(tempoConsulta);
            }

            var consultasAgendadas = db.Agendamentos
                .Where(agendamento => agendamento.medico != null
                    && agendamento.medico.id == medico.id 
                    && agendamento.data.Date == dataConsulta.Date
                    && agendamento.cancelado == false
                    )
                .ToList();

            foreach (var consulta in consultasAgendadas)
            {
                DateTime inicioConsulta = consulta.data;
                DateTime fimConsulta = consulta.data.AddMinutes(tempoConsulta);

                intervalos.RemoveAll(intervalo => intervalo >= inicioConsulta && intervalo < fimConsulta);
            }

            listaHorasDisponiveis.AddRange(intervalos);

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
        public DbSet<Plano> Planos { get; set; } = null!;
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
            PlanoRoutes.CreateRoutes(app);

            app.Run();
        }
    }
}
