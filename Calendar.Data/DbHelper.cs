using Calendar.Logic;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;

namespace Calendar.Data
{
    public static class DbHelper
    {
        public static IDocumentStore CreateStructures()
        {
            var documentConvention = new DocumentConvention
            {
                FindTypeTagName = type =>
                {
                    if (typeof(Event).IsAssignableFrom(type))
                    {
                        return "events";
                    }
                    return DocumentConvention.DefaultTypeTagName(type);
                }
            };
            var store = new EmbeddableDocumentStore
            {
                RunInMemory = true,
                Conventions = documentConvention

            };
            return store.Initialize();
        }
    }
}
