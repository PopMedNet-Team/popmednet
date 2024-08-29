/*
 * Copyright (c) 2012 Lincoln Peak Partners 
 * All rights reserved. This program and the accompanying materials 
 * are made available under the terms of the Lincoln Peak Partners Software License v1.0 
 * which accompanies this distribution. 
 */
package com.lincolnpeak.i2b2.crc.axis2;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.StringReader;

import javax.xml.stream.XMLInputFactory;
import javax.xml.stream.XMLStreamException;
import javax.xml.stream.XMLStreamReader;

import org.apache.axiom.om.OMElement;
import org.apache.axiom.om.impl.builder.StAXOMBuilder;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

import com.lincolnpeak.i2b2.PMNResource;
import com.lincolnpeak.i2b2.crc.axis2.hqmfresponses.HQMFErrors;
import com.lincolnpeak.i2b2.crc.axis2.i2b2requests.I2B2RequestBase;
import com.lincolnpeak.i2b2.exceptions.CannotBuildResponse;
import com.lincolnpeak.i2b2.exceptions.CannotRunRequest;
import com.lincolnpeak.i2b2.utils.RESTRequestHelper;

/**
 * <b>Axis2's service class<b>
 * 
 * <p>
 * This class intercepts CRC requests by pretending to be CRC cell.
 * <p>
 * Implements the CRC required services.
 * 
 * <li>For example http://localhost:8080/axis2/services/crc/serfinderrequest
 * http://localhost:8080/axis2/services/crc/pdorequest
 * 
 * @author ddee
 */
public class QueryService 
{
	protected final Log log = LogFactory.getLog(getClass());
	
	/**
	 * Webservice function to handle setfinder request.
	 * 
	 * @param omElement request message wrapped in OMElement
	 * @return response message in wrapped inside OMElement
	 */
	public OMElement request(OMElement omElement) 
	{
		log.debug("Inside setfinder request " + omElement);
		return handleSetFinderRequest(omElement);
	}

	/**
	 * Webservice function to handle pdo request.
	 * This implementation forwards the request to the real CRC.
	 * 
	 * @param omElement request message wrapped in OMElement
	 * @return response message in wrapped inside OMElement
	 */
	public OMElement pdorequest(OMElement request) 
	{
		OMElement returnElement = null;
		try
		{
			String url = PMNResource.getInstance().getI2B2HiveUrl() + "/pdorequest";
			String response = RESTRequestHelper.postRequest(url, request.toString());
			returnElement = buildOMElementFromString(response);
		}
		catch(XMLStreamException e)
		{
			log.error("xml stream exception", e);
		} 
		catch (CannotRunRequest e) 
		{
			log.error(e);
		}
		
		return returnElement;
	}

	/**
	 * pmnRequestId = queryMasterId
	 * pmnDocumentId = resultInstanceId
	 * 
	 * @param request
	 * @return
	 */
	private OMElement handleSetFinderRequest(OMElement request) 
	{		
		OMElement returnElement = null;
		String response = null;
		String returnUrl = null;
		
		try 
		{
			I2B2RequestBase i2b2Request = new I2B2RequestBase(request);
			boolean isResponse = i2b2Request.getIsPMNResponse();
			boolean isRequest = i2b2Request.getIsPMNRequest();
			
			if(isResponse || isRequest) // request originated from PMN - redirect mode
			{
				PMNRedirectSetFinderHandler handler = new PMNRedirectSetFinderHandler(i2b2Request);
				response = handler.handleSetFinderRequest(request.toString());				
			}
			else // request originated from i2b2 web client - remote plugin mode
			{
				PMNRemoteSetFinderHandler handler = new PMNRemoteSetFinderHandler(i2b2Request);
				response = handler.handleSetFinderRequest(request.toString());
			}
		} 
		catch (Throwable e) 
		{
			log.error("Throwable", e);

			try 
			{
				String message = "Cannot run query.";
				
				if(e instanceof CannotRunRequest && e.getMessage() != null && e.getMessage().trim().length() > 0)
				{
					// See if HQMF error.
					try
					{
						HQMFErrors hqmfErrors = HQMFErrors.load(HQMFErrors.class, e.getMessage());
						message = hqmfErrors.getWarning();
					}
					catch(Exception ex)
					{
						
					}
				}
				response = buildResponseMessage("I2B2ErrorMessage", returnUrl, message);
			} 
			catch (CannotBuildResponse e1) 
			{
				log.error(e1);
			}
		}
		
		try
		{
			returnElement = buildOMElementFromString(response);
		} 
		catch (XMLStreamException e) 
		{
			log.error(e);
		}

		return returnElement;
	}

	/**
	 * Function constructs OMElement for the given String
	 * 
	 * @param xmlString
	 * @return OMElement
	 * @throws XMLStreamException
	 */
	private OMElement buildOMElementFromString(String xmlString)
			throws XMLStreamException 
	{
		XMLInputFactory xif = XMLInputFactory.newInstance();
		StringReader strReader = new StringReader(xmlString);
		XMLStreamReader reader = xif.createXMLStreamReader(strReader);
		StAXOMBuilder builder = new StAXOMBuilder(reader);
		OMElement element = builder.getDocumentElement();
		return element;
	}
	
	private String loadI2B2MessageTemplate(String name) throws IOException
	{
		BufferedReader reader = null;
		try
		{
			reader = new BufferedReader(new InputStreamReader(this.getClass().getResourceAsStream("/com/lincolnpeak/i2b2/crc/axis2/" + name + ".xml")));
			String messageXml = "";
			String line;
			while((line = reader.readLine()) != null)
				messageXml += line;						
			
			return messageXml;
		}
		finally
		{
			if(reader != null)
				reader.close();
		}
		
	}

	private String buildResponseMessage(String responseType, String returnUrl, String message) throws CannotBuildResponse
	{
		try
		{
			String responseMessage = loadI2B2MessageTemplate(responseType);
			responseMessage = responseMessage.replace("${RETURN_URL}", returnUrl == null ? "" : returnUrl);
			responseMessage = responseMessage.replace("${MESSAGE}", message == null ? "" : message);
			
			return responseMessage;
		}
		catch(IOException e)
		{
			log.error(e);
			throw new CannotBuildResponse(e);
		}
		
	}
	
	// The following are unused for this implementation.
	
	public OMElement publishDataRequest(OMElement request) 
	{
		throw new UnsupportedOperationException();
		
		// LoaderQueryRequestDelegate queryDelegate = new
		// LoaderQueryRequestDelegate();
		// OMElement responseElement = null;
		// try {
		// String requestXml = request.toString();
		// PublishDataRequestHandler handler = new PublishDataRequestHandler(
		// requestXml);
		// String response = queryDelegate.handleRequest(requestXml, handler);
		// responseElement = buildOMElementFromString(response);
		//
		// } catch (XMLStreamException e) {
		// log.error("xml stream exception", e);
		// } catch (I2B2Exception e) {
		// log.error("i2b2 exception", e);
		// } catch (Throwable e) {
		// log.error("Throwable", e);
		// }
		// return responseElement;
	}

	public OMElement getLoadDataStatusRequest(OMElement request) 
	{
		throw new UnsupportedOperationException();
		
		// ProviderRestService rs = new ProviderRestService();
		// return rs.getLoadDataStatusRequest(request);
	}

	public OMElement getMissingTermRequest(OMElement request) 
	{
		throw new UnsupportedOperationException();
		
		// ProviderRestService rs = new ProviderRestService();
		// return rs.getMissingTermRequest(request);
	}
}

