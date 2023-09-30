using Trabalho;

public class PacienteRoutes
{

    public static void CreateRoutes(WebApplication app)
    {
        //listar todos os pacientes
        app.MapGet("/pacientes", (BaseDeDados BaseDeDados) =>
    {
        return BaseDeDados.Pacientes.ToList();
    });

        //listar paciente especifico (por email)
        app.MapGet("/paciente/{id}", (BaseDeDados BaseDeDados, int id) =>
    {
        return BaseDeDados.Pacientes.Find(id);
    });

        //cadastrar paciente
        app.MapPost("/paciente", (BaseDeDados BaseDeDados, Paciente paciente) =>
    {
        BaseDeDados.Pacientes.Add(paciente);
        BaseDeDados.SaveChanges();
        return "paciente adicionado";
    });

        //atualizar paciente
        app.MapPut("/paciente/{id}", (BaseDeDados BaseDeDados, Paciente pacienteAtualizado, int id) =>
    {
        var paciente = BaseDeDados.Pacientes.Find(id);
        paciente.nome = pacienteAtualizado.nome;
        paciente.email = pacienteAtualizado.email;
        paciente.cpf = pacienteAtualizado.cpf;
        paciente.endereco = pacienteAtualizado.endereco;
        BaseDeDados.SaveChanges();
        return "paciente atualizado";
    });

        //deletar paciente
        app.MapDelete("/paciente/{id}", (BaseDeDados BaseDeDados, int id) =>
    {
        var paciente = BaseDeDados.Pacientes.Find(id);
        BaseDeDados.Remove(paciente);
        BaseDeDados.SaveChanges();
        return "paciente deletado";
    });

        //------------------------------

    }
}
