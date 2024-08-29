package com.lincolnpeak.i2b2.restlet.pmnresources;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.ArrayList;
import java.util.List;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.restlet.representation.Representation;
import org.restlet.resource.Put;

import com.lincolnpeak.i2b2.exceptions.CannotGetElementValue;
import com.lincolnpeak.i2b2.exceptions.CannotRunRequest;
import com.lincolnpeak.i2b2.restlet.pmnresources.exceptions.CannotBuildRestResponse;
import com.lincolnpeak.i2b2.restlet.pmnresources.exceptions.CannotGetMessage;
import com.lincolnpeak.i2b2.restlet.pmnresources.exceptions.CannotPostCRCRequest;
import com.lincolnpeak.i2b2.restlet.pmnresources.responses.ResponseDocument;
import com.lincolnpeak.i2b2.restlet.pmnresources.responses.StartResponse;
import com.lincolnpeak.i2b2.utils.ConceptTranslator;
import com.lincolnpeak.i2b2.utils.HQMFConverter;
import com.lincolnpeak.i2b2.utils.RESTRequestHelper;
import com.lincolnpeak.i2b2.utils.xml.CreateDate;
import com.lincolnpeak.i2b2.utils.xml.MessageControlId;
import com.lincolnpeak.i2b2.utils.xml.ProjectId;
import com.lincolnpeak.i2b2.utils.xml.QueryDefinitionType;
import com.lincolnpeak.i2b2.utils.xml.RequestType;
import com.lincolnpeak.i2b2.utils.xml.QueryInstanceId;
import com.lincolnpeak.i2b2.utils.xml.QueryMasterId;
import com.lincolnpeak.i2b2.utils.xml.QueryResultInstance;
import com.lincolnpeak.i2b2.utils.xml.ResultInstanceId;
import com.lincolnpeak.i2b2.utils.xml.ResultOutput;
import com.lincolnpeak.i2b2.utils.xml.Security;
import com.lincolnpeak.i2b2.utils.xml.XmlHelper;

public class Start extends PMNResource 
{
	private final Log log = LogFactory.getLog(getClass());

	@Put("xml")
	public Representation StartExecution(Representation rep) throws CannotBuildRestResponse, CannotPostCRCRequest 
	{
		String requestId = (String) getRequestAttributes().get("requestId");	
		I2B2Request i2b2Request = I2B2Requests.getInstance().get(requestId);

		try
		{
			runI2B2Request(i2b2Request);
			i2b2Request.setStatus(StatusCode.Complete);
			return buildRestResponse(StartResponse.class);		
		}
		catch(CannotPostCRCRequest e)
		{
			i2b2Request.setStatus(StatusCode.Error);
			throw e;
		}
		catch(CannotBuildRestResponse e)
		{
			i2b2Request.setStatus(StatusCode.Error);
			throw e;
		}		
	}	
		
	private void runI2B2Request(I2B2Request i2b2Request) throws CannotPostCRCRequest
	{
		try
		{
			String docId = i2b2Request.getDocuments().get(0).getDocumentId();
			String docData = i2b2Request.getDocumentData(docId);

//			Security security = (Security) XmlHelper.getElementValue(docData, "security", Security.class);
			String user = getCRCProxyUser();
			String password = getCRCProxyPassword();
			String group = getCRCProxyGroup();
			String domain = getCRCProxyDomain();
			String projectId = getCRCProxyProject();
			
			String hqmfDomain = getHqmfProxyDomain();
			String hqmfProjectId = getHqmfProxyProject();
			String hqmfUser = getHqmfProxyUser();
			String hqmfPassword = getHqmfProxyPassword();
			
			docData = HQMFConverter.HQMFtoI2B2(docData, hqmfDomain, hqmfProjectId, hqmfUser, hqmfPassword);
			
			int passwordStart = docData.indexOf("<password");
			if (passwordStart < 0) // an hqmf to i2b2 conversion, wrap in i2b2 message
			{
				docData = XmlHelper.getElementFragment(docData, "query_definition");
				String i2b2Message = getRunQueryInstanceFromQueryDefinitionMessage();
				docData = i2b2Message.replace("${QUERY_DEFINITION}", docData);
			}

			// Replace username and password.
			docData = XmlHelper.replaceElementFragment(docData, "domain", "<domain>" + domain + "</domain>");
			docData = XmlHelper.replaceElementFragment(docData, "username", "<username>" + user + "</username>");
			docData = XmlHelper.replaceElementFragment(docData, "password", "<password token_ms_timeout=\"1800000\" is_token=\"false\">" + password + "</password>");
			docData = XmlHelper.replaceElementFragment(docData, "user", "<user group=\"" + group + "\" login=\"" + user + "\">" + user + "</user>");

			docData = ConceptTranslator.getInstance().toLocal(docData);

			String requestType = ((RequestType) XmlHelper.getElementValue(docData, "request_type", RequestType.class)).getValue();
						
			// Run test message?
			if(Boolean.parseBoolean(getResourceValue("useTestMessage", "false")))
				docData = getMessage(getResourceValue("testMessage", "BITestMessage.xml"));
			String httpResponseMessage = RESTRequestHelper.postRequest(getCRCUrl(), docData);
			String crcStatus = ((QueryResultInstance) XmlHelper.getElementValue(httpResponseMessage, "query_result_instance", QueryResultInstance.class)).getQueryStatusType().getName();
			
			if(crcStatus.equals("ERROR"))
				throw new CannotRunRequest(httpResponseMessage);
			
			buildResponseDocuments(i2b2Request, docData, requestType, httpResponseMessage);
		}
		catch(CannotRunRequest e)
		{
			throw new CannotPostCRCRequest(e);
		} 
		catch (CannotGetElementValue e) 
		{
			throw new CannotPostCRCRequest(e);
		}
	}
	
	private void buildResponseDocuments(I2B2Request i2b2Request, String docData, String requestType, String httpResponseMessage) throws CannotPostCRCRequest
	{
		try
		{
			String queryMasterId = ((QueryMasterId) XmlHelper.getElementValue(httpResponseMessage, "query_master_id", QueryMasterId.class)).getValue();
			String queryInstanceId = ((QueryInstanceId) XmlHelper.getElementValue(httpResponseMessage, "query_instance_id", QueryInstanceId.class)).getValue();
			String resultInstanceId = ((ResultInstanceId) XmlHelper.getElementValue(httpResponseMessage, "result_instance_id", ResultInstanceId.class)).getValue();
			String createDate = ((CreateDate) XmlHelper.getElementValue(httpResponseMessage, "create_date", CreateDate.class)).getValue();
			QueryResultInstance queryResultInstance = ((QueryResultInstance) XmlHelper.getElementValue(httpResponseMessage, "query_result_instance", QueryResultInstance.class));
			String queryResultDesc = queryResultInstance.getDescription();
	
			// Retrieve result and store in a DNS Document.
			
			String i2b2Result;
	
			QueryDefinitionType queryDef = (QueryDefinitionType) XmlHelper.getElementValue(docData, "query_definition", QueryDefinitionType.class);

			Security security = (Security) XmlHelper.getElementValue(docData, "security", Security.class);
			String user = security.getUsername();
//			String password = security.getPassword();
			String password = getCRCProxyPassword();
			String domain = security.getDomain();
			String projectId = ((ProjectId) XmlHelper.getElementValue(docData, "project_id", ProjectId.class)).getValue();
			String messageNum = ((MessageControlId) XmlHelper.getElementValue(docData, "message_control_id", MessageControlId.class)).getMessageNum();
			String queryName = queryDef.getQueryName();
			boolean isCount = ((ResultOutput) XmlHelper.getElementValue(docData, "result_output", ResultOutput.class)).getName().indexOf("count") > 0;
			
			if(isCount)
			{
				String getResultMessage = buildRequestMessage("GetCountMessage", queryMasterId, resultInstanceId, user, password, messageNum, domain, projectId);                  		
				i2b2Result = RESTRequestHelper.postRequest(getCRCUrl(), getResultMessage);
			}
			else
			{
				String getResultMessage = buildRequestMessage("GetPatientSetMessage", queryMasterId, resultInstanceId, user, password, messageNum, domain, projectId);                  		
				i2b2Result = RESTRequestHelper.postRequest(getCRCUrl() + "pdorequest", getResultMessage);
			}

			// FIXME: Check message error and throw webservice fault.

			List<ResponseDocument> responseDocs = new ArrayList<ResponseDocument>();
			
			// Main result			
			ResponseDocument responseDoc = new ResponseDocument();
			responseDoc.setDocumentId("1"); // document id
			responseDoc.setFilename(requestType + ".xml");
			responseDoc.setMimeType("text/xml");
			responseDoc.setSize(i2b2Result.length());
			responseDoc.setViewable(true);
			responseDocs.add(responseDoc);

			i2b2Request.putResponseDocumentData("1", i2b2Result);

			// CRC_QRY_getQueryMasterList_fromUserId
			StringBuffer props = new StringBuffer().append("query_instance_id=" + queryInstanceId);
			props.append("\\nresult_instance_id=" + resultInstanceId);
			props.append("\\nquery_name=" + queryName);
			props.append("\\ncreate_date=" + createDate);
			props.append("\\nquery_result_description=" + queryResultDesc);
			props.append("\\nis_count=" + isCount);			
			
			responseDoc = new ResponseDocument();
			responseDoc.setDocumentId("2"); // document id
			responseDoc.setFilename("i2b2Metadata.properties");
			responseDoc.setMimeType("text/plain");
			responseDoc.setSize(props.length());
			responseDoc.setViewable(false);
			responseDocs.add(responseDoc);

			i2b2Request.putResponseDocumentData("2", props.toString());
			
			// CRC_QRY_getResultXml_fromQueryMasterId
			String getResultMessage = buildRequestMessage("CRC_QRY_getRequestXml_fromQueryMasterId", queryMasterId, resultInstanceId, user, password, messageNum, domain, projectId);                  		
			i2b2Result = RESTRequestHelper.postRequest(getCRCUrl(), getResultMessage);
			
			responseDoc = new ResponseDocument();
			responseDoc.setDocumentId("3"); // document id
			responseDoc.setFilename("CRC_QRY_getRequestXml_fromQueryMasterId.xml");
			responseDoc.setMimeType("text/xml");
			responseDoc.setSize(i2b2Result.length());
			responseDoc.setViewable(false);
			responseDocs.add(responseDoc);

			i2b2Request.putResponseDocumentData("3", i2b2Result);

			// CRC_QRY_getQueryInstanceList_fromQueryMasterId
			
			// CRC_QRY_getQueryResultInstanceList_fromQueryInstanceId
			
			
			i2b2Request.setResponseDocuments(responseDocs);
			
		}
		catch(CannotRunRequest e)
		{
			throw new CannotPostCRCRequest(e);
		} 
		catch (CannotGetMessage e) 
		{
			throw new CannotPostCRCRequest(e);
		} 
		catch (CannotGetElementValue e) 
		{
			throw new CannotPostCRCRequest(e);
		}
	}
	
	private String buildRequestMessage(String messageName, String queryMasterId, String resultInstanceId, String username, String passwordToken, String messageNum, String domain, String projectId) throws CannotGetMessage
	{
		BufferedReader reader = null;
		try
		{
			reader = new BufferedReader(new InputStreamReader(this.getClass().getResourceAsStream("/com/lincolnpeak/i2b2/restlet/pmnresources/" + messageName + ".xml")));
			String getResultMessage = "";
			String line;
			while((line = reader.readLine()) != null)
				getResultMessage += line;
						
			try
			{
				getResultMessage = getResultMessage.replace("${CRC_URL}", getCRCUrl())
				                				   .replace("${USERNAME}", username)
				                				   .replace("${PASSWORD}", passwordToken)
				                				   .replace("${MESSAGE_NUM}", messageNum)
				                				   .replace("${DOMAIN}", domain)
				                				   .replace("${PROJECT_ID}", projectId)
				                				   .replace("${QUERY_MASTER_ID}", queryMasterId)
				                				   .replace("${RESULT_INSTANCE_ID}", resultInstanceId);
			}
			catch(Exception e)
			{
				log.error(e);
			}
			
			return getResultMessage;
		}
		catch(IOException e)
		{
			log.error(e);
			throw new CannotGetMessage(e);
		}
		finally
		{
			if(reader != null)
				try 
				{
					reader.close();
				} 
				catch (IOException e) 
				{
					log.error(e);
					throw new CannotGetMessage(e);
				}
		}
		
	}
	
}
