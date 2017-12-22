using System;
using System.Collections.Generic;
using Lpp.Tests;

namespace Lpp.FileSystem.Tests
{
    public class DbFixture : DatabaseFixture
    {
        protected override IEnumerable<string> DatabaseCreateScript()
        {
            return new[] { Properties.Resources.DDL };
        }
    }

    public class TestDomain { }
}