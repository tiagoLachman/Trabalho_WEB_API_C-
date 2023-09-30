using System.Text.Json.Serialization;
using Trabalho;
using System.Text.Json;
public class PacienteRoutes
{

    public static void CreateRoutes(WebApplication app)
    {
        //listar todos os pacientes
        app.MapGet("/pacientes", (HttpContext context, BaseDeDados BaseDeDados) =>
    {
        try
        {
            var pacientes = BaseDeDados.Pacientes.ToList();

            return EndPointReturn.Retornar(context, pacientes);
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });

        //listar paciente especifico (por email)
        app.MapGet("/paciente/{id}", (HttpContext context, BaseDeDados BaseDeDados, int id) =>
    {
        try
        {
            var paciente = BaseDeDados.Pacientes.Find(id);
            if (paciente == null)
            {
                return EndPointReturn.Retornar(context, "Paciente não encontrado", 404);
            }

            return EndPointReturn.Retornar(context, paciente, 404);
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });

        //cadastrar paciente
        app.MapPost("/paciente", (HttpContext context, BaseDeDados BaseDeDados, Paciente paciente) =>
    {
        try
        {
            BaseDeDados.Pacientes.Add(paciente);
            BaseDeDados.SaveChanges();
            paciente = BaseDeDados.Pacientes.Find(paciente.id);
            return EndPointReturn.Retornar(context, "Paciente cadastrado");
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });

        //atualizar paciente
        app.MapPut("/paciente/{id}", (HttpContext context, BaseDeDados BaseDeDados, Paciente pacienteAtualizado, int id) =>
    {
        try
        {
            var paciente = BaseDeDados.Pacientes.Find(id);
            if (paciente == null)
            {
                return EndPointReturn.Retornar(context, "Paciente não encontrado", 404);
            }
            paciente.nome = pacienteAtualizado.nome;
            paciente.email = pacienteAtualizado.email;
            paciente.endereco = pacienteAtualizado.endereco;
            BaseDeDados.SaveChanges();

            return EndPointReturn.Retornar(context, "Paciente atualizado");
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });

        //deletar paciente
        app.MapDelete("/paciente/{id}", (HttpContext context, BaseDeDados BaseDeDados, int id) =>
    {
        try
        {
            var paciente = BaseDeDados.Pacientes.Find(id);
            if (paciente == null)
            {
                return EndPointReturn.Retornar(context, "Paciente não encontrado", 404);
            }
            BaseDeDados.Remove(paciente);
            BaseDeDados.SaveChanges();
            return EndPointReturn.Retornar(context, "Paciente deletado com sucesso");
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });
    }
}
