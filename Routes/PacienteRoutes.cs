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

            foreach (var paciente in pacientes)
            {
                BaseDeDados.Entry(paciente).Reference(p => p.plano).Load();
            }

            if (pacientes.Count == 0)
            {
                return EndPointReturn.Retornar(context, "Nenhum paciente cadastrado!", 404);
            }

            return EndPointReturn.Retornar(context, pacientes);
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });

        //listar paciente especifico (por id)
        app.MapGet("/paciente/{id}", (HttpContext context, BaseDeDados BaseDeDados, int id) =>
    {
        try
        {
            var paciente = BaseDeDados.Pacientes.Find(id);
            if (paciente == null)
            {
                return EndPointReturn.Retornar(context, "Paciente com id " + id + " não encontrado", 404);
            }
            BaseDeDados.Entry(paciente).Reference(p => p.plano).Load();
            return EndPointReturn.Retornar(context, paciente);
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
            paciente.ativo = true;
            if (BaseDeDados.Pacientes.Where(p => p.cpf == paciente.cpf).FirstOrDefault() != null)
            {
                return EndPointReturn.Retornar(context, "Paciente com CPF '" + paciente.cpf + "' já cadastrado", 400);
            }

            paciente.plano = BaseDeDados.Planos.Where(p => p.id == paciente.plano.id && p.ativo == true).FirstOrDefault();
            if (paciente.plano == null)
            {
                return EndPointReturn.Retornar(context, "Plano do paciente não encontrado ou não ativo", 418);
            }
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
                return EndPointReturn.Retornar(context, "Paciente com id " + id + " não encontrado", 404);
            }

            if (
                BaseDeDados.Pacientes.Where(p => p.cpf == pacienteAtualizado.cpf).FirstOrDefault() != null &&
                pacienteAtualizado.cpf != paciente.cpf
            )
            {
                return EndPointReturn.Retornar(context, "Paciente com CPF '" + pacienteAtualizado.cpf + "' já cadastrado", 400);
            }

            string temp = pacienteAtualizado.ehNulo();
            if (temp.Length > 0)
            {
                return EndPointReturn.Retornar(context, "Dados invalidos: " + temp, 400);
            }

            pacienteAtualizado.plano = BaseDeDados.Planos.Where(p => p.id == pacienteAtualizado.plano.id && p.ativo == true).FirstOrDefault();
            if (pacienteAtualizado.plano == null)
            {
                return EndPointReturn.Retornar(context, "Plano do paciente não encontrado ou não ativo", 418);
            }

            paciente.nome = pacienteAtualizado.nome;
            paciente.email = pacienteAtualizado.email;
            paciente.endereco = pacienteAtualizado.endereco;
            paciente.plano = pacienteAtualizado.plano;
            paciente.cpf = pacienteAtualizado.cpf;
            paciente.ativo = pacienteAtualizado.ativo;
            BaseDeDados.SaveChanges();

            return EndPointReturn.Retornar(context, "Paciente com id " + id + " atualizado");
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
                return EndPointReturn.Retornar(context, "Paciente com id " + id + " não encontrado", 404);
            }
            paciente.ativo = false;
            BaseDeDados.SaveChanges();
            return EndPointReturn.Retornar(context, "Paciente com id " + id + " deletado com sucesso");
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });
    }
}
