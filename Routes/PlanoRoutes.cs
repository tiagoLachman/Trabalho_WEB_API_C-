using Trabalho;

public class PlanoRoutes
{

    public static void CreateRoutes(WebApplication app)
    {
        //listar todos os planos
        app.MapGet("/planos", (HttpContext context, BaseDeDados BaseDeDados) =>
    {
        try
        {
            var planos = BaseDeDados.Planos.ToList();

            return EndPointReturn.Retornar(context, planos);
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });

        //listar plano especifico (por email)
        app.MapGet("/plano/{id}", (HttpContext context, BaseDeDados BaseDeDados, int id) =>
    {
        try
        {
            var plano = BaseDeDados.Planos.Find(id);
            if (plano == null)
            {
                return EndPointReturn.Retornar(context, "Plano não encontrado", 404);
            }

            return EndPointReturn.Retornar(context, plano, 404);
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });

        //cadastrar plano
        app.MapPost("/plano", (HttpContext context, BaseDeDados BaseDeDados, Plano plano) =>
    {
        try
        {
            BaseDeDados.Planos.Add(plano);
            BaseDeDados.SaveChanges();
            plano = BaseDeDados.Planos.Find(plano.id);
            return EndPointReturn.Retornar(context, "Plano cadastrada");
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });

        //atualizar plano
        app.MapPut("/plano/{id}", (HttpContext context, BaseDeDados BaseDeDados, Plano planoAtualizado, int id) =>
    {
        try
        {
            var plano = BaseDeDados.Planos.Find(id);
            if (plano == null)
            {
                return EndPointReturn.Retornar(context, "Plano não encontrado", 404);
            }
            plano.convenio = planoAtualizado.convenio;
            plano.desconto = planoAtualizado.desconto;
            BaseDeDados.SaveChanges();

            return EndPointReturn.Retornar(context, "Plano atualizado");
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });

        //deletar plano
        app.MapDelete("/plano/{id}", (HttpContext context, BaseDeDados BaseDeDados, int id) =>
    {
        try
        {
            var plano = BaseDeDados.Planos.Find(id);
            if (plano == null)
            {
                return EndPointReturn.Retornar(context, "Plano não encontrado", 404);
            }
            BaseDeDados.Remove(plano);
            BaseDeDados.SaveChanges();
            return EndPointReturn.Retornar(context, "Plano deletado com sucesso");
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });
    }
}
