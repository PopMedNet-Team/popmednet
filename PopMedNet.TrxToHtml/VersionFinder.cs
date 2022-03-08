using System;
using System.Xml;

namespace PopMedNet.TrxToHtml
{
	internal sealed class VersionFinder
	{
		internal SupportedFormats GetFileVersion(string file)
		{
			SupportedFormats result = SupportedFormats.unknown;
			using (XmlReader reader = XmlReader.Create(file))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(reader);
				if (CheckVersion(xmlDocument.DocumentElement))
				{
					result = SupportedFormats.vs2005;
				}
				if (IsVS2008(xmlDocument.DocumentElement))
				{
					result = SupportedFormats.vs2008;
				}
				if (IsVS2010(xmlDocument.DocumentElement))
				{
					return SupportedFormats.vs2010;
				}
			}
			return result;
		}

		private bool CheckVersion(XmlElement e)
		{
			try
			{
				if (e.Name == "Tests" && e.ChildNodes[0].Name == "edtdocversion" && Convert.ToInt32(e.ChildNodes[0].Attributes["build"].Value) > 50726)
				{
					return true;
				}
			}
			catch
			{
			}
			return false;
		}

		private bool IsVS2008(XmlElement e)
		{
			try
			{
				if (e.Name == "TestRun" && e.NamespaceURI == "http://microsoft.com/schemas/VisualStudio/TeamTest/2006")
				{
					return true;
				}
			}
			catch
			{
			}
			return false;
		}

		private bool IsVS2010(XmlElement e)
		{
			try
			{
				if (e.Name == "TestRun" && e.NamespaceURI == "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")
				{
					return true;
				}
			}
			catch
			{
			}
			return false;
		}
	}
}
