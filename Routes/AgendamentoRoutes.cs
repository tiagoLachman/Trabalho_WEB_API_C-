using Trabalho;

public class AgendamentoRoutes
{

    public static void CreateRoutes(WebApplication app)
    {
        //listar todos os agendamentos
        app.MapGet("/agendamentos", (BaseDeDados BaseDeDados) =>
    {
        return BaseDeDados.Agendamentos.ToList();
    });

        //listar agendamento especifico (por email)
        app.MapGet("/agendamento/{id}", (BaseDeDados BaseDeDados, int id) =>
    {
        return BaseDeDados.Agendamentos.Find(id);
    });

        //cadastrar agendamento
        app.MapPost("/agendamento", (BaseDeDados BaseDeDados, Agendamento agendamento) =>
    {
        BaseDeDados.Agendamentos.Add(agendamento);
        BaseDeDados.SaveChanges();
        return "agendamento adicionado";
    });

        //atualizar agendamento
        app.MapPut("/agendamento/{id}", (BaseDeDados BaseDeDados, Agendamento agendamentoAtualizado, int id) =>
    {
        var agendamento = BaseDeDados.Agendamentos.Find(id);
        agendamentoAtualizado.medico = BaseDeDados.Medicos.Find(agendamentoAtualizado.medico);
        agendamentoAtualizado.paciente = BaseDeDados.Pacientes.Find(agendamentoAtualizado.paciente);

        agendamento.medico = agendamentoAtualizado.medico;
        agendamento.paciente = agendamentoAtualizado.paciente;
        agendamento.valor = agendamentoAtualizado.valor;
        BaseDeDados.SaveChanges();
        return "agendamento atualizado";
    });

        //deletar agendamento
        app.MapDelete("/agendamento/{id}", (BaseDeDados BaseDeDados, int id) =>
    {
        var agendamento = BaseDeDados.Agendamentos.Find(id);
        BaseDeDados.Remove(agendamento);
        BaseDeDados.SaveChanges();
        return "agendamento deletado";
    });

        //------------------------------

    }
}
