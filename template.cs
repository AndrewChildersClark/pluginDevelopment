using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;

namespace Plugins
{
    //Change NameOfPlugin below to the intended purpose of the plugin
    public class NameOfPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            IOrganizationServiceFactory serviceFactory =
                (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                Entity entity = (Entity)context.InputParameters["Target"];

                try
                {
                    //PLUGIN LOGIC
                }

                catch (FaultException<OrganizationServiceFault> ex)
                {
                //Change NameOfPlugin in the next line like you did in line 12
                    throw new InvalidPluginExecutionException("An error occurred in NameOfPlugin in the Plugins Solution.", ex);
                }

                catch (Exception ex)
                {
                //Change NameOfPlugin in the next line like you did in line 12
                    tracingService.Trace("NameOfPlugin: {0}", ex.ToString());
                    throw;
                }
            }
        }
    }
}
