package com.lincolnpeak.i2b2.restlet.pmnresources.requests;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

@XmlRootElement(name = "Stop")
@XmlAccessorType(XmlAccessType.FIELD)
public class StopRequest
{
	@XmlElement(name="Reason")
	private String reason;
	
	public void setReason(String reason)
	{
		this.reason = reason;
	}
	
	public String getReason()
	{
		return reason;
	}
		
}
