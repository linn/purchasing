namespace Linn.Purchasing.Service.Host.Negotiators
{
    using System.Collections.Generic;
    using System.IO;

    public class ViewLoader : IViewLoader
    {
        private static readonly object Key = new object();

        private readonly Dictionary<string, string> loadedViews = new Dictionary<string, string>();

        public string Load(string viewName)
        {
            lock (Key)
            {
                if (!this.loadedViews.ContainsKey(viewName))
                {
                    var viewPath = $"./Views/{viewName}";

                    if (!File.Exists(viewPath))
                    {
                        viewPath = $"/app/views/{viewName}";
                        if (!File.Exists(viewPath))
                        {
                            return null;
                        }
                    }

                    var view = File.ReadAllText(viewPath);
                    this.loadedViews.Add(viewName, view);
                }

                return this.loadedViews[viewName];
            }
        }
    }
}
