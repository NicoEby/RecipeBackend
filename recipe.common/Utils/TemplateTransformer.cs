using System.Collections.Generic;
using System.Linq;
using ch.thommenmedia.common.Extensions;

namespace ch.thommenmedia.common.Utils
{
    /// <summary>
    /// can transform string templates and replace tags with content
    /// </summary>
    public static class TemplateTransformer
    {
        public static string TranslateTemplate(IEnumerable<ITemplateTag> tags, string text)
        {
            foreach (var templateTag in tags)
            {
                var tagConstruct = "[" + templateTag.TagName + "]";
                text = text.Replace(tagConstruct, GetValue(templateTag.Context, templateTag.Path));
            }
            return text;
        }

        private static string GetValue(object objContext, string path)
        {
            if (path.IsNullOrEmpty())
                return objContext != null ? objContext.ToString() : null;

            foreach (var segment in path.Split('.').Where(s => !string.IsNullOrEmpty(s)))
            {
                var value = objContext.GetValue(segment);
                if (value != null)
                {
                    var type = value.GetType();
                    if (!type.IsClass || type.IsAssignableFrom(typeof(string))) // simple type or string
                        return value.ToString();
                    objContext = value;
                }
                else
                    return null;
            }
            return null;
        }
    }

    public interface ITemplateTag
    {
        string TagName { get; }
        string Path { get; }
        object Context { get; }
    }

    public class TemplateTagModel : ITemplateTag
    {
        public TemplateTagModel(string tagName, string path, object context)
        {
            TagName = tagName;
            Path = path;
            Context = context;
        }

        public string TagName { get; }
        public string Path { get; }
        public object Context { get; }
    }
}
