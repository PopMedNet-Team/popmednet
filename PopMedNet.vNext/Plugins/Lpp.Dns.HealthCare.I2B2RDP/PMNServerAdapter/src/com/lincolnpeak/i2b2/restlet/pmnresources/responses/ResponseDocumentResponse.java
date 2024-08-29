package com.lincolnpeak.i2b2.restlet.pmnresources.responses;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

import org.restlet.engine.util.Base64;

@XmlRootElement(name = "ResponseDocumentResponse")
@XmlAccessorType(XmlAccessType.FIELD)
public class ResponseDocumentResponse implements IResponse
{
	@XmlElement(name="ResponseDocumentResult")
	private int numBytes;
	
	@XmlElement(name="Data")
	private String data;
	
	public void setNumBytes(int numBytes)
	{
		this.numBytes = numBytes;
	}
	
	public int getNumBytes()
	{
		return numBytes;
	}
	
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
