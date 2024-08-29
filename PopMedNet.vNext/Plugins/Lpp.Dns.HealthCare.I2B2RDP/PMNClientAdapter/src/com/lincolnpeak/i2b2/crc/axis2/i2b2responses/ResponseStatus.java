package com.lincolnpeak.i2b2.crc.axis2.i2b2responses;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

@XmlRootElement(name="status")
@XmlAccessorType(XmlAccessType.FIELD)
public class ResponseStatus
{
	@XmlElement(name="condition")
	private Status condition;
	
	public ResponseStatus()
	{
	}
	
	public ResponseStatus(String status)
	{
		condition = new Status(status);
	}
	
	public Status getCondition()
	{
		return condition;
	}
	
	public void setCondition(Status condition)
	{
		this.condition = condition;
	}
		
}
