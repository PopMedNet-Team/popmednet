package com.lincolnpeak.i2b2.restlet.pmnresources.requests;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

import org.restlet.engine.util.Base64;

@XmlRootElement(name = "RequestDocument")
@XmlAccessorType(XmlAccessType.FIELD)
public class RequestDocumentRequest
{
	@XmlElement(name="Data")
	private String data;
	
	public void setData(String data)
	{
		this.data = data;
	}
	
	public String getData()
	{
		return data;
	}
		
	/**
	 * Does base64 decoding before return data.
	 * @return
	 */
	public byte[] decodedData()
	{
	    return Base64.decode(data);	
	}
	
	/**
	 * Encodes data in base64.
	 * @param data
	 */
	public void encodeData(byte[] dataBytes)
	{
	    data = Base64.encode(dataBytes, false);
		
	}
}
