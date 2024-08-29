package com.lincolnpeak.i2b2.restlet.pmnresources;

import java.util.HashMap;

public class I2B2Requests 
{
	private static I2B2Requests instance = null;
	private HashMap<String, I2B2Request> map;
	
	public static I2B2Requests getInstance()
	{
		if(instance != null)
			return instance;
		
		instance = new I2B2Requests();
		return instance;
	}
	
	private I2B2Requests()
	{		
		map = new HashMap<String, I2B2Request>();
	}
	
	public void put(String key, I2B2Request value)
	{
		map.put(key, value);
	}
	
	public I2B2Request get(String key)
	{
		return map.get(key);
	}
	
	public void remove(String key)
	{
		map.remove(key);
	}
}
