(function ($) {
    // selectedFiles = [ { FIlename:'A', Size:'A' }, { Filename:'B', Size:'B' } ... ]
    $.fn.FileSelector = function jQuery$FileSelector(hiddenField, selectedFiles, fileColumnClass, sizeColumnClass) {
        var me = this;
        var selectedFilesArea = $(".SelectedFilesArea", me);
        var emptySelectionMessage = $(".EmptySelectionMessage", me);
        var allSelectedFiles = function () { return $("tr", selectedFilesArea); };

        function selectedFile(filename, size, fileColumnClass, sizeColumnClass) {
            return $("<tr>").attr("filename", filename)
                .append($("<td>").append(
                    $("<a href='#' class='Delete'>[remove]</a>")
                    .click(function () {
                        $(this).parents("tr").eq(0).remove();
                        //updateHiddenField();
                        //toggleExistingResultLinks();
                        //updateSelectedFilesArea();
                        return false;
                    }))
                )
                .append($("<td class='" + fileColumnClass + "'>").text(filename))
                .append($("<td class='" + sizeColumnClass + "'>").text(size));
            return res;
        }

        function updateSelectedFilesArea() {
            var files = allSelectedFiles();
            files.alternateClasses("", "Alt");
            emptySelectionMessage.toggle(!files.size());
        }

        function allSelectedFileIds() { return $.map(allSelectedFiles(), function (e) { return $(e).attr("filename"); }) }
 
        function updateHiddenField() {
            hiddenField.val(allSelectedFileIds().join("\t").replace(/,/g, "%comma;").replace(/\t/g, ","));
            updateDataDisplay();
        }

        $.each(selectedFiles, function () { selectedFilesArea.append(selectedFile(this.filename, this.size, fileColumnClass, sizeColumnClass)); });
        function updateDataDisplay() { me.dataDisplay(allSelectedFileIds().join(', ').replace(/%comma;/g, ",") || "No files uploaded"); }
        //updateDataDisplay();
        //updateSelectedFilesArea();
        return me;
    };
})
(jQuery);


/*
@model Lpp.Dns.HealthCare.FileDistribution.Models.FileDistributionModel
@{ this.Stylesheet( "FileDistribution.css" ); }
@{
    //Layout = null;
    //var id = Html.UniqueId();
    //this.ScriptReference("fileSelector.js");
    this.Stylesheet("fileSelector.css");
    //Html.Render<IUtilityFunctions>();
    //var fileColumnClass = "filename";
    //var sizeColumnClass = "size";
}

<script type="text/javascript">
    //$(function () {
    //    $("#@(id).FileSelector").FileSelector(
    //        $("input[name=RequestFiles]"),
    //        [@Html.Raw(string.Join(",", Model.RequestFileList.EmptyIfNull().Select(s => string.Format("{{filename:'{0}', size:'{1}'}}",(s.FileName ?? "").Replace("'", "\\'"), (s.Size ?? "").Replace("'", "\\'")))))],
    //        '@fileColumnClass',
    //        '@sizeColumnClass'
    //    )
    //})
</script>
*/