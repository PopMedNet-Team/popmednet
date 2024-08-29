package com.lincolnpeak.i2b2.utils;

import java.util.Date;

public class Version 
{
	private static final String modifyMe = "a"; // Modify this to force a change.
	private static final String version = "$Revision: 1344 $";
	private static final String buildDate = "$LastChangedDate: 2013-04-10 11:26:40 -0400 (Wed, 10 Apr 2013) $";
	
	public static String getVersion()
	{
		return version;
	}
	
	public static String getBuildDate()
	{
		return buildDate;
	}
}
