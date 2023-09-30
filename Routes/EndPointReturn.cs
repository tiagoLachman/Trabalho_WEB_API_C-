class EndPointReturn
{
    public static Task Retornar(HttpContext context, Object data, int code = 200)
    {
        context.Response.StatusCode = code;
        var json = System.Text.Json.JsonSerializer.Serialize(data);
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsync(json);
    }
    public static Task Retornar(HttpContext context, string data, int code = 200)
    {
        context.Response.StatusCode = code;
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsync(data);
    }
}
