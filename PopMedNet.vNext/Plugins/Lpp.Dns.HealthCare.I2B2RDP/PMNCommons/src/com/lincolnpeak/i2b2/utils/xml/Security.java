package com.lincolnpeak.i2b2.utils.xml;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

@XmlRootElement(name = "security")
@XmlAccessorType(XmlAccessType.FIELD)
public class Security implements IXmlFragment 
{
	@XmlElement(name="domain")
	private String domain;

	@XmlElement(name="username")
	private String username;

	@XmlElement(name="password")
	private String password;
	
	public String getDomain()
	{
		return domain;
	}
	
	public void setDomain(String domain)
	{
		this.domain = domain;
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
