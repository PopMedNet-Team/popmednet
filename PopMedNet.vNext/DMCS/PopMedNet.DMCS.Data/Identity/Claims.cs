using System;
using System.Collections.Generic;
using System.Text;

namespace PopMedNet.DMCS.Data.Identity
{
    public static class Claims
    {
        public static Guid CanView = new Guid("5D6DD388-7842-40A1-A27A-B9782A445E20");
        public static Guid CanUpload = new Guid("0AC48BA6-4680-40E5-AE7A-F3436B0852A0");
        public static Guid CanHold = new Guid("894619BE-9A73-4DA9-A43A-10BCC563031C");
        public static Guid CanReject = new Guid("0CABF382-93D3-4DAC-AA80-2DE500A5F945");
        public static Guid CanModifyResults = new Guid("80500001-D58E-4EEE-8541-A7CA010034F5");
        public static Guid CanViewAttachments = new Guid("50157D72-8EED-45E4-B6F4-2A935191F57F");
        public static Guid CanModifyAttachments = new Guid("D59FA0D4-15FA-4088-9A98-35CDD7902EC1");



        //public const string FirstName_Key = System.Security.Claims.ClaimTypes.GivenName;
        //public const string LastName_Key = System.Security.Claims.ClaimTypes.Surname;
        //public const string Phone_Key = System.Security.Claims.ClaimTypes.OtherPhone;
        ////do not include NameIdentifier in the key's collection so that it does not affect the sync process
        //public const string UserID_Key = System.Security.Claims.ClaimTypes.NameIdentifier;

        //public static readonly string[] Keys = new[] {
        //    FirstName_Key,
        //    LastName_Key,
        //    Phone_Key,
        //};
    }
}
