using Nop.Core.Plugins;
using Nop.Services.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.HomePageNewProducts
{
    public class HomePageNewProductsPlugin : BasePlugin, IWidgetPlugin
    {
        private readonly HomePageNewProductsSettings _homePageNewProductsSettings;

        public HomePageNewProductsPlugin(HomePageNewProductsSettings homePageNewProductsSettings)
        {
            this._homePageNewProductsSettings = homePageNewProductsSettings;
        }
        /// <summary>
        /// Gets a view component for displaying plugin in public store
        /// </summary>
        /// <param name="widgetZone">Name of the widget zone</param>
        /// <param name="viewComponentName">View component name</param>
        public void GetPublicViewComponent(string widgetZone, out string viewComponentName)
        {
            viewComponentName = "WidgetsHomePageNewProducts";
        }

        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>Widget zones</returns>
        public IList<string> GetWidgetZones()
        {
            return !string.IsNullOrWhiteSpace(_homePageNewProductsSettings.WidgetZone)
                       ? new List<string>() { _homePageNewProductsSettings.WidgetZone }
                       : new List<string>() { "home_page_top", "home_page_before_categories",
                           "home_page_before_products", "body_end_html_tag_before",
                       "home_page_before_best_sellers", "home_page_before_news",
                       "home_page_before_poll", "home_page_bottom"};
        }
    }
}
