using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lpp.Dns.Portal.Models
{
    public class ResourcesModel
    {
      
        public string Contact1 
        {
            get { return "<b>Mini-Sentinel Operations Center Contact:</b>&nbsp;<a href=\"mailto:support@popmednet.org\">support@popmednet.org</a>"; } 
        }
        public string Contact2
        {
            get { return "<b>Lincoln Peak Partners Contact:</b>&nbsp;<a href=\"mailto:support@lincolnpeak.com\">support@lincolnpeak.com</a>"; }
        }
        public string CommonDoc
        {
            get { return "<a href=\"../Documentation/PopMedNet Overview and Administrators Guide_v3.2_v2.0.pdf\" class=\"nav-download\" target=\"_blank\">Overview and Administrators Guide</a>"; }
        }
        public string CommonDesc
        {
            get { return "This document describes the overall distributed querying system architecture, details the technical and security approaches implemented, and provides details on administering the network."; }
        }
        public string AdminDoc
        {
            get { return "<a href=\"../Documentation/INSERT_SYSTEM_DOC.pdf\" class=\"nav-download\" target=\"_blank\">System Administration Documentation</a>"; }
        }
        public string AdminDesc
        {
            get { return "This document describes the INSERT SYSTEM ADMIN DESCRIPTION HERE."; }
        }
        public string DMAdminDoc
        {
            get { return "<a href=\"../Documentation/DataMart Administrators Manual_PopMedNet_v3.2_v2.0.pdf\" class=\"nav-download\" target=\"_blank\">DataMart Administrator Manual</a>"; }
        }
        public string DMAdminDesc
        {
            get { return "This DataMart Administrator Manual describes the main features and functions for DataMart Administrators participating in the network."; }
        }
        public string InvestigatorDoc
        {
            get { return "<a href=\"../Documentation/Investigator Manual_PopMedNet_v3.2_v2.0.pdf\" class=\"nav-download\" target=\"_blank\">Investigator Manual</a>"; }
        }
        public string InvestigatorDesc
        {
            get { return "This Investigator Manual describes the main features and functions for Investigators participating in the network."; }
        }
        public string GDMAdminDoc
        {
            get { return "<a href=\"../Documentation/GroupAdminManual_Mini-Sentinel_v2.2_v1.0x.pdf\" class=\"nav-download\" target=\"_blank\">Group DataMart Administrators Manual</a>"; }
        }
        public string GDMAdminDesc
        {
            get { return "This Group DataMart Administrators Manual describes the main features and functions for Group Administrators participating in the network."; }
        }
        public string SummaryTableDoc
        {
            get { return "<a href=\"../Documentation/Mini-Sentinel Query Tool Summary Table Description_v1.4.pdf\" class=\"nav-download\" target=\"_blank\">Summary Table Description</a>"; }
        }
        public string SummaryTableDesc
        {
            get { return "This Summary Table Description document describes the schema and format of the summary tables used in the network."; }
        }
        public string DMSetupDoc
        {
            get { return "<a href=\"../Documentation/PopMedNet_Setup_26Jul11_1.0.pptx\" class=\"nav-download\" target=\"_blank\">DataMart Client Application Setup Instructions</a>"; }
        }
        public string DMSetupDesc
        {
            get { return "This Setup presentation for DataMart Administrators describes how to configure the DataMart Client application."; }
        }
        public string ReleaseNotesDoc
        {
            get { return "<a href=\"../Documentation/PMN Release Notes.pdf\" class=\"nav-download\" target=\"_blank\">Release Notes</a>"; }
        }
        public string ReleaseNotesDesc
        {
            get { return "This document outlines and describes software changes for the most current version of software used by the network."; }
        }
        public string DataMartAvailabilityLink
        {
            get { return "<a href=\"/request/metadatalist\" class=\"nav-download\">Query Types by DataMart - Time Period Availability</a>"; }

            //get { return "<a href=\"notyetimplemented\" class=\"nav-download\" target=\"_blank\">Query Types by DataMart - Time Period Availability</a>"; }
        }
        public string DataMartAvailabilityDesc
        {
            get { return "This table contains information on supported query types for DataMarts within the network."; }
        }
        public string PortalLink
        {
            get { return "<a href=\"http://www.minisentinel.org\" class=\"nav-download\" target=\"_blank\">FDA Mini-Sentinel</a>"; }
        }
        public string PortalDesc
        {
            get { return "A pilot project sponsored by the U.S. Food and Drug Administration (FDA) to inform and facilitate development of a fully operational active surveillance system, the Sentinel System, for monitoring the safety of FDA-regulated medical products."; }
        }
        public string WikiLink
        {
            get { return "<a href=\"http://popmednet.atlassian.net/wiki\" target=\"_blank\">PopMedNet Issue Tracker</a>"; }
        }
        public string WikiDesc
        {
            get { return "Connect to the PopMedNet community of users, developers, and network administrators. Community members may contribute code, find helpful documentation, and learn how others are using the tool."; }
        }
        public string JiraLink
        {
            get { return "<a href=\"http://popmednet.atlassian.net\" target=\"_blank\">PopMedNet Issue Tracker</a>"; }
        }
        public string JiraDesc
        {
            get { return "Report bugs and submit feedback and ideas for new features or enhancements to the PopMedNet software."; }
        }
    }
}