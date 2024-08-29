package com.lincolnpeak.i2b2.utils.xml;

import java.util.List;
import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.io.StringWriter;

import javax.xml.bind.JAXBContext;
import javax.xml.bind.JAXBException;
import javax.xml.bind.Marshaller;
import javax.xml.bind.Unmarshaller;
import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;

import org.w3c.dom.Element;
import org.xml.sax.SAXException;


@XmlRootElement(name="panel")
@XmlAccessorType(XmlAccessType.FIELD)
public class PanelType 
{
	@XmlElement(name="panel_number")
	private String panelNumber;
	
	@XmlElement(name="panel_accuracy_scale")
	private String panelAccuracyScale;
	
	@XmlElement(name="invert")
	private String invert;
	
	@XmlElement(name="panel_timing")
	private String panelTiming;
	
	@XmlElement(name="total_item_occurrences")
	private String totalItemOccurrences;
	
	@XmlElement(name="item")
	private List<ItemType> item;
	
	public String getPanelNumber()
	{
		return panelNumber;
	}
	
	public void setPanelNumber(String panelNumber)
	{
		this.panelNumber = panelNumber;
	}
	
	public String getPanelAccuracyScale()
	{
		return panelAccuracyScale;
	}
	
	public void setPanelAccuracyScale(String panelAccuracyScale)
	{
		this.panelAccuracyScale = panelAccuracyScale;
	}
	
	public String getInvert()
	{
		return invert;
	}
	
	public void setPanelTiming(String panelTiming)
	{
		this.panelTiming = panelTiming;
	}
	
	public String getPanelTiming()
	{
		return panelTiming;
	}
	
	public void setTotalItemOccurrences(String totalItemOccurrences)
	{
		this.totalItemOccurrences = totalItemOccurrences;
	}
	
	public List<ItemType> getItem()
	{
		return item;
	}
	
	public void setItem(List<ItemType> item)
	{
		this.item = item;
	}

}
