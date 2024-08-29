package com.lincolnpeak.i2b2.crc.axis2.pmnresponses;

import java.util.List;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlElementWrapper;
import javax.xml.bind.annotation.XmlRootElement;


@XmlRootElement(name="SessionMetadata")
@XmlAccessorType(XmlAccessType.FIELD)
public class DataMartResponse implements IPMNResponse
{
	@XmlElement(name="DataMart")
	private DataMart datamart;
	
	@XmlElementWrapper(name="Documents")
	@XmlElement(name="Document")
	private List<Document> documents;
	
	public DataMart getDataMart()
	{
		return datamart;
	}
	
	public void setDataMart(DataMart datamart)
	{
		this.datamart = datamart;
	}
	
	public List<Document> getDocuments()
	{
		return documents;
	}
	
	public void setDocuments(List<Document> documents)
	{
		this.documents = documents;
	}

}
