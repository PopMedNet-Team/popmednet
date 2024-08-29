package com.lincolnpeak.i2b2;

import java.util.MissingResourceException;
import java.util.ResourceBundle;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

public class PMNResource 
{
	protected final Log log = LogFactory.getLog(getClass());

	private static PMNResource instance;
	
	private static final String PMN_RESOURCE_BUNDLE = "com.lincolnpeak.i2b2.crc.axis2.QueryService";
	private static final String DEFAULT_SERVICE_URL = "http://localhost:3872/api/rest/remote";
	private static final String DEFAULT_I2B2HIVE_URL = "http://localhost:9090/i2b2/rest/QueryToolService";
	
	public static PMNResource getInstance()
	{
		if(instance == null)
			instance = new PMNResource();
		
		return instance;
	}
	
	public String getProxyUser()
	{
		String proxyUser = "";
		try
		{
			ResourceBundle resource = ResourceBundle.getBundle(PMN_RESOURCE_BUNDLE);
			proxyUser = resource.getString("proxyUser");
		}
		catch(MissingResourceException e)
		{
			log.error(e);
			proxyUser = "Investigator";
		}
		
		return proxyUser;
	}
	
	public String getProxyPassword()
	{
		String proxyPassword = "";
		try
		{
			ResourceBundle resource = ResourceBundle.getBundle(PMN_RESOURCE_BUNDLE);
			proxyPassword = resource.getString("proxyPassword");
		}
		catch(MissingResourceException e)
		{
			log.error(e);
			proxyPassword = "Password1!";
		}
		
		return proxyPassword;
	}
	
	public String getPMNRequestType()
	{
		String pmnRequestType = "";
		try
		{
			ResourceBundle resource = ResourceBundle.getBundle(PMN_RESOURCE_BUNDLE);
			pmnRequestType = resource.getString("pmnRequestType");
		}
		catch(MissingResourceException e)
		{
			log.error(e);
			pmnRequestType = "%7B6EB513D8-C916-4FEE-BA10-70E7ACE3AAB5%7D";
		}
		
		return pmnRequestType;
	}
	
	public String getServiceUrl()
	{
		String serviceUrl = "";
		try
		{
			ResourceBundle resource = ResourceBundle.getBundle(PMN_RESOURCE_BUNDLE);
			serviceUrl = resource.getString("serviceUrl");
		}
		catch(MissingResourceException e)
		{
			log.error(e);
			serviceUrl = DEFAULT_SERVICE_URL;
		}
		
		return serviceUrl;
	}
	
	public String getI2B2HiveUrl()
	{
		String i2b2HiveUrl = "";
		try
		{
			ResourceBundle resource = ResourceBundle.getBundle(PMN_RESOURCE_BUNDLE);
			i2b2HiveUrl = resource.getString("i2b2HiveUrl");
		}
		catch(MissingResourceException e)
		{
			log.error(e);
			i2b2HiveUrl = DEFAULT_I2B2HIVE_URL;
		}
		
		return i2b2HiveUrl;
	}
}
