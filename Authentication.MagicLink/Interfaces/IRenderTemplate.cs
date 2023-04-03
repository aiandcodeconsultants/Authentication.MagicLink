namespace Authentication.MagicLink.Interfaces;

public interface IRenderTemplate
{
    Task<string> RenderAsync<TModel>(string templateName, TModel model);
}

