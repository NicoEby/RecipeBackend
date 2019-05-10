namespace ch.thommenmedia.common.Interfaces
{
    public interface ITranslator
    {
        string TranslateText(string text, string module = "global");
    }
}
