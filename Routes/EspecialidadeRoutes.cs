using Trabalho;

public class EspecialidadeRoutes
{

    public static void CreateRoutes(WebApplication app)
    {
        //listar todos os especialidades
        app.MapGet("/especialidades", (HttpContext context, BaseDeDados BaseDeDados) =>
    {
        try
        {
            var especialidades = BaseDeDados.Especialidades.ToList();

            return EndPointReturn.Retornar(context, especialidades);
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });

        //listar especialidade especifico (por email)
        app.MapGet("/especialidade/{id}", (HttpContext context, BaseDeDados BaseDeDados, int id) =>
    {
        try
        {
            var especialidade = BaseDeDados.Especialidades.Find(id);
            if (especialidade == null)
            {
                return EndPointReturn.Retornar(context, "Especialidade não encontrada", 404);
            }

            return EndPointReturn.Retornar(context, especialidade, 404);
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });

        //cadastrar especialidade
        app.MapPost("/especialidade", (HttpContext context, BaseDeDados BaseDeDados, Especialidade especialidade) =>
    {
        try
        {
            if (BaseDeDados.Especialidades.Where(e => e.nome == especialidade.nome).FirstOrDefault() != null)
            {
                return EndPointReturn.Retornar(context, "Especialidade já cadastrada", 400);
            }
            
            BaseDeDados.Especialidades.Add(especialidade);
            BaseDeDados.SaveChanges();
            especialidade = BaseDeDados.Especialidades.Find(especialidade.id);
            return EndPointReturn.Retornar(context, "Especialidade cadastrada");
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });

        //atualizar especialidade
        app.MapPut("/especialidade/{id}", (HttpContext context, BaseDeDados BaseDeDados, Especialidade especialidadeAtualizado, int id) =>
    {
        try
        {
            if (BaseDeDados.Especialidades.Where(e => e.nome == especialidadeAtualizado.nome).FirstOrDefault() != null)
            {
                return EndPointReturn.Retornar(context, "Especialidade já cadastrada", 400);
            }

            var especialidade = BaseDeDados.Especialidades.Find(id);
            if (especialidade == null)
            {
                return EndPointReturn.Retornar(context, "Especialidade não encontrada", 404);
            }
            especialidade.nome = especialidadeAtualizado.nome;
            BaseDeDados.SaveChanges();

            return EndPointReturn.Retornar(context, "Especialidade atualizada");
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });

        //deletar especialidade
        app.MapDelete("/especialidade/{id}", (HttpContext context, BaseDeDados BaseDeDados, int id) =>
    {
        try
        {
            var especialidade = BaseDeDados.Especialidades.Find(id);
            if (especialidade == null)
            {
                return EndPointReturn.Retornar(context, "Especialidade não encontrada", 404);
            }
            BaseDeDados.Remove(especialidade);
            BaseDeDados.SaveChanges();
            return EndPointReturn.Retornar(context, "Especialidade deletada com sucesso");
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });
    }
}
