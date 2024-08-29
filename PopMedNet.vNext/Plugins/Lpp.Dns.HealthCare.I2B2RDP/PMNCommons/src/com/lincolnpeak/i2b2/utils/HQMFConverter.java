package com.lincolnpeak.i2b2.utils;

import java.util.MissingResourceException;
import java.util.ResourceBundle;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

import com.lincolnpeak.i2b2.exceptions.CannotRunRequest;

public class HQMFConverter 
{
	protected final static Log log = LogFactory.getLog(HQMFConverter.class);

	private static String HQMF_URL = "http://localhost:9090/jersey/hqmf"; // Backup
	private static String PMN_RESOURCE_BUNDLE = "com.lincolnpeak.i2b2.utils.PMNCommons";

	public static String HQMFtoI2B2(String hqmf, String domain, String project, String user, String password) throws CannotRunRequest
	{
		log.debug("About to convert the following hqmf:");
		log.debug(hqmf);
		
		if(!getDoConvert())
			return hqmf;
		
		String url = getHQMFUrl() + "/toi2b2/" + domain + "/" + project + "/" + user + "/password/" + password; 
		String i2b2 = RESTRequestHelper.postRequest(url, hqmf);
		
		log.debug("Converted result:");
		log.debug(i2b2);
		return i2b2;
	}

	public static String I2B2toHQMF(String i2b2) throws CannotRunRequest
	{
		log.debug("About to convert the following i2b2 message:");
		log.debug(i2b2);
		
		if(!getDoConvert())
			return i2b2;
		
		String url = getHQMFUrl() + "/tohqmf";
		String hqmf = RESTRequestHelper.postRequest(url, i2b2);
		
		log.debug("Converted result:");
		log.debug(hqmf);
		return hqmf;
	}
	
	protected static String getHQMFUrl()
	{
		String hqmfUrl = "";
		try
		{
			ResourceBundle resource = ResourceBundle.getBundle(PMN_RESOURCE_BUNDLE);
			hqmfUrl = resource.getString("hqmfUrl");
		}
		catch(MissingResourceException e)
		{
			hqmfUrl = HQMF_URL;
		}
		
		return hqmfUrl;
	}
	
	private static boolean getDoConvert()
	{
		boolean doConvert = false;
		try
		{
			ResourceBundle resource = ResourceBundle.getBundle(PMN_RESOURCE_BUNDLE);
			doConvert = Boolean.parseBoolean(resource.getString("convertHQMF"));
		}
		catch(MissingResourceException e)
		{
		}
		
		return doConvert;
	}
}
