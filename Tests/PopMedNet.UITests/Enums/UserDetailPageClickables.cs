using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.UITests.Enums
{
    public enum UserDetailPageClickables
    {
        SecurityGroupsTab,
        PermissionsTab,
        AddSecurityGroupButton,
        SaveButton,
        OkButton,
        ActivateLink,
        DeleteUserButton,
        RejectLink,
        OrganizationDropDown,
        ConfirmYesDialogButton // Consider refactoring dialogs into their own class that can be invoked
    }
}
