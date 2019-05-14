using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OIMS.Global
{
    /// <summary>
    /// Html helper extension for loading scripts at bottom of the page from partial view.
    /// More at - http://jadnb.wordpress.com/2011/02/16/rendering-scripts-from-partial-views-at-the-end-in-mvc/
    /// </summary>
    public static class HtmlHelpers
    {
        private sealed class ScriptBlock : IDisposable
        {
            readonly WebViewPage _webPageBase;
            private const string ScriptsKey = "scripts";

            public static List<string> PageScripts
            {
                get
                {
                    if (HttpContext.Current.Items[ScriptsKey] == null)
                        HttpContext.Current.Items[ScriptsKey] = new List<string>();
                    return (List<string>)HttpContext.Current.Items[ScriptsKey];
                }
            }

            public ScriptBlock(WebViewPage webPageBase)
            {
                _webPageBase = webPageBase;
                _webPageBase.OutputStack.Push(new StringWriter());
            }

            public void Dispose()
            {
                var stack = (StringWriter)_webPageBase.OutputStack.Pop();
                PageScripts.Add(stack.ToString());
            }
        }

        public static IDisposable BeginScripts(this HtmlHelper helper)
        {
            return new ScriptBlock((WebViewPage)helper.ViewDataContainer);
        }

        public static MvcHtmlString PageScripts(this HtmlHelper helper)
        {
            return MvcHtmlString.Create(string.Join(Environment.NewLine, ScriptBlock.PageScripts.Select(s => s.ToString(CultureInfo.InvariantCulture))));
        }
    }
}