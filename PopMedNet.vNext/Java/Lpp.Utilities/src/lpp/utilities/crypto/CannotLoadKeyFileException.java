package lpp.utilities.crypto;

public class CannotLoadKeyFileException extends Exception {
	
	private Throwable _originalException;
    
    /**
    * Constructor.
    *
    * @param	error	java.lang.Throwable. 	
    */
    public CannotLoadKeyFileException(Throwable error) {
		_originalException = error;
	}
    
    /**
     * This method returns the Throwable containd 
     * @return 	java.lang.Throwable		The original exception.
     */    
    public Throwable getOriginalException() {
		return _originalException;
    }
}
