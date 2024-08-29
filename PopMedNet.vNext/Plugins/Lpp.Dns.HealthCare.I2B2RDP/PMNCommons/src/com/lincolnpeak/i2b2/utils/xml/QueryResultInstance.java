package com.lincolnpeak.i2b2.utils.xml;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlAttribute;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

@XmlRootElement(name = "query_result_instance")
@XmlAccessorType(XmlAccessType.FIELD)
public class QueryResultInstance implements IXmlFragment 
{
	@XmlElement(name="result_instance_id")
	private String resultInstanceId;
	
	@XmlElement(name="query_instance_id")
	private String queryInstanceId;

	@XmlElement(name="description")
	private String description;
	
	@XmlElement(name="query_status_type")
	private QueryStatusType queryStatusType;

	public String getResultInstanceId()
	{
		return resultInstanceId;
	}
	
	public void setResultInstanceId(String resultInstanceId)
	{
		this.resultInstanceId = resultInstanceId;
	}
	
	public String getQueryInstanceId()
	{
		return queryInstanceId;
	}
	
	public void setQueryInstanceId(String queryInstanceId)
	{
		this.queryInstanceId = queryInstanceId;
	}
	
	public String getDescription()
	{
		return description;
	}
	
	public void setDescription(String description)
	{
		this.description = description;
	}
	
	public QueryStatusType getQueryStatusType()
	{
		return queryStatusType;
	}
	
	public void setQueryStatusType(QueryStatusType queryStatusType)
	{
		this.queryStatusType = queryStatusType;
	}
}
