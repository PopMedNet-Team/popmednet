<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Silverlight Project Test Page </title>

    <style type="text/css">
    html, body {
	    height: 100%;
	    overflow: auto;
    }
    body {
	    padding: 0;
	    margin: 0;
    }
    #silverlightControlHost {
	    height: 400px;
    }
    </style>
    
     <script type="text/javascript">
         var slCtl = null;

         //DO NOT FORGET TO REGISTER THIS FUNCTION WITH THE SILVERIGHT CONTROL
         // <param name="onload" value="pluginLoaded" />
         function pluginLoaded(sender) {

             //IMPORTANT: Make sure this is the same ID as the ID in your <OBJECT tag (<object id="MultiFileUploader" etc)
             slCtl = document.getElementById("MultiFileUploader");

                         
             //Register All Files Finished Uploading event
             slCtl.Content.Files.AllFilesFinished = AllFilesFinished;

             //Register single file finished event
             slCtl.Content.Files.SingleFileUploadFinished = SingleFileFinished;

             //Register Error occurred during uploading event
             slCtl.Content.Files.ErrorOccurred = ShowErrorDiv;

             //Set your custom parameter using javascript
             //This parameter will be available in the webservice and you can use it for your business logic
             //Or use it to identity the upload to a sinle row in your database
             //slCtl.Content.Files.CustomParams = "custom_id=1"; 
         }


         function ShowNumberOfFilesUploaded() {
             if (slCtl != null) {
                 alert("Total Files Uploaded: " + slCtl.Content.Files.TotalUploadedFiles);
             }
         }

         function ShowTotalNumberOfFilesSelected() {
             if (slCtl != null) {
                 alert("Total Files Selected: " + slCtl.Content.Files.TotalFilesSelected);
             }
         }

         function ShowUploadProgress() {
             if (slCtl != null) {
                 alert("Progress: " + slCtl.Content.Files.Percentage);
             }
         }



         //This function is registred in the pluginLoaded function (slCtl.Content.Files.AllFilesFinished = AllFilesFinished;)
         function AllFilesFinished() {
             document.getElementById('AllFinishedDiv').style.display = 'block';
         }

         //This function is registred in the pluginLoaded function (slCtl.Content.Files.SingleFileUploadFinished = SingleFileFinished;)
         function SingleFileFinished() {
             document.getElementById('SingleFileFinishedDiv').style.display = 'block';
         }

         //This function is registred in the pluginLoaded function (slCtl.Content.Files.ErrorOccurred = ShowErrorDiv;)
         function ShowErrorDiv() {
             document.getElementById('ErrorDiv').style.display = 'block';
         }

         //Actions
         function StartUpload() {
             if (slCtl != null) {
                 slCtl.Content.Control.StartUpload();
             }
         }

         function ClearList() {
             if (slCtl != null) {
                 slCtl.Content.Control.ClearList();
             }
         }


        
    </script>
    
    
    <script type="text/javascript">
        function onSilverlightError(sender, args) {
        
            var appSource = "";
            if (sender != null && sender != 0) {
                appSource = sender.getHost().Source;
            } 
            var errorType = args.ErrorType;
            var iErrorCode = args.ErrorCode;
            
            var errMsg = "Unhandled Error in Silverlight 2 Application " +  appSource + "\n" ;

            errMsg += "Code: "+ iErrorCode + "    \n";
            errMsg += "Category: " + errorType + "       \n";
            errMsg += "Message: " + args.ErrorMessage + "     \n";

            if (errorType == "ParserError")
            {
                errMsg += "File: " + args.xamlFile + "     \n";
                errMsg += "Line: " + args.lineNumber + "     \n";
                errMsg += "Position: " + args.charPosition + "     \n";
            }
            else if (errorType == "RuntimeError")
            {           
                if (args.lineNumber != 0)
                {
                    errMsg += "Line: " + args.lineNumber + "     \n";
                    errMsg += "Position: " +  args.charPosition + "     \n";
                }
                errMsg += "MethodName: " + args.methodName + "     \n";
            }

            throw new Error(errMsg);
        }
    </script>
</head>


<body>
<h1>PHP version, see initParams configuration...</h1>
initParams value: HttpUploader=true,UploadHandlerName=PHPUpload.php <br /> <br />

    <!-- Runtime errors from Silverlight will be displayed here.
	This will contain debugging information and should be removed or hidden when debugging is completed -->
	<div id='errorLocation' style="font-size: small;color: Gray;"></div>

    <div id="silverlightControlHost">
		<object id="MultiFileUploader" data="data:application/x-silverlight-2," type="application/x-silverlight-2" Width="450" Height="280">
			<param name="source" value="ClientBin/mpost.SilverlightMultiFileUpload.xap"/>
			<param name="onerror" value="onSilverlightError" />
			<param name="background" value="white" />			
			<param name="onload" value="pluginLoaded" />
			<param name="initParams" value="UploadHandlerName=PHPUpload.php,ChunkSize=4194304,DefaultColor=Green" />
			<param name="minRuntimeVersion" value="5.0.61118.0" />
                <param name="autoUpgrade" value="true" />
                <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=5.0.61118.0" style="text-decoration: none">
                    <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="Get Microsoft Silverlight"
                        style="border-style: none" />
                </a>
		</object>
		<iframe style='visibility:hidden;height:0;width:0;border:0px'></iframe>
    </div>
    
     <div id="AllFinishedDiv" style="display:none;">All files finished (javascript triggered).</div>
     <div id="SingleFileFinishedDiv" style="display:none;">Single file upload finished (javascript triggered).</div>
    <div id="ErrorDiv" style="display:none;">Error occurred during upload (javascript triggered).</div>
   
    <button onclick="ShowNumberOfFilesUploaded()">Javascript test: Show Number of files uploaded</button><br />
    <button onclick="ShowTotalNumberOfFilesSelected()">Javascript test: Show total number of files selected</button><br />
    <button onclick="ShowUploadProgress();">Javascript test: Show upload progress</button><br />
    
     <br />
      <br />
      <button onclick="StartUpload();">Start Upload</button><br />
       <button onclick="ClearList();">Clear List</button><br />
</body>
</html>
