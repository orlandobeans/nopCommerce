﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Stores;

namespace Nop.Admin.Components
{
    public class MultistoreDisabledWarningViewComponent : ViewComponent
    {
        private readonly CatalogSettings _catalogSettings;
        private readonly ISettingService _settingService;
        private readonly IStoreService _storeService;

        public MultistoreDisabledWarningViewComponent(CatalogSettings catalogSettings,
            ISettingService settingService,
            IStoreService storeService)
        {
            this._catalogSettings = catalogSettings;
            this._settingService = settingService;
            this._storeService = storeService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            //action displaying notification (warning) to a store owner that "limit per store" feature is ignored

            //default setting
            bool enabled = _catalogSettings.IgnoreStoreLimitations;
            if (!enabled)
            {
                //overridden settings
                var stores = _storeService.GetAllStores();
                foreach (var store in stores)
                {
                    if (!enabled)
                    {
                        var catalogSettings = _settingService.LoadSetting<CatalogSettings>(store.Id);
                        enabled = catalogSettings.IgnoreStoreLimitations;
                    }
                }
            }

            //This setting is disabled. No warnings.
            if (!enabled)
                return Content("");

            return View();
        }
    }
}