package com.lincolnpeak.i2b2.crc.axis2.pmnresponses;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

import com.lincolnpeak.i2b2.crc.axis2.MessageElement;


@XmlRootElement(name="StartProjectSessionResponse")
@XmlAccessorType(XmlAccessType.FIELD)
public class PMNStartProjectSessionResponse extends MessageElement implements IPMNResponse
{
	@XmlElement(name="StartProjectSessionResult")
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
