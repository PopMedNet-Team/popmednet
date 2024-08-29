package com.lincolnpeak.i2b2.crc.axis2.pmnresponses;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

@XmlRootElement(name="Document")
@XmlAccessorType(XmlAccessType.FIELD)
public class Document implements IPMNResponse 
{
	@XmlElement(name="Id")
	private String id;
	
	@XmlElement(name="Name")
	private String name;
	
	@XmlElement(name="Content")
	private byte[] content;
	
	@XmlElement(name="LiveUrl")
	private String liveUrl;
	
	@XmlElement(name="MimeType")
	private String mimeType;
	
	@XmlElement(name="Size")
	private int size;
	
	public String getId()
	{
		return id;
	}
	
	public void setId(String id)
	{
		this.id = id;
	}

	public String getName()
	{
		return name;
	}
	
	public void setName(String name)
	{
		this.name = name;
	}
	
	public byte[] getContent()
	{
		return content;
	}
	
	public void setContent(byte[] content)
	{
		this.content = content;
	}
	
	public String getLiveUrl()
	{
		return liveUrl;
	}
	
	public void setLiveUrl(String liveUrl)
	{
		this.liveUrl = liveUrl;
	}

	public String getMimeType()
	{
		return mimeType;
	}
	
	public void setMimeType(String mimeType)
	{
		this.mimeType = mimeType;
	}
	
	public int getSize()
	{
		return size;
	}

	public void setSize(int size)
	{
		this.size = size;
	}
}