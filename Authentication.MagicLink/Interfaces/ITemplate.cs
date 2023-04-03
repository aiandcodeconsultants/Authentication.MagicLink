namespace Authentication.MagicLink.Interfaces;

public interface ITemplate<TModel>
{
    Task<string> ExecuteAsync(TModel model);
}
