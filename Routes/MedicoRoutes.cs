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
            if(medicos.Count == 0){
                return EndPointReturn.Retornar(context, "Nenhum Medico cadastrado!!", 404);

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
                return EndPointReturn.Retornar(context, "Médico com id " + id + " não encontrado", 404);
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
            if (BaseDeDados.Medicos.Where(m => m.crm == medico.crm).FirstOrDefault() != null)
            {
                return EndPointReturn.Retornar(context, "Médico com CRM " + medico.crm + " já cadastrado no sistema!", 400);
            }
            var aux = medico.especialidade.id;
            medico.especialidade =  BaseDeDados.Especialidades.Find(medico.especialidade.id);
            if (medico.especialidade == null)
            {
                return EndPointReturn.Retornar(context, "Especialidade com id "+ aux + " não encontrada", 418);
            }

            BaseDeDados.Medicos.Add(medico);
            BaseDeDados.SaveChanges();
            medico = BaseDeDados.Medicos.Find(medico.id);
            return EndPointReturn.Retornar(context, "Médico cadastrado");
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context,"DEO MERDA AQ " + e.Message, 500);
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
                return EndPointReturn.Retornar(context, "Médico com id " + id + " não encontrado", 404);
            }

            if (
                BaseDeDados.Medicos.Where(m => m.crm == medicoAtualizado.crm).FirstOrDefault() != null &&
                medicoAtualizado.crm != medico.crm
            )
            {
                return EndPointReturn.Retornar(context, "Médico com CRM '" + medicoAtualizado.crm + "' já cadastrado no sistema!", 400);
            }

            string temp = medicoAtualizado.ehNulo();
            if (temp.Length > 0)
            {
                return EndPointReturn.Retornar(context, "Dados invalidos: " + temp, 400);
            }

            var aux = medicoAtualizado.especialidade.id;
            medicoAtualizado.especialidade = BaseDeDados.Especialidades.Find(medicoAtualizado.especialidade.id);
            if (medicoAtualizado.especialidade == null)
            {
                return EndPointReturn.Retornar(context, "Especialidade com id '" + aux + "' não encontrada", 418);
            }
            medico.nome = medicoAtualizado.nome;
            medico.email = medicoAtualizado.email;
            medico.crm = medicoAtualizado.crm;
            medico.especialidade = medicoAtualizado.especialidade;
            BaseDeDados.SaveChanges();

            return EndPointReturn.Retornar(context, "Médico com id " + id + " atualizado");
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
                return EndPointReturn.Retornar(context, "Médico com id " + id + " não encontrado", 404);
            }
            BaseDeDados.Remove(medico);
            BaseDeDados.SaveChanges();
            return EndPointReturn.Retornar(context, "Médico com id " + id + " deletado com sucesso");
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });
    }
}
