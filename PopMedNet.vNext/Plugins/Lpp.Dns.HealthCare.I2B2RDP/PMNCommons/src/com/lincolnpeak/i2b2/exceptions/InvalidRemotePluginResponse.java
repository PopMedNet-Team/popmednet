package com.lincolnpeak.i2b2.exceptions;

public class InvalidRemotePluginResponse extends Exception
{
	private static final long serialVersionUID = 870602731561419548L;

	public InvalidRemotePluginResponse(Exception e)
	{
		super(e);
	}
	
	public InvalidRemotePluginResponse(String message)
	{
		super(message);
	}
}
