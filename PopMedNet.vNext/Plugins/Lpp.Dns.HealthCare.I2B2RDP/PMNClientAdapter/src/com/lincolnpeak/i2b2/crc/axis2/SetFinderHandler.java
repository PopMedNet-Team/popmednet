package com.lincolnpeak.i2b2.crc.axis2;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.List;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

import com.lincolnpeak.i2b2.crc.axis2.pmnresponses.DataMartResponse;
import com.lincolnpeak.i2b2.crc.axis2.pmnresponses.Document;
import com.lincolnpeak.i2b2.exceptions.CannotBuildResponse;

public class SetFinderHandler 
{
	protected final Log log = LogFactory.getLog(getClass());

	protected String loadI2B2MessageTemplate(String name) throws IOException
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
	
	protected String buildQueryResultInstancesFragment(String i2b2RequestType, List<DataMartResponse> datamartResponses, String queryInstanceId) throws CannotBuildResponse
	{
		String queryResultInstanceFragment = "";

		try
		{
			String queryResultInstanceTemplate = loadI2B2MessageTemplate(i2b2RequestType + "Fragment");

			if(datamartResponses != null)
				for(DataMartResponse r : datamartResponses)
				{
					if(r.getDocuments() != null)
						for(Document doc : r.getDocuments())
						{
							// We're only interested in the result documents.
							if(doc.getName().equals("CRC_QRY_runQueryInstance_fromQueryDefinition.xml"))
							{
								String docId = doc.getId();
								queryResultInstanceFragment += queryResultInstanceTemplate.replace("${QUERY_INSTANCE_ID}", queryInstanceId)
								                                                          .replace("${QUERY_RESULT_INSTANCE_ID}", queryInstanceId + "," + docId);
							}
						}
				}
			
			return queryResultInstanceFragment;
		}
		catch(Exception e)
		{
			log.error(e);
			throw new CannotBuildResponse(e);
		}

	}
	
}
