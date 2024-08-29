package com.lincolnpeak.i2b2.crc.axis2;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

import com.lincolnpeak.i2b2.crc.axis2.pmnrequests.PMNHeader;
import com.lincolnpeak.i2b2.crc.axis2.pmnrequests.PMNPostDocument;
import com.lincolnpeak.i2b2.crc.axis2.pmnresponses.PMNResponseSessionMetadata;
import com.lincolnpeak.i2b2.crc.axis2.pmnresponses.PMNSessionMetadata;
import com.lincolnpeak.i2b2.exceptions.InvalidRemotePluginResponse;
import com.lincolnpeak.i2b2.utils.RESTRequestHelper;

/**
 * Java client counterpart of the C# Redirect REST service in PMN.
 * 
 * @author daniel
 */
public class PMNRedirectPluginService 
{
	protected final Log log = LogFactory.getLog(getClass());

	private String serviceUrl;

	public PMNRedirectPluginService(String serviceUrl)
	{
		this.serviceUrl = serviceUrl;		
	}
	
    /**
     * Retrieve metadata associated with the current session
     * @param pmnSessionToken Identifies the current communication session.
     * @return metadata
     * @throws InvalidRemotePluginResponse 
     */
    public PMNSessionMetadata getSessionMetadata( String pmnSessionToken ) throws InvalidRemotePluginResponse
    {
    	try
    	{
			String url = serviceUrl + "/" + pmnSessionToken + "/Session";
	
			String sessionMetadata = RESTRequestHelper.getRequest(url);
			return PMNSessionMetadata.load(PMNSessionMetadata.class, sessionMetadata);
//			return ((PMNSessionMetadata) PMNSessionMetadata.load(sessionMetadata));
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
     * This must be the last call in the current session. The sessionToken will not be valid for any more calls after this.
     * @param pmnSessionToken Identifies the current communication session.
     * @param requestHeader Header. Common attributes of a request.
     * @param requestMimeType">MIME type of the request.
     * @param requestBody The created request in serialized form.
     * @param applicableDataMartIds The list of data marts that are applicable to the current request.
                                    This should be a subset of the list returned by <see cref="GetApplicableDataMarts"/>. If there are any extra
                                    data marts in this list, they are ignored. This list may be left empty, in which case all applicable data marts are used.
     * @throws InvalidRemotePluginResponse 
     */
    public void requestCreated( String pmnSessionToken, PMNHeader requestHeader, int[] applicableDataMartIds ) throws InvalidRemotePluginResponse
    {
    	try
    	{
			String url = serviceUrl + "/" + pmnSessionToken + "/Commit";
			RESTRequestHelper.postRequest(url);
    	}
    	catch(Exception e)
    	{
    		log.debug(e);
			throw new InvalidRemotePluginResponse(e);       		
    	}

    }
    
    /**
     * Adds an accompanying document to the request that is currently being constructed.
     * @param pmnSessionToken Identifies the current communication session.
     * @param documentName Name of the document. If there is already a document with this name associated.
     *                     with the request being constructed, that document will be replaced.</param>
     * @param documentMimeType MIME type of the document.
     * @param documentBody Content of the document
     * @throws InvalidRemotePluginResponse 
     */
    public void postDocument( String pmnSessionToken, String documentName, String documentMimeType, boolean isViewable, String documentBody ) throws InvalidRemotePluginResponse
    {
    	try
    	{
			String url = serviceUrl + "/" + pmnSessionToken + "/Document";
			
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
     * Retrieve metadata associated with the current session
     * @param pmnSessionToken Identifies the current communication session.
     * @return metadata
     * @throws InvalidRemotePluginResponse 
     */
    public PMNResponseSessionMetadata getResponseSessionMetadata( String pmnSessionToken ) throws InvalidRemotePluginResponse
    {
    	try
    	{
			String url = serviceUrl + "/" + pmnSessionToken + "/Session";
	
			String sessionMetadata = RESTRequestHelper.getRequest(url);
			return PMNResponseSessionMetadata.load(PMNResponseSessionMetadata.class, sessionMetadata);
    	}
    	catch(Exception e)
    	{
    		log.debug(e);
			throw new InvalidRemotePluginResponse(e);   
    	}
    }
}
