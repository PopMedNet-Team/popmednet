package com.lincolnpeak.i2b2.crc.axis2;

import java.util.Date;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

import com.lincolnpeak.i2b2.crc.axis2.pmnrequests.PMNCredentials;
import com.lincolnpeak.i2b2.crc.axis2.pmnrequests.PMNGetRequests;
import com.lincolnpeak.i2b2.crc.axis2.pmnrequests.PMNHeader;
import com.lincolnpeak.i2b2.crc.axis2.pmnrequests.PMNPostDocument;
import com.lincolnpeak.i2b2.crc.axis2.pmnrequests.PMNStartProjectSession;
import com.lincolnpeak.i2b2.crc.axis2.pmnrequests.PMNStartSession;
import com.lincolnpeak.i2b2.crc.axis2.pmnrequests.PMNSubmitRequest;
import com.lincolnpeak.i2b2.crc.axis2.pmnresponses.PMNCreateRequestResponse;
import com.lincolnpeak.i2b2.crc.axis2.pmnresponses.PMNGetRequestsResponse;
import com.lincolnpeak.i2b2.crc.axis2.pmnresponses.PMNRequest;
import com.lincolnpeak.i2b2.crc.axis2.pmnresponses.PMNStartProjectSessionResponse;
import com.lincolnpeak.i2b2.crc.axis2.pmnresponses.PMNStartSessionResponse;
import com.lincolnpeak.i2b2.exceptions.CannotRunRequest;
import com.lincolnpeak.i2b2.exceptions.InvalidRemotePluginResponse;
import com.lincolnpeak.i2b2.utils.RESTRequestHelper;

/**
 * Java client counterpart of the C# Remote REST service in PMN.
 * 
 * @author daniel
 */
public class PMNRemotePluginService 
{
	protected final Log log = LogFactory.getLog(getClass());

	private String serviceUrl;

	public PMNRemotePluginService(String serviceUrl)
	{
		this.serviceUrl = serviceUrl;		
	}
	
	/**
	 * This operation must be called by the model provider before all others 
	 * to get a session token for all subsequent operations.
	 * @param pmnProxyUser Username that can log into PMN. Typically, an account that proxies for other users.
	 * @param pmnProxyPassword Password for the above.
	 * @return unique limited duration session token
	 * @throws InvalidRemotePluginResponse 
	 */
	public String startSession(String pmnProxyUser, String pmnProxyPassword, String requestor) throws CannotRunRequest, InvalidRemotePluginResponse 
	{
		try 
		{
			String url = serviceUrl + "/" + requestor + "/Start"; 
			PMNStartSession pmnStart = new PMNStartSession(new PMNCredentials(pmnProxyUser, pmnProxyPassword));
			String response = RESTRequestHelper.postRequest(url, pmnStart.toString());
			return PMNStartSessionResponse.load(PMNStartSessionResponse.class, response).getStartSessionResult();
		} 
		catch (Exception e) 
		{
			log.debug(e);
			throw new InvalidRemotePluginResponse(e);
		} 
	}
	
	/**
	 * This operation must be called by the model provider before all others 
	 * to get a session token for all subsequent operations.
	 * @param pmnProxyUser Username that can log into PMN. Typically, an account that proxies for other users.
	 * @param pmnProxyPassword Password for the above.
	 * @param pmnProjectAcronym i2b2 project id (i.e., name - this will be matched to PMN acronym)
	 * @return unique limited duration session token
	 * @throws InvalidRemotePluginResponse 
	 */
	public String startSession(String pmnProxyUser, String pmnProxyPassword, String requestor, String pmnProjectAcronym) throws CannotRunRequest, InvalidRemotePluginResponse 
	{
		try 
		{
			String url = serviceUrl + "/" + requestor + "/" + pmnProjectAcronym + "/Start"; 
			PMNStartProjectSession pmnStart = new PMNStartProjectSession(new PMNCredentials(pmnProxyUser, pmnProxyPassword));
			String response = RESTRequestHelper.postRequest(url, pmnStart.toString());
			return PMNStartProjectSessionResponse.load(PMNStartProjectSessionResponse.class, response).getStartSessionResult();
		} 
		catch (Exception e) 
		{
			log.debug(e);
			throw new InvalidRemotePluginResponse(e);
		} 
	}
	
	/**
	 * This operation must be called by the model provider to close the session. 
     * All open requests must be submitted or aborted before closing the session.
     * The session token will not be valid for any more calls after this.
	 * @param pmnSessionToken Identifies the current communication session.
	 * @throws InvalidRemotePluginResponse 
	 */
	public void closeSession(String pmnSessionToken) throws InvalidRemotePluginResponse
	{
		try
		{
			String url = serviceUrl + "/" + pmnSessionToken + "/Close"; 
			RESTRequestHelper.postRequest(url);
		}
		catch(Exception e)
		{
			log.debug(e);
			throw new InvalidRemotePluginResponse(e);			
		}
	}
	
    /**
     * Starts a new request that is to be constructed.
     * @param pmnSessionToken Identifies the current communication session.
     * @param pmnRequestTypeId Identifies the type of request being constructed.
     * @return A request identifier
     * @throws InvalidRemotePluginResponse 
     */
    public String createRequest(String pmnSessionToken, String pmnRequestTypeId) throws InvalidRemotePluginResponse
    {
    	try
    	{
			String url = serviceUrl + "/" + pmnSessionToken + "/%7B" + pmnRequestTypeId + "%7D/Create";
			String response = RESTRequestHelper.postRequest(url);
			return ((PMNCreateRequestResponse) PMNCreateRequestResponse.load(response)).getCreateRequestResult();
    	}
    	catch(Exception e)
    	{
			log.debug(e);
			throw new InvalidRemotePluginResponse(e);			    		
    	}
    }
    
    /**
     * Adds an accompanying document to the request that is being constructed.
     * @param pmnSessionToken Identifies the current communication session.
     * @param requestId Identifies the request to which this document is being added.
     * @param documentName Name of the document. If there is already a document with this name associated
     *                     with the request being constructed, that document will be replaced.
     * @param documentMimeType MIME type of the document.
     * @param documentBody Content of the document.
     * @param filterByDataMartIds List of IDs of datamarts for which to return requests. May be null, in which case all user's datamarts are assumed.
     * @throws InvalidRemotePluginResponse 
     */
    void postDocument(String pmnSessionToken, String requestId, String documentName, String documentMimeType, boolean isViewable, String documentBody) throws InvalidRemotePluginResponse
    {
    	try
    	{
			String url = serviceUrl + "/" + pmnSessionToken + "/" + requestId + "/Document";
			
			PMNPostDocument pmnPostDocument = new PMNPostDocument(documentName, documentMimeType, isViewable);					
			pmnPostDocument.setBody(documentBody);
			String pmnRequest = pmnPostDocument.toString();
			RESTRequestHelper.postRequest(url, pmnRequest);
    	}
    	catch(Exception e)
    	{
			log.debug(e);
			throw new InvalidRemotePluginResponse(e);			    			
    	}
    }
    
    /**
     * This operation must be called by the Model Provider at the end of processing a request
     * to indicate that the creation of the request has been completed, and to provide the
     * result of that process - the request itself in serialized form.
     * This must be the last call in the specified request. The requestId will not be valid for any more Post, Submit or Abort after this.
     * @param pmnSessionToken Identifies the current communication session.
     * @param requestId Identifies the request to be submitted for processing.
     * @param requestHeader Header. Common attributes of a request.
     * @param applicableDataMartIds The list of data marts that are applicable to the current request.
     *                              This should be a subset of the list returned by @see #GetApplicableDataMarts. If there are any extra
     *                              data marts in this list, they are ignored. This list may be left empty, in which case all applicable data marts are used. (unused)
     * @throws InvalidRemotePluginResponse 
     */
    public void submitRequest(String pmnSessionToken, String requestId, PMNHeader requestHeader, int[] applicableDataMartIds) throws InvalidRemotePluginResponse
    {
    	try
    	{
    		String url = serviceUrl + "/" + pmnSessionToken + "/" + requestId + "/Submit";
    		String pmnSubmit = new PMNSubmitRequest(requestHeader).toString();
    		RESTRequestHelper.postRequest(url, pmnSubmit);
    	}
    	catch(Exception e)
    	{
			log.debug(e);
			throw new InvalidRemotePluginResponse(e);			    			
    	}
    }
    
    /**
     * Retrieves a list of requests accessible to the current session,
     * and optionally filtered for particular datamarts
     * @param pmnSessionToken Identifies the current communication session.
     * @param fromDate The earliest date of requests in the returned list. May be null for no start date.
     * @param toDate The latest date of the requests in the returned list. May be null for no end date.
     * @return
     * @throws InvalidRemotePluginResponse 
     */
    public PMNGetRequestsResponse getRequests(String pmnSessionToken, Date fromDate, Date toDate) throws InvalidRemotePluginResponse
    {
    	try
    	{
    		String url = serviceUrl + "/" + pmnSessionToken + "/Requests";
    		PMNGetRequests getRequests = new PMNGetRequests(fromDate, toDate);
    		String requestList = RESTRequestHelper.postRequest(url, getRequests.toString());
    		return PMNGetRequestsResponse.load(PMNGetRequestsResponse.class, requestList);
    	}
    	catch(Exception e)
    	{
			log.debug(e);
			throw new InvalidRemotePluginResponse(e);			    			    		
    	}
    }
    
    /**
     * Retrieves responses associated with a request optionally filtered by DataMarts.
     * @param pmnSessionToken Identifies the current communication session.
     * @param requestId Identifies the request whose responses are desired.
     * @throws InvalidRemotePluginResponse 
     * @returns Request
     */
    public PMNRequest getRequest(String pmnSessionToken, String requestId) throws InvalidRemotePluginResponse
    {
    	try
    	{
    		String url = serviceUrl + "/" + pmnSessionToken + "/" + requestId + "/Request";
    		String request = RESTRequestHelper.getRequest(url);
    		return PMNRequest.load(PMNRequest.class, request);
    	}
    	catch(Exception e)
    	{
			log.debug(e);
			throw new InvalidRemotePluginResponse(e);			    			    		
    	}
    }
}
