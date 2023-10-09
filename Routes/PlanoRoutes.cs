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
            if(planos.Count == 0) {
                return EndPointReturn.Retornar(context, "Nenhum plano cadastrado!", 404);
            }
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
                return EndPointReturn.Retornar(context, "Plano com id " + id + " não encontrado", 404);
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
            if (plano.desconto == 0.0 || plano.desconto > 100.0)
            {
                return EndPointReturn.Retornar(context, "Desconto inválido", 400);
            }
            if (BaseDeDados.Planos.Where(p => p.convenio == plano.convenio).FirstOrDefault() != null)
            {
                return EndPointReturn.Retornar(context, "Plano com convenio '" + plano.convenio + "' já está cadastrado no sistema! Tente outro nome", 400);
            }
            BaseDeDados.Planos.Add(plano);
            BaseDeDados.SaveChanges();
            plano = BaseDeDados.Planos.Find(plano.id);
            return EndPointReturn.Retornar(context, "Plano cadastrado", 201);
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
            if (planoAtualizado.desconto == 0.0 || planoAtualizado.desconto > 100.0)
            {
                return EndPointReturn.Retornar(context, "Desconto inválido", 400);
            }

            var plano = BaseDeDados.Planos.Find(id);
            if (plano == null)
            {
                return EndPointReturn.Retornar(context, "Plano com id " + id + " não encontrado", 404);
            }
            
            if (
                BaseDeDados.Planos.Where(p => p.convenio == planoAtualizado.convenio).FirstOrDefault() != null &&
                planoAtualizado.convenio != plano.convenio
            )
            {
                return EndPointReturn.Retornar(context, "Plano com convenio '" + plano.convenio + "' já está cadastrado no sistema! Tente outro nome", 400);

            }
            

            
            plano.convenio = planoAtualizado.convenio;
            plano.desconto = planoAtualizado.desconto;
            BaseDeDados.SaveChanges();

            return EndPointReturn.Retornar(context, "Plano com id " + id + " atualizado");
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
                return EndPointReturn.Retornar(context, "Plano com id " + id + " não encontrado", 404);
            }
            BaseDeDados.Remove(plano);
            BaseDeDados.SaveChanges();
            return EndPointReturn.Retornar(context, "Plano com id " + id + " deletado com sucesso");
        }
        catch (Exception e)
        {
            return EndPointReturn.Retornar(context, e.Message, 500);
        }
    });
    }
}
