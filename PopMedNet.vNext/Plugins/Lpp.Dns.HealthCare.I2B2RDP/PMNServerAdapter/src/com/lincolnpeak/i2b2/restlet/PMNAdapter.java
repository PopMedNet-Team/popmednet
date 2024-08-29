package com.lincolnpeak.i2b2.restlet;

import org.restlet.Application;  
import org.restlet.Restlet;  
import org.restlet.routing.Router;  

import com.lincolnpeak.i2b2.restlet.pmnresources.Close;
import com.lincolnpeak.i2b2.restlet.pmnresources.GetResponse;
import com.lincolnpeak.i2b2.restlet.pmnresources.GetResponseDocument;
import com.lincolnpeak.i2b2.restlet.pmnresources.GetStatus;
import com.lincolnpeak.i2b2.restlet.pmnresources.GetVersion;
import com.lincolnpeak.i2b2.restlet.pmnresources.PostRequest;
import com.lincolnpeak.i2b2.restlet.pmnresources.PostRequestDocument;
import com.lincolnpeak.i2b2.restlet.pmnresources.Start;
import com.lincolnpeak.i2b2.restlet.pmnresources.Stop;
  
public class PMNAdapter extends Application {  
  
    /** 
     * Creates a root Restlet that will receive all incoming calls. 
     */  
    @Override  
    public synchronized Restlet createInboundRoot() 
    {  
        // Create a router Restlet that routes each call to a  
        // new instance of HelloWorldResource.  
        Router router = new Router(getContext());  
  
        // Defines only one route  
        router.attach("/PostRequest/{requestId}", PostRequest.class);  
        router.attach("/PostRequestDocument/{requestId}/{documentId}/{offset}", PostRequestDocument.class);  
        router.attach("/Start/{requestId}", Start.class);  
        router.attach("/Stop/{requestId}", Stop.class);  
        router.attach("/GetStatus/{requestId}", GetStatus.class);  
        router.attach("/GetResponse/{requestId}", GetResponse.class);  
        router.attach("/GetResponseDocument/{requestId}/{documentId}/{offset}", GetResponseDocument.class);  
        router.attach("/Close/{requestId}", Close.class);  
        router.attach("/Version", GetVersion.class);
  
        return router;  
    }  
}  