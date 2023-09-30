using System.Text.Json.Serialization;
using Trabalho;
using System.Text.Json;
public class MedicoRoutes
{

    public static void CreateRoutes(WebApplication app)
    {
        //listar todos os medicos
        app.MapGet("/medicos", (HttpContext context, BaseDeDados BaseDeDados) =>
    {
        try
        {
            var medicos = BaseDeDados.Medicos.ToList();
            foreach (var medico in medicos)
            {
                BaseDeDados.Entry(medico).Reference(m => m.especialidade).Load();
            }

            return EndPointReturn.Retornar(context, medicos);
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });

        //listar medico especifico (por email)
        app.MapGet("/medico/{id}", (HttpContext context, BaseDeDados BaseDeDados, int id) =>
    {
        try
        {
            var medico = BaseDeDados.Medicos.Find(id);
            if (medico == null)
            {
                return EndPointReturn.Retornar(context, "Médico não encontrado", 404);
            }
            BaseDeDados.Entry(medico).Reference(m => m.especialidade).Load();

            return EndPointReturn.Retornar(context, medico, 404);
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });

        //cadastrar medico
        app.MapPost("/medico", (HttpContext context, BaseDeDados BaseDeDados, Medico medico) =>
    {
        try
        {
            medico.especialidade = BaseDeDados.Especialidades.Find(medico.especialidade.id);
            if (medico.especialidade == null)
            {
                return EndPointReturn.Retornar(context, "Especialidade do médico não encontrada", 418);
            }
            BaseDeDados.Medicos.Add(medico);
            BaseDeDados.SaveChanges();
            medico = BaseDeDados.Medicos.Find(medico.id);
            return EndPointReturn.Retornar(context, "Médico cadastrado");
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });

        //atualizar medico
        app.MapPut("/medico/{id}", (HttpContext context, BaseDeDados BaseDeDados, Medico medicoAtualizado, int id) =>
    {
        try
        {
            var medico = BaseDeDados.Medicos.Find(id);
            if (medico == null)
            {
                return EndPointReturn.Retornar(context, "Médico não encontrado", 404);
            }
            medicoAtualizado.especialidade = BaseDeDados.Especialidades.Find(medicoAtualizado.especialidade.id);
            if (medicoAtualizado.especialidade == null)
            {
                return EndPointReturn.Retornar(context, "Especialidade do médico não encontrada", 418);
            }
            medico.nome = medicoAtualizado.nome;
            medico.email = medicoAtualizado.email;
            medico.crm = medicoAtualizado.crm;
            medico.especialidade = medicoAtualizado.especialidade;
            BaseDeDados.SaveChanges();

            return EndPointReturn.Retornar(context, "Médico atualizado");
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });

        //deletar medico
        app.MapDelete("/medico/{id}", (HttpContext context, BaseDeDados BaseDeDados, int id) =>
    {
        try
        {
            var medico = BaseDeDados.Medicos.Find(id);
            if (medico == null)
            {
                return EndPointReturn.Retornar(context, "Médico não encontrado", 404);
            }
            BaseDeDados.Remove(medico);
            BaseDeDados.SaveChanges();
            return EndPointReturn.Retornar(context, "Médico deletado com sucesso");
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });
    }
}
