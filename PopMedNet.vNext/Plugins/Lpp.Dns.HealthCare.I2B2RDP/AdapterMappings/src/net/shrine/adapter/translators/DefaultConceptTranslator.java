package net.shrine.adapter.translators;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

import com.lincolnpeak.i2b2.utils.xml.ItemType;
import com.lincolnpeak.i2b2.utils.xml.PanelType;
import com.lincolnpeak.i2b2.utils.xml.QueryDefinitionType;


import net.shrine.config.AdapterMappings;


import javax.xml.bind.JAXBException;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;


/**
 * @Author David Ortiz
 * @Date June 28, 2010
 * <p/>
 * This implementation of Translator is particularly intolerant of missing
 * mappings in the Adaptor. It is meant for use in the Harvard deployment
 * where each concept should be mapped and an unmapped concept is an error
 * condition and we should fail as noisily as possible.
 * <p/>
 * Other shrine deployments such as CARRAnet where mapping is less import,
 * the simple translator is more tolerant of errors while still allowing
 * for some mapping to take place.
 * @see SimpleConceptTranslator
 */
public class DefaultConceptTranslator
{
	protected final Log log = LogFactory.getLog(getClass());

    protected final AdapterMappings mappings;

    public DefaultConceptTranslator() //throws ConfigException
    {
        mappings = AdapterMappings.getDefaultInstance();
    }

    public DefaultConceptTranslator(final AdapterMappings mappings)
    {
        this.mappings = mappings;
    }

    public DefaultConceptTranslator(final Map<String, List<String>> adaptorMappings)
    {
        mappings = new AdapterMappings();

        for(final String s : adaptorMappings.keySet())
        {
            for(final String s2 : adaptorMappings.get(s))
            {
                mappings.addMapping(s, s2);
            }
        }
    }

    public void translateQueryDefinition(final QueryDefinitionType queryDef) //throws SerializationException, AdapterMappingException
    {
        final List<PanelType> panels = queryDef.getPanel();

        for(final PanelType panel : panels)
        {
            translatePanel(panel);
        }
    }

    protected void translatePanel(final PanelType panel) //throws SerializationException, AdapterMappingException
    {
        final List<ItemType> items = panel.getItem();
        final List<ItemType> translatedItems = new ArrayList<ItemType>();

        for(final ItemType item : items)
        {
            final List<String> locals = mappings.getMappings(item.getItemKey());

            for(final String local : locals)
            {
//                try
//                {
                    final ItemType translatedItem = item.clone(item);
                    translatedItem.setItemKey(local);
                    translatedItems.add(translatedItem);
//                }
//                catch(final JAXBException jaxbe)
//                {
//                    final String msg = "Translation error- failed to copy construct :" + String.valueOf(item);
//
//                    log.error(msg, jaxbe);
//
////                    throw new SerializationException(msg, jaxbe);
//                }
            }
        }

        if(translatedItems.isEmpty())
        {
//            throw new AdapterMappingException(String.format("Panel %d contains no mappable terms", panel.getPanelNumber()));
        }

        items.clear();
        items.addAll(translatedItems);
    }
        
}
