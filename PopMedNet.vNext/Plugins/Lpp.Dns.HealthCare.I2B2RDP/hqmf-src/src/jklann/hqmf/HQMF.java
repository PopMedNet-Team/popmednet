package jklann.hqmf;


import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;

import javax.ws.rs.Consumes;
import javax.ws.rs.DefaultValue;
import javax.ws.rs.GET;
import javax.ws.rs.POST;
import javax.ws.rs.Path;
import javax.ws.rs.PathParam;
import javax.ws.rs.Produces;
import javax.ws.rs.QueryParam;
import javax.ws.rs.WebApplicationException;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;
import javax.xml.transform.TransformerException;
import javax.xml.transform.stream.StreamResult;
import javax.xml.transform.stream.StreamSource;
import java.io.Reader;
import java.util.HashMap;
import java.util.logging.Level;
import java.util.logging.Logger;

import com.sun.jersey.api.Responses;
import com.sun.jersey.api.client.ClientResponse;
import com.sun.jersey.api.uri.UriTemplate;
import com.sun.jersey.spi.resource.Singleton;

/** Jeff Klann - 7/23/2012 
 * My HQMF resources for Jersey. Handles execution of 
 * XSL translation as well as proxying GET-based ONT lookups to a cell specified
 * in the .properties file. Things to possibly do:
 *  1) When deployed as a servlet, the servletcontext could probably be used to get the ONT cell location
 *  2) When deployed as a servlet, the Jersey-specified killServer will have unknown behavior and should probably be removed.
 *  3) The toHQMF service discards authentication information. Should go either in HQMF text field or in a wrapper.
 */
@Singleton
@Path("/hqmf")
public class HQMF {
	
	private static Logger logger = Logger.getLogger("jklann.hqmf.HQMF");
	
	/** When deployed as a Jersey server, this tries to stop the server. Do not use if deployed as a servlet.
	 * 
	 * @return Nothing useful
	 */
//    @GET @Path("/killServer")
//    @Produces("text/html")
//    public String killServer() {
//
//		JerseyServer.httpServer.stop();
//		System.exit(0);
//		return "Stopped server";
//    }
    
    /** Tries to produce a log of all XSL warnings and errors. Doesn't always work so well.
     * 
     * @return An XML log of XSL warnings and errors.
     */
    @GET @Path("/log")
    @Produces("text/xml")
    public String getLog() {
		return ProcessorErrorHandler.getLog();
    }
    
    /** Just a test service to consume the portion of the path after testpath/ and display it as components.
     * I left it just to make sure things are running.
     * 
     * @param id
     * @return
     */
    @Path("/testpath/{id: .+}") 
    @POST
    @Produces("text/html")
    public String get(@PathParam("id") String id) {
    	UriTemplate ut = new UriTemplate("{domain}/{project}/{username}/{key}"); // If receiving an entire URI, use {context: .*} at the start
    	HashMap<String, String> m = new HashMap<String, String>(); 
    	StringBuilder s = new StringBuilder();
    	if (ut.match(id, m)) {
    		s.append(m.get("domain"));
    		s.append("\n");
    		s.append(m.get("project"));
    		s.append("\n");
    		s.append(m.get("username"));
    		s.append("\n");
    		s.append(m.get("key"));
    		s.append("\n");
    		s.append(m.get("context"));
    	} else throw new WebApplicationException(Response.status(Responses.NOT_FOUND).entity("Path " + id + " should be of form domain/project/username/sessionkey").type("text/plain").build());
		return s.toString(); 
    	
    }
    
    /** Run a static conversion of {xsl-path}/i2b2-query.xml to HQMF, using live ONT lookups. Just a test.
     * 
     * @return HQMF version of i2b2-query.xml
     * @throws Exception
     */
    @GET @Path("/test")
    @Produces("text/xml")
    public String doTest() {
    	Processors p = null;
		p = Processors.getInstance(); 
		MyProps pr = MyProps.getInstance();
		ByteArrayOutputStream result = new ByteArrayOutputStream();
		StreamSource source = new StreamSource(pr.xslLoc+"/i2b2-query.xml");
		try {
			p.hqmf.transform(source,new StreamResult(result));
		} catch(TransformerException e) { return ProcessorErrorHandler.lastFatality; }
		return result.toString();
    }
    
    /** Reload the XSL transforms.
     * TODO: also reload the properties file.
     * 
     * @return
     * @throws Exception
     */
    @GET @Path("/reload")
    @Produces("text/html")
    public String reload() {
    	Processors p = Processors.getInstance();
    	p.reload();
    	MyProps.getInstance().reload();
		return "XSLT and hqmf.properties were reloaded!";
    	
    }
    
    /** An important method. Converts a POSTed i2b2 message to HQMF using live ONT code lookups.
     * The i2b2 XML must be a full request, not just a query definition, as authentication information for the ONT
     * lookups are extracted from it. Now calls two transforms, one to add the basecodes and unravel folders, one
     * to actually convert to HQMF. TODO: Preserve the authentication information.
     * 
     * @param input POST of i2b2 message
     * @return HQMF message
     * @throws Exception
     */
    @POST 
    @Path("/tohqmf") 
    @Consumes({MediaType.APPLICATION_XML,MediaType.TEXT_XML})
    @Produces("text/xml")
    public Response toHQMF(Reader input) 
    {
    	Processors p = null;
		p = Processors.getInstance();
		ByteArrayOutputStream result = new ByteArrayOutputStream();
    	StreamSource source = new StreamSource(input);
    	try {
    		p.i2b2plus.transform(source,new StreamResult(result));
    	} catch(TransformerException e) { return Response.status(ProcessorErrorHandler.lastFatalCode).entity(ProcessorErrorHandler.lastFatality).build(); }
    	logger.log(Level.INFO,"i2b2plus message",result.toString());
    	StreamSource s2 = new StreamSource(new ByteArrayInputStream(result.toByteArray()));
		result = new ByteArrayOutputStream(); 
    	try {
    		p.hqmf.transform(s2,new StreamResult(result));
    	} catch(TransformerException e) { return Response.status(ProcessorErrorHandler.lastFatalCode).entity(ProcessorErrorHandler.lastFatality).build(); }
    	logger.log(Level.INFO,"hqmf message",result.toString());
    	return Response.status(200).entity(result.toString()).build();   	
    }
    
    /** The other most important method. Takes a POSTED HQMF message and converts it to i2b2. Requires authentication
     * information on the path for ONT lookups. Takes the form /toi2b2/{domain}/{project}/{username}/token/{sessionkey} 
     * or /toi2b2/{domain}/{project}/{username}/password/{password}.
     * 
     * @param id The authentication info on the URL path
     * @param input The POSTed HQMF
     * @return i2b2 XML
     * @throws Exception
     */
    @POST 
    @Path("/toi2b2/{id: .+}") 
    @Consumes({MediaType.APPLICATION_XML,MediaType.TEXT_XML})
    @Produces("text/xml")
    public Response toI2B2(@PathParam("id") String id,Reader input) 
    {
    	Processors p = null;
		p = Processors.getInstance();
		HashMap<String, String> m = parseParams(id);
		
		// hqmf to ihqmf
		ByteArrayOutputStream result = new ByteArrayOutputStream();
    	StreamSource source = new StreamSource(input);
    	try {
    		p.ihqmf.transform(source,new StreamResult(result));
    	} catch(TransformerException e) { return Response.status(ProcessorErrorHandler.lastFatalCode).entity(ProcessorErrorHandler.lastFatality).build(); }
    	
    	logger.log(Level.INFO,"ihqmf message",result.toString());
		
    	// set ihqmf to xml auth params
		p.i2b2.setParameter("username", m.get("username"));
		p.i2b2.setParameter("userproject", m.get("project"));
		p.i2b2.setParameter("userdomain", m.get("domain"));
		p.i2b2.setParameter("sessiontoken", "");
		p.i2b2.setParameter("userpassword", "");
		if (m.get("isToken").equals("true")) p.i2b2.setParameter("sessiontoken", m.get("key"));
		else p.i2b2.setParameter("userpassword", m.get("key"));
    	
		// ihqmf to xml
		StreamSource s2 = new StreamSource(new ByteArrayInputStream(result.toByteArray()));
		result = new ByteArrayOutputStream(); 
		try {
			p.i2b2.transform(s2, new StreamResult(result));
		} catch(TransformerException e) { return Response.status(ProcessorErrorHandler.lastFatalCode).entity(ProcessorErrorHandler.lastFatality).build(); }
		logger.log(Level.INFO,"i2b2 message",result.toString());
		return Response.status(200).entity(result.toString()).build();    	
    }
    
    /** Converts HQMF to iHQMF. This is automatically done in the toI2B2 service call so this is only for testing.
     * 
     * @param input POSTed HQMF message.
     * @return iHQMF message.
     * @throws Exception
     */
    /*@POST 
    @Path("/toihqmf") 
    @Consumes({MediaType.APPLICATION_XML,MediaType.TEXT_XML})
    @Produces("text/xml")
    public String toIHQMF(Reader input) 
    {
    	Processors p = null;
		p = Processors.getInstance();
		
		// hqmf to ihqmf
		ByteArrayOutputStream result = new ByteArrayOutputStream();
    	StreamSource source = new StreamSource(input);
    	try {
    		p.ihqmf.transform(source,new StreamResult(result));
    	} catch(TransformerException e) { return ProcessorErrorHandler.lastFatality; }
		return result.toString();    	
    }*/
 
    /** Given an i2b2 key, return the associated child nodes. Authentication info must be on the path.
     * Takes the form /getChildren/{domain}/{project}/{username}/token/{sessionkey}?key={i2b2key}
     * or /getTermInfo/{domain}/{project}/{username}/password/{password}?key={i2b2key}
     * 
     * @param id Auth info on the path
     * @param key I2B2 key specified as a GET parameter.
     * @return The associated children.
     * @throws Exception
     */
    @Path("/getChildren/{id: .+}")
    @GET
    @Produces("text/xml")
    public String getChildren(@DefaultValue("i2b2demo/Demo/demo/password/demouser") @PathParam("id") String id,
    		@DefaultValue("\\\\I2B2\\i2b2\\Diagnoses\\Circulatory system (390-459)\\Acute Rheumatic fever (390-392)\\") @QueryParam("key") String key
    		) {
    	HashMap<String,String> m = parseParams(id);
    	Processors p = Processors.getInstance();
    	String request = p.generateRequest(m.get("username"), m.get("project"), m.get("domain"),m.get("key"),m.get("isToken").equals("true"), key,"getChildren");
    	logger.log(Level.INFO,"getChildren request",request);
    	ClientResponse response = p.getChildren.accept(MediaType.APPLICATION_XML,MediaType.TEXT_XML).type(MediaType.TEXT_XML).post(ClientResponse.class, request);
    	String xmlResponse = response.getEntity(String.class);  	
    	logger.log(Level.INFO,"getChildren response",xmlResponse);
		return xmlResponse.toString();
    }
    
    /** Given an i2b2 key, return the associated code. Authentication info must be on the path.
     * Takes the form /getTermInfo/{domain}/{project}/{username}/token/{sessionkey}?key={i2b2key}
     * or /getTermInfo/{domain}/{project}/{username}/password/{password}?key={i2b2key}
     * 
     * @param id Auth info on the path
     * @param key I2B2 key specified as a GET parameter.
     * @return The associated code.
     * @throws Exception
     */
    @Path("/getTermInfo/{id: .+}")
    @GET
    @Produces("text/xml")
    public String getTermInfo(@DefaultValue("i2b2demo/Demo/demo/password/demouser") @PathParam("id") String id,
    		@DefaultValue("\\\\I2B2\\i2b2\\Diagnoses\\Circulatory system (390-459)\\Acute Rheumatic fever (390-392)\\") @QueryParam("key") String key
    		) {
    	HashMap<String,String> m = parseParams(id);
    	Processors p = Processors.getInstance();
    	String request = p.generateRequest(m.get("username"), m.get("project"), m.get("domain"),m.get("key"),m.get("isToken").equals("true"), key,"getTermInfo");
    	logger.log(Level.INFO,"getTermInfo request for "+id+"?key="+key,request.toString());
    	ClientResponse response = p.getTermInfo.accept(MediaType.APPLICATION_XML,MediaType.TEXT_XML).type(MediaType.TEXT_XML).post(ClientResponse.class, request);
    	String xmlResponse = response.getEntity(String.class);
    	logger.log(Level.INFO,"getTermInfo response",xmlResponse);
		return xmlResponse.toString();
    }
    
    /** Given an i2b2 code, return the associated key. Authentication info must be on the path.
     * Takes the form /getCodeInfo/{domain}/{project}/{username}/token/{sessionkey}?key={i2b2code}
     * or /getCodeInfo/{domain}/{project}/{username}/password/{password}?key={i2b2code}
     * 
     * @param id Auth info on the path
     * @param key I2B2 code specified as a GET parameter.
     * @return The associated key.
     * @throws Exception
     */
    @Path("/getCodeInfo/{id: .+}")
    @GET
    @Produces("text/xml")
    public String getCodeInfo(@DefaultValue("i2b2demo/Demo/demo/password/demouser") @PathParam("id") String id,
    		@DefaultValue("ICD9:250") @QueryParam("key") String key
    		) { 	
    	HashMap<String,String> m = parseParams(id);
    	Processors p = Processors.getInstance();
    	String request = p.generateRequest(m.get("username"), m.get("project"), m.get("domain"),m.get("key"),m.get("isToken").equals("true"), key,"getCodeInfo");
    	logger.log(Level.INFO,"getCodeInfo request for "+id+"?key="+key,request.toString());
    	ClientResponse response = p.getCodeInfo.accept(MediaType.APPLICATION_XML,MediaType.TEXT_XML).type(MediaType.TEXT_XML).post(ClientResponse.class, request);
    	String xmlResponse = response.getEntity(String.class);   	
    	logger.log(Level.INFO,"getCodeInfo response",xmlResponse);
		return xmlResponse;
    }
    
    private HashMap<String, String> parseParams(String id) {
        UriTemplate ut1 = new UriTemplate("{domain}/{project}/{username}/token/{key}"); // If receiving an entire URI, use {context: .*} at the start
        UriTemplate ut2 = new UriTemplate("{domain}/{project}/{username}/password/{key}");
        HashMap<String, String> m = new HashMap<String, String>(); 
        if (ut1.match(id, m)) {
        	m.put("isToken", "true");
        	return m;
        } else if (ut2.match(id, m)) {
        	m.put("isToken","false");
        	return m;
        }
        else {
        	throw new WebApplicationException(Response.status(Responses.NOT_FOUND).entity("Path " + id + 
        			" should be of form {domain}/{project}/{username}/token/{sessionkey} or {domain}/{project}/{username}/password/{password}").type("text/plain").build());
        }

    }
}