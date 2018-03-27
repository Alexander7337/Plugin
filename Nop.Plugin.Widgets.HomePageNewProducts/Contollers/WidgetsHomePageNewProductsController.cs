﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Plugin.Widgets.HomePageNewProducts.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Widgets.HomePageNewProducts.Contollers
{
    public class WidgetsHomePageNewProductsController : BasePluginController
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreService _storeService;
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;

        public WidgetsHomePageNewProductsController(IWorkContext workContext,
            IStoreContext storeContext,
            IStoreService storeService,
            ISettingService settingService,
            ILocalizationService localizationService,
            IPermissionService permissionService)
        {
            this._workContext = workContext;
            this._storeService = storeService;
            this._settingService = settingService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
        }

        [AuthorizeAdmin]
        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var homePageNewProductsSettings = _settingService.LoadSetting<HomePageNewProductsSettings>(storeScope);
            var model = new ConfigurationModel
            {
                //GoogleId = googleAnalyticsSettings.GoogleId,
                //TrackingScript = googleAnalyticsSettings.TrackingScript,
                //EnableEcommerce = googleAnalyticsSettings.EnableEcommerce,
                //IncludingTax = googleAnalyticsSettings.IncludingTax,
                ZoneId = homePageNewProductsSettings.WidgetZone
            };
            model.AvailableZones.Add(new SelectListItem() { Text = "Before body end html tag", Value = "body_end_html_tag_before" });
            model.AvailableZones.Add(new SelectListItem() { Text = "Head html tag", Value = "head_html_tag" });

            model.ActiveStoreScopeConfiguration = storeScope;
            if (storeScope > 0)
            {
                //model.GoogleId_OverrideForStore = _settingService.SettingExists(googleAnalyticsSettings, x => x.GoogleId, storeScope);
                //model.TrackingScript_OverrideForStore = _settingService.SettingExists(googleAnalyticsSettings, x => x.TrackingScript, storeScope);
                //model.EnableEcommerce_OverrideForStore = _settingService.SettingExists(googleAnalyticsSettings, x => x.EnableEcommerce, storeScope);
                //model.IncludingTax_OverrideForStore = _settingService.SettingExists(googleAnalyticsSettings, x => x.IncludingTax, storeScope);
                model.ZoneId_OverrideForStore = _settingService.SettingExists(homePageNewProductsSettings, x => x.WidgetZone, storeScope);
            }

            return View("~/Plugins/Widgets.HomePageNewProducts/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var homePageNewProductsSettings = _settingService.LoadSetting<HomePageNewProductsSettings>(storeScope);
            //googleAnalyticsSettings.GoogleId = model.GoogleId;
            //googleAnalyticsSettings.TrackingScript = model.TrackingScript;
            //googleAnalyticsSettings.EnableEcommerce = model.EnableEcommerce;
            //googleAnalyticsSettings.IncludingTax = model.IncludingTax;
            homePageNewProductsSettings.WidgetZone = model.ZoneId;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            //_settingService.SaveSettingOverridablePerStore(homePageNewProductsSettings, x => x.GoogleId, model.GoogleId_OverrideForStore, storeScope, false);
            //_settingService.SaveSettingOverridablePerStore(homePageNewProductsSettings, x => x.TrackingScript, model.TrackingScript_OverrideForStore, storeScope, false);
            //_settingService.SaveSettingOverridablePerStore(homePageNewProductsSettings, x => x.EnableEcommerce, model.EnableEcommerce_OverrideForStore, storeScope, false);
            //_settingService.SaveSettingOverridablePerStore(homePageNewProductsSettings, x => x.IncludingTax, model.IncludingTax_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(homePageNewProductsSettings, x => x.WidgetZone, model.ZoneId_OverrideForStore, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }
    }
}
