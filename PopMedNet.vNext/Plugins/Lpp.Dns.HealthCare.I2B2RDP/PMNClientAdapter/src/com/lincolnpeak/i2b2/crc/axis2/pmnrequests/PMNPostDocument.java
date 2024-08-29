package com.lincolnpeak.i2b2.crc.axis2.pmnrequests;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

import org.apache.axis2.util.Base64;

import com.lincolnpeak.i2b2.crc.axis2.MessageElement;

@XmlRootElement(name="PostDocument")
@XmlAccessorType(XmlAccessType.FIELD)
public class PMNPostDocument extends MessageElement
{
	@XmlElement(name="Name")
	private String name;
	
	@XmlElement(name="MimeType")
	private String mimeType;
	
	@XmlElement(name="Viewable")
	private boolean viewable;
	
	@XmlElement(name="Body")
	private String body;
	
	public PMNPostDocument()
	{		
	}
	
	public PMNPostDocument(String name, String mimeType, boolean viewable)
	{
		this.name = name;
		this.mimeType = mimeType;
		this.viewable = viewable;
	}
	
	public String getName()
	{
		return name;
	}
	
	public void setName(String name)
	{
		this.name = name;
	}
	
	public String getMimeType()
	{
		return mimeType;
	}
	
	public void setMimeType(String mimeType)
	{
		this.mimeType = mimeType;
	}
	
	public boolean getViewable()
	{
		return viewable;
	}
	
	public void setViewable(boolean viewable)
	{
		this.viewable = viewable;
	}
	
	public String getBody()
	{
		return new String(Base64.decode(body));
	}
	
	public void setBody(String body)
	{
		this.body = Base64.encode(body.toString().getBytes());
	}
}
