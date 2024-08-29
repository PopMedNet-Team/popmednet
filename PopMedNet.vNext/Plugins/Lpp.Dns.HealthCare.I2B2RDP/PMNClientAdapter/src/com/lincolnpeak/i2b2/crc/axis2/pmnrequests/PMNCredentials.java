package com.lincolnpeak.i2b2.crc.axis2.pmnrequests;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

import com.lincolnpeak.i2b2.crc.axis2.MessageElement;

@XmlRootElement(name="credentials")
@XmlAccessorType(XmlAccessType.FIELD)
public class PMNCredentials extends MessageElement
{
	@XmlElement(name="Username")
	private String username;
	
	@XmlElement(name="Password")
	private String password;
	
	public PMNCredentials()
	{		
	}
	
	public PMNCredentials(String username, String password)
	{
		this.username = username;
		this.password = password;
	}
	
	public String getUsername()
	{
		return username;
	}
	
	public void setUsername(String username)
	{
		this.username = username;
	}
	
	public String getPassword()
	{
		return password;
	}
	
	public void setPassword(String password)
	{
		this.password = password;
	}

}
