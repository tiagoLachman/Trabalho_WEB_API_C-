using System.Text.Json.Serialization;
using Trabalho;
using System.Text.Json;
public class AgendamentoRoutes
{

    public static void CreateRoutes(WebApplication app)
    {
        //listar todos os agendamentos
        app.MapGet("/agendamentos", (HttpContext context, BaseDeDados BaseDeDados) =>
    {
        try
        {
            var agendamentos = BaseDeDados.Agendamentos.ToList();
            foreach (var agendamento in agendamentos)
            {
                BaseDeDados.Entry(agendamento).Reference(a => a.medico).Load();
                BaseDeDados.Entry(agendamento).Reference(a => a.paciente).Load();
                BaseDeDados.Entry(agendamento.medico).Reference(m => m.especialidade).Load();
            }

            return EndPointReturn.Retornar(context, agendamentos);
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });

        //listar agendamento especifico (por medico)
        app.MapGet("/agendamento/medico/{idMedico}", (HttpContext context, BaseDeDados BaseDeDados, int idMedico) =>
    {
        try
        {
            var medico = BaseDeDados.Medicos.Find(idMedico);
            if (medico == null)
            {
                return EndPointReturn.Retornar(context, "Medico não encontrado", 404);
            }

            var agendamentos = BaseDeDados.Agendamentos
                .Where(
                    a => a.medico.id == medico.id
                    && a.cancelado == false
                    )
                .ToList();
            foreach (var agendamento in agendamentos)
            {
                BaseDeDados.Entry(agendamento).Reference(a => a.medico).Load();
                BaseDeDados.Entry(agendamento).Reference(a => a.paciente).Load();
                BaseDeDados.Entry(agendamento.medico).Reference(m => m.especialidade).Load();
            }

            return EndPointReturn.Retornar(context, agendamentos);

        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });

        //listar agendamento especifico (por paciente)
        app.MapGet("/agendamento/paciente/{idPaciente}", (HttpContext context, BaseDeDados BaseDeDados, int idPaciente) =>
    {
        try
        {
            var paciente = BaseDeDados.Pacientes.Find(idPaciente);
            if (paciente == null)
            {
                return EndPointReturn.Retornar(context, "Paciente não encontrado", 404);
            }

            var agendamentos = BaseDeDados.Agendamentos
                .Where(
                    a => a.paciente.id == paciente.id
                    && a.cancelado == false
                    )
                .ToList();
            foreach (var agendamento in agendamentos)
            {
                BaseDeDados.Entry(agendamento).Reference(a => a.medico).Load();
                BaseDeDados.Entry(agendamento).Reference(a => a.paciente).Load();
                BaseDeDados.Entry(agendamento.medico).Reference(m => m.especialidade).Load();
            }

            return EndPointReturn.Retornar(context, agendamentos);

        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });

        //listar agendamento especifico (por id)
        app.MapGet("/agendamento/{id}", (HttpContext context, BaseDeDados BaseDeDados, int id) =>
    {
        try
        {
            var agendamento = BaseDeDados.Agendamentos.Find(id);
            if (agendamento == null)
            {
                return EndPointReturn.Retornar(context, "Agendamento não encontrado", 404);
            }
            BaseDeDados.Entry(agendamento).Reference(a => a.medico).Load();
            BaseDeDados.Entry(agendamento).Reference(a => a.paciente).Load();

            return EndPointReturn.Retornar(context, agendamento, 404);
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });

        //cadastrar agendamento
        app.MapPost("/agendamento", (HttpContext context, BaseDeDados BaseDeDados, Agendamento agendamento) =>
    {
        try
        {
            agendamento.cancelado = false;
            string temp = agendamento.ehNulo();
            if (temp.Length > 0)
            {
                return EndPointReturn.Retornar(context, "Dados invalidos: " + temp, 400);
            }

            agendamento.paciente = BaseDeDados.Pacientes.Find(agendamento.paciente.id);
            agendamento.medico = BaseDeDados.Medicos.Find(agendamento.medico.id);

            temp = agendamento.ehNulo();
            if (temp.Length > 0)
            {
                return EndPointReturn.Retornar(context, "Dados invalidos: " + temp, 400);
            }

            var dataDaConsulta = Consultorio.AgendarConsulta(BaseDeDados, agendamento);
            //List<DateTime> datas = Consultorio.HorariosDisponiveisMedico(BaseDeDados, agendamento.medico, agendamento.data);
            return EndPointReturn.Retornar(context, dataDaConsulta);
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });

        //atualizar agendamento
        app.MapPut("/agendamento/{id}", (HttpContext context, BaseDeDados BaseDeDados, Agendamento agendamentoAtualizado, int id) =>
    {
        try
        {
            return EndPointReturn.Retornar(context, "dataDaConsulta");
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });

        //deletar agendamento
        app.MapDelete("/agendamento/{id}", (HttpContext context, BaseDeDados BaseDeDados, int id) =>
    {
        try
        {
            var agendamento = BaseDeDados.Agendamentos.Find(id);
            if (agendamento == null)
            {
                return EndPointReturn.Retornar(context, "Agendamento não encontrado", 404);
            }
            agendamento.cancelado = true;
            BaseDeDados.SaveChanges();
            return EndPointReturn.Retornar(context, "Agendamento deletado com sucesso");
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });
    }
}
