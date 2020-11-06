using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Azure.Data.AppConfiguration;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebAppWithKeyVault.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppConfigsController : ControllerBase
    {
        private readonly ConfigurationClient _configurationClient;

        public AppConfigsController(ConfigurationClient configurationClient)
        {
            _configurationClient = configurationClient;
        }

        [HttpGet]
        [Route("{settingName}")]
        public async IAsyncEnumerable<ConfigurationSetting> Get(string settingName)
        {
            var selector = new SettingSelector {KeyFilter = $"{settingName}*"};

            await foreach (var setting in _configurationClient.GetConfigurationSettingsAsync(selector))
            {
                yield return setting;
            }
        }

        [HttpGet]
        [Route("create")]
        public async Task<ConfigurationSetting> Create()
        {
            var contosoContext = new Context
            {
                Tenant = "contoso",
                Metadata = new List<MetaDatum>
                {
                    new MetaDatum
                    {
                        Key = "connectionString",
                        Value = "contoso-sql-db"
                    }
                }
            };

            var northwindContext = new Context
            {
                Tenant = "northwind",
                Metadata = new List<MetaDatum>
                {
                    new MetaDatum
                    {
                        Key = "connectionString",
                        Value = "northwind-sql-db"
                    }
                }
            };

            var ns = "data-context-ns";
            var name = "sql-cust-db";
            var type = "sql";
            var contextJson = JsonConvert.SerializeObject(new Context[] {contosoContext, northwindContext});
            var setting = new ConfigurationSetting($"{ns}/{name}/{type}", contextJson, "default");

            var response = await _configurationClient.AddConfigurationSettingAsync(setting);

            return response.Value;

        }
    }
}
