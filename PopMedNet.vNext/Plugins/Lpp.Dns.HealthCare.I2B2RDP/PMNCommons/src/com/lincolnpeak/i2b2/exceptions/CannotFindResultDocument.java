package com.lincolnpeak.i2b2.exceptions;

public class CannotFindResultDocument extends Exception
{
	private static final long serialVersionUID = 870602731561419548L;
	private String docId;
	private String docType;
	private String docIdType;
	
	public CannotFindResultDocument(String docType, String docIdType, String docId)
	{
		this.docType = docType;
		this.docIdType = docIdType;
		this.docId = docId;
	}
	
	public String getMessage()
	{
		return "Cannot find " + docType + " with " + docIdType + ": " + docId;
	}
}
