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
            BaseDeDados.Agendamentos.Add(agendamento);
            BaseDeDados.SaveChanges();
            agendamento = BaseDeDados.Agendamentos.Find(agendamento.id);
            return EndPointReturn.Retornar(context, "Agendamento cadastrado");
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
