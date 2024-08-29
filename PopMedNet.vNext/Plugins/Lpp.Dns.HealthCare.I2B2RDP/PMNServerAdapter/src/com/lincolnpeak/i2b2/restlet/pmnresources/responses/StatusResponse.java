package com.lincolnpeak.i2b2.restlet.pmnresources.responses;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

@XmlRootElement(name = "StatusResponse")
@XmlAccessorType(XmlAccessType.FIELD)
public class StatusResponse implements IResponse
{
	@XmlElement(name="StatusResult")
	private StatusResult status;
	
	public void setStatus(StatusResult status)
	{
		this.status = status;
	}
	
	public StatusResult getStatus()
	{
		return status;
	}

}
