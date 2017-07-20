<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Silverlight Multi File Uploader Test Page</title>
    <style type="text/css">
        html, body
        {
            overflow: auto;
        }
        body
        {
            padding: 0;
            margin: 0;
        }
        #silverlightControlHost
        {
        }
    </style>
    <script type="text/javascript">
        function onSilverlightError(sender, args) {

            var appSource = "";
            if (sender != null && sender != 0) {
                appSource = sender.getHost().Source;
            }
            var errorType = args.ErrorType;
            var iErrorCode = args.ErrorCode;

            var errMsg = "Unhandled Error in Silverlight 2 Application " + appSource + "\n";

            errMsg += "Code: " + iErrorCode + "    \n";
            errMsg += "Category: " + errorType + "       \n";
            errMsg += "Message: " + args.ErrorMessage + "     \n";

            if (errorType == "ParserError") {
                errMsg += "File: " + args.xamlFile + "     \n";
                errMsg += "Line: " + args.lineNumber + "     \n";
                errMsg += "Position: " + args.charPosition + "     \n";
            }
            else if (errorType == "RuntimeError") {
                if (args.lineNumber != 0) {
                    errMsg += "Line: " + args.lineNumber + "     \n";
                    errMsg += "Position: " + args.charPosition + "     \n";
                }
                errMsg += "MethodName: " + args.methodName + "     \n";
            }

            throw new Error(errMsg);
        }
    </script>
</head>
<body>
    <div>
        <h1>
            Default HttpHandler Upload</h1>
        <div id="silverlightControlHost">
            <object id="MultiFileUploader" data="data:application/x-silverlight-2," type="application/x-silverlight-2"
                width="450" height="280">
                <param name="source" value="ClientBin/mpost.SilverlightMultiFileUpload.xap" />
                <param name="onerror" value="onSilverlightError" />
                <param name="initParams" value="MaxFileSizeKB=,MaxUploads=2,FileFilter=,ChunkSize=4194304,CustomParams=yourparameters,DefaultColor=White" />
                <param name="background" value="white" />
                <param name="onload" value="pluginLoaded" />
                <param name="minRuntimeVersion" value="5.0.61118.0" />
                <param name="autoUpgrade" value="true" />
                <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=5.0.61118.0" style="text-decoration: none">
                    <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="Get Microsoft Silverlight"
                        style="border-style: none" />
                </a>
            </object>
            <iframe style='visibility: hidden; height: 0; width: 0; border: 0px'></iframe>
        </div>
    </div>
</body>
</html>
