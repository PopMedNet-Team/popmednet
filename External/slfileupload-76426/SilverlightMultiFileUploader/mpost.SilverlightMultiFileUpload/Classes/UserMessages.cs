﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 *
 * http://www.codeplex.com/SLFileUpload/
 *
 * */

namespace mpost.SilverlightMultiFileUpload.Classes
{
    public class UserMessages
    {
        public UserMessages()
        {
        }

        private static mpost.SilverlightMultiFileUpload.UserMessages resource = new SilverlightMultiFileUpload.UserMessages();

        public mpost.SilverlightMultiFileUpload.UserMessages UserMessagesResource { get { return resource; } }
    }

}
