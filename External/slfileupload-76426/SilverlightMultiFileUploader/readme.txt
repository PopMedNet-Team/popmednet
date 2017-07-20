/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 *
 * http://www.codeplex.com/SLFileUpload/
 *
 * */

Silverlight 4

Free Silverlight Multi File Uploader (v4.2)

UPDATED DOCUMENTATION CAN BE FOUND HERE:
http://www.michielpost.nl/Silverlight/MultiFileUploader/

Configuration: 
•MaxFileSizeKB: File size in KBs. 
•MaxUploads: Maximum number of simultaneous uploads 
•FileFilter: File filter, for example ony jpeg use: FileFilter=Jpeg (*.jpg) |*.jpg 
•CustomParam: Your custom parameter, anything here will be available in the WCF webservice 
•DefaultColor: The default color for the control, for example: LightBlue 
•ChunkSize: Size of each uploaded chunk in bytes (minimum is 4096, default is 4194304) (only for the HttpUploader) 
•UploadHandlerName: Custom specified name of the HttpUploadHandler, for example this can be "PHPUpload.php" to use the PHP upload handler. 
Parameters:

<asp:Silverlight ID="Xaml1" runat="server"  Source="~/ClientBin/mpost.SilverlightMultiFileUpload.xap" MinimumVersion="2.0.30523"  Width="415" Height="280" InitParameters ="MaxFileSizeKB=1000,MaxUploads=2,FileFilter=,CustomParam=1,DefaultColor=LightBlue" />

JavaScript Events
For a better integration with your own website, the Silverlight Multi File Uploader control has the following JavaScript events and properties:

Events:

•AllFilesFinished - Fires when all files are finished uploading (does not fire when Errors Occurred during upload)
•SingleFileUploadFinished - Fires when a single file succesfully uploaded
•ErrorOccurred - Fires when an error occurred during uploading
Properties:
The following properties can be read at any time

•TotalUploadedFiles: Total number of files uploaded
•TotalFilesSelected: Total number of files in the list
•Percentage: Percentage of total upload progress

Actions:
The following actions can be triggered using JavaScript:

•StartUpload: Starts the upload process
•ClearList: Clears the list
•SelectFiles: Not available anymore since Silverlight 3 due to security restrictions in Silverlight.
See the included testpages for example usage.

----------

Drop me a line on contact@michielpost.nl if this was useful for you or if you made something nice out of this.


