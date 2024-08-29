package com.lincolnpeak.i2b2.crc.axis2;

import java.io.IOException;
import java.io.StringReader;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.Properties;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

import com.lincolnpeak.i2b2.PMNResource;
import com.lincolnpeak.i2b2.crc.axis2.i2b2requests.I2B2RequestBase;
import com.lincolnpeak.i2b2.crc.axis2.i2b2responses.I2B2QueryMaster;
import com.lincolnpeak.i2b2.crc.axis2.i2b2responses.I2B2QueryMasterList;
import com.lincolnpeak.i2b2.crc.axis2.i2b2responses.ResponseStatus;
import com.lincolnpeak.i2b2.crc.axis2.pmnrequests.PMNHeader;
import com.lincolnpeak.i2b2.crc.axis2.pmnresponses.DataMartResponse;
import com.lincolnpeak.i2b2.crc.axis2.pmnresponses.Document;
import com.lincolnpeak.i2b2.crc.axis2.pmnresponses.PMNResponseSessionMetadata;
import com.lincolnpeak.i2b2.exceptions.CannotBuildResponse;
import com.lincolnpeak.i2b2.exceptions.CannotFindResultDocument;
import com.lincolnpeak.i2b2.exceptions.CannotRunRequest;
import com.lincolnpeak.i2b2.exceptions.InvalidRemotePluginResponse;
import com.lincolnpeak.i2b2.utils.HQMFConverter;
import com.lincolnpeak.i2b2.utils.RESTRequestHelper;

public class PMNRedirectSetFinderHandler extends SetFinderHandler
{
	protected final Log log = LogFactory.getLog(getClass());

	private String serviceUrl;
	private String returnUrl;
	private String queryName;
	private String queryMasterId;
	private String queryInstanceId;
	private String queryResultInstanceId;
	private String i2b2RequestType;
	private String i2b2Username;
	private String i2b2UserId;
	private String i2b2GroupId;
	private String i2b2ProjectId;
	private String pmnSessionToken;
//	private boolean isRequest;
	private boolean isResponse;
	
	public PMNRedirectSetFinderHandler(I2B2RequestBase i2b2Request)
	{
		i2b2UserId = i2b2Request.getUserId();
		i2b2GroupId = i2b2Request.getGroupId();
		i2b2Username = i2b2Request.getUserName();
		i2b2RequestType = i2b2Request.getRequestType();
		i2b2ProjectId = i2b2Request.getProjectId();
		
		serviceUrl = i2b2Request.getPMNServiceUrl();
		queryName = i2b2Request.getQueryName();
		queryMasterId = i2b2Request.getQueryMasterId();
		queryInstanceId = i2b2Request.getQueryInstanceId();
		queryResultInstanceId = i2b2Request.getQueryResultInstanceId();
		
//		isResponse = i2b2Request.getIsPMNResponse();
//		isRequest = i2b2Request.getIsPMNRequest();

	}
	
	public String handleSetFinderRequest(String request) throws CannotRunRequest, InvalidRemotePluginResponse, IOException, CannotBuildResponse, CannotFindResultDocument, ParseException
	{
		PMNRedirectPluginService pmnRedirectPluginService = new PMNRedirectPluginService(serviceUrl);

		// CRC_QRY_runQueryInstance_fromQueryDefinition always sends an i2b2 query through PMN to data sources.
		if(i2b2RequestType.equals("CRC_QRY_runQueryInstance_fromQueryDefinition"))
		{
			String returnUrl = pmnRedirectPluginService.getSessionMetadata(pmnSessionToken).getReturnUrl();
			pmnRedirectPluginService.postDocument(pmnSessionToken, "i2b2Request.xml", "text/xml", true, HQMFConverter.I2B2toHQMF(request.toString()));
			pmnRedirectPluginService.requestCreated(pmnSessionToken, new PMNHeader(queryName, null, null), null);											
			return buildResponseMessage("I2B2DoneMessage", returnUrl);			
		}
		
		// Everything else is for handling what to tell the i2b2 web client when it needs information for the
		// Previous Queries, the Query Tool and Query Status panels.
		else if(isResponse)
		{
			if(i2b2RequestType.equals("CRC_QRY_getQueryMasterList_fromUserId"))
			{						
				PMNResponseSessionMetadata responseSessionMetadata = pmnRedirectPluginService.getResponseSessionMetadata(pmnSessionToken);
				returnUrl = responseSessionMetadata.getSession().getReturnUrl();
				
//				String queryInstanceId = null;
//				String queryResultDesc = null;
//				String queryName = null;
//				Date createDate = null;
				String qmlResponse = "";
				
				I2B2QueryMasterList queryMasterList = new I2B2QueryMasterList();
				queryMasterList.setStatus(new ResponseStatus("DONE"));
				queryMasterList.setQueryMasters(new ArrayList<I2B2QueryMaster>());

				I2B2QueryMaster qm = new I2B2QueryMaster();
				qm.setQueryMasterId(responseSessionMetadata.getSession().getRequestId());
//					qm.setName(responseSessionMetadata.getName());
				qm.setUserId(i2b2Username);
				qm.setGroupId(i2b2GroupId);
//					qm.setCreateDate(pmnRequest.getCreateDate());
				ResultMetadata resultMetadata = new ResultMetadata();
				
				for(DataMartResponse datamartResponse : responseSessionMetadata.getDataMartResponses())
				{
					for(Document doc : datamartResponse.getDocuments())
					{
						if(doc.getName().equals("i2b2Metadata.properties"))
						{
							String liveUrl = doc.getLiveUrl();
							String content = RESTRequestHelper.getRequest(liveUrl);
							content = content.replace("\\n", "\n");
							Properties props = new Properties();
							props.load(new StringReader(content));
							resultMetadata.setQueryName((String) props.get("query_name"));
							resultMetadata.setCreateDate(new SimpleDateFormat("yyyy-MM-DD'T'hh:mm:ss.SD").parse((String) props.get("create_date")));
							resultMetadata.setQueryResultDescription((String) props.get("query_result_description"));
							break;
//								resultMetadata.setQueryInstanceId((String) props.get("query_instance_id"));
//								resultMetadata.setResultInstanceId((String) props.get("result_instance_id"));
//								resultMetadata.setIsCount(Boolean.parseBoolean((String) props.get("is_count")));
//								masterQueryNames.add(resultMetadata);
						}
					}
				}
//
				qm.setName(resultMetadata.getQueryName());
				qm.setCreateDate(resultMetadata.getCreateDate());
				queryMasterList.getQueryMasters().add(qm);
//					
				qmlResponse = queryMasterList.toString();

		        return buildResponse(i2b2RequestType, i2b2Username, i2b2GroupId, i2b2ProjectId, 
		        		qm.getQueryMasterId(), resultMetadata.getQueryResultDescription(), 
		        		resultMetadata.getQueryName(), resultMetadata.getCreateDate(), returnUrl, qmlResponse, "");
			}
			else if(i2b2RequestType.equals("CRC_QRY_getQueryInstanceList_fromQueryMasterId"))
			{						
		        return buildResponse(i2b2RequestType, i2b2UserId, i2b2GroupId, i2b2ProjectId, queryMasterId, null, queryName, null, returnUrl, null, null);
			}
			else if(i2b2RequestType.equals("CRC_QRY_getQueryResultInstanceList_fromQueryInstanceId"))
			{
				PMNResponseSessionMetadata pmnSessionMetadata = pmnRedirectPluginService.getResponseSessionMetadata(pmnSessionToken);
				String queryResultInstanceFragment = buildQueryResultInstancesFragment(i2b2RequestType, pmnSessionMetadata.getDataMartResponses(), queryInstanceId);

		        return buildResponse(i2b2RequestType, i2b2UserId, i2b2GroupId, i2b2ProjectId, queryInstanceId, null, queryName, null, returnUrl, null, queryResultInstanceFragment);
			}
			else if(i2b2RequestType.equals("CRC_QRY_getRequestXml_fromQueryMasterId"))
			{
				return getRequestXml_fromQueryMasterId(i2b2RequestType, serviceUrl, pmnSessionToken);
			}
			else
			{
				String queryMasterIdResultInstanceId = queryResultInstanceId;
				String[] queryIds = queryMasterIdResultInstanceId.split(",");
				String queryMasterId = queryIds[0];
				String queryResultInstanceId = queryIds[1];
				return getResultDocument_fromResultInstanceId(i2b2RequestType, serviceUrl, pmnSessionToken, queryMasterId, queryResultInstanceId);
			}

			// FIXME: Check above responses for errors.

		}
		else
		{			
			String url = PMNResource.getInstance().getI2B2HiveUrl();
			String requestString = request.toString();
			return RESTRequestHelper.postRequest(url, requestString);
		}
	}
	
	private String buildResponseMessage(String responseType, String returnUrl) throws CannotBuildResponse
	{
		try
		{
			String responseMessage = loadI2B2MessageTemplate(responseType);
			responseMessage = responseMessage.replace("${RETURN_URL}", returnUrl == null ? "" : returnUrl);
			
			return responseMessage;
		}
		catch(IOException e)
		{
			log.error(e);
			throw new CannotBuildResponse(e);
		}
		
	}
	
	private String buildResponse(String i2b2RequestType, String userId, String groupId, String projectId, 
			String queryInstanceId, String queryResultDesc, String queryMasterName, Date createDate, 
			String returnUrl, String queryMasterListResponse, String queryResultList) throws CannotBuildResponse
	{
		try
		{
			String responseMessage = loadI2B2MessageTemplate(i2b2RequestType);

			responseMessage = responseMessage.replace("${USER_ID}", userId == null ? "" : userId)
									         .replace("${GROUP_ID}", groupId == null ? "" : groupId)
									         .replace("${PROJECT_ID}", projectId == null ? "" : projectId) //
									         .replace("${QUERY_INSTANCE_ID}", queryInstanceId == null ? "" : queryInstanceId) //
									         .replace("${QUERY_RESULT_DESCRIPTION}", queryResultDesc == null ? "" : queryResultDesc) //
									         .replace("${QUERY_MASTER_NAME}", queryMasterName == null ? "" : queryMasterName);
//										         .replace("${CREATE_DATE}", createDate == null ? "" : createDate);
			
			responseMessage = responseMessage.replace("${RETURN_URL}", returnUrl == null ? "" : returnUrl);
			responseMessage = responseMessage.replace("${QUERY_MASTER_LIST_RESPONSE}", queryMasterListResponse == null ? "" : queryMasterListResponse);
			responseMessage = responseMessage.replace("${QUERY_RESULT_LIST}", queryMasterListResponse == null ? "" : queryResultList);
			
			return responseMessage;
		}
		catch(IOException e)
		{
			log.error(e);
			throw new CannotBuildResponse(e);
		}
		
	}	
	
	private String getResultDocument_fromResultInstanceId(String requestType, String serviceValue, String sessionTokenValue, String queryMasterId, String resultInstanceId) throws CannotFindResultDocument
	{
		try
		{
			String response = RESTRequestHelper.getRequest(serviceValue + "/" + sessionTokenValue + "/Session");			
			PMNResponseSessionMetadata responseSessionMetadata = PMNResponseSessionMetadata.load(PMNResponseSessionMetadata.class, response);
//			PMNResponseSessionMetadata responseSessionMetadata = (PMNResponseSessionMetadata) PMNResponseSessionMetadata.load(response);
			
			for(DataMartResponse datamartResponse : responseSessionMetadata.getDataMartResponses())
			{
				for(Document doc : datamartResponse.getDocuments())
				{
					if(doc.getName().equals("CRC_QRY_runQueryInstance_fromQueryDefinition.xml"))
					{
						String liveUrl = doc.getLiveUrl();
						String content = RESTRequestHelper.getRequest(liveUrl);
						return content;
					}
				}
			}

			return null;
		}
		catch(Exception e)
		{
			log.error(e);
			throw new CannotFindResultDocument("", "", "0");
		} 
	}
	
	private String getRequestXml_fromQueryMasterId(String requestType, String serviceValue, String sessionTokenValue) throws CannotFindResultDocument
	{
		try
		{
			String url = serviceValue + "/" + sessionTokenValue + "/Session";
			String response = RESTRequestHelper.getRequest(url);
			
			PMNResponseSessionMetadata responseSessionMetadata = PMNResponseSessionMetadata.load(PMNResponseSessionMetadata.class, response);
			
			for(DataMartResponse datamartResponse : responseSessionMetadata.getDataMartResponses())
			{
				for(Document doc : datamartResponse.getDocuments())
				{
					if(doc.getName().equals(requestType + ".xml"))
					{
						String liveUrl = doc.getLiveUrl();
						String content = RESTRequestHelper.getRequest(liveUrl);
						return content;
					}
				}
			}
			
			return null;
		}
		catch(Exception e)
		{
			log.error(e);
			throw new CannotFindResultDocument("RequestXml", "QueryMasterId", "0");
		}
	}
}

class ResultMetadata
{
	private String queryName;
	private Date createDate;
	private String queryInstanceId;
	private String queryResultDescription;
	private boolean isCount;
	
	public void setQueryName(String queryName)
	{
		this.queryName = queryName;
	}
	
	public String getQueryName()
	{
		return queryName;
	}
	
	public void setCreateDate(Date createDate)
	{
		this.createDate = createDate;
	}
	
	public Date getCreateDate()
	{
		return createDate;
	}
	
	public void setQueryInstanceId(String queryInstanceId)
	{
		this.queryInstanceId = queryInstanceId;
	}
	
	public String getQueryInstanceId()
	{
		return queryInstanceId;
	}
	
	public void setQueryResultDescription(String queryResultDescription)
	{
		this.queryResultDescription = queryResultDescription;
	}
	
	public String getQueryResultDescription()
	{
		return queryResultDescription;
	}
	
	public void setIsCount(boolean isCount)
	{
		this.isCount = isCount;
	}
	
	public boolean getIsCount()
	{
		return isCount;
	}
}

