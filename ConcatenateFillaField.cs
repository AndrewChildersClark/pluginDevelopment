using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;

namespace Plugins
{
    public class FillAField : IPlugin
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
                    //Create two variables
                    string firstName = string.Empty;
                    string lastName = string.Empty;

                    //Check to see if the non required field/attribute exists in the attribute collection
                    //This makes sure your plugin will not fail if first name is not entered
                    if (entity.Attributes.Contains("firstname"))
                    {
                         //set the variable first name equal to the value from Dynamics
                        firstName = entity.Attributes["firstname"].ToString();
                    }
                    //Required fields are present in the attribute collection on create
                    //so we do not need to check for their presence
                    lastName = entity.Attributes["lastname"].ToString();

                    //Fill in a field/attribute with the values above.
                    entity.Attributes.Add("description", "Hello " + firstName + " " + lastName);
                }

                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in FillAField in the Plugins Solution.", ex);
                }

                catch (Exception ex)
                {
                    tracingService.Trace("FillAField: {0}", ex.ToString());
                    throw;
                }
            }
        }
    }
}
