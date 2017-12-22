namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateUserMembership : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Permissions", "Type", c => c.Int(nullable: false));
            CreateIndex("dbo.Permissions", "Type");

            //Translate the membership

            Sql(@"TRUNCATE TABLE SecurityGroupUsers

INSERT INTO SecurityGroups (ID, Name, OwnerID, [Type]) SELECT [End], Users.Username, Users.OrganizationID, 1 FROM SecurityMembershipClosure INNER JOIN Users ON SecurityMembershipClosure.Start = Users.ID WHERE SecurityMembershipClosure.Start = SecurityMembershipClosure.[End] AND NOT EXISTS(SELECT NULL FROM SecurityGroups sg where sg.ID = SecurityMembershipClosure.Start)


INSERT INTO SecurityGroupUsers (SecurityGroupID, UserID)
SELECT DISTINCT [End], [Start] FROM SecurityMembershipClosure WHERE EXISTS(SELECT NULL FROM Users WHERE Users.ID = SecurityMembershipClosure.Start)");

            //Add all of the permissions

            //Portal level
            Sql(
                "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('5FBA8EF3-F9A3-4ACC-A3D0-09905FA16E8E', 'Login', 'Allows the user to login to the portal', 0)");

            Sql(
                "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('5ECFEC21-CD59-4505-B7F2-F52FFC4C263E', 'List Users', 'Allows the user list to be viewed', 0)");

            Sql(
                "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('ECD72B1B-50F5-4E3A-BED2-375880435FD1', 'List DataMarts', 'Allows the DataMart list to be viewed', 0)");

            Sql(
                "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('FFAB8A4A-35FB-4EE7-A946-5874DE13BA58', 'List Organizations', 'Allows the Organization list to be viewed', 0)");

            Sql(
                "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('65C197D9-8A69-4350-AA73-C5F6E252C84E', 'List Security Groups', 'Allows the Security Group list to be viewed', 0)");

            Sql(
                "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('FB9B0C98-7BFD-4479-ABE5-0DC093ED44CD', 'List Organization Groups', 'Allows the Organization Group list to be viewed', 0)");

            Sql(
                "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('D5C2E426-80C9-40C4-81FB-89ADF85F6362', 'List Tasks', 'Allows the Task list to be viewed', 0)");

            Sql(
                "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('5652252C-0265-4E47-8480-6FEF4690B7A5', 'Create Organizations', 'Allows the creation of Organizations', 0)");

            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('064FBC63-B8F1-4C31-B5AB-AB42DE5779C75', 'Create Organization Groups', 'Allows the creation of Organization Groups', 0)");

            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('E7EFB727-AE14-49D9-8D73-F691B00B8251', 'Create Shared Folders', 'Allows the creation of Shared Folders', 0)");

            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('BA7687E7-E149-4772-8F3F-7C8568769998', 'Run Events Report', 'Allows the user or group to run the Events Report', 0)");

            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('860CEFFB-3006-48B1-AC47-60BDC9C3FD35', 'List Registries', 'Allows the Registry list to be viewed', 0)");

    //        Sql(
    //"INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('39A642B4-E782-4051-9329-3A7246052E16', 'Create Registries', 'Allows the creation of Registries', 0)");

            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('4F3914D9-BD36-4B9F-A6B9-A368199BA94C', 'Create Network Messages', 'Allows the creation of network-wide messages', 0)");

            //Crud
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('1B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 'Edit', 'Allows editing of the item', 0)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('1C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 'Delete', 'Allows deleting of the item', 0)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('4CCB0EC2-006D-4345-895E-5DD2C6C8C791', 'View', 'Allows viewing of the item', 0)");

            //Organization
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('AF37A115-9D40-4F38-8BAF-4B050AC6F185', 'Create Users', 'Allows the creation of Users on the selected Organization', 3)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('135F153D-D0BE-4D51-B55C-4B8807E74584', 'Create DataMarts', 'Allows the creation of DataMarts on the selected Organization', 3)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('92F1A228-44E4-4A5A-9C78-0FC37F4B18C6', 'Create Registries', 'Allows the creation of Registries on the selected Organization', 3)"); //Want to remove this one
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('ECF3B864-7DB3-497B-A2E4-F2B435EF2803', 'Approve/Reject Registrations', 'Allows the user to approve or reject requests for registrations on the portal', 3)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('F9870001-7C06-4B4B-8F76-A2A701102FF0', 'Administer Web-based DataMart', 'Allows the user to administer the settings for the Web-based DataMart client', 3)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('64A00001-A1D6-41DD-AB20-A2B200EEB9A3', 'Copy Organization', 'Allows the user copy the selected organization', 3)");


            //DataMart
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('F487C17A-873B-489B-A0AC-92EC07976D4A', 'Request Metadata Update', 'Allows the user request changes to the metadata for the selected DataMart', 1)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('7710B3EA-B91E-4C85-978F-6BFCDE8C817C', 'Install Models', 'Allows the user to install new data models into the selected DataMart', 1)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('D4770F67-7DB5-4D47-9413-CA1C777179C9', 'Un-Install Models', 'Allows the user to un-install data models from the selected DataMart', 1)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('EFC6DA52-1625-4209-9BBA-5C4BF1D38188', 'Run DataMart Audit Report', 'Allows the user to run the DataMart audit report for the selected DataMart', 1)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('BB640001-5BA7-4658-93AF-A2B201579BFA', 'Copy DataMart', 'Allows the user to copy the selected DataMart', 1)");

            //DataMart In Project
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('5D6DD388-7842-40A1-A27A-B9782A445E20', 'See Request Queue', 'Allows the user see requests for this DataMart and Project', 11)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('0AC48BA6-4680-40E5-AE7A-F3436B0852A0', 'Upload Results', 'Allows the user to upload results using the DataMart Client', 11)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('894619BE-9A73-4DA9-A43A-10BCC563031C', 'Hold Requests', 'Allows the user to place a hold on submitted requests', 11)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('0CABF382-93D3-4DAC-AA80-2DE500A5F945', 'Reject Requests', 'Allows the user to reject submitted requests', 11)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', 'Approve/Reject Response', 'Allows the user to approve or reject responses', 11)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('A0F5B621-277A-417C-A862-801D7B9030A2', 'Skip Response Approval', 'Allows the user to skip response approval when submitting response', 11)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('F6D18BEF-BD4F-4484-AAC2-C80DB3A505EE', 'Group/Ungroup Responses', 'Allows the user to group/ungroup responses', 11)");

            //User
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('4A7C9495-BB01-4EA7-9419-65ACE6B24865', 'Change Password', 'Allows the user change the password on the selected user', 9)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('92687123-6F38-400E-97EC-C837AA92305F', 'Change Login', 'Allows the user change the login on the selected user', 9)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('22FB4F13-0492-417F-ACA1-A1338F705748', 'Manage Notifications', 'Allows the user manage notifications for the selected user', 9)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('FDE2D32E-A045-4062-9969-00962E182367', 'Change Certificate', 'Allows the user change the selected user''s X509 Certificate', 9)");

            //Group
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('8C5E44DC-284E-45D8-A014-A0CD815883AE', 'List Projects', 'Allows the user to list projects that are part of the group', 2)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('93623C60-6425-40A0-91A0-01FA34920913', 'Create Projects', 'Allows the user to create projects in the group', 2)");

            //Project
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('8DCA22F0-EA18-4353-BA45-CC2692C7A844', 'List Requests', 'Allows the user to list requests that are assigned to the selected project', 4)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('B3D4266D-5DC6-497E-848F-567442F946F4', 'Resubmit Requests', 'Allows the user to resubmit requests that are assigned to the selected project', 4)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('25BD0001-4739-41D8-BC74-A2AF01733B64', 'Copy Project', 'Allows the user to copy the selected project', 4)");

            //Project, DataMart & Request Type
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('BA5C26D1-448B-4D7D-B237-0E0F04F406E3', 'Submit For Manual Processing', 'Allows the user to submit a request to be manually processed on the DataMart Client', 12)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('FB5EC59C-7129-41C2-8B77-F4E328E1729B', 'Submit For Automatic Processing', 'Allows the user to submit a request to be automatically processed on the DataMart Client', 12)");

            //Request Shared Folder
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('A811302C-9352-45A2-A721-C16E510C4738', 'Add Requests', 'Allows the user to add a request to a shared folder', 8)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('333A8C57-6543-4C6D-B9DA-8B06E186F71D', 'Remove Requests', 'Allows the user remove a request from a shared folder', 8)");

            //Event
            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('E1A20001-2BE1-48EA-8FC6-A22200E7A7F9', 'Observe Event', 'Allows the user to get notifications of the specified event', 13)");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Permissions", "Type");
        }
    }
}
