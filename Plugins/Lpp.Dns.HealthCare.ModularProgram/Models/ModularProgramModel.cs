using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Lpp.Dns.HealthCare.Models;
using Lpp.Dns.HealthCare.ModularProgram.Data.Serializer;

namespace Lpp.Dns.HealthCare.ModularProgram.Models
{
    public enum ProgramTypeCode
    {
        ModularProgram1 = 1,
        ModularProgram2 = 2,
        ModularProgram3 = 3,
        ModularProgram4 = 4,
        ModularProgram5 = 5,
        ModularProgram6 = 6,
        ModularProgram7= 7,
        DrugUse = 8,
        Other = 9
    }
    
    public class ModularProgramType
    {
        public ModularProgramType()
        {
        }

        public ModularProgramType(string name, ProgramTypeCode code)
        {
            Name = name;
            Code = code;
        }

        public string Name { get; set; }
        public ProgramTypeCode Code { get; set; }

        public static string GetName(ProgramTypeCode code)
        {
            foreach (ModularProgramType programType in ModularProgramTypes)
            {
                if (programType.Code == code)
                    return programType.Name;
            }
            return null;
        }

        public static IEnumerable<ModularProgramType> ModularProgramTypes = new List<ModularProgramType> {
            new ModularProgramType("Program 1", ProgramTypeCode.ModularProgram1),
            new ModularProgramType("Program 2", ProgramTypeCode.ModularProgram2),
            new ModularProgramType("Program 3", ProgramTypeCode.ModularProgram3),
            new ModularProgramType("Program 4", ProgramTypeCode.ModularProgram4),
            new ModularProgramType("Program 5", ProgramTypeCode.ModularProgram5),
            new ModularProgramType("Program 6", ProgramTypeCode.ModularProgram6),
            new ModularProgramType("Program 7", ProgramTypeCode.ModularProgram7),
            new ModularProgramType("Drug Use", ProgramTypeCode.DrugUse),
            new ModularProgramType("Other", ProgramTypeCode.Other)
        };
        /*
        public static Lpp.Dns.HealthCare.ModularProgram.Data.Serializer.ProgramTypeCode Convert(ProgramTypeCode ProgramType)
        {
            Lpp.Dns.HealthCare.ModularProgram.Data.Serializer.ProgramTypeCode code = Data.Serializer.ProgramTypeCode.Other;
            Enum.TryParse<Lpp.Dns.HealthCare.ModularProgram.Data.Serializer.ProgramTypeCode>(ProgramType.ToString(), true, out code);
            return code;
        }

        public static ProgramTypeCode Convert(Lpp.Dns.HealthCare.ModularProgram.Data.Serializer.ProgramTypeCode ProgramType)
        {
            ProgramTypeCode code = ProgramTypeCode.Other;
            Enum.TryParse<ProgramTypeCode>(ProgramType.ToString(), true, out code);
            return code;
        }
        */
    }

    public class ModularProgramItem
    {
        public ModularProgramItem()
        {
        }

        public ModularProgramItem(String typeCode, String programName, String description, String scenarios)
        {

            ProgramName = programName;
            TypeCode = typeCode;
            Description = description;
            Scenarios = scenarios;
        }
        public String ProgramName { get; set; }
        public String TypeCode { get; set; }
        public String Description { get; set; }
        public String Scenarios { get; set; }
    }

    public class File
    {
        public File(String fileName, String size)
        {
            FileName = fileName;
            Size = size;
        }
        public String FileName { get; set; }
        public String Size { get; set; }
    }

    [XmlRootAttribute("ModularProgramModel", Namespace = "", IsNullable = false)]
    public class ModularProgramModel
    {
        public ModularProgramModel()
        {
            PackageManifest = new List<ModularProgramItem>();
            FileLinks = new List<String>();
            RequestFileList = new List<FileSelection>();
        }
        public List<ModularProgramItem> PackageManifest { get; set; }
        public List<String> FileLinks { get; set; }
        public List<FileItem> Files { get; set; }
        public List<FileSelection> RequestFileList { get; set; }    // Files that have previously been uploaded for the request
        public ModularProgramRequestType RequestType { get; set; }
        public string ModularProgramList { get; set; }
        public String RequestTypeName { get; set; }
        public Guid RequestId { get; set; }
        public String UploadedFileList { get; set; }                // Return area for files that have been uploaded with Multifile Upload control
        public String RemovedFileList { get; set; }                 // Return area for files that have been marked for removal 
        public string FileLinkList { get; set; }                    // Return area for file links
        public int ModularProgramType { get; set; }
        public string SignatureData { get; set; }
        public bool HasResponses { get; set; }
    }
}

