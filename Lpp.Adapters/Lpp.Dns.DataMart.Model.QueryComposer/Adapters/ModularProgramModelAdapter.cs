﻿using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.ModularProgram
{
    public class ModularProgramModelAdapter : ModelAdapter
    {
        static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ModularProgramModelAdapter(RequestMetadata requestMetadata) : base(new Guid("1B0FFD4C-3EEF-479D-A5C4-69D8BA0D0154"), requestMetadata) { }

        public override bool CanRunAndUpload
        {
            get
            {
                return false;
            }
        }

        public override bool CanAddResponseFiles
        {
            get
            {
                return true;
            }
        }

        public override bool CanUploadWithoutRun
        {
            get
            {
                return true;
            }
        }

        public override bool CanViewSQL
        {
            get
            {
                return false;
            }
        }

        public override IEnumerable<DTO.QueryComposer.QueryComposerResponseQueryResultDTO> Execute(DTO.QueryComposer.QueryComposerQueryDTO query, bool viewSQL)
        {
            //modular program doesn't actually upload anything.
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
        }
    }
}
