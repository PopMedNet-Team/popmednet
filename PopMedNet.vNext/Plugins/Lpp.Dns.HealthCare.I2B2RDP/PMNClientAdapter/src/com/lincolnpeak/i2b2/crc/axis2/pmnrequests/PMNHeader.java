package com.lincolnpeak.i2b2.crc.axis2.pmnrequests;

import java.util.Date;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

import com.lincolnpeak.i2b2.crc.axis2.MessageElement;

@XmlRootElement(name="Header")
@XmlAccessorType(XmlAccessType.FIELD)
public class PMNHeader extends MessageElement
{
	@XmlElement(name="Name")
	private String name;
	
	@XmlElement(name="DueDate")
	private Date dueDate;
	
	@XmlElement(name="Priority")
	private String priority;
	
	public PMNHeader()
	{		
	}
	
	public PMNHeader(String name, Date dueDate, String priority)
	{
		this.name = name;
		this.dueDate = dueDate;
		this.priority = priority;
	}
	
	public String getName()
	{
		return name;
	}
	
	public void setName(String name)
	{
		this.name = name;
	}
	
	public Date getDueDate()
	{
		return dueDate;
	}
	
	public void setDueDate(Date dueDate)
	{
		this.dueDate = dueDate;
	}
	
	public String getPriority()
	{
		return priority;
	}
	
	public void setPriority(String priority)
	{
		this.priority = priority;
	}
	
}
