package com.lincolnpeak.i2b2.exceptions;

public class CannotRunRequest extends Exception
{
	private static final long serialVersionUID = 870602731561419548L;

	public CannotRunRequest(Exception e)
	{
		super(e);
	}
	
	public CannotRunRequest(String message)
	{
		super(message);
	}
}
