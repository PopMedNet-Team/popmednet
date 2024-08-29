package com.lincolnpeak.i2b2.crc.axis2;

import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.io.StringWriter;

import javax.xml.bind.JAXBContext;
import javax.xml.bind.JAXBException;
import javax.xml.bind.Marshaller;
import javax.xml.bind.Unmarshaller;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.w3c.dom.Element;
import org.xml.sax.SAXException;

public class MessageElement 
{
	protected final static Log log = LogFactory.getLog(MessageElement.class);

	/**
	 * Deserializes the xml to an object of type t.
	 * @param <T> a type of MessageElement
	 * @param t a class object of type T
	 * @param xml the xml to deserialize
	 * @return an object of type T
	 * @throws SAXException
	 * @throws IOException
	 * @throws ParserConfigurationException
	 * @throws JAXBException
	 */
	@SuppressWarnings("unchecked")
	public static <T extends MessageElement> T load(Class<T> t, String xml) throws SAXException, IOException, ParserConfigurationException, JAXBException
	{
 		Element node =  DocumentBuilderFactory.newInstance().newDocumentBuilder()
			.parse(new ByteArrayInputStream(xml.getBytes())).getDocumentElement();		
		JAXBContext context = JAXBContext.newInstance(t);
		Unmarshaller unmarshaller = context.createUnmarshaller();
		T me = (T) unmarshaller.unmarshal(node);
		log.debug(">>>>>>>>>>>>>>>> Unmarshalled back to the following:");
		log.debug(me);
        return me;
	}
	
	/**
	 * Serializes this object to XML.
	 */
	public String toString()
	{
		StringWriter w = new StringWriter();
		
		try
		{
			JAXBContext context = JAXBContext.newInstance(this.getClass());
			Marshaller marshaller = context.createMarshaller();
			marshaller.setProperty(Marshaller.JAXB_FRAGMENT, Boolean.TRUE);
	        marshaller.marshal(this, w);
		}
		catch(JAXBException e)
		{
			log.error(e);
		}
		
        return w.toString();
	}
}
