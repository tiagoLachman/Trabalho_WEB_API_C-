using System.Text.Json.Serialization;
using Trabalho;
using System.Text.Json;
public class MedicoRoutes
{

    public static void CreateRoutes(WebApplication app)
    {
        //listar todos os medicos
        app.MapGet("/medicos", (BaseDeDados BaseDeDados) =>
    {
        var medicos = BaseDeDados.Medicos.ToList();
        return medicos;
    });

        //listar medico especifico (por email)
        app.MapGet("/medico/{id}", (HttpContext context, BaseDeDados BaseDeDados, int id) =>
    {
        var medico = BaseDeDados.Medicos.Find(id);
        if (medico == null)
        {
            return EndPointReturn.Retornar(context,"Médico não encontrado", 404);
        }
        BaseDeDados.Entry(medico).Reference(m => m.especialidade).Load();

        return EndPointReturn.Retornar(context, medico, 404);
    });

        //cadastrar medico
        app.MapPost("/medico", (BaseDeDados BaseDeDados, Medico medico) =>
    {
        medico.especialidade = BaseDeDados.Especialidades.Find(medico.especialidade.id);
        BaseDeDados.Medicos.Add(medico);
        BaseDeDados.SaveChanges();
        medico = BaseDeDados.Medicos.Find(medico.id);
        return "medico adicionado";
    });

        //atualizar medico
        app.MapPut("/medico/{id}", (BaseDeDados BaseDeDados, Medico medicoAtualizado, int id) =>
    {
        var medico = BaseDeDados.Medicos.Find(id);
        medico.especialidade = BaseDeDados.Especialidades.Find(medicoAtualizado.especialidade);

        medico.nome = medicoAtualizado.nome;
        medico.email = medicoAtualizado.email;
        medico.crm = medicoAtualizado.crm;
        medico.especialidade = medicoAtualizado.especialidade;
        BaseDeDados.SaveChanges();
        return "medico atualizado";
    });

        //deletar medico
        app.MapDelete("/medico/{id}", (BaseDeDados BaseDeDados, int id) =>
    {
        var medico = BaseDeDados.Medicos.Find(id);
        BaseDeDados.Remove(medico);
        BaseDeDados.SaveChanges();
        return "medico deletado";
    });

        //------------------------------

    }
}
