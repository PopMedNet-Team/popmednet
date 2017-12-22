using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace Lpp.Dns.Portal.Areas.Workflow
{
    public class WorkflowAreaRegistration : AreaRegistration 
    {
        public static List<IVisualWorkflowActivity> Activities = new List<IVisualWorkflowActivity>();

        public override string AreaName 
        {
            get 
            {
                return "Workflow";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            RegisterActivities();

            context.MapRoute(
                            "Workflow_default",
                            "Workflow/{controller}/{action}"
                        );
        }

        private void RegisterActivities()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.GetName().FullName.StartsWith("System."));

            Activities.AddRange(assemblies.SelectMany(a => a.GetTypes().Where(type => typeof(IVisualWorkflowActivity).IsAssignableFrom(type) && type != typeof(IVisualWorkflowActivity)).Select(t => (IVisualWorkflowActivity)Activator.CreateInstance(t))));

            //check for duplicate registrations: if there are duplicates a situation where ko loads both templates will occur causing binding issues
            var duplicateActivities = Activities.GroupBy(a => new { WorkflowActivityID = a.WorkflowActivityID, WorkflowID = a.WorkflowID }).Where(k => k.Count() > 1).ToArray();
            if (duplicateActivities.Any())
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendLine("The following workflow activities have been registered more than once:");
                foreach (var group in duplicateActivities)
                {
                    sb.AppendLine("    WorkflowActivityID: " + group.Key.WorkflowActivityID);
                    sb.AppendLine("    WorkflowID: " + group.Key.WorkflowID);
                    foreach (var item in group)
                    {
                        sb.AppendLine("        TypeName: " + item.GetType().Name + " View Path: " + item.Path);
                    }
                }
                sb.AppendLine("Having duplicate registrations can cause binding issues, please review.");
                throw new InvalidOperationException(sb.ToString());
            }
        }

    }
}