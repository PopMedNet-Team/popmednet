package com.lincolnpeak.i2b2.crc.axis2.i2b2responses;

import java.util.Date;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

@XmlRootElement(name="query_master")
@XmlAccessorType(XmlAccessType.FIELD)
public class I2B2QueryMaster
{
	@XmlElement(name="query_master_id")
	private String queryMasterId;

	@XmlElement(name="name")
	private String name;
	
	@XmlElement(name="user_id")
	private String userId;
	
	@XmlElement(name="group_id")
	private String groupId;
	
	@XmlElement(name="create_date")
	private Date createDate;
	
	public String getQueryMasterId()
	{
		return queryMasterId;
	}
	
	public void setQueryMasterId(String queryMasterId)
	{
		this.queryMasterId = queryMasterId;
	}

	public String getName()
	{
		return name;
	}
	
	public void setName(String name)
	{
		this.name = name;
	}
	
	public String getUserId()
	{
		return userId;
	}
	
	public void setUserId(String userId)
	{
		this.userId = userId;
	}
	
	public String getGroupId()
	{
		return groupId;
	}
	
	public void setGroupId(String groupId)
	{
		this.groupId = groupId;
	}
	
	public Date getCreateDate()
	{
		return createDate;
	}
	
	public void setCreateDate(Date createDate)
	{
		this.createDate = createDate;
	}
}