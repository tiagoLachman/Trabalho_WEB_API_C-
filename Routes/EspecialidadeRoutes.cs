using Trabalho;

public class EspecialidadeRoutes
{

    public static void CreateRoutes(WebApplication app)
    {
        //listar todos as especialidades
        app.MapGet("/especialidades", (BaseDeDados BaseDeDados) =>
    {
        return BaseDeDados.Especialidades.ToList();
    });

        //listar especialidade especifico (por email)
        app.MapGet("/especialidade/{id}", (BaseDeDados BaseDeDados, int id) =>
    {
        return BaseDeDados.Especialidades.Find(id);
    });

        //cadastrar especialidade
        app.MapPost("/especialidade", (BaseDeDados BaseDeDados, Especialidade especialidade) =>
    {
        BaseDeDados.Especialidades.Add(especialidade);
        BaseDeDados.SaveChanges();
        return "especialidade adicionada";
    });

        //atualizar especialidade
        app.MapPut("/especialidade/{id}", (BaseDeDados BaseDeDados, Especialidade especialidadeAtualizado, int id) =>
    {
        var especialidade = BaseDeDados.Especialidades.Find(id);
        especialidade.nome = especialidadeAtualizado.nome;
        BaseDeDados.SaveChanges();
        return "especialidade atualizada";
    });

        //deletar especialidade
        app.MapDelete("/especialidade/{id}", (BaseDeDados BaseDeDados, int id) =>
    {
        var especialidade = BaseDeDados.Especialidades.Find(id);
        BaseDeDados.Remove(especialidade);
        BaseDeDados.SaveChanges();
        return "especialidade deletada";
    });

        //------------------------------

    }
}
