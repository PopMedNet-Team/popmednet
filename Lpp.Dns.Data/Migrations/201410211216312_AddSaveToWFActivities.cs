namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSaveToWFActivities : DbMigration
    {
        public override void Up()
        {
            Sql(@"-- Summary workflow updates
-- Draft request
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('495288DD-8B92-4240-85B6-25A9144A8192', 'Save', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('495288DD-8B92-4240-85B6-25A9144A8192','197AF4BA-F079-48DD-9E7C-C7BE7F8DC896','197AF4BA-F079-48DD-9E7C-C7BE7F8DC896')
--INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('EF63E416-D823-4D0D-8A27-A2F1780CEB49', 'Copy', '')
--INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('EF63E416-D823-4D0D-8A27-A2F1780CEB49','197AF4BA-F079-48DD-9E7C-C7BE7F8DC896','197AF4BA-F079-48DD-9E7C-C7BE7F8DC896')

-- Review request
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('535E94E5-4D2E-438C-B9F1-37E5B6E2EF02', 'Save', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('535E94E5-4D2E-438C-B9F1-37E5B6E2EF02','CC1BCADD-4487-47C7-BDCA-1010F2C68FE0','CC1BCADD-4487-47C7-BDCA-1010F2C68FE0')
--INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('41B21C4D-2ACD-4F60-9EEC-0759BD2F697A', 'Copy', '')
--INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('41B21C4D-2ACD-4F60-9EEC-0759BD2F697A','CC1BCADD-4487-47C7-BDCA-1010F2C68FE0','197AF4BA-F079-48DD-9E7C-C7BE7F8DC896')

-- Distribute request
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('6410D92A-2AC8-43F8-A06F-D4EE3274BC81', 'Save', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('6410D92A-2AC8-43F8-A06F-D4EE3274BC81','752B83D7-2190-49DF-9BAE-983A7880A899','752B83D7-2190-49DF-9BAE-983A7880A899')
--INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('8A13D4C1-4E8B-4844-AEA5-76622321D4D5', 'Copy', '')
--INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('8A13D4C1-4E8B-4844-AEA5-76622321D4D5','752B83D7-2190-49DF-9BAE-983A7880A899','197AF4BA-F079-48DD-9E7C-C7BE7F8DC896')

-- Review Status and Responses
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('1A9EBEAC-09CB-4BBC-952C-52A1DEB31094', 'View Response', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('1A9EBEAC-09CB-4BBC-952C-52A1DEB31094','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B')
--INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('83933B94-EFC6-4EBF-B265-8CA0E845E690', 'Copy', '')
--INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('83933B94-EFC6-4EBF-B265-8CA0E845E690','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B','197AF4BA-F079-48DD-9E7C-C7BE7F8DC896')


-- Modular Program workflow updates
-- Draft request
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('1E6E14B1-0D41-4E2B-B6E3-289F742296DA', 'Save', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('1E6E14B1-0D41-4E2B-B6E3-289F742296DA','0321E17F-AA1F-4B23-A145-85B159E74F0F','0321E17F-AA1F-4B23-A145-85B159E74F0F')
--INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('25D966ED-F375-491D-A08A-7C89B5518F14', 'Copy', '')
--INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('25D966ED-F375-491D-A08A-7C89B5518F14','0321E17F-AA1F-4B23-A145-85B159E74F0F','0321E17F-AA1F-4B23-A145-85B159E74F0F')

-- Request review
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('FB23A7FF-0A38-4F5E-9457-CCCD74170219', 'Save', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('FB23A7FF-0A38-4F5E-9457-CCCD74170219','A96FBAD0-8FD8-4D10-8891-D749A71912F8','A96FBAD0-8FD8-4D10-8891-D749A71912F8')
--INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('FE675055-04E8-4F15-BFB2-EDE5AC2CA090', 'Copy', '')
--INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('FE675055-04E8-4F15-BFB2-EDE5AC2CA090','A96FBAD0-8FD8-4D10-8891-D749A71912F8','0321E17F-AA1F-4B23-A145-85B159E74F0F')

-- Working Specification
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('3512F473-3E1B-460E-9EAF-12B8DA986ACD', 'Save', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('3512F473-3E1B-460E-9EAF-12B8DA986ACD','31C60BB1-2F6A-423B-A7B7-B52626FD9E97','31C60BB1-2F6A-423B-A7B7-B52626FD9E97')
--INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('FE675055-04E8-4F15-BFB2-EDE5AC2CA090', 'Copy', '')
--INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('FE675055-04E8-4F15-BFB2-EDE5AC2CA090','A96FBAD0-8FD8-4D10-8891-D749A71912F8','0321E17F-AA1F-4B23-A145-85B159E74F0F')

-- Working Specification Review
--INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('CA8E1BC3-145B-4D80-B988-30F89B3824B8', 'Save', '')
--INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('3512F473-3E1B-460E-9EAF-12B8DA986ACD','C8891CFD-80BF-4F71-90DE-6748BF71566C','C8891CFD-80BF-4F71-90DE-6748BF71566C')

-- Specification
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('ADCA656A-8052-4F32-9AC5-5617870C53CE', 'Save', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('ADCA656A-8052-4F32-9AC5-5617870C53CE','C3B13067-3B9D-41E4-8D4A-7114A6E81930','C3B13067-3B9D-41E4-8D4A-7114A6E81930')

-- Specification Review
--INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('ABF9B3EB-7DD3-443E-B92B-06F07E1CB21F', 'Save', '')
--INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('ABF9B3EB-7DD3-443E-B92B-06F07E1CB21F','948B60F0-8CE5-4B14-9AD6-C50EC37DFC77','948B60F0-8CE5-4B14-9AD6-C50EC37DFC77')

-- Pre-distribution Testing
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('04338B5D-9507-4E04-9701-37F55ACB2966', 'Save', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('04338B5D-9507-4E04-9701-37F55ACB2966','49D9E9E8-B1A6-41C8-A36B-4D5FDED3A9B7','49D9E9E8-B1A6-41C8-A36B-4D5FDED3A9B7')

-- Pre-distribution Testing Review
--INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('113AC186-3574-49E4-8918-B905E1476C15', 'Save', '')
--INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('113AC186-3574-49E4-8918-B905E1476C15','EA69E5ED-6029-47E8-9B45-F0F00B07FDC2','EA69E5ED-6029-47E8-9B45-F0F00B07FDC2')

-- View Status and Results
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('37B099CF-329A-425B-B40D-2A7CB52C7767', 'View Results', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('37B099CF-329A-425B-B40D-2A7CB52C7767','D0E659B8-1155-4F44-9728-B4B6EA4D4D55','D0E659B8-1155-4F44-9728-B4B6EA4D4D55')

-- Prepare Draft Report
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('A225E59A-656C-4A63-B5E4-82416D6351D8', 'Save', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('A225E59A-656C-4A63-B5E4-82416D6351D8','43AB48FD-A400-4C0D-92C9-DD2415A5D5B6','43AB48FD-A400-4C0D-92C9-DD2415A5D5B6')

-- Prepare Draft Report Review
--INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('F2D122EB-08B8-4DE3-A0F6-FFA94CB04C36', 'Save', '')
--INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('F2D122EB-08B8-4DE3-A0F6-FFA94CB04C36','C80810A3-CF10-4941-854A-A7E2052A5EBA','C80810A3-CF10-4941-854A-A7E2052A5EBA')

-- Prepare Final Report
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('A54277C0-F08D-4401-AE0D-9F18D2642025', 'Save', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('A54277C0-F08D-4401-AE0D-9F18D2642025','1C6CCB68-6B62-4F10-A605-2FB6EC64B8C9','1C6CCB68-6B62-4F10-A605-2FB6EC64B8C9')
");
        }
        
        public override void Down()
        {
            Sql(@"DELETE WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '495288DD-8B92-4240-85B6-25A9144A8192' and SourceWorkflowActivityID = '197AF4BA-F079-48DD-9E7C-C7BE7F8DC896' and DestinationWorkflowActivityID = '197AF4BA-F079-48DD-9E7C-C7BE7F8DC896' 
DELETE WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '535E94E5-4D2E-438C-B9F1-37E5B6E2EF02' and SourceWorkflowActivityID = 'CC1BCADD-4487-47C7-BDCA-1010F2C68FE0' and DestinationWorkflowActivityID = 'CC1BCADD-4487-47C7-BDCA-1010F2C68FE0' 
DELETE WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '6410D92A-2AC8-43F8-A06F-D4EE3274BC81' and SourceWorkflowActivityID = '752B83D7-2190-49DF-9BAE-983A7880A899' and DestinationWorkflowActivityID = '752B83D7-2190-49DF-9BAE-983A7880A899' 
DELETE WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '1A9EBEAC-09CB-4BBC-952C-52A1DEB31094' and SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' and DestinationWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' 
DELETE WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '1E6E14B1-0D41-4E2B-B6E3-289F742296DA' and SourceWorkflowActivityID = '0321E17F-AA1F-4B23-A145-85B159E74F0F' and DestinationWorkflowActivityID = '0321E17F-AA1F-4B23-A145-85B159E74F0F' 
DELETE WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'FB23A7FF-0A38-4F5E-9457-CCCD74170219' and SourceWorkflowActivityID = 'A96FBAD0-8FD8-4D10-8891-D749A71912F8' and DestinationWorkflowActivityID = 'A96FBAD0-8FD8-4D10-8891-D749A71912F8' 
DELETE WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '3512F473-3E1B-460E-9EAF-12B8DA986ACD' and SourceWorkflowActivityID = '31C60BB1-2F6A-423B-A7B7-B52626FD9E97' and DestinationWorkflowActivityID = '31C60BB1-2F6A-423B-A7B7-B52626FD9E97' 
DELETE WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'ADCA656A-8052-4F32-9AC5-5617870C53CE' and SourceWorkflowActivityID = 'C3B13067-3B9D-41E4-8D4A-7114A6E81930' and DestinationWorkflowActivityID = 'C3B13067-3B9D-41E4-8D4A-7114A6E81930' 
DELETE WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '04338B5D-9507-4E04-9701-37F55ACB2966' and SourceWorkflowActivityID = '49D9E9E8-B1A6-41C8-A36B-4D5FDED3A9B7' and DestinationWorkflowActivityID = '49D9E9E8-B1A6-41C8-A36B-4D5FDED3A9B7' 
DELETE WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '37B099CF-329A-425B-B40D-2A7CB52C7767' and SourceWorkflowActivityID = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55' and DestinationWorkflowActivityID = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55' 
DELETE WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'A225E59A-656C-4A63-B5E4-82416D6351D8' and SourceWorkflowActivityID = '43AB48FD-A400-4C0D-92C9-DD2415A5D5B6' and DestinationWorkflowActivityID = '43AB48FD-A400-4C0D-92C9-DD2415A5D5B6' 
DELETE WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'A54277C0-F08D-4401-AE0D-9F18D2642025' and SourceWorkflowActivityID = '1C6CCB68-6B62-4F10-A605-2FB6EC64B8C9' and DestinationWorkflowActivityID = '1C6CCB68-6B62-4F10-A605-2FB6EC64B8C9' 
DELETE FROM WorkflowActivityResults WHERE ID IN ('495288DD-8B92-4240-85B6-25A9144A8192', '535E94E5-4D2E-438C-B9F1-37E5B6E2EF02', '6410D92A-2AC8-43F8-A06F-D4EE3274BC81', '1A9EBEAC-09CB-4BBC-952C-52A1DEB31094', '1E6E14B1-0D41-4E2B-B6E3-289F742296DA', 'FB23A7FF-0A38-4F5E-9457-CCCD74170219', '3512F473-3E1B-460E-9EAF-12B8DA986ACD', 'ADCA656A-8052-4F32-9AC5-5617870C53CE', '04338B5D-9507-4E04-9701-37F55ACB2966', '37B099CF-329A-425B-B40D-2A7CB52C7767', 'A225E59A-656C-4A63-B5E4-82416D6351D8', 'A54277C0-F08D-4401-AE0D-9F18D2642025')
");
        }
    }
}
