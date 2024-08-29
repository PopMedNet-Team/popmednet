package com.lincolnpeak.i2b2.utils;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.Date;

import junit.framework.Assert;

import org.junit.Test;


public class HQMFConverterTests 
{
	@Test
	public void testRountripConversion()
	{
		try
		{
			String i2b2 = getI2B2Message();
			String hqmf = HQMFConverter.I2B2toHQMF(i2b2);
			String i2b2new = HQMFConverter.HQMFtoI2B2(hqmf, "i2b2demo", "Demo", "demo", "demouser");
			Assert.assertEquals("", i2b2, i2b2new);
		}
		catch(Exception e)
		{
			Assert.fail(e.getMessage());
		}
	}
	
	private String getI2B2Message() throws IOException
	{
		BufferedReader reader = null;
		try
		{
			reader = new BufferedReader(new InputStreamReader(this.getClass().getResourceAsStream("/com/lincolnpeak/i2b2/utils/Sample_CRC_QRY_runQueryInstance_fromQueryDefinition.xml")));

			String i2b2Message = "";
			String line;
			while((line = reader.readLine()) != null)
				i2b2Message += line;
									
			return i2b2Message;
		}
		finally
		{
			if(reader != null)
				reader.close();
		}
				
	}		
}
