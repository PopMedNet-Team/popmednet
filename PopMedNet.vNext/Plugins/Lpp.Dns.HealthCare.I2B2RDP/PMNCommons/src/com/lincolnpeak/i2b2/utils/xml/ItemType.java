package com.lincolnpeak.i2b2.utils.xml;

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


@XmlRootElement(name="item")
@XmlAccessorType(XmlAccessType.FIELD)
public class ItemType 
{
	@XmlElement(name="hlevel")
	private String hlevel;
	
	@XmlElement(name="item_name")
	private String itemName;
	
	@XmlElement(name="item_key")
	private String itemKey;
	
	@XmlElement(name="tooltip")
	private String tooltip;
	
	@XmlElement(name="class")
	private String clazz;
	
	@XmlElement(name="item_icon")
	private String itemIcon;
	
	@XmlElement(name="item_is_synonym")
	private String itemIsSynonym;
	
	
	public String getHLevel()
	{
		return hlevel;
	}
	
	public void setHLevel(String hlevel)
	{
		this.hlevel = hlevel;
	}
	
	public String getItemName()
	{
		return itemName;
	}
	
	public void setItemName(String itemName)
	{
		this.itemName = itemName;
	}
	
	public String getItemKey()
	{
		return itemKey;
	}
	
	public void setItemKey(String itemKey)
	{
		this.itemKey = itemKey;
	}
	

	public String getClazz()
	{
		return clazz;
	}
	
	public void setClazz(String clazz)
	{
		this.clazz = clazz;
	}
	
	public String getTooltip()
	{
		return tooltip;
	}
	
	public void setTooltip(String tooltip)
	{
		this.tooltip = tooltip;
	}
	
	public String getItemIcon()
	{
		return itemIcon;
	}
	
	public void setItemIcon(String itemIcon)
	{
		this.itemIcon = itemIcon;
	}
	
	public String getItemIsSynonum()
	{
		return itemIsSynonym;
	}
	
	public void setItemIsSynonym(String itemIsSynonym)
	{
		this.itemIsSynonym = itemIsSynonym;
	}
	
	public ItemType clone(ItemType item)
	{
		ItemType newItem = new ItemType();
		newItem.clazz = item.clazz;
		newItem.hlevel = item.hlevel;
		newItem.itemIcon = item.itemIcon;
		newItem.itemIsSynonym = item.itemIsSynonym;
		newItem.itemKey = item.itemKey;
		newItem.itemName = item.itemName;
		newItem.tooltip = item.tooltip;
		return newItem;
	}
}
