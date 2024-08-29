package com.lincolnpeak.i2b2.restlet.pmnresources.responses;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

import com.lincolnpeak.i2b2.restlet.pmnresources.StatusCode;

@XmlRootElement(name = "StatusResult")
@XmlAccessorType(XmlAccessType.FIELD)
public class StatusResult 
{
	@XmlElement(name="Code")
	private StatusCode code;
	
	@XmlElement(name="Message")
	private String message;
		
	public void setCode(StatusCode code)
	{
		this.code = code;
	}
	
	public StatusCode getCode()
	{
		return code;
	}
	
	public void setMessage(String message)
	{
		this.message = message;
	}
	
	public String getMessage()
	{
		return message;
	}
}
