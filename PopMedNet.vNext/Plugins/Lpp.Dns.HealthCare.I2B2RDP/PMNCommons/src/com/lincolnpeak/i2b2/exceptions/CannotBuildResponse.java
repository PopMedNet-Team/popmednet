package com.lincolnpeak.i2b2.exceptions;

public class CannotBuildResponse extends Exception
{
	private static final long serialVersionUID = 870602731561419548L;
	
	public CannotBuildResponse(Exception e)
	{
		super(e);
	}	
}
