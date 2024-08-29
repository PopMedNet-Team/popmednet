package lpp.utilities.crypto;

/**
 * @author Daniel Dee
 * @version 1.0
 */

public class InvalidCipherAlgorithmException extends Exception 
{

      /**
       * @param _exception the detail message.
       */
      public InvalidCipherAlgorithmException(Exception _exception) {
          super(_exception);
      }
}