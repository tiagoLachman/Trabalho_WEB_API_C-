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

        //listar agendamento especifico (por email)
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
            agendamento.paciente = BaseDeDados.Pacientes.Find(agendamento.paciente.id);
            agendamento.medico = BaseDeDados.Medicos.Find(agendamento.medico.id);
            
            if (agendamento.paciente == null)
            {
                return EndPointReturn.Retornar(context, "Paciente não encontrado", 404);
            }

            if (agendamento.medico == null)
            {
                return EndPointReturn.Retornar(context, "Medico não encontrado", 404);
            }

            List<DateTime> datas = Consultorio.HorariosDisponiveisMedico(BaseDeDados, agendamento.medico, agendamento.data);
            Consultorio.AgendarConsulta(BaseDeDados, agendamento);
            return EndPointReturn.Retornar(context, datas);
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
            var agendamento = BaseDeDados.Agendamentos.Find(id);
            if (agendamento == null)
            {
                return EndPointReturn.Retornar(context, "Agendamento não encontrado", 404);
            }
            agendamento.medico = agendamentoAtualizado.medico;
            agendamento.paciente = agendamentoAtualizado.paciente;
            agendamento.data = agendamentoAtualizado.data;
            agendamento.valor = agendamentoAtualizado.valor;
            BaseDeDados.SaveChanges();

            return EndPointReturn.Retornar(context, "Agendamento atualizado");
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
            BaseDeDados.Remove(agendamento);
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
