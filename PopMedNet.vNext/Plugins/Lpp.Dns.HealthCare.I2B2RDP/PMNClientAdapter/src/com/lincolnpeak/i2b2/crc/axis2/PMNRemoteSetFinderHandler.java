package com.lincolnpeak.i2b2.crc.axis2;

import java.io.IOException;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

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
import com.lincolnpeak.i2b2.crc.axis2.pmnresponses.PMNGetRequestsResponse;
import com.lincolnpeak.i2b2.crc.axis2.pmnresponses.PMNRequest;
import com.lincolnpeak.i2b2.exceptions.CannotBuildResponse;
import com.lincolnpeak.i2b2.exceptions.CannotFindResultDocument;
import com.lincolnpeak.i2b2.exceptions.CannotGetElementValue;
import com.lincolnpeak.i2b2.exceptions.CannotRunRequest;
import com.lincolnpeak.i2b2.exceptions.InvalidRemotePluginResponse;
import com.lincolnpeak.i2b2.utils.HQMFConverter;
import com.lincolnpeak.i2b2.utils.RESTRequestHelper;
import com.lincolnpeak.i2b2.utils.xml.QueryDefinitionType;
import com.lincolnpeak.i2b2.utils.xml.XmlHelper;

public class PMNRemoteSetFinderHandler extends SetFinderHandler
{
	protected final Log log = LogFactory.getLog(getClass());

	private String serviceUrl;
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
	private String pmnProxyUser;
	private String pmnProxyPassword;
	private String pmnRequestType;
	
	public PMNRemoteSetFinderHandler(I2B2RequestBase i2b2Request)
	{
		pmnProxyUser = PMNResource.getInstance().getProxyUser();
		pmnProxyPassword = PMNResource.getInstance().getProxyPassword();
		pmnRequestType = PMNResource.getInstance().getPMNRequestType();

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
	}
	
	public String handleSetFinderRequest(String request) throws CannotRunRequest, InvalidRemotePluginResponse, IOException, CannotBuildResponse, CannotFindResultDocument
	{
		PMNRemotePluginService pmnRemotePluginService = new PMNRemotePluginService(serviceUrl);
				
		// CRC_QRY_runQueryInstance_fromQueryDefinition always sends an i2b2 query through PMN to data sources.
		if(i2b2RequestType.equals("CRC_QRY_runQueryInstance_fromQueryDefinition"))
		{
			pmnSessionToken = pmnRemotePluginService.startSession(pmnProxyUser, pmnProxyPassword, i2b2Username, i2b2ProjectId);
			String requestId = pmnRemotePluginService.createRequest(pmnSessionToken, pmnRequestType);
			
			// Post i2b2 request document in hqmf if required.
			pmnRemotePluginService.postDocument(pmnSessionToken, requestId, "i2b2Request.xml", "text/xml", true, HQMFConverter.I2B2toHQMF(request.toString()));
			
			// Post the request again in original unconverted plain i2b2 format.
			pmnRemotePluginService.postDocument(pmnSessionToken, requestId, i2b2RequestType + ".xml", "text/xml", false, request.toString());
			
			pmnRemotePluginService.submitRequest(pmnSessionToken, requestId, new PMNHeader(queryName, null, null), null);
			pmnRemotePluginService.closeSession(pmnSessionToken);				
			
			return loadI2B2MessageTemplate("I2B2DoneMessage");
			
		}
		
		// Everything else is for handling what to tell the i2b2 web client when it needs information for the
		// Previous Queries, the Query Tool and Query Status panels.
		
		else if(i2b2RequestType.equals("CRC_QRY_getQueryMasterList_fromUserId"))
		{
			// Get request list for this user for the i2b2 request type.
//			pmnSessionToken = pmnRemotePluginService.startSession(pmnProxyUser, pmnProxyPassword, i2b2Username);
			pmnSessionToken = pmnRemotePluginService.startSession(pmnProxyUser, pmnProxyPassword, i2b2Username, i2b2ProjectId);

			String queryInstanceId = null;
			String queryResultDesc = null;
			String queryName = null;
			Date createDate = null;
			String qmlResponse = "";

			PMNGetRequestsResponse requestResponse = pmnRemotePluginService.getRequests(pmnSessionToken, null, null);
			
			I2B2QueryMasterList queryMasterList = new I2B2QueryMasterList();
			queryMasterList.setStatus(new ResponseStatus("DONE"));
			queryMasterList.setQueryMasters(new ArrayList<I2B2QueryMaster>());
			
			// Some JAXB implementation unmarshall empty collection to null. 
			if(requestResponse != null && requestResponse.getRequests() != null)
				for(PMNRequest pmnRequest : requestResponse.getRequests())
				{
					if(pmnRequest.getRequestTypeId().toUpperCase().equals(pmnRequestType))
					{
						I2B2QueryMaster qm = new I2B2QueryMaster();
						qm.setQueryMasterId(pmnRequest.getId());
						qm.setName(pmnRequest.getName());
						qm.setUserId(i2b2UserId);
						qm.setGroupId(i2b2GroupId);
						qm.setCreateDate(pmnRequest.getCreateDate());
						queryMasterList.getQueryMasters().add(qm);
					}
				}
			else
				log.debug("requestResponse.getRequests() is null.");
			
			qmlResponse = queryMasterList.toString();

			pmnRemotePluginService.closeSession(pmnSessionToken);

	        return buildResponse(i2b2RequestType, i2b2UserId, i2b2GroupId, i2b2ProjectId, 
	        		queryInstanceId, queryResultDesc, 
	        		queryName, createDate, qmlResponse, "");
		}
		else if(i2b2RequestType.equals("CRC_QRY_getQueryInstanceList_fromQueryMasterId"))
		{						
	        return buildResponse(i2b2RequestType, i2b2UserId, i2b2GroupId, i2b2ProjectId, queryMasterId, null, queryName, null, null, null);
		}
		else if(i2b2RequestType.equals("CRC_QRY_getQueryResultInstanceList_fromQueryInstanceId"))
		{
//			pmnSessionToken = pmnRemotePluginService.startSession(pmnProxyUser, pmnProxyPassword, i2b2Username);
			pmnSessionToken = pmnRemotePluginService.startSession(pmnProxyUser, pmnProxyPassword, i2b2Username, i2b2ProjectId);

			PMNRequest pmnRequest = pmnRemotePluginService.getRequest(pmnSessionToken, queryInstanceId);
			String queryResultInstanceFragment = buildQueryResultInstancesFragment(i2b2RequestType, pmnRequest.getDataMartResponses(), queryInstanceId);

			pmnRemotePluginService.closeSession(pmnSessionToken);

	        return buildResponse(i2b2RequestType, i2b2UserId, i2b2GroupId, i2b2ProjectId, queryInstanceId, null, queryName, null, null, queryResultInstanceFragment);
		}
		else if(i2b2RequestType.equals("CRC_QRY_getRequestXml_fromQueryMasterId"))
		{
//			pmnSessionToken = pmnRemotePluginService.startSession(pmnProxyUser, pmnProxyPassword, i2b2Username);
			pmnSessionToken = pmnRemotePluginService.startSession(pmnProxyUser, pmnProxyPassword, i2b2Username, i2b2ProjectId);
			PMNRequest pmnRequest = pmnRemotePluginService.getRequest(pmnSessionToken, queryMasterId);
			pmnRemotePluginService.closeSession(pmnSessionToken);
			return getRequestXml_fromQueryMasterId(i2b2RequestType, pmnRequest, queryMasterId);			
		}
		else if(i2b2RequestType.equals("CRC_QRY_getResultDocument_fromResultInstanceId"))
		{
			String queryMasterIdResultInstanceId = queryResultInstanceId;
			String[] queryIds = queryMasterIdResultInstanceId.split(",");
			String queryMasterId = queryIds[0];
			String queryResultInstanceId = queryIds[1];
			
//			pmnSessionToken = pmnRemotePluginService.startSession(pmnProxyUser, pmnProxyPassword, i2b2Username);	
			pmnSessionToken = pmnRemotePluginService.startSession(pmnProxyUser, pmnProxyPassword, i2b2Username, i2b2ProjectId);	
			PMNRequest pmnRequest = pmnRemotePluginService.getRequest(pmnSessionToken, queryMasterId);
			pmnRemotePluginService.closeSession(pmnSessionToken);
			return getResultDocument_fromResultInstanceId(pmnRequest.getDataMartResponses(), queryResultInstanceId);
		}
		else
		{			
			String url = PMNResource.getInstance().getI2B2HiveUrl();
			String requestString = request.toString();
			return RESTRequestHelper.postRequest(url, requestString);
		}
	}
	
	private String buildResponse(String i2b2RequestType, String userId, String groupId, String projectId, 
			String queryInstanceId, String queryResultDesc, String queryMasterName, Date createDate, 
			String queryMasterListResponse, String queryResultList) throws CannotBuildResponse
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
	//									         .replace("${CREATE_DATE}", createDate == null ? "" : createDate);
			
			responseMessage = responseMessage.replace("${QUERY_MASTER_LIST_RESPONSE}", queryMasterListResponse == null ? "" : queryMasterListResponse);
			responseMessage = responseMessage.replace("${QUERY_RESULT_LIST}", queryResultList == null ? "" : queryResultList);
			
			return responseMessage;
		}
		catch(IOException e)
		{
			log.error(e);
			throw new CannotBuildResponse(e);
		}
		
	}		

	private String getResultDocument_fromResultInstanceId(List<DataMartResponse> datamartResponses, String resultInstanceId) throws CannotFindResultDocument
	{
		if(datamartResponses != null)
			for(DataMartResponse r : datamartResponses)
			{
				if(r.getDocuments() != null)
					for(Document doc : r.getDocuments())
					{
						if(doc.getId().equals(resultInstanceId))
						{
//								String liveUrl = doc.getLiveUrl();
//								String content = RESTRequestHelper.getRequest(liveUrl);
							String content = new String(doc.getContent());
							return content;
						}
					}
			}
		
		log.error("Cannot find i2b2 result document with result instance id (pmn document id): " + resultInstanceId);
		throw new CannotFindResultDocument("ResultDocument", "ResultInstanceId", resultInstanceId);
	}
	
	private String getRequestXml_fromQueryMasterId(String requestType, PMNRequest pmnRequest, String queryMasterId) throws CannotFindResultDocument
	{
		try
		{
//			if(pmnRequest.getDataMartResponses() != null)
//				for(DataMartResponse r : pmnRequest.getDataMartResponses())
//				{
//					if(r.getDocuments() != null)
//						for(Document doc : r.getDocuments())
//						{
//							if(doc.getName().equals(requestType + ".xml"))
//							{
////								String liveUrl = doc.getLiveUrl();
////								String content = RESTRequestHelper.getRequest(liveUrl);
//								String content = new String(doc.getContent());
//								return content; // we just need the first one
//							}
//						}
//				}
			
			// If results aren't available, fake a response so i2b2 won't hang by constructing
			// the panes from the original request.
			String requestXmlTemplate = loadI2B2MessageTemplate("CRC_QRY_getRequestXml_fromQueryMasterId");

			for(Document doc : pmnRequest.getDocuments())
		    {
		    	if(doc.getName().equals("CRC_QRY_runQueryInstance_fromQueryDefinition.xml"))
		    	{
		    		String content = new String(doc.getContent());
		    		QueryDefinitionType queryDef = (QueryDefinitionType) XmlHelper.getElementValue(content, "query_definition", QueryDefinitionType.class);
		    		String requestXml = requestXmlTemplate.replace("${QUERY_DEFINITION}", queryDef.toString())
		    											  .replace("${QUERY_NAME}", queryDef.getQueryName());
		    		return requestXml;
		    	}
		    }
		
			log.error("Cannot find i2b2 result xml document with query master id (pmn request id): " + queryMasterId);
			throw new CannotFindResultDocument("RequestXml", "QueryMasterId", queryMasterId);

		}
		catch (IOException e) 
		{
			log.error(e);
			throw new CannotFindResultDocument("RequestXml", "QueryMasterId", queryMasterId);
		} 
		catch (CannotGetElementValue e) 
		{
			log.error(e);
			throw new CannotFindResultDocument("RequestXml", "QueryMasterId", queryMasterId);
		}	
	}
	
}
