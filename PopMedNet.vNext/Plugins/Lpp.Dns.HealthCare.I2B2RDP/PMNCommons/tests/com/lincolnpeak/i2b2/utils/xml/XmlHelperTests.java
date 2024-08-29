package com.lincolnpeak.i2b2.utils.xml;

import org.junit.Assert;
import org.junit.Test;

import com.lincolnpeak.i2b2.exceptions.CannotGetElementValue;


public class XmlHelperTests 
{
	@Test
	public void testSecurity()
	{
		try 
		{
			String xml = "<message_header>" +
							"<security>" +
								"<domain>i2b2demo</domain>" +
								"<username>demo</username>" +
								"<password token_ms_timeout=\"1800000\" is_token=\"true\">SessionKey:wN2Z3mWBGjQZhGisDqol</password>" +
							"</security>" +
						 "</message_header>";
			
			Security security = (Security) XmlHelper.getElementValue(xml, "security", Security.class);
			Assert.assertEquals("i2b2demo", security.getDomain());
			Assert.assertEquals("demo", security.getUsername());
			Assert.assertEquals("SessionKey:wN2Z3mWBGjQZhGisDqol", security.getPassword());
		} 
		catch (CannotGetElementValue e) 
		{
			Assert.fail(e.getMessage());
		}
	}
	
	@Test
	public void testRequestType()
	{
		try 
		{
			String xml = "<ns4:psmheader>" +
							"<user group=\"Demo\" login=\"demo\">demo</user>" +
							"<patient_set_limit>0</patient_set_limit>" +
							"<request_type>CRC_QRY_runQueryInstance_fromQueryDefinition</request_type>" +
						 "</ns4:psmheader>";
			
			RequestType requestType = (RequestType) XmlHelper.getElementValue(xml, "request_type", RequestType.class);
			Assert.assertEquals("CRC_QRY_runQueryInstance_fromQueryDefinition", requestType.getValue());
		} 
		catch (CannotGetElementValue e) 
		{
			Assert.fail(e.getMessage());
		}
	}
}
