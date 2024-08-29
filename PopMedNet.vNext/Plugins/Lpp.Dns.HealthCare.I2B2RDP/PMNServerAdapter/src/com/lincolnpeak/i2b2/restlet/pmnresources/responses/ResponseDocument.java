package com.lincolnpeak.i2b2.restlet.pmnresources.responses;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

@XmlRootElement(name="Document")
@XmlAccessorType(XmlAccessType.FIELD)
public class ResponseDocument
{
	@XmlElement(name="DocumentId")
	private String documentId;

	@XmlElement(name="Filename")
	private String filename;
	
	@XmlElement(name="MimeType")
	private String mimeType;
	
	@XmlElement(name="Size")
	private int size;
	
	@XmlElement(name="Viewable")
	private boolean viewable;
	

	
    public String getDocumentId()
    { 
    	return documentId;
    }
    
    public void setDocumentId(String documentId)
    {
    	this.documentId = documentId;
    }
    
    public String getMimeType() 
    { 
    	return mimeType;
    }
    
    public void setMimeType(String mimeType)
    {
    	this.mimeType = mimeType;
    }
    
    public int getSize() 
    { 
    	return size;
    }
    
    public void setSize(int size)
    {
    	this.size = size; 
    }
    
    public boolean getViewable()
    { 
    	return viewable;
    }
    
    public void setViewable(boolean viewable)
    {
    	this.viewable = viewable;
    }
    
    public String getFilename() 
    { 
    	return filename;
    }
    
    public void setFilename(String filename)
    {
    	this.filename = filename; 
    }
}
