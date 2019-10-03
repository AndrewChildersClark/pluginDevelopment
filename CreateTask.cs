using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;

namespace Plugins
{
    public class CreateTask : IPlugin
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
                    //Create a variable for the Entity and show what entity will be created
                    Entity taskRecord = new Entity("task");

                    //fill out single line of text fields
                    taskRecord.Attributes.Add("subject", "Call the contact");
                    taskRecord.Attributes.Add("description", "Ask contact for additional information");

                    //fill out date fields
                    taskRecord.Attributes.Add("scheduledend", DateTime.Now.AddDays(7));

                    //fill out option set
                    taskRecord.Attributes.Add("prioritycode", new OptionSetValue(1));

                    //fill out parent record to contact's GUID
                    taskRecord.Attributes.Add("regardingobjectid", entity.ToEntityReference());

                    Guid taskGuid = service.Create(taskRecord);
                }
                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in CreateTask in the Plugins Solution.", ex);
                }
                catch (Exception ex)
                {
                    tracingService.Trace("CreateTask: {0}", ex.ToString());
                    throw;
                }
            }
        }
    }
}
