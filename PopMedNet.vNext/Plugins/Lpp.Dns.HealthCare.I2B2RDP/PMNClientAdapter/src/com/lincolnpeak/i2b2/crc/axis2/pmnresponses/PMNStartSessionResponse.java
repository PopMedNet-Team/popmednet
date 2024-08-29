package com.lincolnpeak.i2b2.crc.axis2.pmnresponses;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

import com.lincolnpeak.i2b2.crc.axis2.MessageElement;


@XmlRootElement(name="StartSessionResponse")
@XmlAccessorType(XmlAccessType.FIELD)
public class PMNStartSessionResponse extends MessageElement implements IPMNResponse
{
	@XmlElement(name="StartSessionResult")
	private String sessionToken;
	
	public String getStartSessionResult()
	{
		return sessionToken;
	}
	
	public void setStartSessionResult(String sessionToken)
	{
		this.sessionToken = sessionToken;
	}

}
