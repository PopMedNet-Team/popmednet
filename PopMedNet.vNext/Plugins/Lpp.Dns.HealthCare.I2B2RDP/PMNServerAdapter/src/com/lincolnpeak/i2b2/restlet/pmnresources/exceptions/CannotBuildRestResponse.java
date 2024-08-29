package com.lincolnpeak.i2b2.restlet.pmnresources.exceptions;

public class CannotBuildRestResponse extends Exception
{
	private static final long serialVersionUID = -1882078924853962390L;

	public CannotBuildRestResponse(Exception e)
	{		
		super(e);
	}
}
