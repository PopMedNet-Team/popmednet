package com.lincolnpeak.i2b2.crc.axis2.pmnrequests;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlRootElement;


@XmlRootElement(name="StartProjectSession")
@XmlAccessorType(XmlAccessType.FIELD)
public class PMNStartProjectSession extends PMNStartSession
{	
	public PMNStartProjectSession()
	{
	}
	
	public PMNStartProjectSession(PMNCredentials credentials)
	{
		super(credentials);
	}
}
