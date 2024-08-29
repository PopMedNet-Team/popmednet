package com.lincolnpeak.i2b2.restlet.pmnresources;

import java.util.HashMap;
import java.util.List;

import com.lincolnpeak.i2b2.restlet.pmnresources.responses.ResponseDocument;

public class I2B2Request 
{
	private List<PMNDocument> docs;
	private List<ResponseDocument> responseDocs;
	private HashMap<String, String> docData;
	private HashMap<String, String> responseDocData;
	private StatusCode status;
	private String statusMessage = "";
	
	public I2B2Request()
	{
		docData = new HashMap<String, String>();
		responseDocData = new HashMap<String, String>();
	}
	
	public void setDocuments(List<PMNDocument> docs)
	{
		this.docs = docs;
	}
	
	public List<PMNDocument> getDocuments()
	{
		return docs;
	}
	
	public boolean containsDocumentData(String docId)
	{
		return docData.containsKey(docId);
	}
	
	public void putDocumentData(String docId, String data, int offset)
	{
		StringBuffer buffer = new StringBuffer(docData.containsKey(docId) ? docData.get(docId) : "");
		String doc = buffer.replace(offset, data.length() + offset, data).toString();
		docData.put(docId, doc);
	}
	
	public String getDocumentData(String docId)
	{
		return docData.get(docId);
	}
	
	public void removeDocumentData(String docId)
	{
		responseDocData.remove(docId);
	}
	
	public void setResponseDocuments(List<ResponseDocument> docs)
	{
		this.responseDocs = docs;
	}
	
	public List<ResponseDocument> getResponseDocuments()
	{
		return responseDocs;
	}
	
	public void putResponseDocumentData(String docId, String data)
	{
		responseDocData.put(docId, data);
	}
	
	public String getResponseDocumentData(String docId)
	{
		return responseDocData.get(docId);
	}
	
	public void removeResponseDocumentData(String docId)
	{
		responseDocData.remove(docId);
	}
	
	public void setStatus(StatusCode code)
	{
		this.status = code;		
	}
	
	public StatusCode getStatus()
	{
		return status;
	}
	
	public void setStatusMessage(String message)
	{
		this.statusMessage = message;		
	}
	
	public String getStatusMessage()
	{
		return statusMessage;
	}
}
