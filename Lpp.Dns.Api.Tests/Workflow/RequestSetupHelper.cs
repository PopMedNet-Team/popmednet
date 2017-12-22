using Lpp.Dns.Data;
using Lpp.Utilities.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Api.Tests.Workflow
{
    [TestClass]
    public class RequestSetupHelper
    {
        [TestMethod, Ignore]
        public void CreateWFRequestWithTaskAndDocuments()
        {
            using (var db = new DataContext())
            {
                var user = db.Users.Where(u => u.UserName == "SystemAdministrator").Select(u => new { u.ID, u.UserName, u.FirstName, u.LastName, u.OrganizationID }).First();

                System.Threading.Thread.CurrentPrincipal = new System.Security.Principal.GenericPrincipal(new ApiIdentity(user.ID, user.UserName, (user.FirstName + " " + user.LastName).Trim(), user.OrganizationID), null);

                var request = new Request();
                db.Requests.Add(request);
                Console.WriteLine("Request ID: " + request.ID.ToString("D"));

                request.Name = "Test Workflow Request";
                request.RequestTypeID = new Guid("A3044773-8387-4C1B-8139-92B281D0467C");//new workflow query composer type
                request.ProjectID = new Guid("17598FF8-7668-4C58-8225-A377011CD582");//change to local project
                request.OrganizationID = new Guid("7C5B0001-7635-4AC4-8961-A2F9013FFC50");//change to applicable organization
                request.CreatedByID = user.ID;
                request.CreatedOn = DateTime.UtcNow;
                request.UpdatedByID = user.ID;
                request.UpdatedOn = DateTime.UtcNow;
                request.Status = DTO.Enums.RequestStatuses.Draft;
                request.AdapterPackageVersion = "5.2.0.0";
                request.WorkFlowActivityID = new Guid("C1380001-4524-49BA-B4B6-A3B5013A3343");//create request of default workflow
                request.Description = string.Empty;

                PmnTask task = new PmnTask();
                db.Actions.Add(task);
                Console.WriteLine("Task ID: " + task.ID.ToString("D"));

                task.Status = DTO.Enums.TaskStatuses.InProgress;
                task.Type = DTO.Enums.TaskTypes.Task;
                task.StartOn = DateTime.UtcNow;
                task.Subject = "Design request";
                task.WorkflowActivityID = request.WorkFlowActivityID.Value;

                task.References.Add(
                    new TaskReference { 
                        TaskID = task.ID,
                        ItemID = request.ID,
                        Type = DTO.Enums.TaskItemTypes.User
                    }
                );

                byte[] documentContent = System.Text.Encoding.UTF8.GetBytes("This is test document content.");

                Document d1 = new Document
                {
                    FileName = "test.txt",
                    Name = "test.txt",
                    Description = "A test document.",
                    Length = documentContent.Length,
                    MimeType = "text/plain",
                    ItemID = task.ID,
                    UploadedByID = request.CreatedByID
                };

                Document d2 = new Document
                {
                    FileName = "test2.txt",
                    Name = "test2.txt",
                    Description = "A test document.",
                    Length = documentContent.Length,
                    MimeType = "text/plain",
                    ItemID = task.ID,
                    UploadedByID = request.CreatedByID
                };

                Document d3 = new Document
                {
                    FileName = "test3.txt",
                    Name = "test3.txt",
                    Description = "A test document.",
                    Length = documentContent.Length,
                    MimeType = "text/plain",
                    ItemID = task.ID,
                    UploadedByID = request.CreatedByID
                };

                Document d4 = new Document
                {
                    FileName = "test4.txt",
                    Name = "test4.txt",
                    Description = "A test document.",
                    Length = documentContent.Length,
                    MimeType = "text/plain",
                    ItemID = task.ID,
                    UploadedByID = request.CreatedByID
                };
                
                d1.RevisionSetID = d1.ID;
                d2.RevisionSetID = d1.ID;
                d3.RevisionSetID = d1.ID;
                d4.RevisionSetID = d1.ID;
                Console.WriteLine("Document revision set ID: " + d1.ID.ToString("D"));

                db.Documents.AddRange(new[] { d1, d2, d3, d4 });

                db.SaveChanges();

                d1.SetData(db, documentContent);
                d2.SetData(db, documentContent);
                d3.SetData(db, documentContent);
                d4.SetData(db, documentContent);
            }
        }

    }
}
