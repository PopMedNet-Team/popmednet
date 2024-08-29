package com.lincolnpeak.i2b2.crc.axis2.pmnrequests;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

import com.lincolnpeak.i2b2.crc.axis2.MessageElement;

@XmlRootElement(name="StartSession")
@XmlAccessorType(XmlAccessType.FIELD)
public class PMNStartSession extends MessageElement
{
	@XmlElement(name="credentials")
	private PMNCredentials credentials;
	
	public PMNStartSession()
	{		
	}
	
	public PMNStartSession(PMNCredentials credentials)
	{
		this.credentials = credentials;
	}
	
	public void setCredentials(PMNCredentials credentials)
	{
		this.credentials = credentials;
	}
	
	public PMNCredentials getCredentials()
	{
		return credentials;
	}

}
