package com.lincolnpeak.i2b2.exceptions;

public class CannotGetElementValue extends Exception
{
	private static final long serialVersionUID = 870602731561419548L;

	public CannotGetElementValue(Exception e)
	{
		super(e);
	}
	
	public CannotGetElementValue(String message, Exception e)
	{
		super(message, e);
	}
}
