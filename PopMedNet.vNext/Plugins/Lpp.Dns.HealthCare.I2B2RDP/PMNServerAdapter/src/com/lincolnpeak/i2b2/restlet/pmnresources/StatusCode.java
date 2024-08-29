package com.lincolnpeak.i2b2.restlet.pmnresources;

import javax.xml.bind.annotation.XmlEnum;

@XmlEnum(String.class)
public enum StatusCode
{
    Complete(66),            // 1000010
    CompleteWithMessage(67), // 1000011
    Pending(4),              // 0000100
    InProgress(8),           // 0001000
    AwaitingApproval(16),    // 0010000
    Canceled(80),            // 1010000
    Error(96);               // 1100000

    private int code;
    
    StatusCode(int code)
    {
    	this.code = code;
    }
    
    StatusCode()
    {
    }
    
    public void setCode(int code)
    {
    	this.code = code;
    }
    
    public int getCode()
    {
    	return code;
    }
}