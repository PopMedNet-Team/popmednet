package com.lincolnpeak.i2b2.crc.axis2.i2b2responses;

import java.io.StringWriter;
import java.util.List;

import javax.xml.bind.JAXBContext;
import javax.xml.bind.JAXBException;
import javax.xml.bind.Marshaller;
import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

@XmlRootElement(name="response")
@XmlAccessorType(XmlAccessType.FIELD)
public class I2B2QueryMasterList 
{
	@XmlElement(name="status")
	private ResponseStatus status;
	
	@XmlElement(name="query_master")
	private List<I2B2QueryMaster> queryMasters;
	
	public I2B2QueryMasterList()
	{		
	}
	
	public void setStatus(ResponseStatus status)
	{
		this.status = status;
	}
		
	public ResponseStatus getStatus()
	{
		return status;
	}
	
	public void setQueryMasters(List<I2B2QueryMaster> queryMasters)
	{
		this.queryMasters = queryMasters;
	}
	
	public List<I2B2QueryMaster> getQueryMasters()
	{
		return queryMasters;
	}

	/**
	 * Serializes this object to XML.
	 */
	public String toString()
	{
		StringWriter w = new StringWriter();
		
		try
		{
			JAXBContext context = JAXBContext.newInstance(this.getClass());
			Marshaller marshaller = context.createMarshaller();
			marshaller.setProperty(Marshaller.JAXB_FRAGMENT, Boolean.TRUE);
	        marshaller.marshal(this, w);
		}
		catch(JAXBException e)
		{
			e.printStackTrace();
		}
		
        return w.toString();
	}
}




