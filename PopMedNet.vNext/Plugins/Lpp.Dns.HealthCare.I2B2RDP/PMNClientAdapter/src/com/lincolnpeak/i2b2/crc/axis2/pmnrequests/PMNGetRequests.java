package com.lincolnpeak.i2b2.crc.axis2.pmnrequests;

import java.util.Date;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

import com.lincolnpeak.i2b2.crc.axis2.MessageElement;

@XmlRootElement(name="GetRequests")
@XmlAccessorType(XmlAccessType.FIELD)
public class PMNGetRequests extends MessageElement
{
	@XmlElement(name="fromDate")
	private Date fromDate;
	
	@XmlElement(name="toDate")
	private Date toDate;
	
	public PMNGetRequests()
	{		
	}
	
	public PMNGetRequests(Date fromDate, Date toDate)
	{
		this.fromDate = fromDate;
		this.toDate = toDate;
	}
	
	public void setFromDate(Date fromDate)
	{
		this.fromDate = fromDate;
	}
	
	public Date getFromDate()
	{
		return fromDate;
	}
	
	public void setToDate(Date toDate)
	{
		this.toDate = toDate;
	}
	
	public Date getToDate()
	{
		return toDate;
	}

}
