package com.lincolnpeak.i2b2.crc.axis2.i2b2requests;

import javax.xml.namespace.QName;

import org.apache.axiom.om.OMElement;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

import com.lincolnpeak.i2b2.PMNResource;

public class I2B2RequestBase 
{
	protected final Log log = LogFactory.getLog(getClass());

	// I2B2 request data
	private String requestType;
	private String projectId;
	private String userId;
	private String groupId;
	private String queryName;
	private String queryMasterId;
	private String queryInstanceId;
	private String queryResultInstanceId;
	private String domain;
	private String username;
	private String sessionToken;

	// PMN data inserted into i2b2 from a redirect plugin
	private String pmnSessionToken;
	private String pmnServiceUrl;
	private boolean isPMNRequest;
	private boolean isPMNResponse;
	
	public I2B2RequestBase(OMElement request)
	{
		OMElement messageBody = request.getFirstChildWithName(new QName("message_body"));
		requestType = messageBody.getFirstChildWithName(new QName("psmheader"))
	 	                         .getFirstChildWithName(new QName("request_type")).getText().trim();
		
		OMElement requestElement = messageBody.getFirstChildWithName(new QName("request"));
		if(requestElement != null)
		{
			OMElement userIdElement = requestElement.getFirstChildWithName(new QName("user_id"));
			if(userIdElement != null)
				userId = userIdElement.getText().trim();
			OMElement groupIdElement = requestElement.getFirstChildWithName(new QName("group_id"));
			if(groupIdElement != null)
				groupId = groupIdElement.getText().trim();
			
			OMElement queryDefElement = requestElement.getFirstChildWithName(new QName("query_definition"));
			if(queryDefElement != null)
			{
				OMElement queryNameElement = queryDefElement.getFirstChildWithName(new QName("query_name"));
				if(queryNameElement != null)
					queryName = queryNameElement.getText().trim();
			}
			
			OMElement queryMasterIdElement = requestElement.getFirstChildWithName(new QName("query_master_id"));
			if(queryMasterIdElement != null)
				queryMasterId = queryMasterIdElement.getText().trim();
			
			// FIXME There could be multiple of these.
			OMElement queryInstanceIdElement = requestElement.getFirstChildWithName(new QName("query_instance_id"));
			if(queryInstanceIdElement != null)
				queryInstanceId = queryInstanceIdElement.getText().trim();

			// FIXME There could be multiple of these.
			OMElement queryResultInstanceIdElement = requestElement.getFirstChildWithName(new QName("query_result_instance_id"));
			if(queryResultInstanceIdElement != null)
				queryResultInstanceId = queryResultInstanceIdElement.getText().trim();

		}
        
		OMElement messageHeader = request.getFirstChildWithName(new QName("message_header"));		
		projectId = messageHeader.getFirstChildWithName(new QName("project_id")).getText().trim();	
		
		OMElement pmnSessionTokenElement = messageHeader.getFirstChildWithName(new QName("sessionToken"));
		if(pmnSessionTokenElement != null)
			pmnSessionToken = pmnSessionTokenElement.getText().trim();
		
		OMElement pmnServiceUrlElement = messageHeader.getFirstChildWithName(new QName("service"));
		if(pmnServiceUrlElement != null)
		{
			pmnServiceUrl = pmnServiceUrlElement.getText().trim();	   
			log.info(">>>>>>>>>>>>>>>>>>>>>> ServiceUrl from i2b2 message: " + pmnServiceUrl);
		}
		
		OMElement securityElement = messageHeader.getFirstChildWithName(new QName("security"));
		domain = securityElement.getFirstChildWithName(new QName("domain")).getText().trim();
		username = securityElement.getFirstChildWithName(new QName("username")).getText().trim();
		sessionToken = securityElement.getFirstChildWithName(new QName("password")).getText().trim();

		log.info(">>>>>>>>>>>>>>>>>>>>>> ServiceUrl from PMNResource: " + PMNResource.getInstance().getServiceUrl());
		pmnServiceUrl = pmnServiceUrlElement == null || pmnServiceUrl.equals("undefined") ? PMNResource.getInstance().getServiceUrl() : pmnServiceUrl;

		isPMNResponse = messageHeader.getFirstChildWithName(new QName("isresponse")) != null;
		isPMNRequest = messageHeader.getFirstChildWithName(new QName("isrequest")) != null;
	}
	
	public void setRequestType(String requestType)
	{
		this.requestType = requestType;
	}
	
	public String getRequestType()
	{
		return requestType;
	}
	
	public void setProjectId(String projectId)
	{
		this.projectId = projectId;
	}
	
	public String getProjectId()
	{
		return projectId;
	}
	
	public void setDomain(String domain)
	{
		this.domain = domain;
	}
	
	public String getDomain()
	{
		return domain;
	}
	
	public void setUserName(String username)
	{
		this.username = username;
	}
	
	public String getUserName()
	{
		return username;
	}
	
	public void setSessionToken(String sessionToken)
	{
		this.sessionToken = sessionToken;
	}
	
	public String getSessionToken()
	{
		return sessionToken;
	}
	
	public void setUserId(String userId)
	{
		this.userId = userId;
	}
	
	public String getUserId()
	{
		return userId;
	}
	
	public void setGroupId(String groupId)
	{
		this.groupId = groupId;
	}
	
	public String getGroupId()
	{
		return groupId;
	}
	
	public void setQueryName(String queryName)
	{
		this.queryName = queryName;
	}
	
	public String getQueryName()
	{
		return queryName;
	}
	
	public void setQueryMasterId(String queryMasterId)
	{
		this.queryMasterId = queryMasterId;
	}
	
	public String getQueryInstanceId()
	{
		return queryInstanceId;
	}
	
	public void setQueryInstanceId(String queryInstanceId)
	{
		this.queryInstanceId = queryInstanceId;
	}
	
	public String getQueryMasterId()
	{
		return queryMasterId;
	}
	
	public void setQueryResultInstanceId(String queryInstanceId)
	{
		this.queryResultInstanceId = queryInstanceId;
	}
	
	public String getQueryResultInstanceId()
	{
		return queryResultInstanceId;
	}
	
	public void setPMNSessionToken(String pmnSessionToken)
	{
		this.pmnSessionToken = pmnSessionToken;
	}
	
	public String getPMNSessionToken()
	{
		return pmnSessionToken;
	}
	
	public void setPMNServiceUrl(String pmnServiceUrl)
	{
		this.pmnServiceUrl = pmnServiceUrl;
	}
	
	public String getPMNServiceUrl()
	{
		return pmnServiceUrl;
	}
	
	public void setIsPMNRequest(boolean isPMNRequest)
	{
		this.isPMNRequest = isPMNRequest;
	}
	
	public boolean getIsPMNRequest()
	{
		return isPMNRequest;
	}
	
	public void setIsPMNResponse(boolean isPMNResponse)
	{
		this.isPMNResponse = isPMNResponse;
	}
	
	public boolean getIsPMNResponse()
	{
		return isPMNResponse;
	}

}
