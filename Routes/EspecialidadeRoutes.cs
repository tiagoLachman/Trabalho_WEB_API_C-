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

            if(especialidades.Count == 0){
                return EndPointReturn.Retornar(context, "Nenhuma especialidade cadastrada!!");
            }

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
                return EndPointReturn.Retornar(context, "Especialidadecom id " + id + " não encontrada", 404);
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
                return EndPointReturn.Retornar(context, "Especialidade com nome '" + especialidade.nome + "' já cadastrada no Sistema!!!", 400);
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
        app.MapPut("/especialidade/{id}", (HttpContext context, BaseDeDados BaseDeDados, Especialidade especialidadeAtualizada, int id) =>
    {
        try
        {
            var especialidade = BaseDeDados.Especialidades.Find(id);
            if (especialidade == null)
            {
                return EndPointReturn.Retornar(context, "Especialidade com id " + id + " não encontrada", 404);
            }

            if (BaseDeDados.Especialidades.Where(e => e.nome == especialidadeAtualizada.nome).FirstOrDefault() != null)
            {
                return EndPointReturn.Retornar(context, "Especialidade com nome '" + especialidadeAtualizada.nome + "' já cadastrada no Sistema!!!", 400);
            }

            
            especialidade.nome = especialidadeAtualizada.nome;
            BaseDeDados.SaveChanges();

            return EndPointReturn.Retornar(context, "Especialidade com id " + id + " atualizada");
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
                return EndPointReturn.Retornar(context, "Especialidade com id " + id + " não encontrada", 404);
            }
            BaseDeDados.Remove(especialidade);
            BaseDeDados.SaveChanges();
            return EndPointReturn.Retornar(context, "Especialidade com id " + id + " deletada com sucesso");
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });
    }
}
