namespace Authentication.MagicLink.Models;

public abstract class TemplateBase<TModel>
{
    public TModel? Model { get; set; }
    public StringWriter Output { get; } = new StringWriter();

    public abstract Task ExecuteAsync();

    public override string ToString()
    {
        return Output.ToString();
    }
}
